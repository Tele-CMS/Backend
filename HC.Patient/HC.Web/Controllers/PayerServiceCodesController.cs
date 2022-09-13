using HC.Model;
using HC.Patient.Model.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/PayerServiceCodes")]
    [ActionFilter]
    public class PayerServiceCodesController : BaseController
    {
        private readonly IPayerServiceCodesService _payerServiceCodesService;
        public PayerServiceCodesController(IPayerServiceCodesService payerServiceCodesService)
        {
            _payerServiceCodesService = payerServiceCodesService;
        }

        /// <summary>
        /// save and update payer service code
        /// </summary>
        /// <param name="payerServiceCodesModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePayerServiceCode")]
        public JsonResult SavePayerServiceCode([FromBody]PayerServiceCodesModel payerServiceCodesModel)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.AddUpdatePayerServiceCode(payerServiceCodesModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete payer service code with check its dependency
        /// </summary>
        /// <param name="payerServiceCodeId"></param>
        /// <returns></returns>
        [HttpPatch("DeletePayerServiceCodes")]
        public JsonResult DeletePayerServiceCodes(int payerServiceCodeId)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.DeletePayerServiceCode(payerServiceCodeId, GetToken(HttpContext))));
        }

        /// <summary>
        /// check payer service code modifier dependency with its appointment type
        /// </summary>
        /// <param name="payerModifierId"></param>
        /// <returns></returns>
        [HttpGet("CheckPayerServiceCodeModifierDependency")]
        public JsonResult CheckPayerServiceCodeModifierDependency(int payerModifierId)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.CheckPayerServiceCodeModifierDependency(payerModifierId, GetToken(HttpContext))));
        }
        /// <summary>
        /// get payer service code by id
        /// </summary>
        /// <param name="payerServiceCodeId"></param>
        /// <returns></returns>
        [HttpGet("GetPayerServiceCodeById")]
        public JsonResult GetPayerServiceCodeById(int payerServiceCodeId)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerServiceCodeById(payerServiceCodeId, GetToken(HttpContext))));
        }

        /// <summary>
        /// save/update payer service codes
        /// </summary>
        /// <param name="payerServiceCodesModels"></param>
        /// <returns></returns>
        [HttpPost("SavePayerServiceCodes")]
        public JsonResult SavePayerServiceCodes([FromBody]List<PayerServiceCodesModel> payerServiceCodesModels)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.SavePayerServiceCode(payerServiceCodesModels, GetToken(HttpContext))));
        }

        /// <summary>
        /// get payer or master service codes
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("PayerOrMasterServiceCodes")]
        public JsonResult PayerOrMasterServiceCodes(SearchFilterModel searchFilterModel)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.PayerOrMasterServiceCodes(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get master service codes which are not added into payer service codes yet
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetMasterServiceCodeEx")]
        public JsonResult GetMasterServiceCodeEx(SearchFilterModel searchFilterModel)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetMasterServiceCodeExcludedFromPayerServiceCodes(searchFilterModel, GetToken(HttpContext))));
        }
    }
}