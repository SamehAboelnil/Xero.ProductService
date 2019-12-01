using AutoMapper;


namespace Xero.Product.API.UnitTests
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
