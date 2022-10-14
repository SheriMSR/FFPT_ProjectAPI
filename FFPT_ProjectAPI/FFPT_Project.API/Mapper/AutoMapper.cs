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
            #region product
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            #endregion

            #region customer
            CreateMap<Customer, CustomerResponse>().ReverseMap();
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<UpdateCustomerRequest, Customer>();
            #endregion

            #region Menu
            CreateMap<Menu, MenuResponse>().ReverseMap();
            CreateMap<CreateMenuRequest, Menu>();
            CreateMap<UpdateMenuRequest, Menu>();
            #endregion
        }
    }
}