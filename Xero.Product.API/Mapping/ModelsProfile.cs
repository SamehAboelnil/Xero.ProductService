using AutoMapper;

namespace Xero.Product.API.Controllers
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Models.ProductData, Models.ProductData>().ReverseMap();
            CreateMap<Domain.Models.ProductOption, Models.ProductOptions>().ReverseMap();
        }
    }
}
