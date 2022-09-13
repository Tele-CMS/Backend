using HC.Model;
using HC.Patient.Model.NotificationSetting;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface INotificationService: IBaseService
    {
        //JsonModel SaveNotification(NotificationModel notificationModel, TokenModel token);
        void SaveNotification(PushNotificationModel saveNotificationModel, TokenModel token);

    }
}
