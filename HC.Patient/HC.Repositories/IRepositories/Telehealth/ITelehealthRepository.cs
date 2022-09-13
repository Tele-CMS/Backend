using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HC.Model;
using HC.Patient.Entity;

namespace HC.Patient.Repositories.IRepositories.Telehealth
{
    public interface ITelehealthRepository
    {
        TelehealthSessionDetails GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime);
        TelehealthTokenDetails GetTelehealthToken(int id,TokenModel tokenModel);
        TelehealthTokenDetails CreateTelehealthToken(int telehealthSessionDetailID, string token, double duration, TokenModel tokenModel, int? invitationId = null);
        TelehealthSessionDetails CreateTelehealthSession(string sessionID, int? patientID, int? staffID, DateTime? startTime, DateTime? endTime, int appointmentId, TokenModel tokenModel);
        string GetUserNameByUserID(TokenModel tokenModel);
        /// <summary>
       /// To get current session detail by session id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthSessionDetails GetOTSession(int? invitationId, TokenModel tokenModel);
        /// <summary>
        /// Get Open Tok Session token by invitation id
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthTokenDetails GetTelehealthTokenByInvitationId(int? invitationId, TokenModel tokenModel);
        /// <summary>
        /// To get Open tok session by appointment id
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthSessionDetails GetTelehealthSessionByAppointmentId(int? appointmentId, TokenModel tokenModel);
        /// <summary>
       /// Get Open Tok Session by id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthSessionDetails GetOTSessionById(int id, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthSessionDetails GetOTSessionBySessionId(string sessionId, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userTypeEnum"></param>
        /// <returns></returns>
        string GetUserNameByUserID(int userId, Common.Enums.CommonEnum.UserTypeEnum userTypeEnum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userTypeEnum"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        TelehealthTokenDetails GetTelehealthToken(int sessionId, Common.Enums.CommonEnum.UserTypeEnum userTypeEnum, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="userId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<TelehealthTokenDetails> GetOTTokenByAppointmentId(int appointmentId, int userId, TokenModel tokenModel);
    }
}
