using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using FFPT_Project.Service.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FFPT_Project.Data.Entity;
using System.Linq.Dynamic.Core;
using Azure.Core;

namespace FFPT_Project.Service.Service
{
    public interface IMenuService
    {
        Task<PagedResults<MenuResponse>> GetListMenu(MenuResponse request, PagingRequest paging);
        Task<MenuResponse> GetMenuById(int menuId);
        Task<PagedResults<MenuResponse>> GetMenuByTimeSlot(int timeSlotId, PagingRequest paging);
        Task<MenuResponse> CreateMenu(CreateMenuRequest request);
        Task<MenuResponse> UpdateMenu(int productId, UpdateMenuRequest request);
        Task<int> DeleteMenu(int menuId);
    }
    public class MenuService : IMenuService
    {
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MenuService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResults<MenuResponse>> GetListMenu(MenuResponse request, PagingRequest paging)
        {
            try
            {
                var menu =await _unitOfWork.Repository<Menu>().GetAll()
                                               .ProjectTo<MenuResponse>(_mapper.ConfigurationProvider)
                                               .DynamicFilter(request)
                                               .ToListAsync();
                var result = PageHelper<MenuResponse>.Paging(menu, paging.Page, paging.PageSize);
                return result;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Menu Error!!!!", e?.Message);
            }
        }

        public async Task<MenuResponse> GetMenuById(int menuId)
        {
            try
            {
                Menu menu = null;
                menu = await _unitOfWork.Repository<Menu>().GetAll()
                    .Where(x => x.Id == menuId)
                    .FirstOrDefaultAsync();

                if (menu == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found menu with id", menuId.ToString());
                }

                return _mapper.Map<Menu, MenuResponse>(menu);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get menu error!!!!", e?.Message);
            }
        }
        public async Task<PagedResults<MenuResponse>> GetMenuByTimeSlot(int timeSlotId, PagingRequest paging)
        {
            try
            {
                var menu = await _unitOfWork.Repository<Menu>().GetAll()
                                            .Where(x => x.TimeSlot.Id == timeSlotId)
                                            .ProjectTo<MenuResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
                var result = PageHelper<MenuResponse>.Paging(menu, paging.Page, paging.PageSize);
                return result;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Menu Error!!!!", e?.Message);
            }
        }

        public async Task<MenuResponse> CreateMenu(CreateMenuRequest request)
        {
            try
            {
                var menu = _mapper.Map<CreateMenuRequest, Menu>(request);

                menu.CreateAt = DateTime.Now;

                await _unitOfWork.Repository<Menu>().InsertAsync(menu);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Menu, MenuResponse>(menu);
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create menu error!!!", ex?.Message);
            }
        }

        public async Task<MenuResponse> UpdateMenu(int menuId, UpdateMenuRequest request)
        {
            try
            {
                Menu menu = _unitOfWork.Repository<Menu>()
                            .Find(x => x.Id == menuId);
                if (menu == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found menu with id", menuId.ToString());
                }
                menu = _mapper.Map<UpdateMenuRequest, Menu>(request, menu);

                menu.UpdateAt = DateTime.Now;

                await _unitOfWork.Repository<Menu>().UpdateDetached(menu);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Menu, MenuResponse>(menu);
            }
            catch (CrudException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update menu error!!!!", ex?.Message);
            }
        }

        public async Task<int> DeleteMenu(int menuId)
        {
            try
            {
                var menu = await _unitOfWork.Repository<Menu>().GetAll()
                    .Where(x => x.Id == menuId)
                .FirstOrDefaultAsync();
                if (menu == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Menu not found.", menuId.ToString());
                }
                else
                {
                    _unitOfWork.Repository<Menu>().Delete(menu);
                    await _unitOfWork.CommitAsync();
                }
                return menuId;
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
    }
}
