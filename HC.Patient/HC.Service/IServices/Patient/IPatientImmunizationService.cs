using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientImmunizationService :IBaseService
    {
        JsonModel SavePatientImmunization(PatientImmunizationModel patientImmunizationModel, TokenModel tokenModel);
        JsonModel GetImmunization(PatientFilterModel filterModel, TokenModel tokenModel);
        JsonModel GetImmunizationById(int id, TokenModel tokenModel);
        JsonModel DeleteImmunization(int id, TokenModel tokenModel);
    }
}
