using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Service.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("GroupSession")]
    public class GroupSessionController : BaseController
    {
        private readonly IGroupSessionInvitationService _groupSessionInvitationService;
        public GroupSessionController(
           IGroupSessionInvitationService groupSessionInvitationService
            )
        {
            _groupSessionInvitationService = groupSessionInvitationService;
        }
        [HttpPost]
        [Route("SaveInvitation")]
        public JsonResult SaveInvitation([FromBody] GroupSessionInvitationModel groupSessionInvitationModel)
        {
            return Json(_groupSessionInvitationService.ExecuteFunctions(() => _groupSessionInvitationService.SaveGroupSessionInvitationModel(groupSessionInvitationModel, GetToken(HttpContext))));
        }


    }
}