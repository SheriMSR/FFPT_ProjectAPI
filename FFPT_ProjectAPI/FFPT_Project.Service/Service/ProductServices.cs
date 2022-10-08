using AutoMapper;
using AutoMapper.QueryableExtensions;
using FFPT_Project.Data.Context;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Reso.Sdk.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.Service
{
    public interface IProductServices
    {
        Task<PagedResults<ProductResponse>> GetProducts(ProductRequest request);
        Task<ProductResponse> GetProductById(int productId);
        Task<ProductResponse> GetProductByStore(int storeId);
        //Task<ProductResponse> GetProductByTimeSlot(TimeOnly request);
        Task<ProductResponse> CreateProduct(CreateProductRequest request);
        Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest request);
        //Task<int> DeleteProduct (int productId);
    }
    public class ProductServices : IProductServices
    {
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductServices(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResults<ProductResponse>> GetProducts(ProductRequest request)
        {
            try
            {
                var product = _unitOfWork.Repository<Product>().GetAll()
                    .Where(x => request.Status == null || x.Status == (int)request.Status)
                    .OrderByDescending(x => x.Id)
                    .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                    .PagingIQueryable(request.Page, request.PageSize, 500, 50);

                return new PagedResults<ProductResponse>()
                {
                    PageNumber = request.Page,
                    PageSize = request.PageSize,
                    TotalNumberOfPages = (int)Math.Ceiling((double)product.Item1 / request.PageSize),
                    TotalNumberOfRecords = product.Item1,
                    Results = product.Item2.ToList()
                };

            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Product Error!!!!", ex?.Message);
            }
        }

        public async Task<ProductResponse> GetProductById(int productId)
        {
            try
            {
                Product product = null;
                product = _unitOfWork.Repository<Product>()
                    .Find(p => p.Id == productId);
                if(product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", productId.ToString());
                }

                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch(CrudException ex)
            {
                throw ex;
            }
            catch(Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product error!!!!", e?.Message);
            }
        }
        public async Task<ProductResponse> GetProductByStore(int storeId)
        {
            try
            {
                Product product = null;
                product = _unitOfWork.Repository<Product>()
                    .Find(p => p.SupplierStore.Id == storeId);
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with storeID", storeId.ToString());
                }

                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product error!!!!", e?.Message);
            }
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest request)
        {
            try
            {
                var product = _mapper.Map<CreateProductRequest, Product>(request);

                product.Status = (int)ProductStatusEmun.New;
                product.CreateAt = DateTime.Now;

                await _unitOfWork.Repository<Product>().InsertAsync(product);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Product Error!!!", ex?.Message);
            }
        }

        public async Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest request)
        {
            try
            {
                Product product = null;
                product = _unitOfWork.Repository<Product>()
                    .Find(p => p.Id == productId);
                if(product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", productId.ToString());
                }
                _mapper.Map<UpdateProductRequest, Product>(request, product);
                product.Status = (int)ProductStatusEmun.New;
                product.UpdatedAt = DateTime.Now;

                await _unitOfWork.Repository<Product>().UpdateDetached(product);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch(Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update product error!!!!", ex?.Message);
            }
        }

        //public Task<ProductResponse> GetProductByTimeSlot(TimeOnly request)
        //{
        //    try
        //    {
        //        var productInMenu = _unitOfWork.Repository<ProductInMenu>().GetAll()
        //            .Where(x => x.Menu
        //    }
        //    catch
        //    {

        //    }
        //}
    }
}
