using HC.Model;
using HC.Patient.Model.RolePermission;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/RolePermission")]
    [ActionFilter]
    public class RolePermissionController : BaseController
    {
        private IRolePermissionService _rolePermissionService;        
        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// Get Role Permissions
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRolePermissions")]
        public JsonResult GetRolePermissions(int roleId)
        {
            return Json(_rolePermissionService.ExecuteFunctions(() => _rolePermissionService.GetUserPermissions(GetToken(HttpContext), roleId)));
        }

        /// <summary>
        /// Save Role Permissions
        /// </summary>
        /// <param name="rolePermissionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRolePermissions")]
        public JsonResult SaveRolePermissions([FromBody]RolePermissionsModel rolePermissionModel)
        {
            return Json(_rolePermissionService.ExecuteFunctions(() => _rolePermissionService.SaveUserPermissions(rolePermissionModel, GetToken(HttpContext))));
        }
    }
}
