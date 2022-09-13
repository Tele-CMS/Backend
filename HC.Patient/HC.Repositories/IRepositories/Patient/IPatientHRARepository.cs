using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientHRARepository : IRepositoryBase<DFA_PatientDocuments>
    {
        //IQueryable<T> GetMemberHealthPlanForHRA<T>(string searchText, TokenModel tokenModel) where T : class, new();
         IQueryable<T> GetMemberHRAListing<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetPatientHRAData<T>(string patDocIdArray, TokenModel tokenModel) where T : class, new();
        // IQueryable<T> GetEmailTemplatesForDD<T>(TokenModel tokenModel) where T : class, new();
         PatientHRAReportModel PrintIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         PatientWHOReportModel PrintWHOIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         PatientAsthmaReportModel PrintAsthmaIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         PatientCOPDReportModel PrintCOPDIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         PatientDiabetesReportModel PrintDiabetesIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         PatientCardiovascularReportModel PrintCardiovascularIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new();
         IQueryable<T> GetPatientAssessmentTpye<T>(int patientId, int patientDocumentId, TokenModel token) where T : class, new();
         PatientExecutiveReportModel PrintExecutiveSummaryReport<T>(DateTime fromDate, DateTime toDate, int documentId, TokenModel token) where T : class, new();
        // HRAAssessmentModel PrintAssessmentPDF<T>(int? patientDocumentId, int? patientId, int documentId, TokenModel token) where T : class, new();
        // IQueryable<T> BulkAssignHRA<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new();
        // IQueryable<T> BulkUpdateHRA<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new();

        // DataTable ExportMemberHRAassessmentToExcel(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);
        // IQueryable<T> PrintMemberHRAPDF<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new();

        //IQueryable<T> ExportMemberHRAassessmentToExcel<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new();

    }
}
