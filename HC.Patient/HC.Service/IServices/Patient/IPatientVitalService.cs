using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientVitalService : IBaseService
    {
        JsonModel GetVitals(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel SaveVital(PatientVitalModel patientVitalModel, TokenModel tokenModel);
        JsonModel GetVitalById(int id, TokenModel tokenModel);
        JsonModel DeleteVital(int id, TokenModel tokenModel);
    }
}
