using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientCurrentMedicationService:IBaseService
    {
        JsonModel GetCurrentMedication(PatientFilterModel patientFilterModel, bool isShowAlert, TokenModel tokenModel);
        JsonModel SaveCurrentMedication(PatientsCurrentMedicationModel patientMedicationModel, TokenModel tokenModel);
        JsonModel GetCurrentMedicationById(int id, TokenModel tokenModel);
        JsonModel DeleteCurrentMedication(int id, int LinkedEncounterId, TokenModel tokenModel);
        MemoryStream PrintPatientCurrentMedication(int patientId, TokenModel tokenModel,PatientFilterModel patientFilterModel);
        JsonModel GetCurrentAndClaimMedicationList(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel GetCurrentMedicationStength(string medicationName, TokenModel tokenModel);
        JsonModel GetCurrentMedicationUnit(string medicationName, TokenModel tokenModel);
        JsonModel GetCurrentMedicationForm(string medicationName, TokenModel tokenModel);
    }
}
