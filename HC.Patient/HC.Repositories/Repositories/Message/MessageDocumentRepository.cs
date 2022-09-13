using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Message;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.Message
{
    public class MessageDocumentRepository : RepositoryBase<MessageDocuments>, IMessageDocumentRepository
    {
        private HCOrganizationContext _context;
        public MessageDocumentRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
