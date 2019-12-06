using AutoMapper;

namespace Xero.Product.Domain
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.ProductData, Data.ProductData>().ReverseMap();
            CreateMap<Domain.ProductOption, Data.ProductOption>().ReverseMap();
        }
    }
}
