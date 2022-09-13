
using HC.Patient.Entity;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IUserRegisterWriteRepository
    {
        /// <summary>
        /// To Save New User Info while registration
        /// </summary>
        /// <param name="user">Entity of User</param>
        /// <param name="staffs">Entity of Staffs</param>
        /// <param name="staffLocation">Entity of staffLocation</param>
        /// <param name="userInvitation">Entity of UserInvitation</param>
        /// <returns></returns>
        int SaveNewUser(Entity.User user, Entity.Staffs staffs, Entity.StaffLocation staffLocation, Entity.UserInvitation userInvitation);

        /// <summary>
        /// To Save New User Without invitation Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="staffs"></param>
        /// <param name="staffLocation"></param>
        /// <returns></returns>
        int SaveNewUser(Entity.User user, Entity.Staffs staffs, Entity.StaffLocation staffLocation);

        /// <summary>
        /// To maintain log of reject invitaions when user invited but refused to register and click on reject instead of register
        /// </summary>
        /// <param name="invitationRejectLog">InvitationRejectLog Entity</param>
        /// <returns>InvitationRejectLog Entity</returns>
        InvitationRejectLog RejectInvitation(InvitationRejectLog invitationRejectLog);
    }
}
