using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Repositories.IRepositories.CareManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.CareManager
{
    public class CareManagerRepository : ICareManagerRepository
    {
        private HCOrganizationContext _context;
        public CareManagerRepository(HCOrganizationContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetCareManagerTeamList<T>(int patientId, CommonFilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@SearchText", filterModel.SearchText),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CM_GetCareTeamManagerList.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetCareManagerList<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID) };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CM_GetTeamManagerList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public bool AssignAndRemoveCareManagerToAllPatient(int careManagerID, bool isAttached, TokenModel token)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@CareTeamMemberID", careManagerID),
                new SqlParameter("@IsAttach", isAttached)
            };
            return _context.ExecStoredProcedureListWithSuccess(SQLObjects.PAT_AttachCareTeamToPatient.ToString(), parameters.Length, parameters);
        }
    }
}