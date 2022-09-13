using HC.Model;
using HC.Patient.Model.Staff;
using HC.Patient.Service.IServices.Staff;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("StaffTimesheet")]
    [ActionFilter]
    public class StaffTimesheetController : BaseController
    {
        private readonly IStaffTimesheetService _staffTimesheetService; 
        private JsonModel response = null;

        public StaffTimesheetController(IStaffTimesheetService staffTimesheetService)
        {
            response = new JsonModel();
            _staffTimesheetService = staffTimesheetService;
        }

        /// <summary>
        /// Get timesheet data
        /// </summary>
        /// <param name="staffIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="weekValue"></param>
        /// <returns></returns>
        [HttpGet("GetStaffTimesheetDataSheetView")]
        public JsonResult GetStaffTimesheetDataSheetView(string staffIds = "", DateTime? startDate = null, DateTime? endDate = null, int weekValue = 0)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.GetStaffTimesheetDataSheetView(staffIds, startDate, endDate, weekValue, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get timesheet data tabular view
        /// </summary>
        /// <param name="staffIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="weekValue"></param>
        /// <returns></returns>
        [HttpGet("GetStaffTimesheetDataTabularView")]
        public JsonResult GetStaffTimesheetDataTabularView(string staffIds = "", DateTime? startDate = null, DateTime? endDate = null, int weekValue = 0)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.GetStaffTimesheetDataTabularView(staffIds, startDate, endDate, weekValue, GetToken(HttpContext))));
        }
        /// <summary>
        /// Update timesheet data
        /// </summary>
        /// <param name="timesheetModel"></param>
        /// <returns></returns>
        [HttpPost("UpdateStaffTimesheet")]
        public JsonResult UpdateStaffTimesheet([FromBody] List<StaffTimesheetModel> timesheetModel)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.UpdateStaffTimesheet(timesheetModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Add/Update timesheet data
        /// </summary>
        /// <param name="staffDetailedTimesheet"></param>
        /// <returns></returns>
        [HttpPost("SaveUserTimesheetDetails")]
        public JsonResult SaveUserTimesheetDetails([FromBody]StaffDetailedTimesheetModel staffDetailedTimesheet)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.SaveUserTimesheetDetails(staffDetailedTimesheet, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete timesheet data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteUserTimesheetDetails")]        
        public JsonResult DeleteUserTimesheetDetails(int Id)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.DeleteUserTimesheetDetails(Id, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get timesheet data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetUserTimesheetDetails/{Id}")]
        public JsonResult GetUserTimesheetDetails(int Id)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.GetUserTimesheetDetails(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Update timesheet Status
        /// </summary>
        /// <param name="staffDetailedTimesheet"></param>
        /// <returns></returns>
        [HttpPatch("UpdateUserTimesheetStatus")]
        public JsonResult UpdateUserTimesheetStatus([FromBody]List<StaffDetailedTimesheetModel> staffDetailedTimesheet)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.UpdateUserTimesheetStatus(staffDetailedTimesheet, GetToken(HttpContext))));
        }
        /// <summary>
        /// Submit User Timesheet
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPatch("SubmitUserTimesheet")]
        public JsonResult SubmitUserTimesheet(string Ids)
        {
            return Json(_staffTimesheetService.ExecuteFunctions(() => _staffTimesheetService.SubmitUserTimesheet(Ids, GetToken(HttpContext))));
        }
    }
}