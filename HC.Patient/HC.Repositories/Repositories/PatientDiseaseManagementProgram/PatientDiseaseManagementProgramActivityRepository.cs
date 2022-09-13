using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram;
using HC.Repositories;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramActivityRepository : RepositoryBase<DiseaseManagementPlanPatientActivity>, IPatientDiseaseManagementProgramActivityRepository
    {
        private HCOrganizationContext _context;
        public PatientDiseaseManagementProgramActivityRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public Dictionary<string,object> GetPatientDiseaseManagementProgramActivityList(int patientId, int diseaseManagmentProgramId, int patientQuestionnaireId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@DiseaseManagementProgramId", diseaseManagmentProgramId),
                                           new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutputForPatientDMPActivities(SQLObjects.DMP_GetDiseaseManagementProgramActivitiesForMember.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> GetPatientDMPActivitiesListByDMPId<T>(int patientDiseaseManagementProgramId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientDiseaseManagementProgramId", patientDiseaseManagementProgramId),

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetDiseaseManagementProgramActivitiesByDMPId.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPatientQuestionnaireAggregatedResponse<T>(int patientQuestionnaireId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientQuestionnaireId", patientQuestionnaireId),
                                           //new SqlParameter("@OrganizationId", token.OrganizationID),
                                           //new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DFA_GetPatientQuestionnnaireAggregatedResponse.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
