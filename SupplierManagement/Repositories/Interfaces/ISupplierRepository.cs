using SupplierManagement.Models.Domain;

namespace SupplierManagement.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<IEnumerable<Supplier>> GetAllWithRatesAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> GetByIdWithRatesAsync(int id);
        Task<Supplier> AddAsync(Supplier supplier);
        Task<Supplier?> UpdateAsync(Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
