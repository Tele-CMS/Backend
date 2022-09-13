
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientAlertRepository : RepositoryBase<Entity.PatientAlerts>, IPatientAlertRepository
    {
        private HCOrganizationContext _context;
        public PatientAlertRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetPatientAlerts<T>(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@AlertTypeIds", patientFilterModel.AlertTypeIds),
                                        new SqlParameter("@StartDate", patientFilterModel.StartDate),
                                        new SqlParameter("@EndDate", patientFilterModel.EndDate),
                                        new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                        new SqlParameter("@UserId", tokenModel.UserID),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetPatientAlerts.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetAllPatientAlertsUsers<T>(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@AlertTypeIds", patientFilterModel.AlertTypeIds),
                                          new SqlParameter("@StartDate", patientFilterModel.StartDate),
                                          new SqlParameter("@EndDate", patientFilterModel.EndDate),
                                          new SqlParameter("@CareManagerIds", patientFilterModel.CareManagerIds),
                                          new SqlParameter("@EnrollmentId", patientFilterModel.EnrollmentId),
                                          new SqlParameter("@SearchText", patientFilterModel.SearchText),
                                          new SqlParameter("@DOB", patientFilterModel.DOB),
                                          new SqlParameter("@MedicalID", patientFilterModel.MedicalID),
                                          new SqlParameter("@EligibilityStatus", patientFilterModel.EligibilityStatus),
                                          new SqlParameter("@StartAge", patientFilterModel.StartAge),
                                          new SqlParameter("@EndAge", patientFilterModel.EndAge),
                                          new SqlParameter("@RiskIds", patientFilterModel.RiskIds),
                                          new SqlParameter("@ProgramIds", patientFilterModel.ProgramIds),
                                          new SqlParameter("@GenderIds", patientFilterModel.GenderIds),
                                          new SqlParameter("@RelationshipIds", patientFilterModel.RelationshipIds),
                                          new SqlParameter("@PrimaryConditionId", patientFilterModel.PrimaryConditionId),
                                          new SqlParameter("@ComorbidConditionIds", patientFilterModel.ComorbidConditionIds),
                                          new SqlParameter("@PageNumber", patientFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                          new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder", patientFilterModel.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@UserId", tokenModel.UserID),
                                          new SqlParameter("@NextAppointmentPresent", patientFilterModel.NextAppointmentPresent),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetAllPatientAlertsUsers.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
