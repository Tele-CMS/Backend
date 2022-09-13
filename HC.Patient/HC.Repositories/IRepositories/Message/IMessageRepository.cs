using HC.Model;
using HC.Repositories.Interfaces;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.Message
{
    public interface IMessageRepository : IRepositoryBase<Entity.Message>
    {
        IQueryable<T> GetInboxData<T>(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
        IQueryable<T> GetSentMessageData<T>(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
        IQueryable<T> GetMessageDetail<T>(int MessageId, TokenModel token) where T : class, new();
        IQueryable<T> UsersDropDown<T>(bool isStaff, string searchText, TokenModel token) where T : class, new();
        IQueryable<T> GetFavouriteMessageList<T>(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
        IQueryable<T> GetDeletedMessageList<T>(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
        IQueryable<T> GetThreadMessages<T>(int parentMessageId, bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
        IQueryable<T> GetMessageCounts<T>(bool forStaff, TokenModel token) where T : class, new();
        Entity.Message AddMessage(Entity.Message message);
    }
}
