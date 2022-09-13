using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram
{
    public interface IPatientDiseaseManagementProgramActivityRepository :IRepositoryBase<DiseaseManagementPlanPatientActivity>
    {
        Dictionary<string, object> GetPatientDiseaseManagementProgramActivityList(int patientId, int diseaseManagmentProgramId, int patientQuestionnaireId, TokenModel token);
        IQueryable<T> GetPatientQuestionnaireAggregatedResponse<T>(int patientQuestionnaireId, TokenModel token) where T : class, new();
        IQueryable<T> GetPatientDMPActivitiesListByDMPId<T>(int patientDiseaseManagementProgramId, TokenModel token) where T : class, new();
    }
}
