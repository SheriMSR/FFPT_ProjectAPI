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
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByCategory(int cateId, int timeSlotId, PagingRequest paging);
        Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByMenu(int menuId, PagingRequest paging);
        Task<PagedResults<ProductInMenuResponse>> SearchProductInMenu(string searchString, int timeSlotId, PagingRequest paging);
        Task<List<ProductInMenuResponse>> CreateProductInMenu(CreateProductInMenuRequest request);
        Task<ProductInMenuResponse> UpdateProductInMenu(int productMenuId, UpdateProductInMenuRequest request);
        Task<int> DeleteProductInMenu(int productMenuId);

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
                    .Include(x => x.Product)
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get list product error!!!!", e.Message);
            }
        }

        public async Task<ProductInMenuResponse> GetProductInMenuById(int productMenuId)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Product)
                    .Where(x => x.Id == productMenuId)
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product by id error!!!!", e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByStore(int storeId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Product)
                    .Where(x => x.Product.SupplierStoreId == storeId)
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product by store error!!!", e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByCategory(int cateId, int timeSlotId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Include(x => x.Product)
                    .Where(x => x.Product.CategoryId == cateId && x.Menu.TimeSlotId == timeSlotId)
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product by category error!!!!", e.Message);
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
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product by time slot error!!!!", e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> GetProductInMenuByMenu(int menuId, PagingRequest paging)
        {
            try
            {
                var products = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.MenuId == menuId)
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
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
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product by menu error!!!!", e.Message);
            }
        }

        public async Task<PagedResults<ProductInMenuResponse>> SearchProductInMenu(string searchString, int timeSlotId, PagingRequest paging)
        {
            try
            {
                var productInMenu = _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.Product.Name.Contains(searchString))
                    .Select(x => new ProductInMenuResponse
                    {
                        ProductMenuId = x.Id,
                        StoreId = x.Product.SupplierStoreId,
                        StoreName = x.Product.SupplierStore.Name,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Image = x.Product.Image,
                        Detail = x.Product.Detail,
                        MenuId = x.MenuId,
                        MenuName = x.Menu.MenuName,
                        Price = x.Price,
                        CreateAt = x.CreateAt,
                        UpdateAt = x.UpdateAt
                    })
                    .ToList();

                var result = PageHelper<ProductInMenuResponse>.Paging(productInMenu, paging.Page, paging.PageSize);
                return result;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Search product in menu error!!!!", e.Message);
            }
        }

        public async Task<List<ProductInMenuResponse>> CreateProductInMenu(CreateProductInMenuRequest request)
        {
            try
            {
                var result = new List<ProductInMenuResponse>(); 
                var check = _unitOfWork.Repository<Menu>()
                        .Find(x => x.Id == request.MenuId);

                if(check == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "not found menu!!!!", request.MenuId.ToString());
                }

                foreach (var item in request.Products)
                {
                    var product = _unitOfWork.Repository<Product>().GetById((int)item.ProductId);
                    if (product != null)
                    {
                        var productInMenu = new ProductInMenu();
                        productInMenu.ProductId = (int)item.ProductId;
                        productInMenu.MenuId = request.MenuId;
                        productInMenu.Price = item.Price;
                        productInMenu.CreateAt = DateTime.Now;
                        productInMenu.Active = 1;

                        await _unitOfWork.Repository<ProductInMenu>().InsertAsync(productInMenu);
                        await _unitOfWork.CommitAsync();
                        var rs = _mapper.Map<ProductInMenu, ProductInMenuResponse>(productInMenu);
                        result.Add(rs);
                    }
                }
                return result;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create product error!!!!", e.Message);
            }
        }



        public async Task<int> DeleteProductInMenu(int productMenuId)
        {
            try
            {
                var product = await _unitOfWork.Repository<ProductInMenu>().GetAll()
                    .Where(x => x.Id == productMenuId)
                .FirstOrDefaultAsync();
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Product not found.", productMenuId.ToString());
                }
                else
                {
                    _unitOfWork.Repository<ProductInMenu>().Delete(product);
                    await _unitOfWork.CommitAsync();
                }
                return productMenuId;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete product error!!!!", e.Message);
            }
        }

        public async Task<ProductInMenuResponse> UpdateProductInMenu(int productMenuId, UpdateProductInMenuRequest request)
        {
            try
            {
                ProductInMenu product = null;
                product = _unitOfWork.Repository<ProductInMenu>()
                    .Find(p => p.Id == productMenuId);
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Product not found.", productMenuId.ToString());
                }
                _mapper.Map<UpdateProductInMenuRequest, ProductInMenu>(request, product);
                product.UpdateAt = DateTime.Now;

                await _unitOfWork.Repository<ProductInMenu>().UpdateDetached(product);
                await _unitOfWork.CommitAsync();

                return new ProductInMenuResponse
                {
                    ProductMenuId = product.Id,
                    StoreId = product.Product.SupplierStoreId,
                    StoreName = product.Product.SupplierStore.Name,
                    ProductId = product.ProductId,
                    ProductName = product.Product.Name,
                    Image = product.Product.Image,
                    Detail = product.Product.Detail,
                    MenuId = product.MenuId,
                    MenuName = product.Menu.MenuName,
                    Price = product.Price,
                    CreateAt = product.CreateAt,
                    UpdateAt = product.UpdateAt
                };
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update product error!!!!", e.Message);
            }
        }
    }
}
