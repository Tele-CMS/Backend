using HC.Common.HC.Common;
using HC.Patient.Model.Message;
using HC.Patient.Service.IServices.Message;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Message")]
    [ActionFilter]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// <Description> send message </Description>
        /// </summary>       
        /// <param name="messageComposeModel"></param>
        /// <returns></returns>
        [HttpPost("Compose")]
        public JsonResult Compose([FromBody]MessageComposeModel messageComposeModel)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.Compose(messageComposeModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get all inbox data
        /// </summary>
        /// <param name="forStaff"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetInboxData")]
        public JsonResult GetInboxData(bool forStaff, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetAllInboxData(forStaff, GetToken(HttpContext), fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        /// get all sent message data
        /// </summary>
        /// <param name="forStaff"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetSentMessageData")]
        public JsonResult GetSentMessageData(bool forStaff, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetSentMessageData(forStaff, GetToken(HttpContext), fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        /// <Description> Delete multiple messages from inbox  </Description>
        /// </summary>
        /// <param name="messageDeleteModel"></param>
        /// <returns></returns>
        [HttpPatch("DeleteInboxMessage")]
        public JsonResult DeleteInboxMessage([FromBody]MessageDeleteModel messageDeleteModel)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.DeleteInboxMessage(messageDeleteModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> Delete multiple messages from sent box  </Description>
        /// </summary>
        /// <param name="messageDeleteModel"></param>
        /// <returns></returns>
        [HttpPatch("DeleteSentMessage")]
        public JsonResult DeleteSentMessage([FromBody]MessageDeleteModel messageDeleteModel)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.DeleteSentMessage(messageDeleteModel, GetToken(HttpContext))));
        }


        /// <summary>
        /// <Description> change status of messages</Description>
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="Unread"></param>
        /// <returns></returns>
        [HttpPatch("ChangeMessageStatus")]
        public JsonResult ChangeMessageStatus(int MessageId, bool Unread)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.ChangeMessageStatus(MessageId, Unread, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> change favourite message status of messages</Description>
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="FromInbox"></param>
        /// <param name="IsFavourite"></param>
        /// <returns></returns>
        [HttpPatch("ChangeFavouriteMessageStatus")]
        public JsonResult ChangeFavouriteMessageStatus(int MessageId, bool FromInbox, bool IsFavourite)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.ChangeFavouriteMessageStatus(MessageId, FromInbox, IsFavourite, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> get messages detail by id </Description>
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        [HttpGet("GetMessageById")]
        public JsonResult GetMessageById(int MessageId)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetMessageById(MessageId, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> forward message </Description>
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        [HttpGet("ForwardMessages")]
        public JsonResult ForwardMessages(int MessageId)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.ForwardMessages(MessageId, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> reply message </Description>
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        [HttpGet("ReplyMessages")]
        public JsonResult ReplyMessages(int MessageId)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.ReplyMessages(MessageId, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> get all favourite messages </Description>
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetFavouriteMessageList")]
        public JsonResult GetFavouriteMessageList(string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetFavouriteMessageList(GetToken(HttpContext), fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        /// <Description> get all deleted messages </Description>
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetDeleteMessageList")]
        public JsonResult GetDeleteMessageList(string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetDeleteMessageList(GetToken(HttpContext), fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        /// <Description> download messages </Description> 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="name"></param>
        /// <param name="isStaff"></param>
        /// <returns></returns>
        [HttpGet("GetMessageDocument")]
        public IActionResult GetMessageDocument(int messageId, string name)
        {

            DownloadMessageDocumentModel response = _messageService.GetMessageDocument(messageId, name, GetToken(HttpContext));
            if (response != null)
            {
                return File(response.File, response.ApplicationType, response.Name);
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.DocumentNotExist,
                    StatusCode = (int)HttpStatusCodes.NotFound
                });
            }

        }

        /// <summary>
        /// <Description> users dropdown data for Client or staff </Description>
        /// </summary>
        /// <param name="isStaff"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet("UsersDropDown")]
        public JsonResult UsersDropDown(bool isStaff, string searchText)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.UsersDropDown(isStaff, searchText, GetToken(HttpContext))));
        }

        /// <summary>
        /// get all messages of thread
        /// </summary>
        /// <param name="parentMessageId"></param>
        /// <param name="forStaff"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetThreadMessages")]
        public JsonResult GetThreadMessages(int parentMessageId, bool forStaff, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetThreadMessages(parentMessageId,forStaff, GetToken(HttpContext), fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        /// <Description> get each message box count </Description>
        /// </summary>
        /// <param name="forStaff"></param>
        /// <returns></returns>
        [HttpGet("GetMessageCounts")]
        public JsonResult GetMessageCounts(bool forStaff)
        {
            return Json(_messageService.ExecuteFunctions(() => _messageService.GetMessageCounts(forStaff, GetToken(HttpContext))));
        }
    }
}