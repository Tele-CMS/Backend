using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientAuthorizationProcedureCPTLinkRepository : RepositoryBase<AuthProcedureCPT>, IPatientAuthorizationProcedureCPTLinkRepository
    {
        private HCOrganizationContext _context;
        public PatientAuthorizationProcedureCPTLinkRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
