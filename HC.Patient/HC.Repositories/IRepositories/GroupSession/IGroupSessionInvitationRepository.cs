using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IGroupSessionInvitationRepository
    {
        /// <summary>
        /// To Save New Group Session Invitation
        /// </summary>
        /// <param name="groupSessionInvitations"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations SaveGroupSessionInvitation(GroupSessionInvitations groupSessionInvitations, TokenModel tokenModel);
        /// <summary>
        /// To Get Group Session By InvitationId
        /// </summary>
        /// <param name="invitaionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations GetGroupSessionByInvitaionId(Guid? invitaionId, TokenModel tokenModel);
        /// <summary>
        /// To Get Group Session Detail By Name and Email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations GetGroupSessionByEmailAndName(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupSessionInvitations"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations UpdateGroupSessionInvitation(GroupSessionInvitations groupSessionInvitations, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitedAppointmentId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations GetGroupSessionByInvitaionAppointmentId(int invitedAppointmentId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupSessionInvitationModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations GetGroupSessionByEmail(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GroupSessionInvitations GetGroupSessionByUserId(int userId, TokenModel tokenModel);
    }
}
