using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.PatientDiseaseManagementProgram;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Filters;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientDiseaseManagementProgram")]
    [ActionFilter]
   // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class PatientDiseaseManagementProgramController : BaseController
    {
        private readonly IPatientDiseaseManagementProgramService _patientDiseaseManagementProgramService;       
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ITokenService _tokenService;
     //   private readonly IBulkMessageService _bulkMessageService;
        public PatientDiseaseManagementProgramController(IPatientDiseaseManagementProgramService patientDiseaseManagementProgramService, IChatService chatService, IHubContext<ChatHub> hubContext, ITokenService tokenService)
        {
            _patientDiseaseManagementProgramService = patientDiseaseManagementProgramService;
            _chatService = chatService;
            _hubContext = hubContext;
            _tokenService = tokenService;
        }

        /// <summary>
        /// This method will get patient DM program list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientDiseaseManagementProgramList")]
     //   [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetPatientDiseaseManagementProgramList(int patientId, FilterModel filterModel)
        {

            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.GetPatientDiseaseManagementProgramList(patientId, filterModel, GetToken(HttpContext))));
        }

        [HttpPatch]
        [Route("EnrollmentPatientInDiseaseManagementProgram")]
     //   [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult EnrollmentPatientInDiseaseManagementProgram(int patientDiseaseManagementProgramId, DateTime enrollmentDate, bool isEnrolled)
        {
            
            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.EnrollmentPatientInDiseaseManagementProgram(patientDiseaseManagementProgramId,enrollmentDate, isEnrolled, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description: post program Ids
        /// </summary>
        /// <param name="assignNewProgramModel"></param>
        /// <returns></returns>
        [HttpPost("AssignNewPrograms")]
//[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult AssignNewPrograms([FromBody]AssignNewProgramModel assignNewProgramModel)
        {
            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions(() => _patientDiseaseManagementProgramService.AssignNewPrograms(assignNewProgramModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// This method will get patient DM program details by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientDiseaseManagementProgramDetails")]
     //   [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientDiseaseManagementProgramDetails(int Id)
        {

            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.GetPatientDiseaseManagementProgramDetails(Id, GetToken(HttpContext))));
        }
        /// <summary>
        /// Method to delete the patient programs by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("deleteDiseaseManagementProgram")]
  //      [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult deleteDiseaseManagementProgram(int Id)
        {

            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.deleteDiseaseManagementProgram(Id, GetToken(HttpContext))));
        }

        // <summary>
        /// This method will get all patients DM programs list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPatientDiseaseManagementProgramsList")]
   //     [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetAllPatientDiseaseManagementProgramsList(ProgramsFilterModel filterModel)
        {

            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.GetAllPatientDiseaseManagementProgramsList(filterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get all patients from program enrollments screen filters then send message.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("SendBulkMessage")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult SendBulkMessage([FromBody] ProgramsFilterModel filterModel)
        //{
        //    TokenModel tokenModel = GetToken(HttpContext);
        //    SignalRNotificationForBulkMessageModel signalRNotification = _bulkMessageService.ExecuteFunctions(() => _bulkMessageService.SendBulkMessageProgramEnrollment(filterModel, tokenModel));
        //    if (signalRNotification != null && signalRNotification.Chat != null && signalRNotification.Chat.Count > 0)
        //    {
        //        foreach (var chat in signalRNotification.Chat)
        //        {
        //            chat.Message = filterModel.Message;
        //            string connectionId = _chatService.GetConnectionId(chat.ToUserId);
        //            JsonModel notificationModel = _tokenService.GetChatAndNotificationCount(chat.ToUserId, tokenModel);

        //            if (!string.IsNullOrEmpty(connectionId))
        //            {
        //                _hubContext.Clients.Client(connectionId).SendAsync("MobileNotificationResponse", notificationModel);
        //                _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", chat);
        //            }
        //        }
        //    }
        //    return File(signalRNotification != null ? signalRNotification.MemoryStream : new MemoryStream(), "application/vnd.ms-excel", "Bulk Message Status");
        //}
        /// <summary>
        /// Get all patients from care gap screen filters then send email.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("SendBulkEmail")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult SendBulkEmail([FromBody] ProgramsFilterModel filterModel)
        //{
        //    TokenModel tokenModel = GetToken(HttpContext);
        //    SignalRNotificationForBulkEmailModel signalRNotification = _bulkMessageService.ExecuteFunctions(() => _bulkMessageService.SendBulkEmailProgramEnrollment(filterModel, tokenModel));
        //    if (signalRNotification != null && signalRNotification.BulkMessageAndPatientMapping != null && signalRNotification.BulkMessageAndPatientMapping.Count > 0)
        //    {
        //        foreach (var data in signalRNotification.BulkMessageAndPatientMapping)
        //        {
        //            if (data.UserId != null)
        //            {
        //                string connectionId = _chatService.GetConnectionId(data.UserId.Value);
        //                JsonModel notificationModel = _tokenService.GetChatAndNotificationCount(data.UserId.Value, tokenModel);

        //                if (!string.IsNullOrEmpty(connectionId))
        //                {
        //                    _hubContext.Clients.Client(connectionId).SendAsync("MobileNotificationResponse", notificationModel);
        //                }
        //            }
        //        }
        //    }
        //    return File(signalRNotification != null ? signalRNotification.MemoryStream : new MemoryStream(), "application/vnd.ms-excel", "Bulk Email Status");



        //}
        /// <summary>
        /// Export Patient Disease ManagementData 
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("ExportPatientDiseaseManagementData")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        //public IActionResult ExportPatientDiseaseManagementData(ProgramsFilterModel filterModel)
        //{
        //    MemoryStream tempStream = _patientDiseaseManagementProgramService.ExecuteFunctions(() => _bulkMessageService.ExportPatientDiseaseManagementData(filterModel, GetToken(HttpContext)));
        //    return File(tempStream != null ? tempStream : new MemoryStream(), "application/vnd.ms-excel", "Test File");
        //}
        /// <summary>
        /// DMP enrollees patient data for printing PDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProgramsEnrollPatientsForPDF")]
      //  [Authorize(Roles = "ADMIN, STAFF")]
        public IActionResult GetProgramsEnrollPatientsForPDF(ProgramsFilterModel filterModel)
        {
            MemoryStream tempStream = null;
            tempStream = _patientDiseaseManagementProgramService.ExecuteFunctions(() => _patientDiseaseManagementProgramService.GetProgramsEnrollPatientsForPDF(filterModel, GetToken(HttpContext)));
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "DMP Patients Report");
        }

        [HttpGet]
        [Route("GetAllReportsHRAPrograms")]
        public JsonResult GetAllReportHRAPrograms(HRALogFilterModel filterModel)
        {

            return Json(_patientDiseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _patientDiseaseManagementProgramService.GetReportHRAProgram(filterModel, GetToken(HttpContext))));
        }

    }
}