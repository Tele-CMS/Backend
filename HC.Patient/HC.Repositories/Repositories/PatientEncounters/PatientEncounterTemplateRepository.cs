using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterTemplateRepository: RepositoryBase<PatientEncounterTemplates>, IPatientEncounterTemplateRepository
    {
        private HCOrganizationContext _context;

        public PatientEncounterTemplateRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;    
        }

        public IQueryable<T> GetPatientEncounterTemplateData<T>(int patientEncounterId, int masterTemplateId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PatientEncounterId", patientEncounterId),
                new SqlParameter("@MasterTemplateId", masterTemplateId),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ENC_GetPatientEncounterTemplateData.ToString(), sqlParameters.Length, sqlParameters).AsQueryable();
        }
    }
}
