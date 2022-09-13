using HC.Model;
using HC.Patient.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IGroupSessionInvitationService:IBaseService
    {
        /// <summary>
        /// To Save New group session invitaion and send email to user
        /// </summary>
        /// <param name="groupSessionInvitationModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveGroupSessionInvitationModel(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel);
    }
}
