using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Common;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : BaseController
    {
        private readonly ITokenService _tokenService;
        public NotificationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("GetHeaderNotification")]
        public JsonResult GetHeaderNotification()
        {
            NotificationModel notificationModel = _tokenService.GetLoginNotification(GetToken(HttpContext));

            return Json (new JsonModel() { data = notificationModel, Message = StatusMessage.FetchMessage, StatusCode = (int)HttpStatusCode.OK });
        }
        /// <summary>
        /// Get Notification Details By Id
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNotificationDetailsById")]
        public JsonResult GetNotificationDetailsById(int notificationId)
        {
            return Json(_tokenService.ExecuteFunctions<JsonModel>(() => _tokenService.GetNotificationDetailsById(notificationId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Notification List
        /// </summary>
        /// <param name="commonFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNotificationList")]
        public JsonResult GetNotificationList(CommonFilterModel commonFilterModel)
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.GetNotificationList(commonFilterModel, GetToken(HttpContext))));

        }


        /// <summary>
        /// Read Notification
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        [Route("ReadNotification")]
        public JsonResult ReadNotification()
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.ReadNotification(GetToken(HttpContext))));
        }


        /// <summary>
        /// Read Chat And All Notification
        /// </summary>
        /// <param name="patientID"></param>
        /// <param name="isChatMessage"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("ReadChatAndAllNotification")]
        public JsonResult ReadChatAndAllNotification(int patientID, bool isChatMessage)
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.ReadChatAndAllNotification(patientID, isChatMessage, GetToken(HttpContext))));
        }

        [HttpPatch]
        [Route("ReadChatAndAllNotificationForPatient")]
        public JsonResult ReadChatAndAllNotificationForPatient(int patientID)
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.ReadChatAndAllNotificationForPatient(patientID, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Patient Notification List
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientNotificationList")]
        public JsonResult GetPatientNotificationList(int patientId, int pageNumber = 1, int pageSize = 10)
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.GetPatientNotificationList(patientId, pageNumber, pageSize, GetToken(HttpContext))));
        }


        [HttpGet]
        [Route("GetPatientPortalNotificationList")]
        public JsonResult GetPatientPortalNotificationList(int patientId, int pageNumber = 1, int pageSize = 10)
        {
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.GetPatientPortalNotificationList(patientId, pageNumber, pageSize, GetToken(HttpContext))));

        }
        /// <summary>
        /// Get Mobile Dashboar Info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMobileDashboarInfo")]
        public JsonResult GetMobileDashboarInfo()
        {
            TokenModel token = GetToken(HttpContext);
            return Json(_tokenService.ExecuteFunctions(() => _tokenService.GetChatAndNotificationCount(token.UserID, token)));
        }
    }
}