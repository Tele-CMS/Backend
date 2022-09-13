using HC.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Model.Common;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HC.Patient.Service.IServices.Chats
{
    public interface IChatService : IBaseService
    {
        Task<JsonModel> ChatConnectedUser(ChatConnectedUserModel chatConnectedUserModel, TokenModel tokenModel);
        string GetConnectionId(int UserID);
        bool SignOutChat(int UserID);
        JsonModel SaveChat(ChatModel chatModel, TokenModel tokenModel);
        JsonModel GetChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="userId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetChatRoomId(int room, int userId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        Task<JsonModel> SendFileInChat(IFormFileCollection files, int roomId, int userId, TokenModel tokenModel);
        void SendChatPushNotification(PushNotificationChatMessageModel pushNotificationChatMessage, string message, TokenModel token);
        Task<JsonModel> SaveCareChat(CareChatModel chatModel, TokenModel tokenModel);
        JsonModel GetCareChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel);
    }
}
