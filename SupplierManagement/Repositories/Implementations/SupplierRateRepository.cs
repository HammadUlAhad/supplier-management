using Microsoft.EntityFrameworkCore;
using SupplierManagement.Database;
using SupplierManagement.Models.Domain;
using SupplierManagement.Repositories.Interfaces;

namespace SupplierManagement.Repositories.Implementations
{
    public class SupplierRateRepository : ISupplierRateRepository
    {
        private readonly SupplierManagementDbContext _context;

        public SupplierRateRepository(SupplierManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierRate>> GetAllAsync()
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .OrderBy(sr => sr.Supplier.Name)
                .ThenBy(sr => sr.RateStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierRate>> GetBySupplierId(int supplierId)
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .Where(sr => sr.SupplierId == supplierId)
                .OrderBy(sr => sr.RateStartDate)
                .ToListAsync();
        }

        public async Task<SupplierRate?> GetByIdAsync(int id)
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .FirstOrDefaultAsync(sr => sr.SupplierRateId == id);
        }

        public async Task<SupplierRate> AddAsync(SupplierRate supplierRate)
        {
            await _context.SupplierRates.AddAsync(supplierRate);
            await _context.SaveChangesAsync();
            return supplierRate;
        }

        public async Task<SupplierRate?> UpdateAsync(SupplierRate supplierRate)
        {
            var existingRate = await _context.SupplierRates.FindAsync(supplierRate.SupplierRateId);
            if (existingRate == null)
                return null;

            existingRate.SupplierId = supplierRate.SupplierId;
            existingRate.Rate = supplierRate.Rate;
            existingRate.RateStartDate = supplierRate.RateStartDate;
            existingRate.RateEndDate = supplierRate.RateEndDate;
            existingRate.CreatedByUser = supplierRate.CreatedByUser;

            await _context.SaveChangesAsync();
            return existingRate;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplierRate = await _context.SupplierRates.FindAsync(id);
            if (supplierRate == null)
                return false;

            _context.SupplierRates.Remove(supplierRate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SupplierRates.AnyAsync(sr => sr.SupplierRateId == id);
        }

        public async Task<IEnumerable<SupplierRate>> GetAllWithSupplierInfoAsync()
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .OrderBy(sr => sr.Supplier.Name)
                .ThenBy(sr => sr.RateStartDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierRate>> GetBySupplierIdWithInfoAsync(int supplierId)
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .Where(sr => sr.SupplierId == supplierId)
                .OrderBy(sr => sr.RateStartDate)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// High-performance overlap detection using SQL-based algorithm
        /// This replaces the O(n²) in-memory algorithm with an efficient SQL query
        /// Scales to millions of records with proper indexing
        /// </summary>
        /// <param name="supplierId">Optional supplier ID to filter results</param>
        /// <returns>Overlapping rate pairs with supplier information</returns>
        public async Task<IEnumerable<OverlapResult>> GetOverlappingRatesAsync(int? supplierId = null)
        {
            var sql = @"
                WITH OverlappingPairs AS (
                    SELECT 
                        r1.SupplierId,
                        s.Name as SupplierName,
                        r1.SupplierRateId as Rate1Id,
                        r1.Rate as Rate1Value,
                        r1.RateStartDate as Rate1Start,
                        r1.RateEndDate as Rate1End,
                        r2.SupplierRateId as Rate2Id,
                        r2.Rate as Rate2Value,
                        r2.RateStartDate as Rate2Start,
                        r2.RateEndDate as Rate2End,
                        CASE 
                            WHEN r1.RateStartDate > r2.RateStartDate THEN r1.RateStartDate 
                            ELSE r2.RateStartDate 
                        END as OverlapStart,
                        CASE 
                            WHEN ISNULL(r1.RateEndDate, '2099-12-31') < ISNULL(r2.RateEndDate, '2099-12-31') 
                            THEN ISNULL(r1.RateEndDate, '2099-12-31')
                            ELSE ISNULL(r2.RateEndDate, '2099-12-31')
                        END as OverlapEnd
                    FROM SupplierRates r1
                    INNER JOIN SupplierRates r2 ON r1.SupplierId = r2.SupplierId 
                        AND r1.SupplierRateId < r2.SupplierRateId
                    INNER JOIN Suppliers s ON r1.SupplierId = s.SupplierId
                    WHERE r1.RateStartDate <= ISNULL(r2.RateEndDate, '2099-12-31')
                        AND r2.RateStartDate <= ISNULL(r1.RateEndDate, '2099-12-31')
                        AND (@SupplierId IS NULL OR r1.SupplierId = @SupplierId)
                )
                SELECT 
                    SupplierId,
                    SupplierName,
                    Rate1Id,
                    Rate1Value,
                    Rate1Start,
                    Rate1End,
                    Rate2Id,
                    Rate2Value,
                    Rate2Start,
                    Rate2End,
                    OverlapStart,
                    CASE 
                        WHEN OverlapEnd = '2099-12-31' THEN CAST(GETDATE() as date)
                        ELSE OverlapEnd 
                    END as OverlapEnd
                FROM OverlappingPairs 
                WHERE OverlapStart <= OverlapEnd
                ORDER BY SupplierId, Rate1Start";

            var parameters = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@SupplierId", supplierId.HasValue ? (object)supplierId.Value : DBNull.Value)
            };

            var results = new List<OverlapResult>();

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new OverlapResult
                {
                    SupplierId = reader.GetInt32(0),  // SupplierId
                    SupplierName = reader.GetString(1),  // SupplierName
                    Rate1Id = reader.GetInt32(2),  // Rate1Id
                    Rate1Value = reader.GetDecimal(3),  // Rate1Value
                    Rate1Start = reader.GetDateTime(4),  // Rate1Start
                    Rate1End = reader.IsDBNull(5) ? null : reader.GetDateTime(5),  // Rate1End
                    Rate2Id = reader.GetInt32(6),  // Rate2Id
                    Rate2Value = reader.GetDecimal(7),  // Rate2Value
                    Rate2Start = reader.GetDateTime(8),  // Rate2Start
                    Rate2End = reader.IsDBNull(9) ? null : reader.GetDateTime(9),  // Rate2End
                    OverlapStart = reader.GetDateTime(10),  // OverlapStart
                    OverlapEnd = reader.GetDateTime(11)  // OverlapEnd
                });
            }

            return results;
        }
    }

    /// <summary>
    /// Represents an overlapping rate pair result from the optimized SQL query
    /// Used to avoid complex object mapping and improve performance
    /// </summary>
    public class OverlapResult
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int Rate1Id { get; set; }
        public decimal Rate1Value { get; set; }
        public DateTime Rate1Start { get; set; }
        public DateTime? Rate1End { get; set; }
        public int Rate2Id { get; set; }
        public decimal Rate2Value { get; set; }
        public DateTime Rate2Start { get; set; }
        public DateTime? Rate2End { get; set; }
        public DateTime OverlapStart { get; set; }
        public DateTime OverlapEnd { get; set; }
    }
}
