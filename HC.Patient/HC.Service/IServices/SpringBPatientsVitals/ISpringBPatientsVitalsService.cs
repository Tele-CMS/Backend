using HC.Model;
using HC.Patient.Model.SpringBPatient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.IServices.SpringBPatientsVitals
{
    public interface ISpringBPatientsVitalsService: IBaseService
    {
        JsonModel GetSpringBPatientVitals(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel tokenModel);
        JsonModel GetSpringBPatientDiagnosis(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, bool isShowAlert, TokenModel tokenModel);
        //JsonModel GetSpringBPatientVitalsMobileView(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel tokenModel);
        JsonModel GetMedication(PatientFilterModel patientFilterModel,bool isShowAlert, TokenModel tokenModels);
        //JsonModel GetSpringBPatientLabResults(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, TokenModel tokenModel);
       // JsonModel GetLoincCodeDetail(LabFilterModel labFilterModel, TokenModel tokenModel);
        //JsonModel SaveLoincCodeDetails(List<LoincCodeDetailModel> loincCodeDetailModels, int LinkedEncounterId, TokenModel tokenModel);
        JsonModel GetDistinctDateOfVitals(int patientId, TokenModel tokenModel);
        JsonModel GetMasterMedicationAutoComplete(string SearchText, TokenModel tokenModel);
        JsonModel GetPatientMedicationDetail(int PatientMedicationID, TokenModel tokenModel);
        JsonModel SavePatientMedication(PatMedicationModel patMedicationModel, TokenModel tokenModel);
        JsonModel DeletePatientMedication(int PatientMedicationID, int linkedEncounterId, TokenModel tokenModel);
        JsonModel AddClaimsMedToCurrent(MedicationDataFilterModel medicationDataFilterModel, TokenModel tokenModel);

        JsonModel GetLatestPatientVitalDetail(int patientId, DateTime dateTime, TokenModel tokenModel);
    }
}
