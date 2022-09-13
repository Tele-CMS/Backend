using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.OnboardingHeaders;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.OnboardingHeaders
{
    public class OnboardingHeaderRepository : RepositoryBase<OnboardingHeader>, IOnboardingHeaderRepository
    {
        private HCOrganizationContext _context;
        public OnboardingHeaderRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
