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
    public class MasterInsuranceTypeService : BaseService, IMasterInsuranceTypeService
    {
        private readonly IMasterInsuranceTypeRepository _masterInsuranceTypeRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public MasterInsuranceTypeService(IMasterInsuranceTypeRepository masterInsuranceTypeRepository, IUserCommonRepository userCommonRepository)
        {
            _masterInsuranceTypeRepository = masterInsuranceTypeRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetInsuranceTypes(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterInsuranceTypeModel> MasterInsuranceTypeModel = _masterInsuranceTypeRepository.GetInsuranceTypes<MasterInsuranceTypeModel>(searchFilterModel, tokenModel).ToList();
            if (MasterInsuranceTypeModel != null && MasterInsuranceTypeModel.Count > 0)
            {
                response = new JsonModel(MasterInsuranceTypeModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(MasterInsuranceTypeModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveInsuranceType(MasterInsuranceTypeModel masterInsuranceTypeModel, TokenModel tokenModel)
        {
            MasterInsuranceType masterInsuranceType = null;
            if (masterInsuranceTypeModel.Id == 0)
            {
                masterInsuranceType = _masterInsuranceTypeRepository.Get(l => l.InsuranceType == masterInsuranceTypeModel.InsuranceType && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterInsuranceType != null)//duplicate check on new insertion
                {
                    response = new JsonModel(null, StatusMessage.InsuarnceTypeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //insert new
                {
                    masterInsuranceType = new MasterInsuranceType();
                    AutoMapper.Mapper.Map(masterInsuranceTypeModel, masterInsuranceType);
                    masterInsuranceType.OrganizationID = tokenModel.OrganizationID;
                    masterInsuranceType.CreatedBy = tokenModel.UserID;
                    masterInsuranceType.CreatedDate = DateTime.UtcNow;
                    masterInsuranceType.IsDeleted = false;
                    masterInsuranceType.IsActive = true;
                    _masterInsuranceTypeRepository.Create(masterInsuranceType);
                    _masterInsuranceTypeRepository.SaveChanges();
                    response = new JsonModel(masterInsuranceType, StatusMessage.InsuranceTypeSave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                masterInsuranceType = _masterInsuranceTypeRepository.Get(l => l.InsuranceType == masterInsuranceTypeModel.InsuranceType && l.Id != masterInsuranceTypeModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterInsuranceType != null) //duplicate check
                {
                    response = new JsonModel(null, StatusMessage.InsuarnceTypeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //update existing
                {
                    masterInsuranceType = _masterInsuranceTypeRepository.Get(a => a.Id == masterInsuranceTypeModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (masterInsuranceType != null)
                    {
                        AutoMapper.Mapper.Map(masterInsuranceTypeModel, masterInsuranceType);
                        masterInsuranceType.UpdatedBy = tokenModel.UserID;
                        masterInsuranceType.UpdatedDate = DateTime.UtcNow;
                        _masterInsuranceTypeRepository.Update(masterInsuranceType);
                        _masterInsuranceTypeRepository.SaveChanges();
                        response = new JsonModel(masterInsuranceType, StatusMessage.InsuranceTypeUpdated, (int)HttpStatusCode.OK);
                    }
                    else
                        response = new JsonModel(null, StatusMessage.InsuranceTypeDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        public JsonModel GetInsuranceTypeById(int id, TokenModel tokenModel)
        {
            MasterInsuranceType masterInsuranceType = _masterInsuranceTypeRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (masterInsuranceType != null)
            {
                MasterInsuranceTypeModel masterInsuranceTypeModel = new MasterInsuranceTypeModel();
                AutoMapper.Mapper.Map(masterInsuranceType, masterInsuranceTypeModel);
                response = new JsonModel(masterInsuranceTypeModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteInsuranceType(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterInsuranceTypes"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(null, StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                MasterInsuranceType masterInsuranceType = _masterInsuranceTypeRepository.Get(a => a.Id == id && a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false);
                if (masterInsuranceType != null)
                {
                    masterInsuranceType.IsDeleted = true;
                    masterInsuranceType.DeletedBy = tokenModel.UserID;
                    masterInsuranceType.DeletedDate = DateTime.UtcNow;
                    _masterInsuranceTypeRepository.Update(masterInsuranceType);
                    _masterInsuranceTypeRepository.SaveChanges();
                    response = new JsonModel(null, StatusMessage.InsuranceTypeDeleted, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.InsuranceTypeDoesNotExist, (int)HttpStatusCodes.BadRequest);
                }
            }
            return response;
        }
    }
}
