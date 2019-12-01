using AutoMapper;


namespace Xero.Product.API.UnitTests
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Models.ProductData, API.Models.ProductData>().ReverseMap();
            CreateMap<Domain.Models.ProductOption, Models.ProductOption>().ReverseMap();
        }
    }
}
