using AutoMapper;
using SupplierManagement.Models.Domain;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Models.Api;

namespace SupplierManagement.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Domain to ViewModel mappings
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();
            CreateMap<SupplierRate, SupplierRateViewModel>().ReverseMap();

            // Domain to API DTO mappings
            CreateMap<Supplier, SupplierApiDto>()
                .ForMember(dest => dest.Rates, opt => opt.MapFrom(src => src.SupplierRates));
            
            CreateMap<SupplierRate, SupplierRateApiDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty));

            // ViewModel to API DTO mappings (if needed)
            CreateMap<SupplierViewModel, SupplierApiDto>()
                .ForMember(dest => dest.Rates, opt => opt.MapFrom(src => src.SupplierRates));
            
            CreateMap<SupplierRateViewModel, SupplierRateApiDto>();
        }
    }
}
