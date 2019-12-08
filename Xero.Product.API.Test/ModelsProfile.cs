using AutoMapper;


namespace Xero.Product.API.UnitTests
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Domain.ProductData, API.Contracts.ProductData>().ReverseMap();
            CreateMap<Domain.Domain.ProductOption, Contracts.ProductOption>().ReverseMap();
        }
    }
}
