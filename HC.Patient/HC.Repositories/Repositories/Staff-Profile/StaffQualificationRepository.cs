using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffQualificationRepository : RepositoryBase<StaffQualification>, IStaffQualificationRepository
    {
        private HCOrganizationContext _context;
        public StaffQualificationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public StaffQualification SaveUpdateStaffQualification(StaffQualification staffQualification, TokenModel tokenModel)
        {
            if (staffQualification.Id > 0)
                _context.Update(staffQualification);
            else
                _context.Add(staffQualification);
            if (_context.SaveChanges() > 0)
                return staffQualification;
            else
                return null;
        }
        public List<StaffQualification> GetStaffQualification(int staffId, TokenModel tokenModel)
        {
            return _context.StaffQualifications
                .Where(u => u.StaffId == staffId
                && u.IsDeleted == false)
                .ToList();
        }
        public StaffQualification GetStaffQualificationById(int id, TokenModel tokenModel)
        {
            return _context.StaffQualifications
                .Where(u => u.Id == id
                && u.IsDeleted == false)
                .FirstOrDefault();
        }
    }
}
