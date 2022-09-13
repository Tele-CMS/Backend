using HC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.IRepositories.SpringBPatientsVitals
{
    public interface ISpringBPatientsVitalsRepository
    {
        IQueryable<T> GetSpringBPatientVital<T>(int patientId, DateTime? fromDate, DateTime? toDate,FilterModel filterModel, TokenModel tokenModel) where T:class, new();
        IQueryable<T> GetSpringBPatientDiagnosis<T>(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel,bool isShowAlert, TokenModel token) where T : class, new();
        //IQueryable<T> GetSpringBPatientVitalsMobileView<T>(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMedication<T>(PatientFilterModel patientFilterModel, bool isShowAlert,TokenModel tokenModels) where T : class, new();
       // IQueryable<T> GetSpringBPatientLabResults<T>(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, TokenModel tokenModel) where T : class, new();
        //IQueryable<T> GetLoincCodeDetail<T>(LabFilterModel labFilterModel, TokenModel tokenModel) where T : class, new();
        //
        IQueryable<T> GetMasterMedicationAutoComplete<T>(string SearchText, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetPatientMedicationDetail<T>(int PatientMedicationID, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetDistinctDateOfVitals<T>(int patientId, TokenModel token) where T : class, new();
        IQueryable<T> AddClaimsMedToCurrent<T>(MedicationDataFilterModel medicationDataFilterModel, TokenModel token) where T : class, new();
        IQueryable<T> GetLatestPatientVitalDetail<T>(int patientId, DateTime dateTime, TokenModel tokenModel) where T : class, new();
    }
}
