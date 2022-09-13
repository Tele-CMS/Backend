using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.Organizations
{
    public class OrganizationSubscriptionPlanRepository : RepositoryBase<OrganizationSubscriptionPlan>, IOrganizationSubscriptionPlanRepository
    {
        private HCOrganizationContext _context;
        public OrganizationSubscriptionPlanRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
