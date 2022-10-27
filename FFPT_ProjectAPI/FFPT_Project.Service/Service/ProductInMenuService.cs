using AutoMapper;
using AutoMapper.QueryableExtensions;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
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
    public interface IProductInMenuService
    {
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenu(ProductInMenuResponse request, PagingRequest paging);
        Task<ProductInMenuResponse> GetProductInMenuById(int productMenuId);
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByTimeSlot(int timeSlotId, PagingRequest paging);
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByStore(int storeId, PagingRequest paging);
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByCategory(int cateId, PagingRequest paging);
        Task<PagedResults<ProductInMenuResponse>> SearchProductInMenu(string searchString, int timeSlotId, PagingRequest paging);
        Task<StatusCodeResult> CreateProductInMenu(CreateProductInMenuRequest request);
        Task<StatusCodeResult> UpdateProductInMenu(int productMenuId, UpdateProductInMenuRequest request);
        Task<StatusCodeResult> DeleteProductInMenu(int productMenuId);

    }
    public class ProductInMenuService : IProductInMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ProductInMenuService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenu(ProductInMenuResponse request, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Store)
                    .Include(x => x.Product)
                    .Select(x => new ProductInMenuResponse
                    {
                        StoreName = x.Store.Name,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                    .ToListAsync();

                var result = PageHelper<ProductInMenuResponse>.Paging(products, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get list product in menu error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ProductInMenuResponse> GetProductInMenuById(int productMenuId)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Store)
                    .Include(x => x.Product)
                    .Where(x => x.Id == productMenuId)
                    .Select(x => new ProductInMenuResponse
                    {
                        StoreName = x.Store.Name,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                .FirstOrDefaultAsync();
                return products;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product in menu by id error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByStore(int storeId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Store)
                    .Include(x => x.Product)
                    .Where(x => x.StoreId == storeId)
                    .Select(x => new ProductInMenuResponse
                    {
                        StoreName = x.Store.Name,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                .ToListAsync();

                var result = PageHelper<ProductInMenuResponse>.Paging(products, paging.Page, paging.PageSize);

                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product in menu by store error!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByCategory(int cateId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Store)
                    .Include(x => x.Product)
                    .Where(x => x.Product.CategoryId == cateId)
                    .Select(x => new ProductInMenuResponse
                    {
                        StoreName = x.Store.Name,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                .ToListAsync();

                var result = PageHelper<ProductInMenuResponse>.Paging(products, paging.Page, paging.PageSize);

                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product in menu by category error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByTimeSlot(int timeSlotId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.Menu.TimeSlotId == timeSlotId)
                    .Select(x => new ProductInMenuResponse
                    {
                        StoreName = x.Store.Name,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                .ToListAsync();

                var result = PageHelper<ProductInMenuResponse>.Paging(products, paging.Page, paging.PageSize);

                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product in menu by category error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> SearchProductInMenu(string searchString, int timeSlotId, PagingRequest paging)
        {
            try
            {
                var productInMenu = _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.Product.Name.Contains(searchString))
                    .ProjectTo<ProductInMenuResponse>(_mapper.ConfigurationProvider)
                    .ToList();

                var result = PageHelper<ProductInMenuResponse>.Paging(productInMenu, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Search product in menu error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<StatusCodeResult> CreateProductInMenu(CreateProductInMenuRequest request)
        {
            try
            {
                var productInMenu = _mapper.Map<CreateProductInMenuRequest, ProductInMenu>(request);
                productInMenu.Id = _unitOfWork.Repository<ProductInMenu>().GetAll().Count() + 1;
                productInMenu.CreateAt = DateTime.Now;

                await _unitOfWork.Repository<ProductInMenu>().InsertAsync(productInMenu);
                await _unitOfWork.CommitAsync();

                _mapper.Map<ProductInMenu, ProductInMenuResponse>(productInMenu);

                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create product for this menu error!!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<StatusCodeResult> DeleteProductInMenu(int productMenuId)
        {
            try
            {
                StatusCodeResult result;
                var product = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.Id == productMenuId)
                .FirstOrDefaultAsync();
                if (product == null)
                {
                    return result = new StatusCodeResult((int)HttpStatusCode.NotFound);
                }
                else
                {
                    _unitOfWork.Repository<ProductInMenu>().Delete(product);
                    await _unitOfWork.CommitAsync();
                    return result = new StatusCodeResult((int)HttpStatusCode.OK);
                }
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete product in this menu error!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<StatusCodeResult> UpdateProductInMenu(int productMenuId, UpdateProductInMenuRequest request)
        {
            try
            {
                ProductInMenu product = null;
                product = _unitOfWork.Repository<ProductInMenu>()
                    .Find(p => p.Id == productMenuId);
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", productMenuId.ToString());
                }
                _mapper.Map<UpdateProductInMenuRequest, ProductInMenu>(request, product);
                product.UpdateAt = DateTime.Now;

                await _unitOfWork.Repository<ProductInMenu>().UpdateDetached(product);
                await _unitOfWork.CommitAsync();
                _mapper.Map<ProductInMenu, ProductInMenuResponse>(product);

                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update product in menu error!!!!", ex?.Message);
            }
        }
    }
}
