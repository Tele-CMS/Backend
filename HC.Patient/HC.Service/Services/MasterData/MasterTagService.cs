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
    public class MasterTagService : BaseService, IMasterTagService
    {
        private readonly IMasterTagRepository _masterTagRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public MasterTagService(IMasterTagRepository masterTagRepository, IUserCommonRepository userCommonRepository)
        {
            _masterTagRepository = masterTagRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetMasterTag(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterTagModel> masterTagModels = _masterTagRepository.GetMasterTagList<MasterTagModel>(searchFilterModel, tokenModel).ToList();
            if (masterTagModels != null && masterTagModels.Count > 0)
            {
                response = new JsonModel(masterTagModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterTagModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveMasterTag(MasterTagModel masterTagModel, TokenModel tokenModel)
        {
            MasterTags masterTags = null;
            if (masterTagModel.Id == 0)
            {
                masterTags = new MasterTags();
                AutoMapper.Mapper.Map(masterTagModel, masterTags);
                masterTags.CreatedBy = tokenModel.UserID;
                masterTags.CreatedDate = DateTime.UtcNow;
                masterTags.IsDeleted = false;
                masterTags.IsActive = true;
                masterTags.OrganizationID = tokenModel.OrganizationID;
                _masterTagRepository.Create(masterTags);
                _masterTagRepository.SaveChanges();
                response = new JsonModel(masterTags, StatusMessage.MasterTagCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                masterTags = _masterTagRepository.Get(a => a.Id == masterTagModel.Id && a.IsActive == true && a.IsDeleted == false);
                AutoMapper.Mapper.Map(masterTagModel, masterTags);
                masterTags.UpdatedBy = tokenModel.UserID;
                masterTags.UpdatedDate = DateTime.UtcNow;
                _masterTagRepository.Update(masterTags);
                _masterTagRepository.SaveChanges();
                response = new JsonModel(masterTags, StatusMessage.MasterTagUpdated, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel GetMasterTagById(int id, TokenModel tokenModel)
        {
            MasterTags masterTags = _masterTagRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (masterTags != null)
            {
                MasterTagModel masterTagModel = new MasterTagModel();
                AutoMapper.Mapper.Map(masterTags, masterTagModel);
                response = new JsonModel(masterTagModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteMasterTag(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterTags"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                MasterTags masterTags = _masterTagRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (masterTags != null)
                {
                    masterTags.IsDeleted = true;
                    masterTags.DeletedBy = tokenModel.UserID;
                    masterTags.DeletedDate = DateTime.UtcNow;
                    _masterTagRepository.Update(masterTags);
                    _masterTagRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.MasterTagDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }
    }
}
