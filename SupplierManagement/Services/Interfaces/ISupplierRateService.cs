using SupplierManagement.Models.ViewModels;
using SupplierManagement.Models.Api;

namespace SupplierManagement.Services.Interfaces
{
    public interface ISupplierRateService
    {
        Task<IEnumerable<SupplierRateViewModel>> GetAllSupplierRatesAsync();
        Task<IEnumerable<SupplierRateViewModel>> GetSupplierRatesBySupplierIdAsync(int supplierId);
        Task<SupplierRateViewModel?> GetSupplierRateByIdAsync(int id);
        Task<SupplierRateViewModel> CreateSupplierRateAsync(SupplierRateViewModel supplierRateViewModel);
        Task<SupplierRateViewModel?> UpdateSupplierRateAsync(SupplierRateViewModel supplierRateViewModel);
        Task<bool> DeleteSupplierRateAsync(int id);
        Task<bool> SupplierRateExistsAsync(int id);
        Task<IEnumerable<SupplierViewModel>> GetSuppliersForDropdownAsync();
        
        // API methods for Exercise 2
        Task<IEnumerable<OverlappingRateDto>> GetOverlappingRatesAsync(int? supplierId = null);
    }
}
