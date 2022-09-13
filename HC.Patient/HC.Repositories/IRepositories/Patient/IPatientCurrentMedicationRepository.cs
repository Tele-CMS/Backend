using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientCurrentMedicationRepository :IRepositoryBase<PatientCurrentMedication>
    {
        IQueryable<T> GetCurrentMedication<T>(PatientFilterModel patientFilterModel,bool isShowAlert, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetCurrentMedicationById<T>(int Id, TokenModel tokenModel) where T : class, new();
        PatientCurrentMedicationModel PrintPatientCurrentMedication<T>(int patientId, TokenModel tokenModel);
        IQueryable<T> GetCurrentAndClaimMedicationList<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetCurrentMedicationStength<T>(string medicationName, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetCurrentMedicationUnit<T>(string medicationName, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetCurrentMedicationForm<T>(string medicationName, TokenModel tokenModel) where T : class, new();

    }
}
