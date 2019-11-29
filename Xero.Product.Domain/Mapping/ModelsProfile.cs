using AutoMapper;

namespace Xero.Product.Domain
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Models.ProductData, Data.ProductData>().ReverseMap();
            CreateMap<Models.ProductOption, Data.ProductOption>().ReverseMap();
        }
    }
}
