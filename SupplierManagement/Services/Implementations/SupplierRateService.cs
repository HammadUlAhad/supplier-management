using AutoMapper;
using SupplierManagement.Models.Domain;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Repositories.Interfaces;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Services.Implementations
{
    public class SupplierRateService : ISupplierRateService
    {
        private readonly ISupplierRateRepository _supplierRateRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierRateService(
            ISupplierRateRepository supplierRateRepository,
            ISupplierRepository supplierRepository,
            IMapper mapper)
        {
            _supplierRateRepository = supplierRateRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierRateViewModel>> GetAllSupplierRatesAsync()
        {
            var supplierRates = await _supplierRateRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierRateViewModel>>(supplierRates);
        }

        public async Task<IEnumerable<SupplierRateViewModel>> GetSupplierRatesBySupplierIdAsync(int supplierId)
        {
            var supplierRates = await _supplierRateRepository.GetBySupplierId(supplierId);
            return _mapper.Map<IEnumerable<SupplierRateViewModel>>(supplierRates);
        }

        public async Task<SupplierRateViewModel?> GetSupplierRateByIdAsync(int id)
        {
            var supplierRate = await _supplierRateRepository.GetByIdAsync(id);
            return supplierRate == null ? null : _mapper.Map<SupplierRateViewModel>(supplierRate);
        }

        public async Task<SupplierRateViewModel> CreateSupplierRateAsync(SupplierRateViewModel supplierRateViewModel)
        {
            var supplierRate = _mapper.Map<SupplierRate>(supplierRateViewModel);
            supplierRate.CreatedOn = DateTime.UtcNow;
            
            var createdSupplierRate = await _supplierRateRepository.AddAsync(supplierRate);
            return _mapper.Map<SupplierRateViewModel>(createdSupplierRate);
        }

        public async Task<SupplierRateViewModel?> UpdateSupplierRateAsync(SupplierRateViewModel supplierRateViewModel)
        {
            var supplierRate = _mapper.Map<SupplierRate>(supplierRateViewModel);
            var updatedSupplierRate = await _supplierRateRepository.UpdateAsync(supplierRate);
            
            return updatedSupplierRate == null ? null : _mapper.Map<SupplierRateViewModel>(updatedSupplierRate);
        }

        public async Task<bool> DeleteSupplierRateAsync(int id)
        {
            return await _supplierRateRepository.DeleteAsync(id);
        }

        public async Task<bool> SupplierRateExistsAsync(int id)
        {
            return await _supplierRateRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<SupplierViewModel>> GetSuppliersForDropdownAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
        }
    }
}
