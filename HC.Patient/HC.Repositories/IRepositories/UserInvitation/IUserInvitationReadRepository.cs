using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IUserInvitationReadRepository
    {
        /// <summary>
        /// Get User Invitation Detail By Email
        /// </summary>
        /// <param name="email">Email id to search with</param>
        /// <param name="orgId">current logged in user organization id</param>
        /// <returns>Entity => UserInvitation</returns>
        UserInvitation GetUserInvitationByEmailAndOrganizationId(string email, int orgId);


        /// <summary>
        /// To get all invitations based on organization id
        /// </summary>
        /// <param name="TokenModel">current token</param>
        /// <param name="locationModel">location model for time zone</param>
        /// <returns>List of UserInvitationResponseModel Model</returns>
        List<UserInvitationResponseModel> GetUserInvitationList(TokenModel tokenModel, LocationModel locationModel, UserInvitationFilterModel userInvitationFilterModel);


        /// <summary>
        /// Get User Invitation Detail based on Id And Organisation id
        /// </summary>
        /// <param name="id">Selected Invitation id</param>
        /// <param name="tokenModel">Current Logged in User Token</param>
        /// <returns></returns>
        UserInvitation GetUserInvitationByIdAndOrganizationId(int id, TokenModel tokenModel);


        /// <summary>
        /// To Check Whether user name existnace or not while registration/add new user
        /// </summary>
        /// <param name="username">string username</param>
        /// <param name="tokenModel">TokenModel</param>
        /// <returns>User Entity</returns>
        Entity.User CheckUserNameExistance(string username, TokenModel tokenModel);
    }
}
