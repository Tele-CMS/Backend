using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.OnboardingDetails;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.OnboardingDetails
{
    public class OnboardingDetailsRepository : RepositoryBase<OnboardingDetail>, IOnboardingDetailsRepository
    {
        private HCOrganizationContext _context;
        public OnboardingDetailsRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
