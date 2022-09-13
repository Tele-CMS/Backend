using HC.Model;
using HC.Patient.Model;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices
{
    public interface IUserRegistrationWriteService : IBaseService
    {
        /// <summary>
        /// To Register New User when invited
        /// </summary>
        /// <param name="registerUser">RegisterUserModel Model</param>
        /// <param name="tokenModel">TokenModel</param>
        /// <param name="Request"></param>
        /// <returns>JsonModel</returns>
        JsonModel RegisterNewUser(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request);

        /// <summary>
        /// To Register new user without token id or invitation
        /// </summary>
        /// <param name="registerUser"></param>
        /// <param name="tokenModel"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        JsonModel RegisterNewUserWithoutToken(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request);

        /// <summary>
        /// To Reject User Invitation sent through email to register on portal
        /// </summary>
        /// <param name="rejectInvitationModel">RejectInvitationModel Model</param>
        /// <param name="tokenModel">TokenModel</param>
        /// <returns></returns>

        JsonModel RejectInvitation(RejectInvitationModel rejectInvitationModel, TokenModel tokenModel);
    }
}
