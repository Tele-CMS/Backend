using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payroll;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payroll
{
    public class AgencyHolidaysService : BaseService, IAgencyHolidaysService
    {
        private readonly IAgencyHolidaysRepository _agencyHolidaysRepository;
        private JsonModel response;
        public AgencyHolidaysService(IAgencyHolidaysRepository agencyHolidaysRepository)
        {
            _agencyHolidaysRepository = agencyHolidaysRepository;
        }
        public JsonModel DeleteAgencyHolidays(int Id, TokenModel token)
        {
            Holidays holidaysEntity = _agencyHolidaysRepository.Get(x=> x.Id == Id && x.IsActive == true && x.IsDeleted == false );
            if (holidaysEntity != null)
            {
                holidaysEntity.IsDeleted = true;
                holidaysEntity.DeletedBy = token.UserID;
                holidaysEntity.DeletedDate = DateTime.UtcNow;
                _agencyHolidaysRepository.Update(holidaysEntity);
                _agencyHolidaysRepository.SaveChanges();
                response = new JsonModel(null, StatusMessage.HolidaysDeleted, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
                response = new JsonModel(null, StatusMessage.HolidayDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
            return response;
        }

        public JsonModel GetAgencyHolidaysById(int Id, TokenModel token)
        {
            Holidays holidaysEntity = _agencyHolidaysRepository.GetByID(Id);
            HolidaysModel holidaysModel = new HolidaysModel();
            AutoMapper.Mapper.Map(holidaysEntity, holidaysModel);
            response = new JsonModel(holidaysModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            return response;
        }

        public JsonModel GetAgencyHolidaysList(int pageNumber, int pageSize, TokenModel token)
        {
            var query = _agencyHolidaysRepository.GetAll(x => x.IsDeleted == false && x.IsActive == true);
            decimal totalCount = query.ToList().Count;
            List<HolidaysModel> agencyHolidaysList = query.Select(x => new HolidaysModel()
            {
                Id = x.Id,
                Date = x.Date,
                Description = x.Description
            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            response= new JsonModel(agencyHolidaysList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            response.meta = new Meta()
            {
                TotalRecords = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                DefaultPageSize = pageSize,
                TotalPages = Math.Ceiling(totalCount / pageSize)
            };
            return response;
        }

        public JsonModel SaveAgencyHolidays(HolidaysModel holidaysModel, TokenModel token)
        {
            Holidays holidaysEntity = null;
            if (holidaysModel.Id == 0)
            {
                holidaysEntity = new Holidays();
                AutoMapper.Mapper.Map(holidaysModel, holidaysEntity);
                holidaysEntity.CreatedBy = token.UserID;
                holidaysEntity.CreatedDate = DateTime.UtcNow;
                holidaysEntity.IsActive = true;
                holidaysEntity.IsDeleted = false;
                _agencyHolidaysRepository.Create(holidaysEntity);
                response = new JsonModel(null, StatusMessage.HolidaysAdded, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
            {
                holidaysEntity = _agencyHolidaysRepository.Get(x => x.Id == holidaysModel.Id && x.IsActive == true && x.IsDeleted == false);
                if (holidaysEntity != null)
                {
                    holidaysEntity.Date = holidaysModel.Date;
                    holidaysEntity.Description = holidaysModel.Description;
                    holidaysEntity.UpdatedBy = token.UserID;
                    holidaysEntity.UpdatedDate = DateTime.UtcNow;
                    _agencyHolidaysRepository.Update(holidaysEntity);
                    response = new JsonModel(null, StatusMessage.HolidaysUpdated, (int)HttpStatusCodes.OK, string.Empty);
                }
                else
                    response = new JsonModel(null, StatusMessage.HolidayDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
            }
            _agencyHolidaysRepository.SaveChanges();
            return response;
        }
    }
}
