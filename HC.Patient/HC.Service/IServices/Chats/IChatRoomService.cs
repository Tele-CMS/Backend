using HC.Model;
using HC.Patient.Model.Chat;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IChatRoomService : IBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatRoomModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveChatRoomAsync(ChatRoomModel chatRoomModel, TokenModel tokenModel);
    }
}
