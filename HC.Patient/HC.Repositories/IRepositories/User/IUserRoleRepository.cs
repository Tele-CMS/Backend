using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Entity;
using System.Linq;
using HC.Model;

namespace HC.Patient.Repositories.IRepositories.User
{
    public interface IUserRoleRepository : IRepositoryBase<UserRoles>
    {
        IQueryable<T> GetRoles<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}