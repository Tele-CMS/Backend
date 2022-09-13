using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.eHealthScore;
using HC.Patient.Repositories.IRepositories.eHealthScore;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.Repositories.eHealthScore
{
    public class eHealthScoreRepository : RepositoryBase<PatientHealtheScore>,IeHealthScoreRepository
    {
        private HCOrganizationContext _context;
        public eHealthScoreRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public eHealthScoreModel PrinteHealthScoreReport<T>(int patientId,int patientHealtheScoreId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PatientHealtheScoreId",patientHealtheScoreId),
                                          new SqlParameter("@OrganizationID", token.OrganizationID),
                                          new SqlParameter("@UserID", token.UserID),};

            return _context.ExecStoredProcedureListWithOutputForeHealthScore(SQLObjects.EHS_GeteHealthScoreDataForPDFReport.ToString(), parameters.Length, parameters);
        }
        public IQueryable<T> GetMemberHealtheScoreListing<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHealtheScore.HealthPlanId),
                                          new SqlParameter("@DocumentId", filterModelForMemberHealtheScore.DocumentId),
                                          new SqlParameter("@ConditionId", filterModelForMemberHealtheScore.ConditionId),
                                          new SqlParameter("@SearchText", filterModelForMemberHealtheScore.SearchText),
                                          new SqlParameter("@Status", filterModelForMemberHealtheScore.StatusId),
                                          new SqlParameter("@AssignedStartDate", filterModelForMemberHealtheScore.AssignedStartDate),
                                          new SqlParameter("@AssignedEndDate", filterModelForMemberHealtheScore.AssignedEndDate),
                                          new SqlParameter("@ExpirationStartDate", filterModelForMemberHealtheScore.ExpirationStartDate),
                                          new SqlParameter("@ExpirationEndDate", filterModelForMemberHealtheScore.ExpirationEndDate),
                                          new SqlParameter("@CompletionStartDate", filterModelForMemberHealtheScore.CompletionStartDate),
                                          new SqlParameter("@CompletionEndDate", filterModelForMemberHealtheScore.CompletionEndDate),
                                          new SqlParameter("@EligibilityStartDate", filterModelForMemberHealtheScore.EligibilityStartDate),
                                          new SqlParameter("@EligibilityEndDate", filterModelForMemberHealtheScore.EligibilityEndDate),
                                          new SqlParameter("@EncounterStartDate", filterModelForMemberHealtheScore.EncounterStartDate),
                                          new SqlParameter("@EncounterEndDate", filterModelForMemberHealtheScore.EncounterEndDate),
                                          new SqlParameter("@IsEligible", filterModelForMemberHealtheScore.IsEligible),
                                          new SqlParameter("@EncounterTypeId", filterModelForMemberHealtheScore.EncounterTypeId),
                                          new SqlParameter("@Relationship", filterModelForMemberHealtheScore.Relationship),
                                          new SqlParameter("@HealthEScoreStatusFilterId" , filterModelForMemberHealtheScore.HealthEScoreStatusFilterId),
                                          new SqlParameter("@HealthEScoreFilterStartDate", filterModelForMemberHealtheScore.HealthEScoreFilterStartDate),
                                          new SqlParameter("@HealthEScoreFilterEndDate", filterModelForMemberHealtheScore.HealthEScoreFilterEndDate),                                          new SqlParameter("@PageNumber", filterModelForMemberHealtheScore.pageNumber),
                                          new SqlParameter("@PageSize", filterModelForMemberHealtheScore.pageSize),
                                          new SqlParameter("@SortColumn", filterModelForMemberHealtheScore.sortColumn),
                                          new SqlParameter("@SortOrder", filterModelForMemberHealtheScore.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
        };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMemberHealtheScoreData.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> AssignHealtheScoreToMember<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHealtheScore.HealthPlanId),
                                          new SqlParameter("@DocumentId", filterModelForMemberHealtheScore.DocumentId),
                                          new SqlParameter("@ConditionId", filterModelForMemberHealtheScore.ConditionId),
                                          new SqlParameter("@SearchText", filterModelForMemberHealtheScore.SearchText),
                                          new SqlParameter("@Status", filterModelForMemberHealtheScore.StatusId),
                                          new SqlParameter("@AssignedStartDate", filterModelForMemberHealtheScore.AssignedStartDate),
                                          new SqlParameter("@AssignedEndDate", filterModelForMemberHealtheScore.AssignedEndDate),
                                          new SqlParameter("@ExpirationStartDate", filterModelForMemberHealtheScore.ExpirationStartDate),
                                          new SqlParameter("@ExpirationEndDate", filterModelForMemberHealtheScore.ExpirationEndDate),
                                          new SqlParameter("@CompletionStartDate", filterModelForMemberHealtheScore.CompletionStartDate),
                                          new SqlParameter("@CompletionEndDate", filterModelForMemberHealtheScore.CompletionEndDate),
                                          new SqlParameter("@EligibilityStartDate", filterModelForMemberHealtheScore.EligibilityStartDate),
                                          new SqlParameter("@EligibilityEndDate", filterModelForMemberHealtheScore.EligibilityEndDate),
                                          new SqlParameter("@Date", filterModelForMemberHealtheScore.HealtheScoreDate),
                                          new SqlParameter("@EncounterStartDate", filterModelForMemberHealtheScore.EncounterStartDate),
                                          new SqlParameter("@EncounterEndDate", filterModelForMemberHealtheScore.EncounterEndDate),
                                          new SqlParameter("@IsEligible", filterModelForMemberHealtheScore.IsEligible),
                                          new SqlParameter("@EncounterTypeId", filterModelForMemberHealtheScore.EncounterTypeId),
                                          new SqlParameter("@Relationship", filterModelForMemberHealtheScore.Relationship),
                                          new SqlParameter("@HealthEScoreStatusFilterId" , filterModelForMemberHealtheScore.HealthEScoreStatusFilterId),
                                          new SqlParameter("@HealthEScoreFilterStartDate", filterModelForMemberHealtheScore.HealthEScoreFilterStartDate),
                                          new SqlParameter("@HealthEScoreFilterEndDate", filterModelForMemberHealtheScore.HealthEScoreFilterEndDate),
                                          new SqlParameter("@AssessmentId", filterModelForMemberHealtheScore.AssessmentId),
                                          new SqlParameter("@StartDate", filterModelForMemberHealtheScore.StartDate),
                                          new SqlParameter("@EndDate", filterModelForMemberHealtheScore.EndDate),
                                          new SqlParameter("@PageNumber", filterModelForMemberHealtheScore.pageNumber),
                                          new SqlParameter("@PageSize", filterModelForMemberHealtheScore.pageSize),
                                          new SqlParameter("@SortColumn", filterModelForMemberHealtheScore.sortColumn),
                                          new SqlParameter("@SortOrder", filterModelForMemberHealtheScore.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
      };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_AssignHealtheScoreToMultipleMember.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAssignedHealtheScore<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                          new SqlParameter("@PageNumber", patientFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                          new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder", patientFilterModel.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
       };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetHealtheScoreDataForMember.ToString(), parameters.Length, parameters).AsQueryable();
    }

        public IQueryable<T> BulkUpdateHealtheScore<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@HealthPlanId", filterModelForMemberHealtheScore.HealthPlanId),
                                          new SqlParameter("@DocumentId", filterModelForMemberHealtheScore.DocumentId),
                                          new SqlParameter("@ConditionId", filterModelForMemberHealtheScore.ConditionId),
                                          new SqlParameter("@SearchText", filterModelForMemberHealtheScore.SearchText),
                                          new SqlParameter("@Status", filterModelForMemberHealtheScore.StatusId),
                                          new SqlParameter("@AssignedStartDate", filterModelForMemberHealtheScore.AssignedStartDate),
                                          new SqlParameter("@AssignedEndDate", filterModelForMemberHealtheScore.AssignedEndDate),
                                          new SqlParameter("@ExpirationStartDate", filterModelForMemberHealtheScore.ExpirationStartDate),
                                          new SqlParameter("@ExpirationEndDate", filterModelForMemberHealtheScore.ExpirationEndDate),
                                          new SqlParameter("@CompletionStartDate", filterModelForMemberHealtheScore.CompletionStartDate),
                                          new SqlParameter("@CompletionEndDate", filterModelForMemberHealtheScore.CompletionEndDate),
                                          new SqlParameter("@EligibilityStartDate", filterModelForMemberHealtheScore.EligibilityStartDate),
                                          new SqlParameter("@EligibilityEndDate", filterModelForMemberHealtheScore.EligibilityEndDate),
                                          new SqlParameter("@Date", filterModelForMemberHealtheScore.HealtheScoreDate),
                                          new SqlParameter("@EncounterStartDate", filterModelForMemberHealtheScore.EncounterStartDate),
                                          new SqlParameter("@EncounterEndDate", filterModelForMemberHealtheScore.EncounterEndDate),
                                          new SqlParameter("@IsEligible", filterModelForMemberHealtheScore.IsEligible),
                                          new SqlParameter("@EncounterTypeId", filterModelForMemberHealtheScore.EncounterTypeId),
                                          new SqlParameter("@Relationship", filterModelForMemberHealtheScore.Relationship),
                                          new SqlParameter("@UpdatedStatusId" , filterModelForMemberHealtheScore.UpdatedStatusId),
                                          new SqlParameter("@HealthEScoreStatusFilterId" , filterModelForMemberHealtheScore.HealthEScoreStatusFilterId),
                                          new SqlParameter("@HealthEScoreFilterStartDate", filterModelForMemberHealtheScore.HealthEScoreFilterStartDate),
                                          new SqlParameter("@HealthEScoreFilterEndDate", filterModelForMemberHealtheScore.HealthEScoreFilterEndDate),
                                          new SqlParameter("@PageNumber", filterModelForMemberHealtheScore.pageNumber),
                                          new SqlParameter("@PageSize", filterModelForMemberHealtheScore.pageSize),
                                          new SqlParameter("@SortColumn", filterModelForMemberHealtheScore.sortColumn),
                                          new SqlParameter("@SortOrder", filterModelForMemberHealtheScore.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
      };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_UpdateHealtheScoreForMultipleMember.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetHealtheScoreDataForBulkUpdate<T>(string patientHealtheScoreIdArray, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientHealtheScoreIdArray", patientHealtheScoreIdArray),
                                          new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                                          new SqlParameter("@UserID", tokenModel.UserID),};

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMemberHealtheScoreRequiredData.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public DataTable ExportHealtheScoreToExcel(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel)
        {
            SqlParameter[] parameters = {
                             new SqlParameter("@HealthPlanId", filterModelForMemberHealtheScore.HealthPlanId),
                                          new SqlParameter("@DocumentId", filterModelForMemberHealtheScore.DocumentId),
                                          new SqlParameter("@ConditionId", filterModelForMemberHealtheScore.ConditionId),
                                          new SqlParameter("@SearchText", filterModelForMemberHealtheScore.SearchText),
                                          new SqlParameter("@Status", filterModelForMemberHealtheScore.StatusId),
                                          new SqlParameter("@AssignedStartDate", filterModelForMemberHealtheScore.AssignedStartDate),
                                          new SqlParameter("@AssignedEndDate", filterModelForMemberHealtheScore.AssignedEndDate),
                                          new SqlParameter("@ExpirationStartDate", filterModelForMemberHealtheScore.ExpirationStartDate),
                                          new SqlParameter("@ExpirationEndDate", filterModelForMemberHealtheScore.ExpirationEndDate),
                                          new SqlParameter("@CompletionStartDate", filterModelForMemberHealtheScore.CompletionStartDate),
                                          new SqlParameter("@CompletionEndDate", filterModelForMemberHealtheScore.CompletionEndDate),
                                          new SqlParameter("@EligibilityStartDate", filterModelForMemberHealtheScore.EligibilityStartDate),
                                          new SqlParameter("@EligibilityEndDate", filterModelForMemberHealtheScore.EligibilityEndDate),
                                          new SqlParameter("@EncounterStartDate", filterModelForMemberHealtheScore.EncounterStartDate),
                                          new SqlParameter("@EncounterEndDate", filterModelForMemberHealtheScore.EncounterEndDate),
                                          new SqlParameter("@IsEligible", filterModelForMemberHealtheScore.IsEligible),
                                          new SqlParameter("@EncounterTypeId", filterModelForMemberHealtheScore.EncounterTypeId),
                                          new SqlParameter("@Relationship", filterModelForMemberHealtheScore.Relationship),
                                          new SqlParameter("@HealthEScoreStatusFilterId" , filterModelForMemberHealtheScore.HealthEScoreStatusFilterId),
                                          new SqlParameter("@HealthEScoreFilterStartDate", filterModelForMemberHealtheScore.HealthEScoreFilterStartDate),
                                          new SqlParameter("@HealthEScoreFilterEndDate", filterModelForMemberHealtheScore.HealthEScoreFilterEndDate),                                          new SqlParameter("@PageNumber", filterModelForMemberHealtheScore.pageNumber),
                                          new SqlParameter("@PageSize", filterModelForMemberHealtheScore.pageSize),
                                          new SqlParameter("@SortColumn", filterModelForMemberHealtheScore.sortColumn),
                                          new SqlParameter("@SortOrder", filterModelForMemberHealtheScore.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),};

            return _context.ExecStoredProcedureForDatatableExcel(SQLObjects.PAT_GetHealtheScoreToExcel.ToString(), parameters.Length, parameters);

        }
    }
}
