using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.Organizations
{
    public class OrganizationSMTPRepository : RepositoryBase<OrganizationSMTPDetails>, IOrganizationSMTPRepository
    {
        private HCOrganizationContext _context;
        public OrganizationSMTPRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
