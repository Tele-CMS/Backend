using HC.Model;
using HC.Patient.Model;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices
{
    public interface IUserInvitationWriteService : IBaseService
    {
        /// <summary>
        /// To Send User Invitation for registration Over Email
        /// </summary>
        /// <param name="userInvitationModel">Model properties => UserInvitationModel</param>
        /// <param name="tokenModel">Token => TokenModel</param>
        /// <param name="Request">HttpRequest => Current Request</param>
        /// <returns>Model => JsonModel</returns>
        JsonModel SendUserInvitation(UserInvitationModel userInvitationModel, TokenModel tokenModel, HttpRequest Request);


        /// <summary>
        /// To Delete User Invitation When Send Incorrectly or organisation doesn't want to register invited user
        /// </summary>
        /// <param name="id">Int : invitation id</param>
        /// <param name="tokenModel">Current Logged in user Token </param>
        /// <returns></returns>
        JsonModel DeleteUserInvitation(int id, TokenModel tokenModel);

        /// <summary>
        /// To resend invitaion and update invitaion info simultaneously
        /// </summary>
        /// <param name="userInvitationModel">UserInvitationModel Model</param>
        /// <param name="tokenModel">Current Logged in user Token</param>
        /// <param name="Request">Current HttpRequest</param>
        /// <param name="invitationId">Selected Invitation Id</param>
        /// <returns>JsonModel</returns>
        JsonModel ReSendUserInvitation(UserInvitationModel userInvitationModel, TokenModel tokenModel, HttpRequest Request, int invitationId);
    }
}
