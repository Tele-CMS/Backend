using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.SecurityQuestion;
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
    public class MasterSecurityQuestionService : BaseService, IMasterSecurityQuestionService
    {
        private readonly IMasterSecurityQuestionRepository _masterSecurityQuestionRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response;
        public MasterSecurityQuestionService(IMasterSecurityQuestionRepository masterSecurityQuestionRepository, IUserCommonRepository userCommonRepository)
        {
            _masterSecurityQuestionRepository = masterSecurityQuestionRepository;
            _userCommonRepository = userCommonRepository;
        }
        public JsonModel DeleteSecurityQuestions(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterICD"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(null, StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                SecurityQuestions securityQuestions = _masterSecurityQuestionRepository.Get(a => a.Id == id && a.IsDeleted == false);
                if (securityQuestions != null)
                {
                    securityQuestions.IsDeleted = true;
                    securityQuestions.DeletedBy = tokenModel.UserID;
                    securityQuestions.DeletedDate = DateTime.UtcNow;
                    _masterSecurityQuestionRepository.Update(securityQuestions);
                    _masterSecurityQuestionRepository.SaveChanges();
                    response = new JsonModel(null, StatusMessage.SecurityQuestionDeleted, (int)HttpStatusCodes.OK);
                }
                else
                    response = new JsonModel(null, StatusMessage.SecurityQuestionDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }

        public JsonModel GetSecurityQuestions(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterSecurityQuestionModel> masterSecurityQuestionModel = _masterSecurityQuestionRepository.GetSecurityQuestions<MasterSecurityQuestionModel>(searchFilterModel, tokenModel).ToList();
            if (masterSecurityQuestionModel != null && masterSecurityQuestionModel.Count > 0)
            {
                response = new JsonModel(masterSecurityQuestionModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterSecurityQuestionModel, searchFilterModel);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.SecurityQuestionDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }

        public JsonModel GetSecurityQuestionsById(int id, TokenModel tokenModel)
        {
            SecurityQuestions securityQuestionsEntity = _masterSecurityQuestionRepository.GetByID(id);
            if (securityQuestionsEntity != null)
            {
                MasterSecurityQuestionModel masterSecurityQuestionModel = new MasterSecurityQuestionModel();
                AutoMapper.Mapper.Map(securityQuestionsEntity, masterSecurityQuestionModel);
                response = new JsonModel(masterSecurityQuestionModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel SaveSecurityQuestions(MasterSecurityQuestionModel masterSecurityQuestionModel, TokenModel tokenModel)
        {
            SecurityQuestions securityQuestions= null;
            if (masterSecurityQuestionModel.Id == 0)
            {
                securityQuestions = new SecurityQuestions();
                AutoMapper.Mapper.Map(masterSecurityQuestionModel, securityQuestions);
                securityQuestions.CreatedBy = tokenModel.UserID;
                securityQuestions.CreatedDate = DateTime.UtcNow;
                securityQuestions.IsDeleted = false;                
                securityQuestions.OrganizationID = tokenModel.OrganizationID;
                _masterSecurityQuestionRepository.Create(securityQuestions);
                _masterSecurityQuestionRepository.SaveChanges();
                response = new JsonModel(securityQuestions, StatusMessage.SecurityQuestionCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                securityQuestions = _masterSecurityQuestionRepository.Get(a => a.Id == masterSecurityQuestionModel.Id && a.IsDeleted == false);
                if (securityQuestions != null)
                {
                    AutoMapper.Mapper.Map(masterSecurityQuestionModel, securityQuestions);
                    securityQuestions.UpdatedBy = tokenModel.UserID;
                    securityQuestions.UpdatedDate = DateTime.UtcNow;
                    _masterSecurityQuestionRepository.Update(securityQuestions);
                    _masterSecurityQuestionRepository.SaveChanges();
                    response = new JsonModel(securityQuestions, StatusMessage.SecurityQuestionUpdated, (int)HttpStatusCode.OK);
                }
                else
                    response = new JsonModel(null, StatusMessage.SecurityQuestionDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}
