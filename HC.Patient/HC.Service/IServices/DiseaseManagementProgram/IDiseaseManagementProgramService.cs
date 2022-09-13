using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.DiseaseManagementProgram
{
   public interface IDiseaseManagementProgramService : IBaseService
    {
        JsonModel GetDiseaseManagementProgramList(FilterModel filterModel, TokenModel token);
        JsonModel GetDiseaseProgramsListWithEnrollments(TokenModel token);
        JsonModel GetDiseaseConditionsFromProgramIds(string ProgramIds, TokenModel token);
        JsonModel GetDiseaseManagementProgramListById(int id, TokenModel tokenModel);

        JsonModel SaveDiseaseManagementProgram(DiseaseManagementProgramListModel diseaseManagementProgramListModel, TokenModel tokenModel);
    }
}
