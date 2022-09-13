using HC.Model;
using HC.Patient.Service.IServices.CareManager;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("CareManger")]
    [ActionFilter]
   // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class CareMangerController : BaseController
    {
        private readonly ICareManagerService _careManagerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="careManagerService"></param>
        public CareMangerController(ICareManagerService careManagerService)
        {
            _careManagerService = careManagerService;
        }

        /// <summary>
        /// GetCareTeamMemberList
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCareTeamMemberList")]
       // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetCareTeamMemberList(int patientId, CommonFilterModel filterModel)
        {
            return Json(_careManagerService.ExecuteFunctions<JsonModel>(() => _careManagerService.GetCareManagerTeamList(patientId, filterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// GetCareTeamManagerList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCareTeamManagerList")]
        [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetCareTeamManagerList()
        {
            return Json(_careManagerService.ExecuteFunctions<JsonModel>(() => _careManagerService.GetCareManagerList(GetToken(HttpContext))));

        }
        [HttpPatch]
        [Route("AssignAndRemoveCareManagerToAllPatient")]
        [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult AssignAndRemoveCareManagerToAllPatient(int careTeamMemberId, bool isAttached)
        {
            return Json(_careManagerService.ExecuteFunctions<JsonModel>(() => _careManagerService.AssignAndRemoveCareManagerToAllPatient(careTeamMemberId, isAttached, GetToken(HttpContext))));

        }
    }
}