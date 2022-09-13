using HC.Model;
using HC.Patient.Model.eHealthScore;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.eHealthScore;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("eHealthScore")]
    [ActionFilter]
   // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class eHealthScoreController : BaseController
    {
        private readonly IeHealthScoreService _eHealthScoreService;
        public eHealthScoreController(IeHealthScoreService eHealthScoreService)
        {
            _eHealthScoreService = eHealthScoreService;
        }

        /// <summary>
        /// eHeatlh Score Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PrinteHealthScoreReport")]
      //  [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public IActionResult PrinteHealthScoreReport(int patientId, int patientHealtheScoreId)
        {
            MemoryStream tempStream = null;
            tempStream = _eHealthScoreService.ExecuteFunctions(() => _eHealthScoreService.PrinteHealthScoreReport(patientId, patientHealtheScoreId, GetToken(HttpContext)));
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "eHealth Score Report");
        }
        /// <summary>
        /// Get Patient HRA List for eHealth Score
        /// </summary>
        /// <param name="filterModelForMemberHealtheScore"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMemberHealtheScoreListing")]
   //     [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetMemberHealtheScoreListing(FilterModelForHealtheScore filterModelForMemberHealtheScore)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.GetMemberHealtheScoreListing(filterModelForMemberHealtheScore, GetToken(HttpContext))));
        }
        /// <summary>
        /// Assign health-e score to patients
        /// </summary>
        /// <param name="filterModelForMemberHealtheScore"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AssignHealtheScoreToMember")]
     //   [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult AssignHealtheScoreToMember([FromBody]FilterModelForHealtheScore filterModelForMemberHealtheScore)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.AssignHealtheScoreToMember(filterModelForMemberHealtheScore, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get assigned health-e score list
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAssignedHealtheScore")]
      //  [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetAssignedHealtheScore(PatientFilterModel patientFilterModel)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.GetAssignedHealtheScore(patientFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Update assigned health-e score to patients
        /// </summary>
        /// <param name="filterModelForMemberHealtheScore"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BulkUpdateHealtheScore")]
     //   [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult BulkUpdateHealtheScore([FromBody]FilterModelForHealtheScore filterModelForMemberHealtheScore)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.BulkUpdateHealtheScore(filterModelForMemberHealtheScore, GetToken(HttpContext))));
        }
        /// <summary>
        /// Update assigned health-e score to particular patient in member tab
        /// </summary>
        /// <param name="patientHealtheScoreUpdateModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateHealtheScoreForMemberTab")]
    //    [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult UpdateHealtheScoreForMemberTab([FromBody]PatientHealtheScoreUpdateModel patientHealtheScoreUpdateModel)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.UpdateHealtheScoreForMemberTab(patientHealtheScoreUpdateModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Health-e Score Data for Bulk Update
        /// </summary>
        /// <param name="patientHealtheScoreIdArray"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHealtheScoreDataForBulkUpdate")]
      //  [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetHealtheScoreDataForBulkUpdate(string patientHealtheScoreIdArray)
        {
            return Json(_eHealthScoreService.ExecuteFunctions<JsonModel>(() => _eHealthScoreService.GetHealtheScoreDataForBulkUpdate(patientHealtheScoreIdArray, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description - This method is used to export HealtheScore to excel
        /// </summary>
        /// <param name="filterModelForMemberHealtheScore"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("ExportHealtheScoreToExcel")]
      //  [Authorize(Roles = "ADMIN, STAFF")]
        public IActionResult ExportHealtheScoreToExcel(FilterModelForHealtheScore filterModelForMemberHealtheScore)
        {
            MemoryStream tempStream = _eHealthScoreService.ExecuteFunctions(() => _eHealthScoreService.ExportHealtheScoreToExcel(filterModelForMemberHealtheScore, GetToken(HttpContext)));
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/vnd.ms-excel", "Test File");
        }
    }
}