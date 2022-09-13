using HC.Model;
using HC.Patient.Model.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PayersActivity")]
    [ActionFilter]
    public class PayersActivityController : BaseController
    {
        private readonly IPayerActivityService _payerActivityService;
        private readonly IPayerServiceCodesService _payerServiceCodesService;
        public PayersActivityController(IPayerActivityService payerActivityService, IPayerServiceCodesService payerServiceCodesService)
        {
            _payerActivityService = payerActivityService;
            _payerServiceCodesService = payerServiceCodesService;
        }

        /// <summary>
        /// get payer's all activities
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetActivities")]
        public JsonResult GetActivities(SearchFilterModel searchFilterModel)
        {
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.GetActivities(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get payer's activity service codes        /// 
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerActivityServiceCodes")]
        public JsonResult GetPayerActivityServiceCodes(SearchFilterModel searchFilterModel)
        {
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.GetPayerActivityServiceCodes(searchFilterModel, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get master activities types associated with payer with green else red.
        /// </summary>
        [HttpGet]
        [Route("GetMasterActivitiesForPayer")]
        public JsonResult GetMasterActivitiesForPayer(SearchFilterModel searchFilterModel)
        {
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.GetMasterActivitiesForPayer(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// this will get payer service codes which are not yet added into payer activity
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetExcludedServiceCodesFromActivity")]
        public JsonResult GetExcludedServiceCodesFromActivity(SearchFilterModel searchFilterModel)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetExcludedServiceCodesFromActivity(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get payer service code detail by Id
        /// </summary>
        /// <param name="payerAppointmentTypeId"></param>
        /// <param name="payerServiceCodeId"></param>
        /// <returns></returns>
        [HttpGet("GetPayerActivityServiceCodeDetailsById")]        
        public JsonResult GetPayerActivityServiceCodeDetailsById(int payerAppointmentTypeId = 0, int payerServiceCodeId = 0)
        {   
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerActivityServiceCodeDetailsById(payerAppointmentTypeId, payerServiceCodeId, GetToken(HttpContext))));
        }

        /// <summary>
        /// update payer activity service code modifiers
        /// </summary>
        /// <param name="payerAppointmentTypesModel"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("UpdatePayerActivityModifiers")]
        public JsonResult UpdatePayerActivityModifiers([FromBody]PayerAppointmentTypesModel payerAppointmentTypesModel)
        {   
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.UpdatePayerActivityModifiers(payerAppointmentTypesModel, GetToken(HttpContext))));
        }

        [HttpPatch("DeletePayerActivityCode")]
        public JsonResult DeletePayerActivityCode(int id)
        {
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.DeletePayerActivityCode(id, GetToken(HttpContext))));
        }

        [HttpPost("SavePayerActivityCode")]
        public JsonResult SavePayerActivityCode([FromBody]PayerActivityCodeModel payerActivityCodeModel)
        {
            return Json(_payerActivityService.ExecuteFunctions(() => _payerActivityService.SavePayerActivityCode(payerActivityCodeModel, GetToken(HttpContext))));
        }
    }
}