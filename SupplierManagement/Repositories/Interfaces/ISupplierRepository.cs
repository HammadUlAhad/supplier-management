using SupplierManagement.Models.Domain;

namespace SupplierManagement.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> AddAsync(Supplier supplier);
        Task<Supplier?> UpdateAsync(Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // API methods for Exercise 2
        Task<IEnumerable<Supplier>> GetAllWithRatesAsync();
    }
}
