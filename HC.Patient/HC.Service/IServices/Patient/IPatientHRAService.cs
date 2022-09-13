using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientHRAService : IBaseService
    {
        //JsonModel GetMemberHealthPlanForHRA(string searchText, TokenModel tokenModel);
        JsonModel GetMemberHRAListing(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);
        //JsonModel BulkAssignHRA(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);
        //JsonModel BulkUpdateHRA(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);
        JsonModel GetPatientHRAData(string patDocIdArray, TokenModel tokenModel);
        //JsonModel GetEmailTemplatesForDD(TokenModel tokenModel);
        JsonModel UpdatePatientHRAData(List<PatientHRAModel> patientHRAModel, TokenModel tokenModel);
        MemoryStream PrintIndividualSummaryReport(int patientDocumentId, int patientId, TokenModel token);
        //MemoryStream PrintExecutiveSummaryReport(DateTime fromDate, DateTime toDate, int documentId, TokenModel token);
        //MemoryStream PrintHRAAssessment(int? patientDocumentId, int? patientId, int documentId, TokenModel token);

        //MemoryStream ExportMemberHRAassessmentToExcel(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);
        //MemoryStream PrintMemberHRAPDF(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel);

    }
}
