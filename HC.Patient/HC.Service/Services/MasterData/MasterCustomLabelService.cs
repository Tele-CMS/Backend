using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
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
    public class MasterCustomLabelService : BaseService, IMasterCustomLabelService
    {
        private readonly IMasterCustomLabelRepository _masterCustomLabelRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public MasterCustomLabelService(IMasterCustomLabelRepository masterCustomLabelRepository, IUserCommonRepository userCommonRepository)
        {
            _masterCustomLabelRepository = masterCustomLabelRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetCustomLabel(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterCustomLabelModel> masterCustomLabelModels = _masterCustomLabelRepository.GetCustomLabel<MasterCustomLabelModel>(searchFilterModel, tokenModel).ToList();
            if (masterCustomLabelModels != null && masterCustomLabelModels.Count > 0)
            {
                response = new JsonModel(masterCustomLabelModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterCustomLabelModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveCustomLabel(MasterCustomLabelModel masterCustomLabelModel, TokenModel tokenModel)
        {
            MasterCustomLabels masterCustomLabels = null;
            if (masterCustomLabelModel.Id == 0)
            {
                masterCustomLabels = _masterCustomLabelRepository.Get(l => l.CustomLabelName == masterCustomLabelModel.CustomLabelName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterCustomLabels != null)
                {
                    response = new JsonModel(new object(), StatusMessage.CustomLabelAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    masterCustomLabels = new MasterCustomLabels();
                    AutoMapper.Mapper.Map(masterCustomLabelModel, masterCustomLabels);
                    masterCustomLabels.OrganizationID = tokenModel.OrganizationID;
                    masterCustomLabels.CreatedBy = tokenModel.UserID;
                    masterCustomLabels.CreatedDate = DateTime.UtcNow;
                    masterCustomLabels.IsActive = true;
                    masterCustomLabels.IsDeleted = false;
                    _masterCustomLabelRepository.Create(masterCustomLabels);
                    _masterCustomLabelRepository.SaveChanges();
                    response = new JsonModel(masterCustomLabels, StatusMessage.PatientProfile, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                masterCustomLabels = _masterCustomLabelRepository.Get(l => l.CustomLabelName == masterCustomLabelModel.CustomLabelName && l.Id != masterCustomLabelModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterCustomLabels != null)
                {
                    response = new JsonModel(new object(), StatusMessage.CustomLabelAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    masterCustomLabels = _masterCustomLabelRepository.Get(l => l.Id == masterCustomLabelModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                    AutoMapper.Mapper.Map(masterCustomLabelModel, masterCustomLabels);
                    masterCustomLabels.UpdatedBy = tokenModel.UserID;
                    masterCustomLabels.UpdatedDate = DateTime.UtcNow;
                    _masterCustomLabelRepository.Update(masterCustomLabels);
                    _masterCustomLabelRepository.SaveChanges();
                    response = new JsonModel(masterCustomLabels, StatusMessage.CustomLabelUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }

        public JsonModel GetCustomLabelById(int id, TokenModel tokenModel)
        {
            MasterCustomLabels masterCustomLabels = _masterCustomLabelRepository.Get(l => l.Id == id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
            if (masterCustomLabels != null)
            {
                MasterCustomLabelModel masterCustomLabelModel = new MasterCustomLabelModel();
                AutoMapper.Mapper.Map(masterCustomLabels, masterCustomLabelModel);
                response = new JsonModel(masterCustomLabelModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel DeleteCustomLabel(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.MasterCustomLabels, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                MasterCustomLabels masterCustomLabels = _masterCustomLabelRepository.Get(l => l.Id == id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterCustomLabels != null)
                {
                    masterCustomLabels.IsDeleted = true;
                    masterCustomLabels.DeletedBy = tokenModel.UserID;
                    masterCustomLabels.DeletedDate = DateTime.UtcNow;
                    _masterCustomLabelRepository.Update(masterCustomLabels);
                    _masterCustomLabelRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.CustomLabelDeleted, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }
    }
}
;