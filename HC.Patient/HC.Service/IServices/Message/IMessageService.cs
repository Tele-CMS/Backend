using HC.Model;
using HC.Patient.Model.Message;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices.Message
{
    public interface IMessageService : IBaseService
    {
        JsonModel Compose(MessageComposeModel messageComposeModel, TokenModel token);
        JsonModel GetAllInboxData(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        JsonModel GetSentMessageData(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        JsonModel DeleteInboxMessage(MessageDeleteModel messageDeleteModel, TokenModel token);
        JsonModel DeleteSentMessage(MessageDeleteModel messageDeleteModel, TokenModel token);
        JsonModel ChangeMessageStatus(int MessageId, bool Unread, TokenModel token);
        JsonModel ChangeFavouriteMessageStatus(int MessageId, bool FromInbox, bool IsFavourite, TokenModel token);
        JsonModel GetMessageById(int MessageId, TokenModel token);
        JsonModel ForwardMessages(int MessageId, TokenModel token);
        JsonModel ReplyMessages(int MessageId, TokenModel token);
        JsonModel GetFavouriteMessageList(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        JsonModel GetDeleteMessageList(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        DownloadMessageDocumentModel GetMessageDocument(int messageId, string name, TokenModel token);
        JsonModel UsersDropDown(bool isStaff, string searchText, TokenModel token);
        JsonModel GetThreadMessages(int parentMessageId, bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        JsonModel GetMessageCounts(bool forStaff, TokenModel token);
        MessagesInfoFromSignalRModel GetMessagesInfoFromSignalR(bool forStaff, HttpContext httpContext);
    }
}
