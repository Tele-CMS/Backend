using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffExperienceRepository : RepositoryBase<StaffExperience>, IStaffExperienceRepository
    {
        private HCOrganizationContext _context;
        public StaffExperienceRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public StaffExperience SaveUpdateStaffExperience(StaffExperience staffExperience, TokenModel tokenModel)
        {
            if (staffExperience.Id > 0)
                _context.Update(staffExperience);
            else
                _context.Add(staffExperience);
            if (_context.SaveChanges() > 0)
                return staffExperience;
            else
                return null;
        }
        public List<StaffExperience> GetStaffExperience(int staffId, TokenModel tokenModel)
        {
            return _context.StaffExperiences
                .Where(u => u.StaffId == staffId
                && u.IsDeleted == false)
                .ToList();
        }
        public StaffExperience GetStaffExperienceById(int id, TokenModel tokenModel)
        {
            return _context.StaffExperiences
                .Where(u => u.Id == id
                && u.IsDeleted == false)
                .FirstOrDefault();
        }
    }
}
