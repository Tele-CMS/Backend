using HC.Model;
using HC.Patient.Model.Staff;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("StaffLeave")]
    [ActionFilter]
    public class StaffLeaveController : BaseController
    {
        private readonly IStaffLeaveService _staffLeaveService;        
        #region Construtor of the class
        public StaffLeaveController(IStaffLeaveService staffLeaveService)
        {
            _staffLeaveService = staffLeaveService;
        }
        #endregion
        /// <summary>
        /// <Description> Get Staff Applied Leave List </Description>
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetStaffLeaveList")]
        public JsonResult GetStaffLeaveList(SearchFilterModel staffLeaveFilterModel, int staffId)
        {
            return Json(_staffLeaveService.ExecuteFunctions(() => _staffLeaveService.GetStaffLeaveList(staffLeaveFilterModel, staffId, GetToken(HttpContext))));
        }
        /// <summary>
        /// <Description> Save and Update Staff Applied Leaves </Description>
        /// </summary>
        /// <param name="staffLeaveModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveStaffAppliedLeave")]
        public JsonResult SaveStaffAppliedLeave([FromBody]StaffLeaveModel staffLeaveModel)
        {
            return Json(_staffLeaveService.ExecuteFunctions(() => _staffLeaveService.AddUpdateStaffAppliedLeave(staffLeaveModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// <Description> Get Applied Staff Leave By Id </Description>
        /// </summary>
        /// <param name="StaffLeaveId"></param>
        /// <returns></returns>
        [HttpGet("GetAppliedStaffLeaveById")]
        public JsonResult GetAppliedStaffLeaveById(int StaffLeaveId)
        {
            return Json(_staffLeaveService.ExecuteFunctions(() => _staffLeaveService.GetAppliedStaffLeaveById(StaffLeaveId, GetToken(HttpContext))));
        }
        /// <summary>
        /// <Description> Cancel/Delete Applied Leave </Description>
        /// </summary>
        /// <param name="StaffLeaveId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteAppliedLeave")]
        public JsonResult DeleteAppliedLeave(int StaffLeaveId)
        {
            return Json(_staffLeaveService.ExecuteFunctions(() => _staffLeaveService.DeleteAppliedLeave(StaffLeaveId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Approve or Decline Staff Applied Leaves
        /// </summary>
        /// <param name="leaveStatusModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateLeaveStatus")]
        public JsonResult UpdateLeaveStatus([FromBody]List<LeaveStatusModel> leaveStatusModel)
        {
            return Json(_staffLeaveService.ExecuteFunctions(() => _staffLeaveService.UpdateLeaveStatus(leaveStatusModel, GetToken(HttpContext))));
        }

    }
}