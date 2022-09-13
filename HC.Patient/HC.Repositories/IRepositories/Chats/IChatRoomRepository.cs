using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Repositories.IRepositories
{
   public interface IChatRoomRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ChatRoom SaveNewChatRoom(ChatRoom chatRoom, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ChatRoom GetChatRoomByName(string roomName, TokenModel tokenModel);
        string GetChatRoomNameById(int roomId);
    }
}
