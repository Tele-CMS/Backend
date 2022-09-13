using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.IServices.User;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    [ActionFilter]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        #region Construtor of the class
        public UserController(IUserCommonRepository userCommonRepository, IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Class Methods

        [HttpPatch]
        [Route("UpdateUserPassword")]
        public JsonResult UpdateUserPassword([FromBody]UpdatePasswordModel updatePassword)
        {   
            return Json(_userService.ExecuteFunctions(()=> _userService.UpdateUserPassword(updatePassword, GetToken(HttpContext))));
        }

        [HttpPatch]
        [Route("UpdateUserStatus/{userId}/{status}")]
        public JsonResult UpdateUserStatus(int userId, bool status)
        {
            return Json(_userService.ExecuteFunctions(()=> _userService.UpdateUserStatus(userId, status, GetToken(HttpContext))));
        }

        /// <summary>
        /// this will set user offline
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckUserPasswordStatus")]
        public JsonResult CheckUserPasswordStatus()
        {
            return Json(_userService.ExecuteFunctions(() => _userService.CheckUserPasswordStatus(GetToken(HttpContext))));
        }

        [HttpPatch("UpdateExpiredPassword")]
        public JsonResult UpdateExpiredPassword([FromBody]UpdatePasswordModel updatePassword)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.UpdateExpiredPassword(updatePassword, GetToken(HttpContext))));
        }

        [HttpGet("changePasscodeSettings")]
        public JsonResult changePasscodeSettings(int userId, bool isPasscodeEnable)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.ChangePasscodeSettings(userId, isPasscodeEnable)));
        }

        [HttpGet]
        [Route("SaveUpdateMobileTokens")]
        public JsonResult SaveUpdateMobileTokens(string ApnToken, string deviceToken, int userId)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.SaveUpdateMobileTokens(ApnToken, deviceToken, userId)));
        }
        #endregion

        #region Helping Methods        
        #endregion
    }
}