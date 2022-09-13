using HC.Model;
using HC.Patient.Model.Chat;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IChatRoomUserService : IBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatRoomUserModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveChatRoomUser(ChatRoomUserModel chatRoomUserModel, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetUserInChatRoom(int roomId, TokenModel tokenModel);
    }
}
