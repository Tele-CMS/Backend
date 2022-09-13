using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HC.Common;
using HC.Model;
using HC.Patient.Service.IServices.Claim;
using HC.Patient.Service.IServices.EDI;

namespace HC.Patient.Web.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("EDIParser")]
    public class EDIParserController : BaseController
    {   
        private readonly IEDI835ParserService _ediParserService;
        #region Construtor of the class
        public EDIParserController(IEDI835ParserService ediParserService)
        {
            _ediParserService = ediParserService;            
        }
        #endregion

        [HttpGet]
        [Route("ReadEDI835")]
        public async Task<IActionResult> ReadEDI835()
        {
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.ReadEDI835(GetToken(HttpContext)))));
        }

        [HttpGet]
        [Route("Get835SegmentData")]
        public async Task<IActionResult> Get835SegmentData()
        {
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.Get835SegmentData(GetToken(HttpContext)))));
        }

        [HttpGet]
        [Route("GetProcessedClaims")]
        public async Task<IActionResult> GetProcessedClaims(int pageNumber=1, int pageSize=10, string claimId = "", string patientIds = "", string adjustmentsGroupCodes = "", string fromDate = "", string toDate = "", string payerName = "", string sortColumn="", string sortOrder="")
        {
            int? claimIdTrim = !string.IsNullOrEmpty(claimId) && (claimId.ToLower().IndexOf("cl") > -1) ? Convert.ToInt32(claimId.Remove(0, 2)) : (int?)null;
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.GetProcessedClaims(pageNumber, pageSize, claimIdTrim, patientIds, adjustmentsGroupCodes, fromDate, toDate, payerName, sortColumn, sortOrder, GetToken(HttpContext)))));
        }
        [HttpPatch]
        [Route("Apply835PaymentsToPatientAccount")]
        public async Task<IActionResult> Apply835PaymentsToPatientAccount(string responseClaimIds)
        {
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.Apply835PaymentsToPatientAccount(responseClaimIds, GetToken(HttpContext)))));
        }
        [HttpPatch]
        [Route("Apply835ServiceLinePaymentsToPatientAccount")]
        public async Task<IActionResult> Apply835ServiceLinePaymentsToPatientAccount(string responseClaimServiceLineIds)
        {
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.Apply835ServiceLinePaymentsToPatientAccount(responseClaimServiceLineIds, GetToken(HttpContext)))));
        }

        [HttpPatch]
        [Route("Apply835ServiceLineAdjustmentsToPatientAccount")]
        public async Task<IActionResult> Apply835ServiceLineAdjustmentsToPatientAccount(string responseClaimServiceLineAdjIds)
        {
            return Json(await Task.Run(() =>_ediParserService.ExecuteFunctions<JsonModel>(()=> _ediParserService.Apply835ServiceLineAdjustmentsToPatientAccount(responseClaimServiceLineAdjIds, GetToken(HttpContext)))));
        }
    }
}