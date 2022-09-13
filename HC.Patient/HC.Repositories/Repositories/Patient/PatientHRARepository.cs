using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientHRARepository : RepositoryBase<DFA_PatientDocuments>, IPatientHRARepository
    {
        private HCOrganizationContext _context;
        public PatientHRARepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        //public IQueryable<T> GetMemberHealthPlanForHRA<T>(string searchText, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@SearchText", searchText),
        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),};

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetPayerHealthPlanForMemberHRA.ToString(), parameters.Length, parameters).AsQueryable();
        //}

        public IQueryable<T> GetMemberHRAListing<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
                                          new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
                                          new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
                                          new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
                                          new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
                                          new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
                                          new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
                                          new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
                                          new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
                                          new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
                                          new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
                                          new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
                                          new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
                                          new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
                                          new SqlParameter("@EnrollmentId", filterModelForMemberHRA.EnrollmentId),
                                          new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
                                          new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),
                                          new SqlParameter("@CareManagerIds", filterModelForMemberHRA.CareManagerIds),
                                          new SqlParameter("@PageNumber", filterModelForMemberHRA.pageNumber),
                                          new SqlParameter("@PageSize", filterModelForMemberHRA.pageSize),
                                          new SqlParameter("@SortColumn", filterModelForMemberHRA.sortColumn),
                                          new SqlParameter("@SortOrder", filterModelForMemberHRA.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
                                          new SqlParameter("@NextAppointmentPresent", filterModelForMemberHRA.NextAppointmentPresent),
        };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMemberHRAData.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPatientHRAData<T>(string patDocIdArray, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatDocIdArray", patDocIdArray),
                                          new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                                          new SqlParameter("@UserID", tokenModel.UserID),};

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMemberHRARequiredData.ToString(), parameters.Length, parameters).AsQueryable();
        }
        //public IQueryable<T> GetEmailTemplatesForDD<T>(TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = {
        //                                  new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserID", tokenModel.UserID),
        //                                };

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetEmailTemplatesForHRA.ToString(), parameters.Length, parameters).AsQueryable();
        //}
        public IQueryable<T> GetPatientAssessmentTpye<T>(int patientId, int patientDocumentId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PatientDocumentId", patientDocumentId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),};

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetAssessmentType.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public PatientHRAReportModel PrintIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientDocumentId", patientDocumentId),
                                          new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@OrganizationID", token.OrganizationID),
                                          new SqlParameter("@UserID", token.UserID),};

            return _context.ExecStoredProcedureListWithOutputForMemberIndividualReport(SQLObjects.PAT_GetHRADataIndividualReport.ToString(), parameters.Length, parameters);

        }
        public PatientWHOReportModel PrintWHOIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientDocumentId",patientDocumentId),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
                                        new SqlParameter("@UserID", token.UserID)};

            return _context.ExecStoredProcedureListWithOutputForWHOMemberIndividualReport(SQLObjects.PAT_GetWHODataIndividualReport.ToString(), parameters.Length, parameters);
        }
        public PatientAsthmaReportModel PrintAsthmaIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientDocumentId",patientDocumentId),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
                                        new SqlParameter("@UserID", token.UserID)};

            return _context.ExecStoredProcedureListWithOutputForAsthmaMemberIndividualReport(SQLObjects.PAT_GetAsthmaDataIndividualReport.ToString(), parameters.Length, parameters);
        }
        public PatientCOPDReportModel PrintCOPDIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientDocumentId",patientDocumentId),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
                                        new SqlParameter("@UserID", token.UserID)};

            return _context.ExecStoredProcedureListWithOutputForCOPDMemberIndividualReport(SQLObjects.PAT_GetCOPDDataIndividualReport.ToString(), parameters.Length, parameters);
        }
        public PatientDiabetesReportModel PrintDiabetesIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientDocumentId",patientDocumentId),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
                                        new SqlParameter("@UserID", token.UserID)};

            return _context.ExecStoredProcedureListWithOutputForDiabetesMemberIndividualReport(SQLObjects.PAT_GetDiabetesDataIndividualReport.ToString(), parameters.Length, parameters);
        }

        public PatientCardiovascularReportModel PrintCardiovascularIndividualSummaryReport<T>(int patientDocumentId, int patientId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientDocumentId",patientDocumentId),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
                                        new SqlParameter("@UserID", token.UserID)};

            return _context.ExecStoredProcedureListWithOutputForCardiovascularMemberIndividualReport(SQLObjects.PAT_GetCardiovascularDataIndividualReport.ToString(), parameters.Length, parameters);
        }
        public PatientExecutiveReportModel PrintExecutiveSummaryReport<T>(DateTime fromDate, DateTime toDate, int documentId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@DocumentId", documentId),
                                          new SqlParameter("@OrganizationID", token.OrganizationID),
                                          new SqlParameter("@UserID", token.UserID),};
            return _context.ExecStoredProcedureListWithOutputForMemberExecutiveReport(SQLObjects.PAT_GetDataForExecutiveReport.ToString(), parameters.Length, parameters);
        }
        //public HRAAssessmentModel PrintAssessmentPDF<T>(int? patientDocumentId, int? patientId, int documentId, TokenModel token) where T : class, new()
        //{
        //    SqlParameter[] parameters = {
        //                                  new SqlParameter("@DocumentId", documentId),
        //                                  new SqlParameter("@PatientDocumentId",patientDocumentId),
        //                                  new SqlParameter("@PatientId",patientId),
        //                                  new SqlParameter("@OrganizationID", token.OrganizationID),
        //                                  new SqlParameter("@UserID", token.UserID),};
        //    return _context.ExecStoredProcedureListWithOutputForMemberAssessmentData(SQLObjects.DFA_GetDataForPrintingAssessment.ToString(), parameters.Length, parameters);
        //}
        //public IQueryable<T> BulkAssignHRA<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
        //                                  new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
        //                                  new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
        //                                  new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
        //                                  new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
        //                                  new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
        //                                  new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
        //                                  new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
        //                                  new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
        //                                  new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
        //                                  new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
        //                                  new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
        //                                  new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
        //                                  new SqlParameter("@ExpirationDate", filterModelForMemberHRA.ExpirationDate),
        //                                  new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
        //                                  new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
        //                                  new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),
        //                                  new SqlParameter("@AssessmentId", filterModelForMemberHRA.AssessmentId),
        //                                   new SqlParameter("@EnrollmentId", filterModelForMemberHRA.EnrollmentId),
        //                                  new SqlParameter("@PageNumber", filterModelForMemberHRA.pageNumber),
        //                                  new SqlParameter("@PageSize", filterModelForMemberHRA.pageSize),
        //                                  new SqlParameter("@SortColumn", filterModelForMemberHRA.sortColumn),
        //                                  new SqlParameter("@SortOrder", filterModelForMemberHRA.sortOrder),
        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),
        //                                  new SqlParameter("@CareManagerIds", filterModelForMemberHRA.CareManagerIds)
        //};

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_AssignAssessmentToMultipleMember.ToString(), parameters.Length, parameters).AsQueryable();
        //}
        //public IQueryable<T> BulkUpdateHRA<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
        //                                  new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
        //                                  new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
        //                                  new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
        //                                  new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
        //                                  new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
        //                                  new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
        //                                  new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
        //                                  new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
        //                                  new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
        //                                  new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
        //                                  new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
        //                                  new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
        //                                  new SqlParameter("@ExpirationDate", filterModelForMemberHRA.ExpirationDate),
        //                                  new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
        //                                  new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
        //                                  new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),
        //                                  new SqlParameter("@StatusForUpdate", filterModelForMemberHRA.StatusForUpdate),
        //                                   new SqlParameter("@EnrollmentId", filterModelForMemberHRA.EnrollmentId),
        //                                  new SqlParameter("@PageNumber", filterModelForMemberHRA.pageNumber),
        //                                  new SqlParameter("@PageSize", filterModelForMemberHRA.pageSize),
        //                                  new SqlParameter("@SortColumn", filterModelForMemberHRA.sortColumn),
        //                                  new SqlParameter("@SortOrder", filterModelForMemberHRA.sortOrder),
        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),
        //                                  new SqlParameter("@CareManagerIds", filterModelForMemberHRA.CareManagerIds),
        //                                  new SqlParameter("@NextAppointmentPresent", filterModelForMemberHRA.NextAppointmentPresent)
        //};

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_UpdateAssessmentToMultipleMember.ToString(), parameters.Length, parameters).AsQueryable();
        //}

        //public DataTable ExportMemberHRAassessmentToExcel(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel)
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
        //                                  new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
        //                                  new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
        //                                  new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
        //                                  new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
        //                                  new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
        //                                  new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
        //                                  new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
        //                                  new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
        //                                  new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
        //                                  new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
        //                                  new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
        //                                  new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
        //                                  new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
        //                                  new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
        //                                  new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),
        //                                  new SqlParameter("@EnrollmentId", filterModelForMemberHRA.EnrollmentId),
        //                                  new SqlParameter("@PageNumber", filterModelForMemberHRA.pageNumber),
        //                                  new SqlParameter("@PageSize", filterModelForMemberHRA.pageSize),
        //                                  new SqlParameter("@SortColumn", filterModelForMemberHRA.sortColumn),
        //                                  new SqlParameter("@SortOrder", filterModelForMemberHRA.sortOrder),
        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),
        //                                  new SqlParameter("@CareManagerIds", filterModelForMemberHRA.CareManagerIds),
        //                                  new SqlParameter("@NextAppointmentPresent", filterModelForMemberHRA.NextAppointmentPresent)
        //    };

        //    return _context.ExecStoredProcedureForDatatableExcel(SQLObjects.PAT_GetMemberHRADataExportExcel.ToString(), parameters.Length, parameters);

        //}
        //public IQueryable<T> PrintMemberHRAPDF<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
        //                                  new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
        //                                  new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
        //                                  new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
        //                                  new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
        //                                  new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
        //                                  new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
        //                                  new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
        //                                  new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
        //                                  new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
        //                                  new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
        //                                  new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
        //                                  new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
        //                                  new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
        //                                  new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
        //                                  new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),
        //                                  new SqlParameter("@EnrollmentId", filterModelForMemberHRA.EnrollmentId),
        //                                  new SqlParameter("@PageNumber", filterModelForMemberHRA.pageNumber),
        //                                  new SqlParameter("@PageSize", filterModelForMemberHRA.pageSize),
        //                                  new SqlParameter("@SortColumn", filterModelForMemberHRA.sortColumn),
        //                                  new SqlParameter("@SortOrder", filterModelForMemberHRA.sortOrder),
        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),
        //                                  new SqlParameter("@CareManagerIds", filterModelForMemberHRA.CareManagerIds),
        //                                  new SqlParameter("@NextAppointmentPresent", filterModelForMemberHRA.NextAppointmentPresent)
        //    };

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_PrintMemberHRADataToPDF.ToString(), parameters.Length, parameters).AsQueryable();
        //}
        //public IQueryable<T> ExportMemberHRAassessmentToExcel<T>(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHRA.HealthPlanId),
        //                                  new SqlParameter("@DocumentId", filterModelForMemberHRA.DocumentId),
        //                                  new SqlParameter("@ConditionId", filterModelForMemberHRA.ConditionId),
        //                                  new SqlParameter("@SearchText", filterModelForMemberHRA.SearchText),
        //                                  new SqlParameter("@Status", filterModelForMemberHRA.StatusId),
        //                                  new SqlParameter("@AssignedStartDate", filterModelForMemberHRA.AssignedStartDate),
        //                                  new SqlParameter("@AssignedEndDate", filterModelForMemberHRA.AssignedEndDate),
        //                                  new SqlParameter("@ExpirationStartDate", filterModelForMemberHRA.ExpirationStartDate),
        //                                  new SqlParameter("@ExpirationEndDate", filterModelForMemberHRA.ExpirationEndDate),
        //                                  new SqlParameter("@CompletionStartDate", filterModelForMemberHRA.CompletionStartDate),
        //                                  new SqlParameter("@CompletionEndDate", filterModelForMemberHRA.CompletionEndDate),
        //                                  new SqlParameter("@EligibilityStartDate", filterModelForMemberHRA.EligibilityStartDate),
        //                                  new SqlParameter("@EligibilityEndDate", filterModelForMemberHRA.EligibilityEndDate),
        //                                  new SqlParameter("@IsEligible", filterModelForMemberHRA.IsEligible),
        //                                  new SqlParameter("@ProgramTypeId", filterModelForMemberHRA.ProgramTypeId),
        //                                  new SqlParameter("@Relationship", filterModelForMemberHRA.Relationship),

        //                                  new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
        //                                  new SqlParameter("@UserId", tokenModel.UserID),
        //};

        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMemberHRADataExportExcel.ToString(), parameters.Length, parameters).AsQueryable();
        //}

    }
}
