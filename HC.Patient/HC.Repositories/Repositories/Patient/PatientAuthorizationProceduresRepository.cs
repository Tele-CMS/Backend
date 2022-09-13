using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientAuthorizationProceduresRepository : RepositoryBase<AuthorizationProcedures>, IPatientAuthorizationProceduresRepository
    {
        private HCOrganizationContext _context;
        public PatientAuthorizationProceduresRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
