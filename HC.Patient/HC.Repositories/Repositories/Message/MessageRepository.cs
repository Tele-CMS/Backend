using HC.Repositories;
using HC.Patient.Entity;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.Message;
using System.Linq;
using System.Data.SqlClient;
using HC.Model;
using static HC.Common.Enums.CommonEnum;
using System;

namespace HC.Patient.Repositories.Repositories.Message
{
    public class MessageRepository : RepositoryBase<Entity.Message>, IMessageRepository
    {
        private HCOrganizationContext _context;
        public MessageRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetInboxData<T>(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Id", token.UserID),
                                          new SqlParameter("@ForStaff", forStaff),
                                           new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetInboxData.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetSentMessageData<T>(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Id", token.UserID),
                                          new SqlParameter("@ForStaff", forStaff),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_SentMessageData.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetMessageDetail<T>(int MessageId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@MessageId", MessageId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetMessageDetail.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> UsersDropDown<T>(bool isStaff, string searchText, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@IsStaff", isStaff),
                                          new SqlParameter("@SearchText", searchText),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_UsersData.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetFavouriteMessageList<T>(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetFavouriteMessage.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetDeletedMessageList<T>(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetDeletedMessage.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetThreadMessages<T>(int parentMessageId, bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Id", token.UserID),
                                          new SqlParameter("@ForStaff", forStaff),
                                          new SqlParameter("@ParentMessageId",parentMessageId),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetAllMessagesOfThread.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetMessageCounts<T>(bool forStaff, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@ForStaff", forStaff),
                                         new SqlParameter("@UserID", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MSG_GetMessageCounts.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public Entity.Message AddMessage(Entity.Message message)
        {
            try
            {
                _context.Message.Add(message);
                _context.SaveChanges();

            }
            catch (Exception)
            {

            }
            return message;
        }
    }
}
