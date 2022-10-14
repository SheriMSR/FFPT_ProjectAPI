using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using FFPT_Project.Data.Context;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using FFPT_Project.Service.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.Service
{
    public interface IProductServices
    {
        Task<PagedResults<ProductResponse>> GetProducts(ProductResponse request, PagingRequest paging);
        Task<ProductResponse> GetProductById(int productId);
        Task<PagedResults<ProductResponse>> GetProductByStore(int storeId, PagingRequest paging);
        Task<PagedResults<ProductResponse>> GetProductByTimeSlot(DateTime request, PagingRequest paging);
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

        public async Task<PagedResults<ProductResponse>> GetProducts(ProductResponse request, PagingRequest paging)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetAll()
                                               .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                                               .DynamicFilter(request)
                                               .ToListAsync();
                var result = PageHelper<ProductResponse>.Paging(product, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Product Error!!!!", ex?.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<ProductResponse> GetProductById(int productId)
        {
            try
            {
                Product product = null;
                product = await _unitOfWork.Repository<Product>().GetAll()
                    .Where(x => x.Id == productId)
                    .FirstOrDefaultAsync();

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
        public async Task<PagedResults<ProductResponse>> GetProductByStore(int storeId, PagingRequest paging)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetAll()
                    .Where(p => p.SupplierStore.Id == storeId)
                    .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with storeID", storeId.ToString());
                }
                var result = PageHelper<ProductResponse>.Paging(product, paging.Page, paging.PageSize);
                return result;
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

                product.Id = _unitOfWork.Repository<Product>().GetAll().Count() + 1;
                product.Status = (int)ProductStatusEnum.New;
                product.CreateAt = DateTime.Now;

                await _unitOfWork.Repository<Product>().InsertAsync(product);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Product Error!!!", ex?.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                product.Status = (int)ProductStatusEnum.New;
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
        public async Task<PagedResults<ProductResponse>> GetProductByTimeSlot(DateTime request, PagingRequest paging)
        {
            try
            {

                var timeSlot = await _unitOfWork.Repository<TimeSlot>().GetAll()
                    .Where(x => x.ArriveTime < request.TimeOfDay && x.CheckoutTime > request.TimeOfDay)
                    .FirstOrDefaultAsync();
                var productInMenu = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                                    .Where(x => x.Menu.TimeSlotId == timeSlot.Id)
                                    .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                                    .ToListAsync();

                var result = PageHelper<ProductResponse>.Paging(productInMenu, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Product By Time Slot Error!!!!", ex?.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
