using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Filters;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IO;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsAlert")]
    [ActionFilter]
  //  [Authorize(Roles = "ADMIN, STAFF")]
    public class PatientsAlertController : BaseController
    {
        private readonly IPatientAlertService _patientAlertService;
        private readonly IChatService _chatService;
        private readonly ITokenService _tokenService;
        private readonly IHubContext<ChatHub> hubContext;
       private readonly IPatientReminderService _patientReminderService;

        public PatientsAlertController(IPatientAlertService patientAlertService, IChatService chatService, ITokenService tokenService, IHubContext<ChatHub> hub
            ,
            IPatientReminderService patientReminderService
            )
        {
            _patientAlertService = patientAlertService;
            _chatService = chatService;
            _tokenService = tokenService;
            hubContext = hub;
            _patientReminderService = patientReminderService;
        }

        /// <summary>
        /// Description : get listing of patient alerts
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPatientAlerts")]
        public JsonResult GetPatientAlerts(PatientAlertFilterModel patientFilterModel)
        {
            return Json(_patientAlertService.ExecuteFunctions(()=> _patientAlertService.GetPatientAlerts(patientFilterModel, GetToken(HttpContext))));
        }

        //[HttpPost]
        //[Route("SendBulkMessage")]
        //public IActionResult SendBulkMessage([FromBody] PatientAlertFilterModel patientFilterModel)
        //{
        //    TokenModel token = GetToken(HttpContext);
        //    SignalRNotificationForBulkMessageModel signalRNotification = _patientAlertService.ExecuteFunctions(() => _patientAlertService.SendBulkMessagePatientAlerts(patientFilterModel, token));
        //    if (signalRNotification != null && signalRNotification.Chat != null && signalRNotification.Chat.Count > 0)
        //    {
        //        foreach (var chat in signalRNotification.Chat)
        //        {
        //            chat.Message = patientFilterModel.Message;
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
        //public IActionResult SendBulkEmail([FromBody] PatientAlertFilterModel filterModel)
        //{
        //    TokenModel token = GetToken(HttpContext);
        //    SignalRNotificationForBulkEmailModel signalRNotification = _patientAlertService.ExecuteFunctions(() => _patientAlertService.SendBulkEmailPatientAlerts(filterModel, GetToken(HttpContext)));
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

        ///<summary>
        ///Save reminder for Task page(client-alerts-listing)
        ///</summary>
        [HttpPost]
        [Route("SaveAlertsReminder")]
        public JsonResult SaveAlertsReminder([FromBody] AlertReminderModel alertReminderModel)
        {
            return Json(_patientReminderService.ExecuteFunctions<JsonModel>(() => _patientReminderService.SaveReminder(alertReminderModel, GetToken(HttpContext))));
        }
    }
}