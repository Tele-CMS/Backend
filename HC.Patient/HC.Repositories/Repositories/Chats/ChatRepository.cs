using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model.Chat;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Repositories;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Entity;

namespace HC.Patient.Repositories.Repositories.Chats
{
    public class ChatRepository : RepositoryBase<Entity.Chat>, IChatRepository
    {
        private HCOrganizationContext _context;
        public ChatRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetChatHistory<T>(ChatParmModel chatParmModel,TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@FromUserID", chatParmModel.FromUserId),
                                          new SqlParameter("@ToUserId", chatParmModel.ToUserId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@PageNumber", chatParmModel.pageNumber),
                                          new SqlParameter("@PageSize",chatParmModel.pageSize),
                                          new SqlParameter("@SortColumn",chatParmModel.sortColumn),
                                          new SqlParameter("@SortOrder",chatParmModel.sortOrder),
                                          new SqlParameter("@RoomId",chatParmModel.RoomId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHT_GetChatHistory.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetCareChatHistory<T>(ChatParmModel chatParmModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@FromUserID", chatParmModel.FromUserId),
                                          new SqlParameter("@ToUserId", chatParmModel.ToUserId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@PageNumber", chatParmModel.pageNumber),
                                          new SqlParameter("@PageSize",chatParmModel.pageSize),
                                          new SqlParameter("@SortColumn",chatParmModel.sortColumn),
                                          new SqlParameter("@SortOrder",chatParmModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHT_GetCareChatHistory.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public Chat SaveChat(Chat chat)
        {
            _context.Add(chat);
            if (_context.SaveChanges() > 0)
                return chat;
            else
                return null;
        }
       
    }
}
