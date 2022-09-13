using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Eligibility;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.PatientEncounters;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Filters;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Route("patient-encounter")]
    [ActionFilter]
    //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class PatientEncounterController : BaseController
    {
        private readonly IPatientCommonService _patientCommonService;
        private readonly IPatientEncounterService _patientEncounterService;
        private readonly IChatService _chatService;
        private readonly ITokenService _tokenService;
        private readonly IHubContext<ChatHub> hubContext;

        #region Construtor of the class
        public PatientEncounterController(IPatientCommonService patientCommonService, IPatientEncounterService patientEncounterService,
            IChatService chatService, ITokenService tokenService, IHubContext<ChatHub> hub)
        {
            _patientEncounterService = patientEncounterService;
            _patientCommonService = patientCommonService;
            _chatService = chatService;
            _tokenService = tokenService;
            hubContext = hub;
        }
        #endregion

        /// <summary>
        /// Get patient encounter details whether it is saved or during add time to get data related to appointment type
        /// Added appointment details to remove the data flow from scheduler to encounter screen in front end
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="encounterId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounterDetails/{appointmentId}/{encounterId}")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientEncounterDetails(int appointmentId, int encounterId, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientEncounterDetails(appointmentId, encounterId, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get patient non billable encounter details whether it is saved or during add time to get data related to appointment type
        /// Added appointment details to remove the data flow from scheduler to encounter screen in front end
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="encounterId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpGet("GetPatientNonBillableEncounterDetails/{appointmentId}/{encounterId}")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientNonBillableEncounterDetails(int appointmentId, int encounterId, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientNonBillableEncounterDetails(appointmentId, encounterId, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save patient encounter
        /// </summary>
        /// <param name="requestObj"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost("SavePatientEncounter")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SavePatientEncounter([FromBody] PatientEncounterModel requestObj, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounter(requestObj, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save patient non billable encounter
        /// </summary>
        /// <param name="requestObj"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost("SavePatientNonBillableEncounter")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SavePatientNonBillableEncounter([FromBody] PatientEncounterModel requestObj, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientNonBillableEncounter(requestObj, isAdmin, GetToken(HttpContext))));
        }
        /// <summary>
        /// track encounter logs on add/edit click actions
        /// </summary>
        /// <param name="encounterClickLogsModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TrackEncounterAddUpdateClicks")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult TrackEncounterAddUpdateClicks([FromBody] EncounterClickLogsModel encounterClickLogsModel)
        {
            // return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.TrackEncounterAddUpdateClicks(encounterClickLogsModel, GetToken(HttpContext))));
            return Json(_patientEncounterService.ExecuteFunctions<JsonModel>(() => _patientEncounterService.TrackEncounterAddUpdateClicks(encounterClickLogsModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Patient encounters
        /// </summary>
        /// <param name="patientID"></param>
        /// <param name="appointmentType"></param>
        /// <param name="staffName"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounter")]
        //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<PatientEncounterModel> response = _patientEncounterService.GetPatientEncounter(patientID, appointmentType, staffName, status, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext));
            if (response != null && response.Count > 0)
            {
                return Json(new
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = response[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(response[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                });
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Unprocessable Entity)
                });
            }
        }
        /// <summary>
        /// To get All patient Encounter list
        /// </summary>
        /// <param name="filtermodel"></param>
        /// <returns></returns>
        [HttpGet("GetAllPatientEncounter")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetAllPatientEncounter(EncounterFilterModel filtermodel)
        {
            List<PatientEncounterModel> response = _patientEncounterService.GetAllPatientEncounter(filtermodel, GetToken(HttpContext));
            if (response != null && response.Count > 0)
            {
                return Json(new
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = response[0].TotalRecords,
                        CurrentPage = filtermodel.pageNumber,
                        PageSize = filtermodel.pageSize,
                        DefaultPageSize = filtermodel.pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(response[0].TotalRecords / filtermodel.pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                });
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Unprocessable Entity)
                });
            }
        }

        [HttpPost]
        [Route("SaveEncounterSignature")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SaveEncounterSignature([FromBody] EncounterSignatureModel encounterSignatureModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SaveEncounterSignature(encounterSignatureModel)));
        }

        /// <summary>
        /// This method will download encounter in pdf format
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet("DownloadEncounter")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public ActionResult DownloadEncounter(int encounterId)
        {
            MemoryStream ms = _patientEncounterService.DownloadEncounter(encounterId, GetToken(HttpContext));
            return File(ms, "application/pdf", "EncounterFile.pdf");
        }

        /// <summary>
        /// This method will save Patient Encounter Template form data
        /// </summary>
        /// <param name="patientEncounterTemplateModel"></param>
        /// <returns></returns>
        [HttpPost("SaveEncounterTemplateData")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SaveEncounterTemplateData([FromBody] PatientEncounterTemplateModel patientEncounterTemplateModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounterTemplateData(patientEncounterTemplateModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// This method will get Patient Encounter Template form with data
        /// </summary>
        /// <param name="patientEncounterId"></param>
        /// <param name="masterTemplateId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounterTemplateData")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientEncounterTemplateData(patientEncounterId, masterTemplateId, GetToken(HttpContext))));
        }
        /// <summary>
        /// delete encounter
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpPatch("DeleteEncounter")]
        public JsonResult DeleteEncounter(int encounterId)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.DeleteEncounter(encounterId, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Encounter Summary print
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PrintEncounterSummaryDetails")]
        //  [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public IActionResult PrintEncounterSummaryDetails(int encounterId, string checkListIds, string portalkey)
        {
            MemoryStream tempStream = null;
            tempStream = _patientEncounterService.ExecuteFunctions(() => _patientEncounterService.PrintEncounterSummaryDetails(encounterId, checkListIds, portalkey, GetToken(HttpContext)));
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Encounter Summary");
        }
        /// <summary>
        /// Get Encounter Summary on Modal Pop Up for confirmation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEncounterSummaryDetailsForPDF")]
        // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetEncounterSummaryDetailsForPDF(int encounterId)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetEncounterSummaryDetailsForPDF(encounterId, GetToken(HttpContext))));
        }
        /// <summary>
        /// Send Email with encounter summary attachment
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("EmailEncounterSummary")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult EmailEncounterSummary(int encounterId, string checkListIds)
        //{
        //    return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.EmailEncounterSummary(encounterId,checkListIds, GetToken(HttpContext))));
        //}
        /// <summary>
        /// Discard the current encounter on going to Save.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DiscardEncounterChanges")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult DiscardEncounterChanges(int encounterId)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.DiscardEncounterChanges(encounterId, GetToken(HttpContext))));
        }
        //[HttpPost]
        //[Route("SendBulkMessage")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult SendBulkMessage([FromBody] EncounterFilterModel filterModel)
        //{
        //    TokenModel token = GetToken(HttpContext);
        //    SignalRNotificationForBulkMessageModel signalRNotification = _patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SendBulkMessageForEncounters(filterModel, token));
        //    if (signalRNotification != null && signalRNotification.Chat != null && signalRNotification.Chat.Count > 0)
        //    {
        //        foreach (var chat in signalRNotification.Chat)
        //        {
        //            chat.Message = filterModel.Message;
        //            string connectionId = _chatService.GetConnectionId(chat.ToUserId);
        //            JsonModel notificationModel = _tokenService.GetChatAndNotificationCount(chat.ToUserId, token);

        //            if (!string.IsNullOrEmpty(connectionId))
        //            {
        //                hubContext.Clients.Client(connectionId).SendAsync("MobileNotificationResponse", notificationModel);
        //                hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", chat);
        //            }
        //        }
        //    }

        //    return File(signalRNotification != null ? signalRNotification.MemoryStream : new MemoryStream(), "application/vnd.ms-excel", "Bulk Email Status");
        //}
        //[HttpPost]
        //[Route("SendBulkEmail")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult SendBulkEmail([FromBody] EncounterFilterModel filterModel)
        //{
        //    TokenModel token = GetToken(HttpContext);
        //    SignalRNotificationForBulkEmailModel signalRNotification = _patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SendBulkEmailForEncounters(filterModel, GetToken(HttpContext)));
        //    if (signalRNotification != null && signalRNotification.BulkMessageAndPatientMapping != null && signalRNotification.BulkMessageAndPatientMapping.Count > 0)
        //    {
        //        foreach (var data in signalRNotification.BulkMessageAndPatientMapping)
        //        {
        //            if (data.UserId != null)
        //            {
        //                string connectionId = _chatService.GetConnectionId(data.UserId.Value);
        //                JsonModel notificationModel = _tokenService.GetChatAndNotificationCount(data.UserId.Value, token);

        //                if (!string.IsNullOrEmpty(connectionId))
        //                {
        //                    hubContext.Clients.Client(connectionId).SendAsync("MobileNotificationResponse", notificationModel);
        //                }
        //            }
        //        }
        //    }
        //    return File(signalRNotification != null ? signalRNotification.MemoryStream : new MemoryStream(), "application/vnd.ms-excel", "Bulk Email Status");
        //}
        /// <summary>
        /// Export Excel for Encounters
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("ExportEncounters")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult ExportEncounters(EncounterFilterModel filterModel)
        //{
        //    MemoryStream tempStream = _patientEncounterService.ExecuteFunctions(() => _patientEncounterService.ExportEncounters(filterModel, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/vnd.ms-excel", "Bulk Email Status");
        //}
        /// <summary>
        /// encounter data for printing PDF
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("PrintEncountersPDF")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult PrintEncountersPDF(EncounterFilterModel filterModel)
        //{
        //    MemoryStream tempStream = null;
        //    tempStream = _patientEncounterService.ExecuteFunctions(() => _patientEncounterService.PrintEncountersPDF(filterModel, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Encounter Report");
        //}

        /// <summary>
        /// Save patient encounter Notes
        /// </summary>
        /// <returns></returns>
        [HttpPost("SavePatientEncounterNotes")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SavePatientEncounterNotes([FromBody] PatientEncounterNotesModel patientEncounterNotesModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounterNotes(patientEncounterNotesModel, GetToken(HttpContext))));
        }

        //[HttpGet("GetPatientDiagnosisDetails/{patientId}")]
        [HttpGet("GetPatientDiagnosisDetails")]
        public JsonResult GetPatientDiagnosisDetails(int patientId, FilterModel filterModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientDiagnosisCodes(patientId, filterModel, GetToken(HttpContext))));
        }
        [HttpPost("SavePatientEncounterSOAP")]
        public JsonResult SavePatientEncounterSOAP([FromBody] PatientEncounterModel requestObj, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounterSOAP(requestObj, isAdmin, GetToken(HttpContext))));
        }

        [HttpPost("CheckPatientEligibility")]
        public JsonResult CheckPatientEligibility([FromBody] PatientEligibilityRequestModel eligibilityRequestModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.CheckPatientEligibility(eligibilityRequestModel, GetToken(HttpContext))));
        }

        [HttpPost("SendBBIntructionsMail")]
        public JsonResult SendBBIntructionsMail([FromBody] BlueButtonModel blueButtonModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SendBBIntructionsMail(blueButtonModel, GetToken(HttpContext))));
        }
    }
}

