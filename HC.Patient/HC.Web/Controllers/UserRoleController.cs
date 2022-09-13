using HC.Model;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.User;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{

    [Produces("application/json")]
    [Route("UserRole")]
    [ActionFilter]
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;
        #region Construtor of the class
        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        #endregion

        #region Class Methods     
        /// <summary>
        /// Description  : get all roles
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public JsonResult GetRoles(SearchFilterModel searchFilterModel)
        {
            return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.GetRoles(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : save/update roles
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <returns></returns>
        [HttpPost("SaveRole")]
        public JsonResult SaveRole([FromBody]UserRoleModel userRoleModel)
        {
            return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.SaveRole(userRoleModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : get role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRoleById")]
        public JsonResult GetRoleById(int id)
        {
            return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.GetRoleById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : delete role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteRole")]
        public JsonResult DeleteRole(int id)
        {
            return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.DeleteRole(id, GetToken(HttpContext))));
        }

        //[HttpGet("GetLoggedinUserRole")]
        //public JsonResult GetLoggedinUserRole()
        //{
        //    return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.GetLoggedinUserRole(GetToken(HttpContext))));
        //}
        #endregion
    }
}