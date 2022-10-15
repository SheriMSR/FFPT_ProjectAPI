using AutoMapper;
using AutoMapper.QueryableExtensions;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FFPT_Project.Service.Service
{
    public interface ICategoryService
    {
        Task<PagedResults<CategoryResponse>> GetListCategory(PagingRequest paging);
    }
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResults<CategoryResponse>> GetListCategory(PagingRequest paging)
        {
            try
            {
                var list = _unitOfWork.Repository<Category>().GetAll()
                                .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                                .ToList();
                var result = PageHelper<CategoryResponse>.Paging(list, paging.Page, paging.PageSize);

                return result;
            }
            catch(CrudException e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get category list error!!!!!", e.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
