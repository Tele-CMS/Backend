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
    public class MasterTemplatesService : BaseService, IMasterTemplatesService
    {
        private readonly IMasterTemplatesRepository _masterTemplatesRepository;
        private readonly IMasterTemplatesCategoryRepository _masterTemplatesCategoryRepository;
        private readonly IMasterTemplatesSubCategoryRepository _masterTemplatesSubCategoryRepository;
        private readonly IUserCommonRepository _userCommonRepository;

        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public MasterTemplatesService(IMasterTemplatesRepository masterTemplatesRepository, IUserCommonRepository userCommonRepository, IMasterTemplatesCategoryRepository masterTemplatesCategoryRepository, IMasterTemplatesSubCategoryRepository masterTemplatesSubCategoryRepository)
        {
            _masterTemplatesRepository = masterTemplatesRepository;
            _masterTemplatesCategoryRepository = masterTemplatesCategoryRepository;
            _masterTemplatesSubCategoryRepository = masterTemplatesSubCategoryRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetMasterTemplates(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterTemplatesModel> masterTemplatesModels = _masterTemplatesRepository.GetMasterTemplates<MasterTemplatesModel>(searchFilterModel, tokenModel).ToList();
            if (masterTemplatesModels != null && masterTemplatesModels.Count > 0)
            {
                response = new JsonModel(masterTemplatesModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterTemplatesModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveMasterTemplate(MasterTemplatesModel masterTemplatesModel, TokenModel tokenModel)
        {
            MasterTemplates masterTemplate = null;
            if (masterTemplatesModel.Id == 0)
            {
                masterTemplate = _masterTemplatesRepository.Get(m => m.TemplateName == masterTemplatesModel.TemplateName && m.IsDeleted == false && m.IsActive == true && m.OrganizationID == tokenModel.OrganizationID);
                if (masterTemplate != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.MasterTemplateAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //insert new
                {
                    masterTemplate = new MasterTemplates();
                    AutoMapper.Mapper.Map(masterTemplatesModel, masterTemplate);
                    masterTemplate.OrganizationID = tokenModel.OrganizationID;
                    masterTemplate.CreatedBy = tokenModel.UserID;
                    masterTemplate.CreatedDate = DateTime.UtcNow;
                    masterTemplate.IsDeleted = false;
                    masterTemplate.IsActive = true;
                    _masterTemplatesRepository.Create(masterTemplate);
                    _masterTemplatesRepository.SaveChanges();
                    response = new JsonModel(masterTemplate, StatusMessage.MasterTemplateCreated, (int)HttpStatusCodes.OK);
                }
            }
            else
            {
                masterTemplate = _masterTemplatesRepository.Get(m => m.TemplateName == masterTemplatesModel.TemplateName && m.Id != masterTemplatesModel.Id && m.IsDeleted == false && m.IsActive == true && m.OrganizationID == tokenModel.OrganizationID);
                if (masterTemplate != null) // duplicate check on update
                {
                    response = new JsonModel(new object(), StatusMessage.MasterTemplateAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // update existing
                {
                    masterTemplate = _masterTemplatesRepository.Get(m => m.Id == masterTemplatesModel.Id && m.IsDeleted == false && m.IsActive == true && m.OrganizationID == tokenModel.OrganizationID);
                    if (masterTemplate != null)
                    {
                        AutoMapper.Mapper.Map(masterTemplatesModel, masterTemplate);
                        masterTemplate.UpdatedBy = tokenModel.UserID;
                        masterTemplate.UpdatedDate = DateTime.UtcNow;
                        _masterTemplatesRepository.Update(masterTemplate);
                        _masterTemplatesRepository.SaveChanges();
                        response = new JsonModel(masterTemplate, StatusMessage.MasterTemplateUpdated, (int)HttpStatusCodes.OK);
                    }
                }
            }
            return response;
        }

        public JsonModel GetMasterTemplateById(int id, TokenModel tokenModel)
        {
            MasterTemplates masterTemplates = _masterTemplatesRepository.Get(m => m.Id == id && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID);
            if (masterTemplates != null)
            {
                MasterTemplatesModel masterTemplatesModel = new MasterTemplatesModel();
                AutoMapper.Mapper.Map(masterTemplates, masterTemplatesModel);
                response = new JsonModel(masterTemplatesModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteMasterTemplate(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.MasterTemplates, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                DateTime CurrentDate = DateTime.UtcNow;
                MasterTemplates masterTemplate = _masterTemplatesRepository.Get(m => m.Id == id && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID);
                if (masterTemplate != null)
                {
                    masterTemplate.IsDeleted = true;
                    masterTemplate.DeletedBy = tokenModel.UserID;
                    masterTemplate.DeletedDate = CurrentDate;
                    _masterTemplatesRepository.Update(masterTemplate);
                    _masterTemplatesRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.MasterTemplateDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }

        public JsonModel GetMasterTemplatesCategoryForDD(TokenModel tokenModel)
        {
            List<MasterTemplateCategory> masterTemplateCategory = _masterTemplatesCategoryRepository.GetAll(m =>m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).ToList();
            if (masterTemplateCategory != null)
            {
                List<MasterTemplateCategoryModel> masterTemplateCategoryModel = new List<MasterTemplateCategoryModel>();
                    AutoMapper.Mapper.Map(masterTemplateCategory, masterTemplateCategoryModel);
                response = new JsonModel(masterTemplateCategoryModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel GetMasterTemplatesSubCategoryForDD(int masterTemplateCategoryId,TokenModel tokenModel)
        {
            List<MasterTemplateSubCategory> masterTemplateSubCategory = _masterTemplatesSubCategoryRepository.GetAll(m => m.MasterTemplateCategoryId == masterTemplateCategoryId && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).OrderBy(x=>x.DisplayOrder).ToList();
            if (masterTemplateSubCategory != null)
            {
                List<MasterTemplateSubCategoryModel> masterTemplateSubCategoryModel = new List<MasterTemplateSubCategoryModel>();
                    AutoMapper.Mapper.Map(masterTemplateSubCategory, masterTemplateSubCategoryModel);
                response = new JsonModel(masterTemplateSubCategoryModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
