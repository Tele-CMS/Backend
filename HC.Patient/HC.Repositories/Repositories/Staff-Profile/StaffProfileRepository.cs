using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffProfileRepository : IStaffProfileRepository
    {
        private HCOrganizationContext _context;
        public StaffProfileRepository(HCOrganizationContext context)
        {
            this._context = context;
        }

    }
}
