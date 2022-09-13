using HC.Model;
using HC.Patient.Model.RolePermission;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.RolePermission
{
    public interface IRolePermissionService : IBaseService
    {
        JsonModel GetUserPermissions(TokenModel token,int roleId);
        JsonModel SaveUserPermissions(RolePermissionsModel rolePermissionModel, TokenModel token);
    }
}
