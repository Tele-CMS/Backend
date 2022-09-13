using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.RolePermission;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Model.RolePermission;
using System.Linq;
using System.Data.SqlClient;
using HC.Model;
using HC.Common;
using System.Data;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.RolePermission
{
    public class RolePermissionRepository : RepositoryBase<Modules>, IRolePermissionRepository
    {
        private HCOrganizationContext _context;        
        public RolePermissionRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;            
        }

        public RolePermissionsModel GetRolePermissions(int roleId, int organizationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@RoleId", roleId),
                                          new SqlParameter("@OrganizationId", organizationId),
                                         };
            return _context.ExecStoredProcedureListWithOutputForRolePermissions(SQLObjects.USR_GetUserRolePermissions.ToString(), parameters.Length, parameters);

        }

        public IQueryable<T> SaveRolePermissions<T>(RolePermissionsModel rolePermissionsModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                 new SqlParameter()
                                    {
                                        ParameterName = "@ModulePermissions",
                                       SqlDbType = SqlDbType.Structured,
                                        Direction = ParameterDirection.Input,
                                        Value = CommonMethods.ListToDatatable(rolePermissionsModel.ModulePermissions)
                                    },
                 new SqlParameter()
                                    {
                                        ParameterName = "@ScreenPermissions",
                                       SqlDbType = SqlDbType.Structured,
                                        Direction = ParameterDirection.Input,
                                        Value = CommonMethods.ListToDatatable(rolePermissionsModel.ScreenPermissions)
                                    },
                   new SqlParameter()
                                    {
                                        ParameterName = "@ActionPermissions",
                                       SqlDbType = SqlDbType.Structured,
                                        Direction = ParameterDirection.Input,
                                        Value = CommonMethods.ListToDatatable(rolePermissionsModel.ActionPermissions)
                                    },

                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@LocationId", token.LocationID),
                                          new SqlParameter("@IpAddress", string.IsNullOrEmpty(token.IPAddress) ? "44.233.103.106": token.IPAddress),
                                          new SqlParameter("@UserId", token.UserID)
                                         };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.USR_SaveUserRolePermissions.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
