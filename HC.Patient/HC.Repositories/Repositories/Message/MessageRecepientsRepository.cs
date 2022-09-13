using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.Message;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.Message
{
    public class MessageRecepientsRepository : RepositoryBase<Entity.MessageRecepient>, IMessageRecepientsRepository
    {
        private HCOrganizationContext _context;
        public MessageRecepientsRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
