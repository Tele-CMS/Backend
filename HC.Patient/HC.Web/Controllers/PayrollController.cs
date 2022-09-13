using HC.Model;
using HC.Patient.Service.IServices.Payroll;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Payroll")]
    [ActionFilter]
    public class PayrollController : BaseController
    {
        private readonly IPayrollService _payrollService;
        
        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;        
        }
        
        /// <summary>
        /// Get Payroll Report Data
        /// </summary>
        /// <param name="staffIds"></param>
        /// <param name="payrollGroupId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollReport")]
        public JsonResult GetPayrollReport(string staffIds, int payrollGroupId, DateTime fromDate, DateTime toDate)
        {
            return Json(_payrollService.ExecuteFunctions<JsonModel>(() => _payrollService.GetPayrollReport(staffIds,payrollGroupId,fromDate,toDate, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Users By Payroll Group
        /// </summary>
        /// <param name="payrollGroupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsersByPayrollGroup/{payrollGroupId}")]
        public JsonResult GetUsersByPayrollGroup(int payrollGroupId)
        {
            return Json(_payrollService.ExecuteFunctions<JsonModel>(() => _payrollService.GetUsersByPayrollGroup(payrollGroupId, GetToken(HttpContext))));
        }
    }
}