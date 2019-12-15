using AutoMapper;
using System.Linq;

namespace Xero.Product.API.Controllers
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            CreateMap<Domain.Domain.ProductOption, Contracts.ProductOption>().ReverseMap();
            CreateMap<Domain.Domain.ProductData, Contracts.ProductData>().ReverseMap();
        }
    }
}
