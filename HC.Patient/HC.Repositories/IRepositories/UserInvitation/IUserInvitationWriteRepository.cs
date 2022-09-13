using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IUserInvitationWriteRepository
    {
        /// <summary>
        /// To Save User Info while invitation
        /// </summary>
        /// <param name="userInvitation">Passing User Invitation Entity</param>
        /// <returns>Int Value(Current Identity)</returns>
        int SaveUpdateUserInvitation(UserInvitation userInvitation);


        /// <summary>
        /// To delete user invitation(Basically we are updating isDeleted = true with this functionality)
        /// </summary>
        /// <param name="userInvitation">UserInvitation Entity</param>
        /// <returns></returns>
        int DeleteUserInvitation(UserInvitation userInvitation);
    }
}
