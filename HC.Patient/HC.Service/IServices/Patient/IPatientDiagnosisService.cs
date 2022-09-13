using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientDiagnosisService:IBaseService
    {
        JsonModel SavePatientDiagnosis(PatientDiagnosisModel patientDiagnosisModel, TokenModel tokenModel);
        JsonModel GetDiagnosis(PatientFilterModel filterModel, TokenModel tokenModel);
        JsonModel GetDiagnosisById(int id, TokenModel tokenModel);
        JsonModel DeleteDiagnosis(int id, TokenModel tokenModel);
    }
}
