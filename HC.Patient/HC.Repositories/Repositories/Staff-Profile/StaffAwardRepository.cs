using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Repositories
{
    public class StaffAwardRepository : RepositoryBase<StaffAward>, IStaffAwardRepository
    {
        private HCOrganizationContext _context;
        public StaffAwardRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public StaffAward SaveUpdateStaffAward(StaffAward staffAward, TokenModel tokenModel)
        {
            if (staffAward.Id > 0)
                _context.Update(staffAward);
            else
                _context.Add(staffAward);
            if (_context.SaveChanges() > 0)
                return staffAward;
            else
                return null;
        }
        public List<StaffAward> GetStaffAward(int staffId, TokenModel tokenModel)
        {
            return _context.StaffAwards
                .Where(u => u.StaffId == staffId
                && u.IsDeleted == false)
                .ToList();
        }
        public StaffAward GetStaffAwardById(int id, TokenModel tokenModel)
        {
            return _context.StaffAwards
                .Where(u => u.Id == id
                && u.IsDeleted == false)
                .FirstOrDefault();
        }
    }
    }
