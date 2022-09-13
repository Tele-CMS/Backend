using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class Claim837ServiceLineRepository : RepositoryBase<Claim837ServiceLine>, IClaim837ServiceLineRepository
    {
        private HCOrganizationContext _context;
        public Claim837ServiceLineRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
