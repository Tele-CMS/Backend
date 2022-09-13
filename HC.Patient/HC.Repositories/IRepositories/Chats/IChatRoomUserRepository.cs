using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IChatRoomUserRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatRoomUser"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ChatRoomUser SaveNewChatRoomUser(ChatRoomUser chatRoomUser, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatRoomUserModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ChatRoomUser GetRoomInfoByRoomIdAndUserId(ChatRoomUserModel chatRoomUserModel, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<ChatRoomUser> GetRoomInfoByRoomId(int roomId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<T> GetConectionIdInParticularRoom<T>(int roomId, int userId, TokenModel tokenModel) where T : class, new();
    }
}
