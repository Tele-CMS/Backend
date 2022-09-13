using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Patient.Model.Availability;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AvailabilityTemplates")]
    [ActionFilter]
    public class AvailabilityTemplatesController : BaseController
    {
        private readonly IStaffAvailabilityService _staffAvailabilityService;

        public AvailabilityTemplatesController(IStaffAvailabilityService staffAvailabilityService)
        {
            _staffAvailabilityService = staffAvailabilityService;
        }
        /// <summary>        
        /// Description  : get all avaliability for staff
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="isLeaveNeeded"></param>
        /// <returns></returns>
        [HttpGet("GetAvailability/{staffID}/{isLeaveNeeded}")]
        public JsonResult GetAvailability(string staffID = "", bool isLeaveNeeded = false)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.GetStaffAvailabilty(staffID, GetToken(HttpContext), isLeaveNeeded)));
        }

        /// <summary>        
        /// Description  : save/update avaliability for staff
        /// </summary>
        /// <param name="availabilityModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUpdateAvailability")]
        public JsonResult PostAsync([FromBody]AvailabilityModel availabilityModel)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.SaveStaffAvailabilty(availabilityModel, GetToken(HttpContext))));
        }

        
        /// <summary>        
        /// Description  : save/update avaliability for staff
        /// </summary>
        /// <param name="availabilityModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUpdateAvailabilityWithLocation")]
        public JsonResult SaveUpdateAvailabilityWithLocation([FromBody]AvailabilityModel availabilityModel)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.SaveStaffAvailabiltyWithLocation(availabilityModel, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description  : get avaliability for staff with Location
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetStaffAvailabilityWithLocation")]
        public JsonResult GetStaffAvailabilityWithLocation(string staffID, int locationId, bool isLeaveNeeded = false)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.GetStaffAvailabilityWithLocation(staffID, locationId, isLeaveNeeded, GetToken(HttpContext))));
        }
        [HttpGet("GetStaffAvailabilityWithLocationMobile")]
        public JsonResult GetStaffAvailabilityWithLocationMobile(string date, string staffID, int locationId, bool isLeaveNeeded = false)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.GetStaffAvailabilityWithLocationMobile(staffID, locationId, isLeaveNeeded, GetToken(HttpContext), Convert.ToDateTime(date))));
        }
    }
}