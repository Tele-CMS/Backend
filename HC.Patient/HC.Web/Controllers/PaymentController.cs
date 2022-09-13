using HC.Model;
using HC.Patient.Model.Payment;
using HC.Patient.Service.IServices.Payment;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Payment")]
    [ActionFilter]
    public class PaymentController : BaseController
    {        
        private IPaymentService _paymentService;

        #region Construtor of the class
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        #endregion

        #region Class Methods        

        /// <summary>
        /// get all claims with service line with specific payer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllClaimsWithServiceLinesForPayer")]
        public JsonResult GetAllClaimsWithServiceLinesForPayer(string payerIds = "", string patientIds = "", string tags = "", string fromDate = "", string toDate = "", int locationId = 0, string claimBalanceStatus = "")
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.GetAllClaimsWithServiceLinesForPayer(payerIds, patientIds, tags, fromDate, toDate, locationId, claimBalanceStatus, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("ApplyPayment")]
        public JsonResult ApplyPayment([FromBody]PaymentApplyModel paymentCheckDetailModel)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.ApplyPayment(paymentCheckDetailModel, GetToken(HttpContext))));
        }
        [HttpPost]
        [Route("EOBPayement")]
        public JsonResult EOBPayement([FromBody]PaymentApplyModel paymentCheckDetailModel)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.EOBPayement(paymentCheckDetailModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Save/Update payment details respective to service line
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveServiceLinePayment")]
        public JsonResult SaveServiceLinePayment([FromBody]PaymentModel payment)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.SaveServiceLinePayment(payment, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description - Delete payment details respective to service line
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteServiceLinePayment/{paymentDetailId}")]
        public JsonResult DeleteServiceLinePayment(int paymentDetailId)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.DeleteServiceLinePayment(paymentDetailId, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description - Get payment detail by Id for a service line
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPaymentDetailsById")]
        public JsonResult GetPaymentDetailsById(int paymentDetailId)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.GetPaymentDetailsById(paymentDetailId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Add new card to stripe
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CreateCard")]
        public JsonResult CreateCard(string id)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.CreateCard(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - All Cards Saved by client
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ListAllCards")]
        public JsonResult ListAllCards()
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.ListAllCards(GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Delete Card on Stripe
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCard")]
        public JsonResult DeleteCard(string cardId)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.DeleteCard(cardId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Set Default Card
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SetDefaultCard")]
        public JsonResult SetDefaultCard(string cardId)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.SetDefaultCard(cardId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Check Default Card
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckDefaultCard")]
        public JsonResult CheckDefaultCard(string cardId)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.CheckDefaultCard(GetToken(HttpContext))));
        }


        [HttpPost]
        [Route("GetProvidersFeesAndRefundsSettings")]
        public JsonResult GetProvidersFeesAndRefundsSettings([FromBody] List<int> providerIds)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.GetProvidersFeesAndRefundsSettings(providerIds, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("UpdateProvidersFeesAndRefundsSettings")]
        public JsonResult UpdateProvidersFeesAndRefundsSettings([FromBody]  ManageFeesRefundsModel model)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.UpdateProvidersFeesAndRefundsSettings(model, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("SaveUpdateProviderFeesforMobile")]
        public JsonResult SaveUpdateProviderFeesforMobile([FromBody] ProviderFeesModel providerfeemodel)
        {
            return Json(_paymentService.ExecuteFunctions(() => _paymentService.SaveUpdateProviderFeesforMobile(providerfeemodel, GetToken(HttpContext))));
        }

        #endregion
    }
}