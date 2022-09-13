using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.IRepositories
{
    public interface INotificationRepository : IRepositoryBase<Notifications>
    {
        IQueryable<T> GetAllUnReadALerts<T>(TokenModel tokenModel) where T : class, new();
        void SaveNotificationTypeMapping(NotificationTypeMapping notificationTypeMapping);
        void SaveNotificationTypeMapping(List<NotificationTypeMapping> notificationTypeMapping);
        IQueryable<T> GetPatientNotificationList<T>(int patientId, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new();

        IQueryable<T> GetPatientPortalNotificationList<T>(int patientId, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new();
        List<NotificationTypeMapping> ReadChatAndAllNotification(int patientId, bool isChatNotification, TokenModel tokenModel);
        IQueryable<T> ReadChatAndAllNotificationForPateint<T>(int patientId, TokenModel tokenModel) where T : class, new();
        MasterNotificationActionType SaveMasterNotificationActionType(MasterNotificationActionType masterNotificationActionType);
        MasterNotificationActionType GetMasterNotificationActionType(NotificationActionType notificationActionType, NotificationActionSubType notificationActionSubType);
        MasterNotificationType SaveMasterNotificationType(MasterNotificationType masterNotificationType); 
        MasterNotificationType GetMasterNotificationType(string type);
        string GetNotificationMessage(string fullName, int? staffId, int? appointmentId, string notificationType, LocationModel locationModel = null);
        string GetNotificationMessage(object p, object staffId, object id, string v);
        string GetNotificationMessage(object p, int? staffId, object id, string v);
    }
}
