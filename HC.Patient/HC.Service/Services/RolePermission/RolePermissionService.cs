using HC.Patient.Repositories.IRepositories.RolePermission;
using HC.Patient.Service.IServices.RolePermission;
using System;
using HC.Model;
using HC.Patient.Model.RolePermission;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.CustomMessage;
using System.Linq;
using HC.Service;

namespace HC.Patient.Service.Services.RolePermission
{
    public class RolePermissionService : BaseService, IRolePermissionService
    {

        private IRolePermissionRepository _rolePermissionRepository;
        public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }

        public JsonModel GetUserPermissions(TokenModel token,int roleId)
        {

            try
            {
                RolePermissionsModel rolePermissions = _rolePermissionRepository.GetRolePermissions(roleId, token.OrganizationID);
                return new JsonModel()
                {
                    data = rolePermissions,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };

            }
            catch
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel SaveUserPermissions(RolePermissionsModel rolePermissionModel, TokenModel token)
        {
            try
            {
                if (rolePermissionModel.ModulePermissions.FirstOrDefault().RoleId > 0) {
                    SQLResponseModel sqlResponse = _rolePermissionRepository.SaveRolePermissions<SQLResponseModel>(rolePermissionModel, token).FirstOrDefault();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = sqlResponse.Message,
                        StatusCode = sqlResponse.StatusCode
                    };
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.SelectRole,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                    };
                }
                
            }
            catch(Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError=ex.Message
                };
            }
        }
    }
}
