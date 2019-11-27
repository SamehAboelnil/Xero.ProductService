using AutoMapper;

namespace Xero.Product.API.Controllers
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Models.Product, Models.Product>().ReverseMap();
            CreateMap<Domain.Models.ProductOption, Models.ProductOptions>().ReverseMap();
        }
    }
}
