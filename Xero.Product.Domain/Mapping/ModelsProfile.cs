using AutoMapper;

namespace Xero.Product.Domain
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Models.Product, Data.Product>().ReverseMap();
            CreateMap<Models.ProductOption, Data.ProductOption>().ReverseMap();
        }
    }
}
