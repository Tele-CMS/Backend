using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IUserInvitationReadService : IBaseService
    {
        /// <summary>
        /// Get User Invitation List by organization id
        /// </summary>
        /// <param name="TokenModel">Current Logged In Token</param>
        /// <returns>JsonModel Model</returns>
        JsonModel GetUserInvitationList(TokenModel tokenModel, UserInvitationFilterModel userInvitationFilterModel);

        /// <summary>
        /// Get User Invitation Detail by Invitation id
        /// </summary>
        /// <param name="invitationId">Selected Invitation Id</param>
        /// <param name="tokenModel">Current Logged in user token values</param>
        /// <returns>UserInvitation Json with UserInvitation Entity</returns>
        JsonModel GetUserInvitation(int invitationId, TokenModel tokenModel);

        /// <summary>
        /// To Check whether token is still valid or not(when user try to register on email invitaion)
        /// </summary>
        /// <param name="tokenModel">Current token model</param>
        /// <param name="invitationId">Encrypted Invitation Id</param>
        /// <returns></returns>
        JsonModel CheckTokenAccessibilty(TokenModel tokenModel, string invitationId);

        /// <summary>
        /// To Check whether username existed in DB or not
        /// </summary>
        /// <param name="tokenModel">TokenModel</param>
        /// <param name="username">string username</param>
        /// <returns>JsonModel</returns>
        JsonModel CheckUsernameExistance(TokenModel tokenModel, string username);
    }
}
