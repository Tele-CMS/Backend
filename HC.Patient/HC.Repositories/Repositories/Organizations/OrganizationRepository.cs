using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.Organizations
{
    public class OrganizationRepository : RepositoryBase<HC.Patient.Entity.Organization>, IOrganizationRepository
    {
        private HCOrganizationContext _context;
        public OrganizationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
