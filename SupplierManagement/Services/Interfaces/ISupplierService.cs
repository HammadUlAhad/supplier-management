using SupplierManagement.Models.ViewModels;

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
    }
}
