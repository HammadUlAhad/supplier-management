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
    }
}
