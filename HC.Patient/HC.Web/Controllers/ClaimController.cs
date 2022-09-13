//using Audit.WebApi;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Claim;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.Claim;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Claim")]
    [ActionFilter]
    public class ClaimController : BaseController
    {
        private readonly IClaimService _claimService;
        private readonly IAuditLogService _auditLogService;
        private JsonModel response;
        #region Construtor of the class
        public ClaimController(IClaimService claimService, IAuditLogService auditLogService)
        {
            _claimService = claimService;
            _auditLogService = auditLogService;
        }
        #endregion

        #region Class Methods        
        /// <summary>
        /// Get Claim
        /// </summary>
        /// <param name="masterDataNames"></param>
        /// <returns></returns>

        #endregion


        /// <summary>
        /// Get claims
        /// </summary>
        /// <param name="pageNumber">which page number you want to access from listing of pages</param>
        /// <param name="pageSize">how many records you want on a single page</param>
        /// <param name="claimId">id of claim</param>
        /// <param name="lastName">pass last name of the claim owner</param>
        /// <param name="firstName">pass first name of the claim owner</param>
        /// <param name="fromDate">pass from date range to get specific date set results</param>
        /// <param name="toDate">pass to date range to get specific date set results</param>
        /// <param name="payerName">pass specific payer for specfic result set</param>
        /// <param name="sortColumn">pass sort column for sorting</param>
        /// <param name="sortOrder">pass sort order for sorting (asc/desc)</param>
        /// <param name="claimStatusId">pass claim status id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaims")]
        public JsonResult GetClaims(int pageNumber = 1, int pageSize = 10, string claimId = "", string lastName = "", string firstName = "", string fromDate = "", string toDate = "", string payerName = "", string sortColumn = "", string sortOrder = "", int? claimStatusId = null)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                int? claimIdTrim = !string.IsNullOrEmpty(claimId) && (claimId.ToLower().IndexOf("cl") > -1) ? Convert.ToInt32(claimId.Remove(0, 2)) : !string.IsNullOrEmpty(claimId) ? Convert.ToInt32(claimId) : (int?)null;

                response = _claimService.GetClaims(token.OrganizationID, pageNumber, pageSize, claimIdTrim, lastName, firstName, fromDate, toDate, payerName, sortColumn, sortOrder, claimStatusId);
                if (response.StatusCode == (int)HttpStatusCodes.OK)
                    _auditLogService.AccessLogs(AuditLogsScreen.Billing, AuditLogAction.Access, null, token.UserID, token, LoginLogLoginAttempt.Success);
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                });
            }
        }

        /// <summary>
        /// get claim service lines
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimServiceLines")]
        public JsonResult GetClaimServiceLines(int claimId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimServiceLines(claimId)));
        }
        /// <summary>
        /// Get Open Charges For Balance
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="payerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOpenChargesForPatient")]
        public JsonResult GetOpenChargesForPatient(int patientId, int payerId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetOpenChargesForPatient(patientId, payerId, GetToken(HttpContext))));
        }
        /// <summary>
        /// create claim
        /// </summary>
        /// <param name="patientEncounterId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateClaim")]
        public JsonResult CreateClaim(int patientEncounterId, bool isAdmin = false)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.CreateClaim(patientEncounterId, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claim service line detail by id
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="serviceLineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimServiceLineDetail")]
        public JsonResult GetClaimServiceLineDetail(int claimId, int serviceLineId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimServiceLineDetails(claimId, serviceLineId)));
        }

        /// <summary>
        /// save new service line
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="claimServiceLineModel"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("SaveClaimServiceLine")]
        public JsonResult SaveClaimServiceLine(int claimId, [FromBody]ClaimServiceLineModel claimServiceLineModel)
        {
            return Json(_claimService.ExecuteFunctions(() => _claimService.SaveClaimServiceLine(claimId, claimServiceLineModel, GetToken(HttpContext))));
        }


        /// <summary>
        /// delete claom service line
        /// </summary>
        /// <param name="serviceLineId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteClaimServiceLine")]
        public JsonResult DeleteClaimServiceLine(int serviceLineId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.DeleteClaimServiceLine(serviceLineId, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete claim
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteClaim/{claimId}")]
        public JsonResult DeleteClaim(int claimId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.DeleteClaim(claimId, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claim detail by id
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimDetailsById")]
        public JsonResult GetClaimDetailsById(int claimId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimDetailsById(claimId)));
        }


        /// <summary>
        /// update claim detail
        /// </summary>
        /// <param name="claimModel"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("UpdateClaimDetails")]
        public JsonResult UpdateClaimDetails([FromBody]ClaimModel claimModel)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.UpdateClaimDetails(claimModel, GetToken(HttpContext))));
        }


        /// <summary>
        /// get all claim with all service lines
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="claimId"></param>
        /// <param name="patientIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="payerName"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="claimStatusId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllClaimsWithServiceLines")]
        public JsonResult GetAllClaimsWithServiceLines(int pageNumber = 1, int pageSize = 10, string claimId = "", string patientIds = "", string fromDate = "", string toDate = "", string payerName = "", string sortColumn = "", string sortOrder = "", int? claimStatusId = null)
        {
            int? claimIdTrim = !string.IsNullOrEmpty(claimId) && (claimId.ToLower().IndexOf("cl") > -1) ? Convert.ToInt32(claimId.Remove(0, 2)) : (int?)null;
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetAllClaimsWithServiceLines(pageNumber, pageSize, claimIdTrim, patientIds, fromDate, toDate, payerName, sortColumn, sortOrder, claimStatusId, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claim for patient ledger
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="claimBalanceStatus"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimsForPatientLedger")]
        public JsonResult GetClaimsForPatientLedger(int patientId = 0, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", string claimBalanceStatus = "")
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimsForPatientLedger(patientId, pageNumber, pageSize, sortColumn, sortOrder, claimBalanceStatus, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claim service line for patient
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimServiceLinesForPatientLedger")]
        public JsonResult GetClaimServiceLinesForPatientLedger(int claimId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimServiceLinesForPatientLedger(claimId, GetToken(HttpContext))));
        }
        
        /// <summary>
        /// get claim history
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimHistory")]
        public JsonResult GetClaimHistory(string claimId, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            int? claimIdTrim = !string.IsNullOrEmpty(claimId) && (claimId.ToLower().IndexOf("cl") > -1) ? Convert.ToInt32(claimId.Remove(0, 2)) : !string.IsNullOrEmpty(claimId) ? Convert.ToInt32(claimId) : (int?)null;
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimHistory(claimIdTrim, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Update claim payment status in client ledger
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="paymentStatusId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("UpdatePaymentStatus")]
        public JsonResult UpdatePaymentStatus(string claimId, int? paymentStatusId)
        {
            int? claimIdTrim = !string.IsNullOrEmpty(claimId) && (claimId.ToLower().IndexOf("cl") > -1) ? Convert.ToInt32(claimId.Remove(0, 2)) : !string.IsNullOrEmpty(claimId) ? Convert.ToInt32(claimId) : (int?)null;
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.UpdatePaymentStatus(claimIdTrim, paymentStatusId, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claims for ledger
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="patientIds"></param>
        /// <param name="payerIds"></param>
        /// <param name="tags"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="claimBalanceStatus"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimsForLedger")]
        public JsonResult GetClaimsForLedger(int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", string patientIds = "", string payerIds = "", string tags = "", string fromDate = "", string toDate = "", string claimBalanceStatus = "")
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimsForLedger(pageNumber, pageSize, sortColumn, sortOrder, patientIds, payerIds, tags, fromDate, toDate, claimBalanceStatus, GetToken(HttpContext))));
        }

        /// <summary>
        /// get claim service l;ine for ledger
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClaimServiceLinesForLedger")]
        public JsonResult GetClaimServiceLinesForLedger(int claimId)
        {
            return Json(_claimService.ExecuteFunctions<JsonModel>(() => _claimService.GetClaimServiceLinesForLedger(claimId, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetHashValue")]
        public JsonResult GetHashValue(string plainText)
        {
            return Json(CommonMethods.GetHashValue(plainText));
        }
    }
}