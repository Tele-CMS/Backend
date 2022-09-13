using HC.Model;
using HC.Patient.Model.PayrollBreaktime;
using HC.Patient.Service.IServices.Payroll;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PayrollBreakTime")]
    [ActionFilter]
    public class PayrollBreakTimeController : BaseController
    {
        private readonly IPayrollBreaktimeService _payrollBreakTimeService;
        public PayrollBreakTimeController(IPayrollBreaktimeService payrollBreakTimeService)
        {
            _payrollBreakTimeService = payrollBreakTimeService;
        }

        /// <summary>
        /// Get Payroll Break Time List
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollBreakTimeList")]
        public JsonResult GetPayrollBreakTimeList(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_payrollBreakTimeService.ExecuteFunctions<JsonModel>(() => _payrollBreakTimeService.GetPayrollBreakTimeList(pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get Payroll Break Time details By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollBreakTimeById/{Id}")]
        public JsonResult GetUsersByPayrollGroup(int Id)
        {
            return Json(_payrollBreakTimeService.ExecuteFunctions<JsonModel>(() => _payrollBreakTimeService.GetPayrollBreakTimeById(Id,GetToken(HttpContext))));
        }

        /// <summary>
        /// Save Payroll Break Time details
        /// </summary>
        /// <param name="payrollBreaktimeDetailsModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePayrollBreakTime")]
        public JsonResult SavePayrollBreakTime([FromBody]PayrollBreaktimeModel payrollBreaktimeDetailsModel)
        {
            return Json(_payrollBreakTimeService.ExecuteFunctions<JsonModel>(() => _payrollBreakTimeService.SavePayrollBreakTime(payrollBreaktimeDetailsModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Payroll Break Time details
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPatch("DeletePayrollBreakTime")]        
        public JsonResult DeletePayrollBreakTime(int Id)
        {
            return Json(_payrollBreakTimeService.ExecuteFunctions<JsonModel>(() => _payrollBreakTimeService.DeletePayrollBreakTime(Id, GetToken(HttpContext))));
        }
    }
}