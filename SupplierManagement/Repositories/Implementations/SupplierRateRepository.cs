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
                .AsNoTracking()
                .OrderBy(sr => sr.Supplier.Name)
                .ThenBy(sr => sr.RateStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupplierRate>> GetBySupplierId(int supplierId)
        {
            return await _context.SupplierRates
                .Where(sr => sr.SupplierId == supplierId)
                .Include(sr => sr.Supplier)
                .AsNoTracking()
                .OrderBy(sr => sr.RateStartDate)
                .ToListAsync();
        }

        public async Task<SupplierRate?> GetByIdAsync(int id)
        {
            return await _context.SupplierRates
                .Include(sr => sr.Supplier)
                .AsNoTracking()
                .FirstOrDefaultAsync(sr => sr.SupplierRateId == id);
        }

        public async Task<SupplierRate> AddAsync(SupplierRate supplierRate)
        {
            _context.SupplierRates.Add(supplierRate);
            await _context.SaveChangesAsync();
            return supplierRate;
        }

        public async Task<SupplierRate?> UpdateAsync(SupplierRate supplierRate)
        {
            var existingRate = await _context.SupplierRates.FindAsync(supplierRate.SupplierRateId);
            if (existingRate == null)
            {
                return null;
            }

            _context.Entry(existingRate).CurrentValues.SetValues(supplierRate);
            await _context.SaveChangesAsync();
            return existingRate;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplierRate = await _context.SupplierRates.FindAsync(id);
            if (supplierRate == null)
            {
                return false;
            }

            _context.SupplierRates.Remove(supplierRate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SupplierRates.AnyAsync(sr => sr.SupplierRateId == id);
        }

        public async Task<IEnumerable<OverlapResult>> GetOverlappingRatesAsync(int? supplierId = null)
        {
            // Use Entity Framework LINQ for overlap detection - more reliable than raw SQL
            var query = _context.SupplierRates
                .Include(sr => sr.Supplier)
                .AsNoTracking();

            if (supplierId.HasValue)
            {
                query = query.Where(sr => sr.SupplierId == supplierId.Value);
            }

            var rates = await query.ToListAsync();
            
            var results = new List<OverlapResult>();

            // Group by supplier and check for overlaps within each supplier
            var supplierGroups = rates.GroupBy(r => r.SupplierId);

            foreach (var supplierGroup in supplierGroups)
            {
                var supplierRates = supplierGroup.OrderBy(r => r.RateStartDate).ToList();
                var supplierName = supplierRates.First().Supplier?.Name ?? "Unknown";

                // Check each rate against all other rates for this supplier
                for (int i = 0; i < supplierRates.Count; i++)
                {
                    for (int j = i + 1; j < supplierRates.Count; j++)
                    {
                        var rate1 = supplierRates[i];
                        var rate2 = supplierRates[j];

                        // Check if rates overlap
                        var overlapInfo = GetOverlapInfo(rate1, rate2);
                        if (overlapInfo.HasValue)
                        {
                            results.Add(new OverlapResult
                            {
                                SupplierId = rate1.SupplierId,
                                SupplierName = supplierName,
                                Rate1Id = rate1.SupplierRateId,
                                Rate1Value = rate1.Rate,
                                Rate1StartDate = rate1.RateStartDate,
                                Rate1EndDate = rate1.RateEndDate,
                                Rate2Id = rate2.SupplierRateId,
                                Rate2Value = rate2.Rate,
                                Rate2StartDate = rate2.RateStartDate,
                                Rate2EndDate = rate2.RateEndDate,
                                OverlapStartDate = overlapInfo.Value.StartDate,
                                OverlapEndDate = overlapInfo.Value.EndDate
                            });
                        }
                    }
                }
            }

            return results.OrderBy(r => r.SupplierId)
                          .ThenBy(r => r.Rate1StartDate)
                          .ThenBy(r => r.Rate2StartDate);
        }

        private (DateTime StartDate, DateTime EndDate)? GetOverlapInfo(SupplierRate rate1, SupplierRate rate2)
        {
            // Calculate effective end dates (use far future date for null end dates)
            var rate1End = rate1.RateEndDate ?? new DateTime(9999, 12, 31);
            var rate2End = rate2.RateEndDate ?? new DateTime(9999, 12, 31);

            // Check if there's an overlap: start1 <= end2 AND start2 <= end1
            if (rate1.RateStartDate <= rate2End && rate2.RateStartDate <= rate1End)
            {
                // Calculate overlap period
                var overlapStart = rate1.RateStartDate > rate2.RateStartDate ? rate1.RateStartDate : rate2.RateStartDate;
                var overlapEnd = rate1End < rate2End ? rate1End : rate2End;

                // Convert far future date back to null representation
                if (overlapEnd == new DateTime(9999, 12, 31))
                {
                    overlapEnd = DateTime.MaxValue;
                }

                if (overlapStart <= overlapEnd)
                {
                    return (overlapStart, overlapEnd);
                }
            }

            return null;
        }
    }
}
