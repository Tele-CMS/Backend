using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using HC.Patient.Service.IServices;
using HC.Patient.Model;
using HC.Model;

namespace HC.Patient.Web.Controllers
{
    //[Produces("application/json")]
    //[Route("UserRegister")]
    public class UserRegisterController : Controller
    {
        //private readonly IUserInvitationReadService _userInvitationReadService;
        //private readonly IUserInvitationWriteService _userInvitationWriteService;
        public UserRegisterController()//(IUserInvitationReadService userInvitationReadService, IUserInvitationWriteService userInvitationWriteService)
        {
           // _userInvitationReadService = userInvitationReadService;
            //_userInvitationWriteService = userInvitationWriteService;
        }

        [HttpGet]
        [Route("CheckTokenAccessibility")]
        public JsonResult CheckTokenAccessibility(int id)
        {
            return null;
            //return Json(_userInvitationWriteService.ExecuteFunctions(() => _userInvitationReadService.CheckTokenAccessibilty(GetToken(HttpContext), id)));
        }
        
    }
}