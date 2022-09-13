using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class Claim837ClaimRepository : RepositoryBase<Claim837Claims>, IClaim837ClaimRepository
    {
        private HCOrganizationContext _context;
        public Claim837ClaimRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
