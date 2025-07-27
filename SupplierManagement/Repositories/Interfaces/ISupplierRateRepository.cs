using SupplierManagement.Models.Domain;

namespace SupplierManagement.Repositories.Interfaces
{
    public interface ISupplierRateRepository
    {
        Task<IEnumerable<SupplierRate>> GetAllAsync();
        Task<IEnumerable<SupplierRate>> GetBySupplierId(int supplierId);
        Task<SupplierRate?> GetByIdAsync(int id);
        Task<SupplierRate> AddAsync(SupplierRate supplierRate);
        Task<SupplierRate?> UpdateAsync(SupplierRate supplierRate);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
