using AutoMapper;
using FFPT_Project.Data.Entity;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;

namespace FFPT_Project.API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<CreateProductRequest, Product>();
        }
    }
}