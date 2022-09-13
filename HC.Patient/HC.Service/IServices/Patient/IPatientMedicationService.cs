using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientMedicationService:IBaseService
    {
        JsonModel GetMedication(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel SaveMedication(PatientsMedicationModel patientMedicationModel, TokenModel tokenModel);
        JsonModel GetMedicationById(int id, TokenModel tokenModel);
        JsonModel DeleteMedication(int id, TokenModel tokenModel);
    }
}
