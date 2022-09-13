using HC.Common;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.IRepositories.User;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.User
{
    public class UserPasswordHistoryRepository :RepositoryBase<UserPasswordHistory>, IUserPasswordHistoryRepository
    {
        private HCOrganizationContext _context;
        
        public UserPasswordHistoryRepository(HCOrganizationContext context):base(context)
        {
            _context = context;        
        }

        public List<UserPasswordHistoryModel> GetUserPasswordHistory(int userId)
        {
            return _context.UserPasswordHistory.Select(x => new UserPasswordHistoryModel()
            {
                Id = x.Id,
                UserId = x.UserId,
                LogDate = x.LogDate,
                Password = CommonMethods.Decrypt(x.Password)
            }).OrderByDescending(y => y.LogDate).Skip(0).Take(3).ToList();
        }
    }
}
