using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
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
    public class PatientCustomLabelRepository : RepositoryBase<PatientCustomLabels>, IPatientCustomLabelRepository
    {
        private HCOrganizationContext _context;
        public PatientCustomLabelRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public Dictionary<string, object> GetPatientCustomLabel(int patientId, TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId",patientId),
                                              new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                };

            return _context.ExecStoredProcedureForPatientCustomLabels(SQLObjects.PAT_GetPatientCustomLabels.ToString(), parameters.Length, parameters);
        }
    }
}
