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
    public class MasterICDService : BaseService, IMasterICDService
    {
        private readonly IMasterICDRepository _masterICDRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response;
        public MasterICDService(IMasterICDRepository masterICDRepository, IUserCommonRepository userCommonRepository)
        {
            _masterICDRepository = masterICDRepository;
            _userCommonRepository = userCommonRepository;
        }
        public JsonModel DeleteMasterICDCodes(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterICD"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                MasterICD masterICD = _masterICDRepository.Get(a => a.Id == id);
                masterICD.IsDeleted = true;
                masterICD.DeletedBy = tokenModel.UserID;
                masterICD.DeletedDate = DateTime.UtcNow;
                _masterICDRepository.Update(masterICD);
                _masterICDRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.MasterICDDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel GetMasterICDCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterICDModel> masterICDModel = _masterICDRepository.GetMasterICDList<MasterICDModel>(searchFilterModel, tokenModel).ToList();
            if (masterICDModel != null && masterICDModel.Count > 0)
            {
                response = new JsonModel(masterICDModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterICDModel, searchFilterModel);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.MasterICDDoesNotExist, (int)HttpStatusCodes.BadRequest);
            }
            return response;
        }

        public JsonModel GetMasterICDCodesById(int id, TokenModel tokenModel)
        {
            MasterICD masterICDEntity = _masterICDRepository.GetByID(id);
            if (masterICDEntity != null)
            {
                MasterICDModel masterICDModel = new MasterICDModel();
                AutoMapper.Mapper.Map(masterICDEntity, masterICDModel);
                response = new JsonModel(masterICDModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel SaveMasterICDCodes(MasterICDModel masterICDModel, TokenModel tokenModel)
        {
            MasterICD masterICD = null;
            if (masterICDModel.Id == 0)
            {
                masterICD = new MasterICD();
                AutoMapper.Mapper.Map(masterICDModel, masterICD);
                masterICD.CreatedBy = tokenModel.UserID;
                masterICD.CreatedDate = DateTime.UtcNow;
                masterICD.IsDeleted = false;
                masterICD.IsActive = true;
                masterICD.OrganizationID = tokenModel.OrganizationID;
                _masterICDRepository.Create(masterICD);
                _masterICDRepository.SaveChanges();
                response = new JsonModel(masterICD, StatusMessage.MasterICDCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                masterICD = _masterICDRepository.Get(a => a.Id == masterICDModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (masterICD != null)
                {
                    AutoMapper.Mapper.Map(masterICDModel, masterICD);
                    masterICD.UpdatedBy = tokenModel.UserID;
                    masterICD.UpdatedDate = DateTime.UtcNow;
                    _masterICDRepository.Update(masterICD);
                    response = new JsonModel(masterICD, StatusMessage.MasterICDUpdated, (int)HttpStatusCode.OK);
                }
                else
                    response = new JsonModel(null, StatusMessage.MasterICDDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            _masterICDRepository.SaveChanges();
            return response;
        }
    }
}
