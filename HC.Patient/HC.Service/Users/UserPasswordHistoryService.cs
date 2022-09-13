using HC.Patient.Entity;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.User;
using HC.Service;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.Users
{
    public class UserPasswordHistoryService : BaseService, IUserPasswordHistoryService
    {
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        public UserPasswordHistoryService(IUserPasswordHistoryRepository userPasswordHistoryRepository)
        {
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
        }
        public List<UserPasswordHistoryModel> GetUserPasswordHistory(int userId)
        {
            return _userPasswordHistoryRepository.GetUserPasswordHistory(userId);
        }

        public void SaveUserPasswordHistory(int userId, DateTime logDate, string password)
        {
            UserPasswordHistory userPasswordHistory = new UserPasswordHistory();
            userPasswordHistory.UserId = userId;
            userPasswordHistory.LogDate = DateTime.UtcNow;
            userPasswordHistory.Password = password;
            _userPasswordHistoryRepository.Create(userPasswordHistory);
            _userPasswordHistoryRepository.SaveChanges();
        }
    }
}
