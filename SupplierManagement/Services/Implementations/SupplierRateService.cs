using AutoMapper;
using SupplierManagement.Models.Domain;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Models.Api;
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

        public async Task<IEnumerable<OverlappingRateDto>> GetOverlappingRatesAsync(int? supplierId = null)
        {
            var result = new List<OverlappingRateDto>();

            // Use the high-performance SQL-based overlap detection
            var overlapResults = await _supplierRateRepository.GetOverlappingRatesAsync(supplierId);

            // Group by supplier and convert to DTOs
            var groupedBySupplier = overlapResults.GroupBy(r => r.SupplierId);

            foreach (var supplierGroup in groupedBySupplier)
            {
                var overlappingPairs = new List<OverlappingRatePairDto>();

                foreach (var overlap in supplierGroup)
                {
                    overlappingPairs.Add(new OverlappingRatePairDto
                    {
                        Rate1 = new SupplierRateApiDto
                        {
                            SupplierRateId = overlap.Rate1Id,
                            SupplierId = overlap.SupplierId,
                            Rate = overlap.Rate1Value,
                            RateStartDate = overlap.Rate1Start,
                            RateEndDate = overlap.Rate1End,
                            CreatedOn = DateTime.UtcNow, // Default value
                            CreatedByUser = "system", // Default value
                            SupplierName = overlap.SupplierName
                        },
                        Rate2 = new SupplierRateApiDto
                        {
                            SupplierRateId = overlap.Rate2Id,
                            SupplierId = overlap.SupplierId,
                            Rate = overlap.Rate2Value,
                            RateStartDate = overlap.Rate2Start,
                            RateEndDate = overlap.Rate2End,
                            CreatedOn = DateTime.UtcNow, // Default value
                            CreatedByUser = "system", // Default value
                            SupplierName = overlap.SupplierName
                        },
                        OverlapStartDate = overlap.OverlapStart,
                        OverlapEndDate = overlap.OverlapEnd,
                        OverlapDays = (int)(overlap.OverlapEnd - overlap.OverlapStart).TotalDays + 1
                    });
                }

                if (overlappingPairs.Any())
                {
                    result.Add(new OverlappingRateDto
                    {
                        SupplierId = supplierGroup.Key,
                        SupplierName = supplierGroup.First().SupplierName,
                        OverlappingRates = overlappingPairs
                    });
                }
            }

            return result;
        }

        private List<OverlappingRatePairDto> FindOverlappingRatesForSupplier(List<SupplierRate> rates)
        {
            var overlappingPairs = new List<OverlappingRatePairDto>();

            for (int i = 0; i < rates.Count; i++)
            {
                for (int j = i + 1; j < rates.Count; j++)
                {
                    var rate1 = rates[i];
                    var rate2 = rates[j];

                    // Check for overlap considering nullable end dates
                    var overlapInfo = GetOverlapInfo(rate1, rate2);
                    
                    if (overlapInfo != null)
                    {
                        overlappingPairs.Add(new OverlappingRatePairDto
                        {
                            Rate1 = _mapper.Map<SupplierRateApiDto>(rate1),
                            Rate2 = _mapper.Map<SupplierRateApiDto>(rate2),
                            OverlapStartDate = overlapInfo.Value.StartDate,
                            OverlapEndDate = overlapInfo.Value.EndDate,
                            OverlapDays = (int)(overlapInfo.Value.EndDate - overlapInfo.Value.StartDate).TotalDays + 1
                        });
                    }
                }
            }

            return overlappingPairs;
        }

        private (DateTime StartDate, DateTime EndDate)? GetOverlapInfo(SupplierRate rate1, SupplierRate rate2)
        {
            // Determine effective end dates (if null, consider as ongoing - use far future date)
            var maxDate = new DateTime(2099, 12, 31);
            var rate1EndDate = rate1.RateEndDate ?? maxDate;
            var rate2EndDate = rate2.RateEndDate ?? maxDate;

            // Calculate overlap period
            var overlapStart = rate1.RateStartDate > rate2.RateStartDate ? rate1.RateStartDate : rate2.RateStartDate;
            var overlapEnd = rate1EndDate < rate2EndDate ? rate1EndDate : rate2EndDate;

            // Check if there's an actual overlap
            if (overlapStart <= overlapEnd)
            {
                // If the overlap end is our max date, use today's date for display purposes
                var displayEndDate = overlapEnd == maxDate ? DateTime.Today : overlapEnd;
                
                return (overlapStart, displayEndDate);
            }

            return null;
        }
    }
}
