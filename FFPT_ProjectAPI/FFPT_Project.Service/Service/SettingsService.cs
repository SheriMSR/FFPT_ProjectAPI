using AutoMapper;
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
    public interface ISettingsService
    {
        Task<PagedResults<TimeslotResponse>> GetListTimeslot(TimeslotResponse request, PagingRequest paging);
        Task<List<RoomResponse>> GetRoomList(RoomResponse request);
    }
    public class SettingsService : ISettingsService
    {
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SettingsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResults<TimeslotResponse>> GetListTimeslot(TimeslotResponse request, PagingRequest paging)
        {
            try
            {
                TimeSlot[] list = _unitOfWork.Repository<TimeSlot>().GetAll().ToArray();
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
        public async Task<List<RoomResponse>> GetRoomList(RoomResponse request)
        {
            try
            {
                var result = _unitOfWork.Repository<Room>().GetAll()
                    .Select(x => new RoomResponse
                    {
                        Id = x.Id,
                        RoomNumber = x.RoomNumber,
                        FloorId = x.FloorId,
                        FloorNumber = x.Floor.FloorNumber,
                        AreaId = x.AreaId,
                        AreaName = x.Area.Name
                    })
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get room error", e.Message);
            }
        }
    }
}
