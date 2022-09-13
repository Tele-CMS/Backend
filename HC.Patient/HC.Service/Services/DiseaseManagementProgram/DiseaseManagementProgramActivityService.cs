using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Repositories.IRepositories.DiseaseManagementProgram;
using HC.Patient.Service.IServices.DiseaseManagementProgram;
using HC.Service;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.DiseaseManagementProgram
{
    public class DiseaseManagementProgramActivityService : BaseService , IDiseaseManagementProgramActivityService
    {
        private readonly IDiseaseManagementProgramActivityRepository _diseaseManagementProgramActivityRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public DiseaseManagementProgramActivityService(IDiseaseManagementProgramActivityRepository diseaseManagementProgramActivityRepository)
        {
            _diseaseManagementProgramActivityRepository = diseaseManagementProgramActivityRepository;
        }
        public JsonModel GetDiseaseManagementProgramActivityList(int diseaseManagementProgramId, FilterModel filterModel, TokenModel token)
        {
            List<DiseasemanagementProgramActivityListModel> programActivityListModels = _diseaseManagementProgramActivityRepository.GetDiseaseManagementProgramActivitiesList<DiseasemanagementProgramActivityListModel>(diseaseManagementProgramId, filterModel, token).ToList();
            if (programActivityListModels != null && programActivityListModels.Count > 0)
            {
                response = new JsonModel()
                {
                    data = programActivityListModels,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
                response.meta = new Meta(programActivityListModels, filterModel);
            }
            return response;

        }
    }
}
