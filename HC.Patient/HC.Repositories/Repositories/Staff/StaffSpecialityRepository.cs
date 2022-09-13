using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using HC.Patient.Entity;
using HC.Patient.Data;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffSpecialityRepository : RepositoryBase<StaffSpeciality>, IStaffSpecialityRepository
    {
        private HCOrganizationContext _context;
        public StaffSpecialityRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
