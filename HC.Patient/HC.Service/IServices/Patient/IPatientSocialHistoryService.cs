using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientSocialHistoryService : IBaseService
    {
        JsonModel SavePatientSocialHistory(PatientSocialHistoryModel patientSocialHistoryModel, TokenModel tokenModel);
        JsonModel GetPatientSocialHistory(int patientId, TokenModel tokenModel);
    }
}
