using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.DiseaseManagementProgram
{
    public interface IDiseaseManagementProgramActivityService : IBaseService
    {
        JsonModel GetDiseaseManagementProgramActivityList(int diseaseManagementProgramId, FilterModel filterModel, TokenModel token);
    }
}
