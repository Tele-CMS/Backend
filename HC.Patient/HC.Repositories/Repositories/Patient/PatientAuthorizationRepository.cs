using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientAuthorizationRepository : RepositoryBase<Entity.Authorization>, IPatientAuthorizationRepository
    {
        private HCOrganizationContext _context;
        public PatientAuthorizationRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public Dictionary<string, object> GetAllAuthorizationsForPatient(int patientId, int pageNumber, int pageSize,string authType)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@AuthType", authType)
            };
            return _context.ExecStoredProcedureForAuthInfo(SQLObjects.PAT_GetAllAuthorizationsForPatient.ToString(), parameters.Length, parameters);
        }
    }
}
