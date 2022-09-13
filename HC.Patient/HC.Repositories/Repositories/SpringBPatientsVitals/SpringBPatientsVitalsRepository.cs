using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.SpringBPatient;
using HC.Patient.Repositories.IRepositories.SpringBPatientsVitals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.Repositories.SpringBPatientsVitals
{
    public class SpringBPatientsVitalsRepository : ISpringBPatientsVitalsRepository
    {
        private HCOrganizationContext _context;
        public SpringBPatientsVitalsRepository(HCOrganizationContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetSpringBPatientVital<T>(int patientId, DateTime? fromDate, DateTime? toDate,FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PatientID", patientId),
                new SqlParameter("@OrganizationId", token.OrganizationID),
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                new SqlParameter("@PageNumber", filterModel.pageNumber),
                new SqlParameter("@PageSize", filterModel.pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.SpringB_GetSpringBVitals.ToString(), parameters.Length, parameters).AsQueryable();

        }

        public IQueryable<T> GetSpringBPatientDiagnosis<T>(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, bool isShowAlert, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationId", token.OrganizationID),
                 new SqlParameter("@isShowAlert", isShowAlert),
                new SqlParameter("@PatientId", patientId),
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                new SqlParameter("@SearchText", filterModel.SearchText),
                new SqlParameter("@PageNumber", filterModel.pageNumber),
                new SqlParameter("@PageSize", filterModel.pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.SpringB_GetPatientDiagnosis.ToString(), parameters.Length, parameters).AsQueryable();
        }

        //public IQueryable<T> GetSpringBPatientVitalsMobileView<T>(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel token) where T : class, new()
        //{
        //    SqlParameter[] parameters = {
        //        new SqlParameter("@PatientID", patientId),
        //        new SqlParameter("@OrganizationId", token.OrganizationID),
        //        new SqlParameter("@FromDate", fromDate),
        //        new SqlParameter("@ToDate", toDate),
        //        new SqlParameter("@PageNumber", filterModel.pageNumber),
        //        new SqlParameter("@PageSize", filterModel.pageSize),
        //        //new SqlParameter("@DeviceType", token.DeviceType),
        //        new SqlParameter("@UserID", token.UserID)
        //    };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.SpringB_PatientVitalsMobileView.ToString(), parameters.Length, parameters).AsQueryable();

        //}
        public IQueryable<T> GetMedication<T>(PatientFilterModel patientFilterModel,bool isShowAlert,  TokenModel tokenModels) where T : class, new()
        {
            SqlParameter[] parameters ={
                new SqlParameter("@PatientId", patientFilterModel.PatientId),
                new SqlParameter("@isShowAlert",isShowAlert),
                new SqlParameter("@OrganizationId", tokenModels.OrganizationID),
                new SqlParameter("@SearchText", patientFilterModel.SearchText),
                new SqlParameter("@PageNumber", patientFilterModel.pageNumber),
                new SqlParameter("@PageSize", patientFilterModel.pageSize),
                new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                new SqlParameter("@SortOrder", patientFilterModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.SpringB_GetMedication, parameters.Length, parameters).AsQueryable();
        }

        //public IQueryable<T> GetSpringBPatientLabResults<T>(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters ={
        //        new SqlParameter("@PatientId", patientId),
        //        new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //        new SqlParameter("@SearchText", filterModel.SearchText != null ? filterModel.SearchText : string.Empty),
        //        new SqlParameter("@FromDate", fromDate),
        //        new SqlParameter("@ToDate", toDate),
        //        new SqlParameter("@PageNumber", filterModel.pageNumber),
        //        new SqlParameter("@PageSize", filterModel.pageSize),
        //        new SqlParameter("@SortColumn", filterModel.sortColumn),
        //        new SqlParameter("@SortOrder", filterModel.sortOrder),
        //        //new SqlParameter("@DeviceType", tokenModel.DeviceType),
        //        new SqlParameter("@UserID", tokenModel.UserID)

        //    };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.SprignB_GetSpringBPatientLabResults, parameters.Length, parameters).AsQueryable();
        //}

        //public IQueryable<T> GetLoincCodeDetail<T>(LabFilterModel labFilterModel, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters ={
        //        new SqlParameter("@LoincCode", labFilterModel.LoincCode),
        //        new SqlParameter("@PatientId", labFilterModel.patientId),
        //        new SqlParameter("@FromDate", labFilterModel.FromDate),
        //        new SqlParameter("@ToDate", labFilterModel.ToDate),
        //        new SqlParameter("@OrganizationId", tokenModel.OrganizationID)
        //    };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetLoincCodeDetail, parameters.Length, parameters).AsQueryable();
        //}


        #region Patient Medication
        public IQueryable<T> GetMasterMedicationAutoComplete<T>(string SearchText, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters ={
                new SqlParameter("@SearchText", SearchText),
                new SqlParameter("@OrganizationId", tokenModel.OrganizationID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetMasterMedicationAutoComplete, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPatientMedicationDetail<T>(int PatientMedicationID, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters ={
                new SqlParameter("@PatientMedicationID", PatientMedicationID),
                new SqlParameter("@OrganizationId", tokenModel.OrganizationID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetPatientMedicationDetail, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetDistinctDateOfVitals<T>(int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PatientID", patientId),
                new SqlParameter("@OrganizationId", token.OrganizationID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetDistinctDateOfVitals.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> AddClaimsMedToCurrent<T>(MedicationDataFilterModel medicationDataFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters ={
                new SqlParameter("@PatientId", medicationDataFilterModel.PatientId),
                new SqlParameter("@NDCCode", medicationDataFilterModel.NDCCode),
                new SqlParameter("@MedSource", medicationDataFilterModel.MedSource),
                new SqlParameter("@RecordId", medicationDataFilterModel.RecordId),
                new SqlParameter("@ClaimId", medicationDataFilterModel.ClaimId),
                new SqlParameter("@OrganizationId", token.OrganizationID),
                new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_AddClaimsMedToCurrent, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetLatestPatientVitalDetail<T>(int patientId, DateTime filterDateTime, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PatientID", patientId),
                new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                new SqlParameter("@FilterDateTime", filterDateTime)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetLatestPatientVitalDetail.ToString(), parameters.Length, parameters).AsQueryable();

        }
        #endregion
    }
}