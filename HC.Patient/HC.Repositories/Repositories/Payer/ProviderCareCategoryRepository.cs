using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class ProviderCareCategoryRepository : RepositoryBase<ProviderCareCategory>, IProviderCareCategoryRepository
    {
        private HCOrganizationContext _context;
        public ProviderCareCategoryRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
