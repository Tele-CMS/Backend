using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Repositories.IRepositories.DiseaseManagementProgram;
using HC.Patient.Service.IServices.DiseaseManagementProgram;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HC.Patient.Entity;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.DiseaseManagementProgram
{
   public class DiseaseManagementProgramService : BaseService , IDiseaseManagementProgramService
    {
        private readonly IDiseaseManagementProgramRepository _diseaseManagementProgramRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public DiseaseManagementProgramService(IDiseaseManagementProgramRepository diseaseManagementProgramRepository)
        {
            _diseaseManagementProgramRepository = diseaseManagementProgramRepository;
        }

        public JsonModel GetDiseaseManagementProgramList(FilterModel filterModel, TokenModel token)
        {
            List<DiseaseManagementProgramListModel> programListModels = _diseaseManagementProgramRepository.GetDiseaseManagementProgramList<DiseaseManagementProgramListModel>(filterModel, token).ToList();

            if (programListModels != null && programListModels.Count > 0)
            {
                response = new JsonModel()
                {
                    data = programListModels,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
                response.meta = new Meta(programListModels, filterModel);
            }
            return response;

        }

        public JsonModel GetDiseaseProgramsListWithEnrollments(TokenModel token)
        {
            List<DiseaseProgramsListWithEnrollModel> programListModels = _diseaseManagementProgramRepository.GetDiseaseProgramsListWithEnrollments<DiseaseProgramsListWithEnrollModel>(token).ToList();

            if (programListModels != null && programListModels.Count > 0)
            {
                response = new JsonModel()
                {
                    data = programListModels,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return response;
        }

        public JsonModel GetDiseaseConditionsFromProgramIds(string ProgramIds, TokenModel token)
        {
            List<DiseaseConditionsListModel> conditionListModels = _diseaseManagementProgramRepository.GetDiseaseConditionsFromProgramIds<DiseaseConditionsListModel>(ProgramIds, token).ToList();

            if (conditionListModels != null && conditionListModels.Count > 0)
            {
                response = new JsonModel()
                {
                    data = conditionListModels,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return response;
        }

        public JsonModel GetDiseaseManagementProgramListById(int id, TokenModel tokenModel)
        {
           Entity.DiseaseManagementProgram diseaseManagementProgram = _diseaseManagementProgramRepository.GetByID(id);
            if (diseaseManagementProgram != null)
            {
                DiseaseManagementProgramListModel diseaseManagementProgramListModel = new DiseaseManagementProgramListModel();
                diseaseManagementProgramListModel.ID = diseaseManagementProgram.ID;
                diseaseManagementProgramListModel.Description = diseaseManagementProgram.Description;
                diseaseManagementProgramListModel.IsActive = diseaseManagementProgram.IsActive;

                //  AutoMapper.Mapper.Map(diseaseManagementProgram, diseaseManagementProgramListModel);
                response = new JsonModel(diseaseManagementProgramListModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }


        public JsonModel SaveDiseaseManagementProgram(DiseaseManagementProgramListModel diseaseManagementProgramListModel, TokenModel tokenModel)
        {
            //SecurityQuestions securityQuestions = null;

            Entity.DiseaseManagementProgram diseaseManagementProgram = _diseaseManagementProgramRepository.Get(a => a.ID == diseaseManagementProgramListModel.ID && a.IsDeleted == false);
                if (diseaseManagementProgram != null)
                {
                // AutoMapper.Mapper.Map(diseaseManagementProgramListModel, diseaseManagementProgram);
                diseaseManagementProgram.IsActive = diseaseManagementProgramListModel.IsActive;
                diseaseManagementProgram.Description = diseaseManagementProgramListModel.Description;
                diseaseManagementProgram.UpdatedBy = tokenModel.UserID;
                diseaseManagementProgram.UpdatedDate = DateTime.UtcNow;
                _diseaseManagementProgramRepository.Update(diseaseManagementProgram);
                _diseaseManagementProgramRepository.SaveChanges();
                    response = new JsonModel(diseaseManagementProgram, StatusMessage.DiseaseManagementProgramUpdated, (int)HttpStatusCode.OK);
                }
                else
                    response = new JsonModel(null, StatusMessage.DiseaseManagementProgramDoesNotExist, (int)HttpStatusCode.BadRequest);
           
            return response;
        }
        

    }
}
