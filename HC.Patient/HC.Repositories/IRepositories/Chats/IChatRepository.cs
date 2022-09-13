using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Entity;
using System.Linq;
using HC.Patient.Model.Chat;
using HC.Model;

namespace HC.Patient.Repositories.IRepositories.Chats
{
    public interface IChatRepository : IRepositoryBase<Chat>
    {
        IQueryable<T> GetChatHistory<T>(ChatParmModel chatParmModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetCareChatHistory<T>(ChatParmModel chatParmModel, TokenModel tokenModel) where T : class, new();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        Chat SaveChat(Chat chat);
    }
}
