using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientAllergyService : IBaseService
    {
        JsonModel GetAllergies(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel SaveAllergy(PatientAllergyModel patientAllergyModel, TokenModel tokenModel);
        JsonModel GetAllergyById(int id, TokenModel tokenModel);
        JsonModel DeleteAllergy(int id, TokenModel tokenModel);
    }
}
