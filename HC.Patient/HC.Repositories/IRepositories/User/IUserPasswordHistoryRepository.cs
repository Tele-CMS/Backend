using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Users;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.User
{
    public interface IUserPasswordHistoryRepository :IRepositoryBase<UserPasswordHistory>
    {
        List<UserPasswordHistoryModel> GetUserPasswordHistory(int userId);
    }
}
