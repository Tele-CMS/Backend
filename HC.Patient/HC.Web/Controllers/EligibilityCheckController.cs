using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Service.IServices.EDI;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("EligibilityCheck")]
    [ActionFilter]
    public class EligibilityCheckController : BaseController
    {
        private IEDI270GenerationService _edi270RequestService;
        private IEDI999ParserService _edi999AcknowledgementService;
        private IEDI271ParserService _edi271ParserService;
        private JsonModel response;
        public EligibilityCheckController(IEDI270GenerationService edi270RequestService, IEDI999ParserService edi999AcknowledgementService, IEDI271ParserService edi271ParserService)
        {
            _edi270RequestService = edi270RequestService;
            _edi999AcknowledgementService = edi999AcknowledgementService;
            _edi271ParserService = edi271ParserService;
            response = new JsonModel();
        }
        [HttpGet]
        [Route("Download270")]
        public async Task<IActionResult> Download270(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds)
        {
            string ediText = string.Empty;
            byte[] byteArray = null;
            var stream = new MemoryStream();
            ediText = await Task.Run(() => _edi270RequestService.ExecuteFunctions(() => _edi270RequestService.Download270(patientId, patientInsuranceId, serviceTypeCodeIds, serviceCodeIds, GetToken(HttpContext))));
            if (ediText != string.Empty)
            {
                byteArray = Encoding.ASCII.GetBytes(ediText);
                stream = new MemoryStream(byteArray);
            }
            return File(stream, "text/plain", patientId.ToString()+ "-EligibilityEnquiry.X12");
        }


        [HttpGet]
        [Route("Generate270EligibilityRequestFile")]
        public async Task<IActionResult> Generate270EligibilityRequestFile(int patientId, int patientInsuranceId,string serviceTypeCodeIds,string serviceCodeIds)
        {
            response=await Task.Run(() => _edi270RequestService.ExecuteFunctions<JsonModel>(() => _edi270RequestService.Generate270EligibilityRequestFile(patientId, patientInsuranceId,serviceTypeCodeIds,serviceCodeIds, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpGet]
        [Route("ReadEDI999")]
        public async Task<IActionResult> ReadEDI999()
        {
            return Json(await Task.Run(() => _edi999AcknowledgementService.ExecuteFunctions<JsonModel>(() => _edi999AcknowledgementService.ReadEDI999(GetToken(HttpContext)))));
        }

        [HttpGet]
        [Route("ReadEDI271")]
        public async Task<IActionResult> ReadEDI271()
        {
            return Json(await Task.Run(() => _edi271ParserService.ExecuteFunctions<JsonModel>(() => _edi271ParserService.ReadEDI271(GetToken(HttpContext)))));
        }
        /// <summary>
        /// <Description> this method is used to get eligibility enquiry service types  </Description>        
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEligibilityEnquiryServiceCodes")]
        public JsonResult GetEligibilityEnquiryServiceCodes()
        {
            return Json(_edi270RequestService.ExecuteFunctions(() => _edi270RequestService.GetEligibilityEnquiryServiceCodes(GetToken(HttpContext))));
        }
    }
}