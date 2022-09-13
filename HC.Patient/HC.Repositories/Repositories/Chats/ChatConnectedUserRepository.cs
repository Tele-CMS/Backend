using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Chats
{
    public class ChatConnectedUserRepository : RepositoryBase<ChatConnectedUser>, IChatConnectedUserRepository
    {
        private HCOrganizationContext _context;
        public ChatConnectedUserRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
    }
}
