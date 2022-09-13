using System.Threading.Tasks;
using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Telehealth
{
    public interface ITelehealthService : IBaseService
    {
        JsonModel GetTelehealthSession(int appointmentId, TokenModel tokenModel,bool isMobile=false);
        //JsonModel GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime,int appointmentId, TokenModel tokenModel);
        /// <summary>
        /// To get open tok new session by invitationId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetOTSession(string invitationId, TokenModel tokenModel);
        /// <summary>
        /// To Get Open Tok Session By Appointment Id
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetOTSessionByAppointmentId(int appointmentId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitedAppointmentId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetTelehealthSessionForInvitedAppointmentId(int invitedAppointmentId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        Task<JsonModel> StartVideoRecordingAsync(string sessionId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archievId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        Task<JsonModel> StopVideoRecordingAsync(string archiveId, int appointmentId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        Task<JsonModel> GetVideoRecordingAsync(string archiveId, TokenModel tokenModel);
    }
}
