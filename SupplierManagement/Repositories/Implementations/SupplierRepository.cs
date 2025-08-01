using Microsoft.EntityFrameworkCore;
using SupplierManagement.Database;
using SupplierManagement.Models.Domain;
using SupplierManagement.Repositories.Interfaces;

namespace SupplierManagement.Repositories.Implementations
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierManagementDbContext _context;

        public SupplierRepository(SupplierManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetAllWithRatesAsync()
        {
            return await _context.Suppliers
                .Include(s => s.SupplierRates.OrderBy(sr => sr.RateStartDate))
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SupplierId == id);
        }

        public async Task<Supplier?> GetByIdWithRatesAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.SupplierRates.OrderBy(sr => sr.RateStartDate))
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SupplierId == id);
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier?> UpdateAsync(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(supplier.SupplierId);
            if (existingSupplier == null)
            {
                return null;
            }

            _context.Entry(existingSupplier).CurrentValues.SetValues(supplier);
            await _context.SaveChangesAsync();
            return existingSupplier;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Suppliers.AnyAsync(s => s.SupplierId == id);
        }
    }
}
