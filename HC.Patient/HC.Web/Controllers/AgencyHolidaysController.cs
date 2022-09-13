using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AgencyHolidays")]
    [ActionFilter]
    public class AgencyHolidaysController : BaseController
    {
        private readonly IAgencyHolidaysService _agencyHolidayService;
        public AgencyHolidaysController(IAgencyHolidaysService agencyHolidayService)
        {
            _agencyHolidayService = agencyHolidayService;
          
        }
        /// <summary>        
        /// Get Holiday List of Agency
        /// </summary>
        /// <param name="pageNumber"></param>
        /// /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHolidayList")]
        public JsonResult GetHolidayList(int pageNumber = 1, int pageSize = 10)
        {
            return Json(_agencyHolidayService.ExecuteFunctions<JsonModel>(() => _agencyHolidayService.GetAgencyHolidaysList(pageNumber, pageSize,GetToken(HttpContext))));
        }
        /// <summary>        
        /// Save Agency Holidays
        /// </summary>
        /// <param name="holidaysModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAgencyHolidays")]
        public JsonResult SaveAgencyHolidays([FromBody]HolidaysModel holidaysModel)
        {
            return Json(_agencyHolidayService.ExecuteFunctions<JsonModel>(() => _agencyHolidayService.SaveAgencyHolidays(holidaysModel, GetToken(HttpContext))));
        }
        /// <summary>        
        /// Get Agency Holidays By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAgencyHolidaysById/{Id}")]
        public JsonResult GetAgencyHolidaysById(int Id)
        {
            return Json(_agencyHolidayService.ExecuteFunctions<JsonModel>(() => _agencyHolidayService.GetAgencyHolidaysById(Id, GetToken(HttpContext))));
        }
        /// <summary>        
        /// Delete Agency Holidays
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteAgencyHolidays/{Id}")]
        public JsonResult DeleteAgencyHolidays(int Id)
        {
            return Json(_agencyHolidayService.ExecuteFunctions<JsonModel>(() => _agencyHolidayService.DeleteAgencyHolidays(Id, GetToken(HttpContext))));
        }
    }
}