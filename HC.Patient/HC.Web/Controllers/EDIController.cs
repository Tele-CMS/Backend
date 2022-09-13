using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//using Audit.WebApi;
using HC.Common;
using HC.Model;
using System.IO;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Service.IServices.Claim;
using System.Text;
using HC.Patient.Model.Claim;
using HC.Patient.Web.Filters;
using HC.Patient.Service.IServices.EDI;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("EDI")]
    [ActionFilter]
    public class EDIController : BaseController
    {
        private readonly IEDI837GenerationService _ediService;
        private JsonModel response;
        #region Construtor of the class
        public EDIController(IEDI837GenerationService ediService)
        {
            _ediService = ediService;
        }
        #endregion

        [HttpGet]
        [Route("GenerateSingleEDI837")]
        public async Task<IActionResult> GenerateSingleEDI837(int claimId, int patientInsuranceId)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateSingleEDI837(claimId, patientInsuranceId, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GenerateBatchEDI837")]
        public async Task<IActionResult> GenerateBatchEDI837(string claimIds)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateBatchEDI837(claimIds, GetToken(HttpContext))));
            return Json(response);
        }


        [HttpGet]
        [Route("DownloadSingleEDI837")]
        public async Task<IActionResult> DownloadSingleEDI837(int claimId, int patientInsuranceId)
        {
            string ediText = string.Empty;
            byte[] byteArray = null;
            var stream = new MemoryStream();
            TokenModel token = GetToken(HttpContext);
            int locationId = token.LocationID;
            if (!ReferenceEquals(token, null))
            {
                ediText = await Task.Run(() => _ediService.ExecuteFunctions(() => _ediService.DownloadSingleEDI837(claimId, patientInsuranceId, locationId, token)));
                if (ediText != string.Empty)
                {
                    byteArray = Encoding.ASCII.GetBytes(ediText);
                    stream = new MemoryStream(byteArray);
                }
                return File(stream, "text/plain", "OATEST0101-419-101011110022.X12");
            }
            else
            {
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.InvalidToken,
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                };
                return Json(response);
            }
        }


        [HttpGet]
        [Route("DownloadBatchEDI837")]
        public async Task<IActionResult> DownloadBatchEDI837(string claimIds)
        {
            string ediText = string.Empty;
            byte[] byteArray = null;
            var stream = new MemoryStream();
            TokenModel token = GetToken(HttpContext);
            int locationId = token.LocationID;
            if (!ReferenceEquals(token, null))
            {
                ediText = await Task.Run(() => _ediService.ExecuteFunctions(() => _ediService.DownloadBatchEDI837(claimIds, locationId, "", token)));
                if (ediText != string.Empty)
                {
                    byteArray = Encoding.ASCII.GetBytes(ediText);
                    stream = new MemoryStream(byteArray);
                }
                return File(stream, "text/plain", "OATEST0101-419-101011110022.X12");
            }
            else
            {
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.InvalidToken,
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                };
                return Json(response);
            }
        }

        [HttpGet]
        [Route("SubmitClaimsForNonEDI")]
        public async Task<IActionResult> SubmitClaimsForNonEDI(string claimIds)
        {
            try
            {
                TokenModel token = GetToken(HttpContext);
                int locationId = token.LocationID;
                response = await Task.Run(() => _ediService.ExecuteFunctions(() => _ediService.SubmitClaimsForNonEDI(claimIds, token)));
                return Json(response);
            }
            catch
            {
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
                return Json(response);
            }
        }

        [HttpGet]
        [Route("ResubmitClaim")]
        public async Task<IActionResult> ResubmitEDI837(int claimId, int patientInsuranceId, string resubmissionReason = "OR", string payerControlReferenceNumber = "")
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.ResubmitClaim(claimId, patientInsuranceId, resubmissionReason, payerControlReferenceNumber, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GenerateSingleEDI837_Secondary")]
        public async Task<IActionResult> GenerateSingleEDI837_Secondary(int claimId, int patientInsuranceId)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateSingleEDI837_Secondary(claimId, patientInsuranceId, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GenerateBatchEDI837_Secondary")]
        public async Task<IActionResult> GenerateBatchEDI837_Secondary(string claimIds)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateBatchEDI837_Secondary(claimIds, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpPost]
        [Route("ResubmitBatchEDI837")]
        public async Task<IActionResult> ResubmitBatchEDI837([FromBody]List<ResubmitInputModel> claimInfo)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.ResubmitBatchClaim(claimInfo, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GetSubmittedClaimsBatch")]
        public JsonResult GetSubmittedClaimsBatch(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate)
        {
            return Json(_ediService.GetSubmittedClaimsBatch(pageNumber, pageSize, fromDate, toDate, GetToken(HttpContext)));
        }

        [HttpGet]
        [Route("GetSubmittedClaimsBatchDetails")]
        public JsonResult GetSubmittedClaimsBatchDetails(string claim837ClaimIds)
        {
            return Json(_ediService.GetSubmittedClaimsBatchDetails(claim837ClaimIds, GetToken(HttpContext)));
        }
        [HttpGet]
        [Route("GenerateSingleEDI837_Tertiary")]
        public async Task<IActionResult> GenerateSingleEDI837_Tertiary(int claimId, int patientInsuranceId)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateSingleEDI837_Tertiary(claimId, patientInsuranceId, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GenerateBatchEDI837_Tertiary")]
        public async Task<IActionResult> GenerateBatchEDI837_Tertiary(string claimIds)
        {
            response = await Task.Run(() => _ediService.ExecuteFunctions<JsonModel>(() => _ediService.GenerateBatchEDI837_Tertiary(claimIds, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpGet]
        [Route("GetEDIInfo")]
        public async Task<IActionResult> GetEDIInfo(int claimId, int patientInsuranceId)
        {
         
            return Json(_ediService.GetEDIInfo(claimId, patientInsuranceId, GetToken(HttpContext)));
        }
    }
}