using SupplierManagement.Models.ViewModels;
using SupplierManagement.Models.Api;

namespace SupplierManagement.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierViewModel>> GetAllSuppliersAsync();
        Task<SupplierViewModel?> GetSupplierByIdAsync(int id);
        Task<SupplierViewModel> CreateSupplierAsync(SupplierViewModel supplierViewModel);
        Task<SupplierViewModel?> UpdateSupplierAsync(SupplierViewModel supplierViewModel);
        Task<bool> DeleteSupplierAsync(int id);
        Task<bool> SupplierExistsAsync(int id);
        
        // API methods for Exercise 2
        Task<IEnumerable<SupplierApiDto>> GetAllSuppliersWithRatesAsync();
    }
}
