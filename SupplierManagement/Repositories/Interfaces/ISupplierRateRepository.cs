using SupplierManagement.Models.Domain;
using SupplierManagement.Repositories.Implementations;

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
        
        // API methods for Exercise 2
        Task<IEnumerable<SupplierRate>> GetAllWithSupplierInfoAsync();
        Task<IEnumerable<SupplierRate>> GetBySupplierIdWithInfoAsync(int supplierId);
        
        // High-performance overlap detection
        Task<IEnumerable<OverlapResult>> GetOverlappingRatesAsync(int? supplierId = null);
    }
}
