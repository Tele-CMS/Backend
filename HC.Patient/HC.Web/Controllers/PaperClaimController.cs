using HC.Model;
using HC.Patient.Service.IServices.Claim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace HC.Patient.Web.Controllers
{
    [AllowAnonymous]    
    [Produces("application/json")]
    [Route("api/PaperClaim")]
    public class PaperClaimController : BaseController
    {
        private IPaperClaimService _paperClaimService;        
        #region Construtor of the class
        public PaperClaimController(IPaperClaimService paperClaimService)
        {        
            _paperClaimService = paperClaimService;
        }
        #endregion

        [HttpGet]
        [Route("PaperClaim")]
        public IActionResult PaperClaim(int claimId, int patientInsuranceId,bool markSubmitted=false,int printFormat=1)
        {
            MemoryStream tempStream = null;
            tempStream = _paperClaimService.GeneratePaperClaim(claimId, patientInsuranceId,markSubmitted,printFormat,GetToken(HttpContext));
            return File((tempStream!=null?tempStream:new MemoryStream()), "application/pdf",Convert.ToString(claimId)+"-"+"PaperClaimFile");
        }
        [HttpGet]
        [Route("PaperClaim_Secondary")]
        public IActionResult PaperClaim_Secondary(int claimId, int patientInsuranceId, int printFormat = 1)
        {
            MemoryStream tempStream = null;
            tempStream = _paperClaimService.GeneratePaperClaim_Secondary(claimId, patientInsuranceId, printFormat, GetToken(HttpContext));
            return File((tempStream != null ? tempStream : new MemoryStream()), "application/pdf", Convert.ToString(claimId) + "-" + "PaperClaimFile");
        }

        [HttpGet]
        [Route("BatchPaperClaim")]
        public IActionResult BatchPaperClaim(string claimIds, string payerPreference, bool markSubmitted = false, int printFormat = 1)
        {
            MemoryStream tempStream = null;
            tempStream = _paperClaimService.GenerateBatchPaperClaims(claimIds, payerPreference, markSubmitted, printFormat, GetToken(HttpContext));
            return File((tempStream != null ? tempStream : new MemoryStream()), "application/pdf", "Batch-" + "PaperClaimFile");
        }

        [HttpGet]
        [Route("BatchPaperClaim_Clubbed")]
        public IActionResult BatchPaperClaim_Clubbed(string claimIds, string payerPreference, bool markSubmitted = false, int printFormat = 1)
        {
            MemoryStream tempStream = null;
            tempStream = _paperClaimService.GenerateBatchPaperClaims_Clubbed(claimIds, payerPreference, markSubmitted, printFormat, GetToken(HttpContext));
            return File((tempStream != null ? tempStream : new MemoryStream()), "application/pdf", "Batch-" + "PaperClaimFile");
        }

    }
}