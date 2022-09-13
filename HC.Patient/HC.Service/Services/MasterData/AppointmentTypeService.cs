using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterData
{
    public class AppointmentTypeService : BaseService, IAppointmentTypeService
    {
        private readonly IAppointmentTypeRepository _appointmentTypeRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public AppointmentTypeService(IAppointmentTypeRepository appointmentTypeRepository, IUserCommonRepository userCommonRepository)
        {
            _appointmentTypeRepository = appointmentTypeRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetAppointmentType(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<AppointmentTypesModel> appointmentTypeModels = _appointmentTypeRepository.GetAppointmentType<AppointmentTypesModel>(searchFilterModel, tokenModel).ToList();
            if (appointmentTypeModels != null && appointmentTypeModels.Count > 0)
            {
                response = new JsonModel(appointmentTypeModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(appointmentTypeModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveAppointmentType(AppointmentTypesModel appointmentTypesModel, TokenModel tokenModel)
        {
            AppointmentType appointmentType = null;
            if (appointmentTypesModel.Id == 0)
            {
                appointmentType = _appointmentTypeRepository.Get(l => l.Name == appointmentTypesModel.Name && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (appointmentType != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.AppointmentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //insert new
                {
                    appointmentType = new AppointmentType();
                    AutoMapper.Mapper.Map(appointmentTypesModel, appointmentType);
                    appointmentType.OrganizationID = tokenModel.OrganizationID;
                    appointmentType.CreatedBy = tokenModel.UserID;
                    appointmentType.CreatedDate = DateTime.UtcNow;
                    appointmentType.IsDeleted = false;
                    appointmentType.IsActive = true;
                    _appointmentTypeRepository.Create(appointmentType);
                    _appointmentTypeRepository.SaveChanges();
                    response = new JsonModel(appointmentType, StatusMessage.AppointmentTypeCreated, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                appointmentType = _appointmentTypeRepository.Get(l => l.Name == appointmentTypesModel.Name && l.Id != appointmentTypesModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (appointmentType != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.AppointmentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //update existing
                {
                    appointmentType = _appointmentTypeRepository.Get(a => a.Id == appointmentTypesModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (appointmentType != null)
                    {
                        AutoMapper.Mapper.Map(appointmentTypesModel, appointmentType);
                        appointmentType.UpdatedBy = tokenModel.UserID;
                        appointmentType.UpdatedDate = DateTime.UtcNow;
                        _appointmentTypeRepository.Update(appointmentType);
                        _appointmentTypeRepository.SaveChanges();
                        response = new JsonModel(appointmentType, StatusMessage.AppointmentTypeUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }

        public JsonModel GetAppointmentTypeById(int id, TokenModel tokenModel)
        {
            AppointmentType appointmentType = _appointmentTypeRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (appointmentType != null)
            {
                AppointmentTypesModel appointmentTypesModel = new AppointmentTypesModel();
                AutoMapper.Mapper.Map(appointmentType, appointmentTypesModel);
                response = new JsonModel(appointmentTypesModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteAppointmentType(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.AppointmentType, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0 && recordDependenciesModel.TableName.ToLower() != DatabaseTables.PayerAppointmentTypes.ToLower())
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                AppointmentType appointmentType = _appointmentTypeRepository.Get(a => a.Id == id && a.IsDeleted == false);
                if (appointmentType != null)
                {
                    appointmentType.IsDeleted = true;
                    appointmentType.DeletedBy = tokenModel.UserID;
                    appointmentType.DeletedDate = DateTime.UtcNow;
                    _appointmentTypeRepository.Update(appointmentType);
                    _appointmentTypeRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.AppointmentTypeDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }
    }
}