using HC.Patient.Model.Users;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.User
{
    public interface IUserPasswordHistoryService :IBaseService
    {
        List<UserPasswordHistoryModel> GetUserPasswordHistory(int userId);
        void SaveUserPasswordHistory(int userId, DateTime logDate, string password);
    }
}
