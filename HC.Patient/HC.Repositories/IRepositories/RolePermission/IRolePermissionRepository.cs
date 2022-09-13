using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.RolePermission;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.RolePermission
{
    public interface IRolePermissionRepository:IRepositoryBase<Modules>
    {
        RolePermissionsModel GetRolePermissions(int roleId, int organizationId);
        IQueryable<T> SaveRolePermissions<T>(RolePermissionsModel rolePermissionsModel, TokenModel token) where T : class, new();
    }
}
