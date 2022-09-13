using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using HC.Patient.Service.IServices;
using HC.Patient.Model;
using HC.Model;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("UserInvitation")]
    [ActionFilter]

    public class UserInvitationController : BaseController
    {
        private readonly IUserInvitationReadService _userInvitationReadService;
        private readonly IUserInvitationWriteService _userInvitationWriteService;
        public UserInvitationController(IUserInvitationReadService userInvitationReadService, IUserInvitationWriteService userInvitationWriteService)
        {
            _userInvitationReadService = userInvitationReadService;
            _userInvitationWriteService = userInvitationWriteService;
        }

        [HttpPost]
        [Route("SendInvitation")]
        public JsonResult SendUserInvitation([FromBody] UserInvitationModel userInvitationModel)
        {
            return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationWriteService.SendUserInvitation(userInvitationModel, GetToken(HttpContext), HttpContext.Request)));
        }

        [HttpPut]
        [Route("SendInvitation/{invitationId}")]
        public JsonResult ReSendUserInvitation([FromBody] UserInvitationModel userInvitationModel, int invitationId)
        {
            return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationWriteService.ReSendUserInvitation(userInvitationModel, GetToken(HttpContext), HttpContext.Request, invitationId)));
        }

        [HttpPost]
        [Route("GetUserInvitations")]
        public JsonResult GetUserInvitations([FromBody] UserInvitationFilterModel userInvitationFilterModel)
        {
            return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationReadService.GetUserInvitationList(GetToken(HttpContext), userInvitationFilterModel)));
        }
        [HttpPut]
        [Route("DeleteInvitation/{invitationId}")]
        public JsonResult DeleteInvitation(int invitationId)
        {
            return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationWriteService.DeleteUserInvitation(invitationId, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("GetUserInvitation/{invitationId}")]
        public JsonResult GetUserInvitation(int invitationId)
        {
            return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationReadService.GetUserInvitation(invitationId, GetToken(HttpContext))));
        }
    }
}