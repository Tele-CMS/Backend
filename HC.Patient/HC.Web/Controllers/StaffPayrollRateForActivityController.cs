using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Patient.Service.IServices.Payroll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("StaffPayrollRateForActivity")]
    public class StaffPayrollRateForActivityController : BaseController
    {
        private readonly IStaffPayrollRateForActivityService _staffPayrollRateForActivityService;
        public StaffPayrollRateForActivityController(IStaffPayrollRateForActivityService staffPayrollRateForActivityService)
        {
            _staffPayrollRateForActivityService = staffPayrollRateForActivityService;
        }

        /// <summary>
        /// save pay rate for staff with respective of activity
        /// </summary>
        /// <param name="staffPayrollRateForActivityModels"></param>
        /// <returns></returns>
        [HttpPost("SaveUpdateStaffPayrollRateForActivity")]
        public JsonResult SaveUpdateStaffPayrollRateForActivity([FromBody]List<StaffPayrollRateForActivityModel> staffPayrollRateForActivityModels)
        {
            return Json(_staffPayrollRateForActivityService.ExecuteFunctions(()=>_staffPayrollRateForActivityService.SaveUpdateStaffPayrollRateForActivity(staffPayrollRateForActivityModels,GetToken(HttpContext))));
        }

        /// <summary>
        /// get listing of staff payrate with respective of activity
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffPayRateOfActivity")]
        public JsonResult GetStaffPayRateOfActivity(SearchFilterModel searchFilterModel)
        {
            return Json(_staffPayrollRateForActivityService.ExecuteFunctions(() => _staffPayrollRateForActivityService.GetStaffPayRateOfActivity(searchFilterModel, GetToken(HttpContext))));
        }
    }
}