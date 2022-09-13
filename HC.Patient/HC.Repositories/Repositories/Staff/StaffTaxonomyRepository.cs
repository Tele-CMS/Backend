using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Data;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffTaxonomyRepository : RepositoryBase<StaffTaxonomy>, IStaffTaxonomyRepository
    {
        private HCOrganizationContext _context;
        public StaffTaxonomyRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
