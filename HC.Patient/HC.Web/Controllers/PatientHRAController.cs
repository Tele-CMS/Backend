using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientHRA")]
    [ActionFilter]
   // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class PatientHRAController : BaseController
    {
        private readonly IPatientHRAService _patientHRAService;
        public PatientHRAController(IPatientHRAService patientHRAService)
        {
            _patientHRAService = patientHRAService;
        }

        #region 
        ///// <summary>
        ///// Get Patient HRA Dropdown Autocomplete
        ///// </summary>
        ///// <param name="searchText"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetMemberHealthPlanForHRA")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult GetMemberHealthPlanForHRA(string searchText)
        //{
        //    return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.GetMemberHealthPlanForHRA(searchText, GetToken(HttpContext))));
        //}
        ///// <summary>
        ///// Get Patient HRA List
        ///// </summary>
        ///// <param name="filterModelForMemberHRA"></param>
        ///// <returns></returns>
        [HttpGet]
        [Route("GetMemberHRAListing")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetMemberHRAListing(FilterModelForMemberHRA filterModelForMemberHRA)
        {
            return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.GetMemberHRAListing(filterModelForMemberHRA, GetToken(HttpContext))));
        }
        ///// <summary>
        /////Bulk assign HRA documents to member
        ///// </summary>
        ///// <param name="filterModelForMemberHRA"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("BulkAssignHRA")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult BulkAssignHRA([FromBody]FilterModelForMemberHRA filterModelForMemberHRA)
        //{
        //    return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.BulkAssignHRA(filterModelForMemberHRA, GetToken(HttpContext))));
        //}
        ///// <summary>
        /////Bulk assign HRA documents to member
        ///// </summary>
        ///// <param name="filterModelForMemberHRA"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("BulkUpdateHRA")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult BulkUpdateHRA([FromBody]FilterModelForMemberHRA filterModelForMemberHRA)
        //{
        //    return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.BulkUpdateHRA(filterModelForMemberHRA, GetToken(HttpContext))));
        //}
        ///// <summary>
        ///// Get Bulk Patient HRA Data
        ///// </summary>
        ///// <param name="patDocIdArray"></param>
        ///// <returns></returns>
        [HttpGet]
        [Route("GetPatientHRAData")]
        [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientHRAData(string patDocIdArray)
        {
            return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.GetPatientHRAData(patDocIdArray, GetToken(HttpContext))));
        }
        ///// <summary>
        ///// Update Bulk Patient HRA Data
        ///// </summary>
        ///// <param name="patientHRAModel"></param>
        ///// <returns></returns>
        [HttpPost]
        [Route("UpdatePatientHRAData")]
       // [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult UpdatePatientHRAData([FromBody]List<PatientHRAModel> patientHRAModel)
        {
            return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.UpdatePatientHRAData(patientHRAModel, GetToken(HttpContext))));
        }
        ///// <summary>
        ///// View individual summary report of member
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        [Route("PrintIndividualSummaryReport")]
     //   [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public IActionResult PrintIndividualSummaryReport(int patientDocumentId, int patientId)
        {
            MemoryStream tempStream = null;
            tempStream = _patientHRAService.ExecuteFunctions(() => _patientHRAService.PrintIndividualSummaryReport(patientDocumentId, patientId, GetToken(HttpContext)));
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Individual Report");
        }
        ///// <summary>
        ///// View executive summary report of member
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("PrintExecutiveSummaryReport")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult PrintExecutiveSummaryReport(DateTime fromDate, DateTime toDate, int documentId)
        //{
        //    MemoryStream tempStream = null;
        //    tempStream = _patientHRAService.ExecuteFunctions(() => _patientHRAService.PrintExecutiveSummaryReport(fromDate, toDate, documentId, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Executive Report");
        //}
        ///// <summary>
        ///// View assessment form for member HRA
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("PrintHRAAssessment")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult PrintHRAAssessment(int? patientDocumentId, int? patientId, int documentId)
        //{
        //    MemoryStream tempStream = null;
        //    tempStream = _patientHRAService.ExecuteFunctions(() => _patientHRAService.PrintHRAAssessment(patientDocumentId, patientId, documentId, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Assessment Form");
        //}

        ///// <summary>
        ///// Get Email templates for DD
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetEmailTemplatesForDD")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult GetEmailTemplatesForDD()
        //{
        //    return Json(_patientHRAService.ExecuteFunctions<JsonModel>(() => _patientHRAService.GetEmailTemplatesForDD(GetToken(HttpContext))));
        //}

        //[HttpGet]
        //[Route("ExportMemberHRAassessmentToExcel")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult ExportMemberHRAassessmentToExcel(FilterModelForMemberHRA filterModelForMemberHRA)
        //{
        //    MemoryStream tempStream = _patientHRAService.ExecuteFunctions(() => _patientHRAService.ExportMemberHRAassessmentToExcel(filterModelForMemberHRA, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/vnd.ms-excel", "Test File");
        //}
        //[HttpGet]
        //[Route("PrintMemberHRAPDF")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult PrintMemberHRAPDF(FilterModelForMemberHRA filterModelForMemberHRA)
        //{
        //    MemoryStream tempStream = null;
        //    tempStream = _patientHRAService.ExecuteFunctions(() => _patientHRAService.PrintMemberHRAPDF(filterModelForMemberHRA, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/vnd.ms-excel", "Task list");
        //}
        #endregion
    }
}