using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PayrollGroup")]
    [ActionFilter]
    public class PayrollGroupController : BaseController
    {
        private readonly IPayrollGroupService _payrollGroupService;        
        public PayrollGroupController(IPayrollGroupService payrollGroupService)
        {
            _payrollGroupService = payrollGroupService;            
        }

        /// <summary>
        ///  Get All Payroll Groups For Organization
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollGroupList")]
        public JsonResult GetPayrollGroupList(int pageNumber=1,int pageSize=10,string sortColumn="",string sortOrder="")
        {
            return Json(_payrollGroupService.ExecuteFunctions<JsonModel>(() => _payrollGroupService.GetPayrollGroupList(pageNumber, pageSize, sortColumn, sortOrder,GetToken(HttpContext))));
        }
        /// <summary>
        ///  Get All Payroll Groups For Dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollGroupListForDropdown")]
        public JsonResult GetPayrollGroupListForDropdown()
        {
            return Json(_payrollGroupService.ExecuteFunctions<JsonModel>(() => _payrollGroupService.GetPayrollGroupListForDropdown(GetToken(HttpContext))));
        }
        /// <summary>
        /// Insert/Update Payroll Groups For Organization
        /// </summary>
        /// <param name="payrollGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePayrollGroup")]
        public JsonResult SavePayrollGroup([FromBody]PayrollGroupModel payrollGroup)
        {
            return Json(_payrollGroupService.ExecuteFunctions<JsonModel>(()=>_payrollGroupService.SavePayrollGroup(payrollGroup,GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Payroll Group Detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayrollGroupById/{Id}")]
        public JsonResult GetPayrollGroupById(int Id)
        {
            return Json(_payrollGroupService.ExecuteFunctions<JsonModel>(() => _payrollGroupService.GetPayrollGroupById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Payroll Group Detail by Id
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPatch("DeletePayrollGroup")]        
        public JsonResult DeletePayrollGroup(int Id)
        {
            return Json(_payrollGroupService.ExecuteFunctions<JsonModel>(() => _payrollGroupService.DeletePayrollGroup(Id, GetToken(HttpContext))));
        }
    }
}