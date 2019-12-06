using AutoMapper;

namespace Xero.Product.API.Controllers
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Domain.ProductData, Models.ProductData>().ReverseMap();
            CreateMap<Domain.Domain.ProductOption, Models.ProductOption>().ReverseMap();
        }
    }
}
