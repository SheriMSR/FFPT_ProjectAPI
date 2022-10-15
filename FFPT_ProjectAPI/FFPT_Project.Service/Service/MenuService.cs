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
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FFPT_Project.Data.Entity;
using System.Linq.Dynamic.Core;

namespace FFPT_Project.Service.Service
{
    public interface IMenuService
    {
        Task<PagedResults<MenuResponse>> GetListMenu(MenuResponse request, PagingRequest paging);
        Task<MenuResponse> GetMenuById(int menuId);
        Task<PagedResults<MenuResponse>> GetMenuByTimeSlot(DateTime request, PagingRequest paging);
        Task<MenuResponse> CreateMenu(CreateMenuRequest request);
        Task<MenuResponse> UpdateMenu(int productId, UpdateMenuRequest request);
        Task<PagedResults<TimeslotResponse>> GetListTimeslot(TimeslotResponse request, PagingRequest paging);
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
                var menu = await _unitOfWork.Repository<Menu>().GetAll()
                                               .ProjectTo<MenuResponse>(_mapper.ConfigurationProvider)
                                               .DynamicFilter(request)
                                               .ToListAsync();
                var result = PageHelper<MenuResponse>.Paging(menu, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Menu Error!!!!", ex?.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
        public Task<PagedResults<MenuResponse>> GetMenuByTimeSlot(DateTime request, PagingRequest paging)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuResponse> CreateMenu(CreateMenuRequest request)
        {
            try
            {
                var menu = _mapper.Map<CreateMenuRequest, Menu>(request);

                menu.Id = _unitOfWork.Repository<Menu>().GetAll().Count() + 1;
                menu.CreateAt = DateTime.Now;

                await _unitOfWork.Repository<Menu>().InsertAsync(menu);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Menu, MenuResponse>(menu);
            }
            catch (CrudException e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create menu error!!!", e?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new CrudException(HttpStatusCode.BadRequest, "Update menu error!!!!", e?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedResults<TimeslotResponse>> GetListTimeslot(TimeslotResponse request, PagingRequest paging)
        {
            try
            {
                TimeSlot[] list =  _unitOfWork.Repository<TimeSlot>().GetAll().ToArray();
                List<TimeslotResponse> listResult = _mapper.Map<TimeSlot[], TimeslotResponse[]>(list).ToList();
                var result = PageHelper<TimeslotResponse>.Paging(listResult, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
