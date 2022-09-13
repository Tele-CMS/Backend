using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;

namespace HC.Patient.Repositories
{
    public class UserInvitationWriteRepository : RepositoryBase<UserInvitation>, IUserInvitationWriteRepository
    {
        private HCOrganizationContext _context;
        public UserInvitationWriteRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public int SaveUpdateUserInvitation(UserInvitation userInvitation)
        {
            int result = 0;

            if (userInvitation.Id == 0)
                _context.UserInvitation.Add(userInvitation);
            else
                _context.UserInvitation.Update(userInvitation);
            result = _context.SaveChanges();

            return result;
        }
        public int DeleteUserInvitation(UserInvitation userInvitation)
        {
            int result = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.UserInvitation.Update(userInvitation);
                    result = _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    result = -1;
                    transaction.Rollback();
                }
            }
            return result;
        }
    }
}
