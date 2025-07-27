using AutoMapper;
using SupplierManagement.Models.Domain;
using SupplierManagement.Models.ViewModels;

namespace SupplierManagement.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Supplier mappings
            CreateMap<Supplier, SupplierViewModel>()
                .ForMember(dest => dest.SupplierRates, opt => opt.MapFrom(src => src.SupplierRates));
            
            CreateMap<SupplierViewModel, Supplier>()
                .ForMember(dest => dest.SupplierRates, opt => opt.Ignore());

            // SupplierRate mappings
            CreateMap<SupplierRate, SupplierRateViewModel>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));
            
            CreateMap<SupplierRateViewModel, SupplierRate>()
                .ForMember(dest => dest.Supplier, opt => opt.Ignore());
        }
    }
}
