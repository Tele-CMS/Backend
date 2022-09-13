using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Entity;
using System.Linq;
using HC.Model;

namespace HC.Patient.Repositories.IRepositories.User
{
    public interface IUserRepository : IRepositoryBase<Entity.User>
    {
        T GetApnToken<T>(int AppointmentId, int RoleId) where T : class, new();
        bool ChangePasscodeSettings(int userId, bool isPasscodeEnable);
        Entity.User GetUserByID(int UserID);
        bool UpdateUser(Entity.User user);
        IQueryable<T> GetFilteredStaff<T>(string @LocationIds, string RoleIds, string SearchKey, string StartWith, string Tags, string sortColumn, string sortOrder, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new();
    }
}