using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System.IO;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientPrescriptionService : IBaseService
    {
        JsonModel GetprescriptionDrugList();
        JsonModel SavePrescription(PatientsPrescriptionModel patientsPrescriptionModel, TokenModel tokenModel);
        JsonModel GetPrescription(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel GetPrescriptionById(int id, TokenModel tokenModel);
        JsonModel DeletePrescription(int id, TokenModel tokenModel);
        MemoryStream GetPrescriptionPdf(PatientFaxModel patientsPrescriptionfaxModel, TokenModel tokenModel);
        JsonModel SendFax(PatientFaxModel patientsPrescriptionfaxModel, TokenModel tokenModel);
        JsonModel GetMasterprescriptionDrugs(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetMasterPharmacy(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetSentPrescriptions(PatientFilterModel patientFilterModel, TokenModel tokenModel);
    }
}
