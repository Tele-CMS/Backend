using HC.Model;
using HC.Patient.Model.PatientMedicalFamilyHistory;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.PatientMedicalFamilyHistories
{
    public interface IPatientMedicalFamilyHistoryService :IBaseService
    {
        JsonModel SavePatientMedicalFamilyHistory(PatientMedicalFamilyHistoryModel patientMedicalFamilyHistory, TokenModel token);        
        JsonModel GetPatientMedicalFamilyHistoryById(int Id = 0, int patientID = 0);
        JsonModel GetPatientMedicalFamilyHistory(string firstName = "", string lastName = "", string Disease = "", string sortOrder = "", int page = 0, int pageSize = 10);
        JsonModel DeletePatientMedicalFamilyHistory(int Id, TokenModel token);
        
    }
}
