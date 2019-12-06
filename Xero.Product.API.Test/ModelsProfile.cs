using AutoMapper;


namespace Xero.Product.API.UnitTests
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Domain.ProductData, API.Models.ProductData>().ReverseMap();
            CreateMap<Domain.Domain.ProductOption, Models.ProductOption>().ReverseMap();
        }
    }
}
