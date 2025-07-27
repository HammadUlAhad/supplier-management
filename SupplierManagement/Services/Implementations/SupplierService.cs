using AutoMapper;
using SupplierManagement.Models.Domain;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Models.Api;
using SupplierManagement.Repositories.Interfaces;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierViewModel>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
        }

        public async Task<SupplierViewModel?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier == null ? null : _mapper.Map<SupplierViewModel>(supplier);
        }

        public async Task<SupplierViewModel> CreateSupplierAsync(SupplierViewModel supplierViewModel)
        {
            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            supplier.CreatedOn = DateTime.UtcNow;
            
            var createdSupplier = await _supplierRepository.AddAsync(supplier);
            return _mapper.Map<SupplierViewModel>(createdSupplier);
        }

        public async Task<SupplierViewModel?> UpdateSupplierAsync(SupplierViewModel supplierViewModel)
        {
            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            var updatedSupplier = await _supplierRepository.UpdateAsync(supplier);
            
            return updatedSupplier == null ? null : _mapper.Map<SupplierViewModel>(updatedSupplier);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _supplierRepository.DeleteAsync(id);
        }

        public async Task<bool> SupplierExistsAsync(int id)
        {
            return await _supplierRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<SupplierApiDto>> GetAllSuppliersWithRatesAsync()
        {
            var suppliers = await _supplierRepository.GetAllWithRatesAsync();
            return _mapper.Map<IEnumerable<SupplierApiDto>>(suppliers);
        }
    }
}
