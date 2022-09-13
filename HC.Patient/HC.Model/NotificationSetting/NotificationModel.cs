using System;
using System.Collections.Generic;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Model.NotificationSetting
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public bool IsNotificationSend { get; set; }
        public int NotificationTypeId { get; set; }
        public int ActionTypeId { get; set; }
        public int? PatientId { get; set; }
        public int? PatientAppointmentId { get; set; }
        public int OrganizationID { get; set; }
        public int? StaffId { get; set; }
        public int? ChatId { get; set; }
        public string Message { get; set; }
        public bool IsMobileUser { get; set; }
        public int?UserId { get; set; }
    }

    public class NotificationMappingModel
    {
        public int Id { get; set; }
        public int NotificationTypeId { get; set; }
        public int NotificationId { get; set; }
        public bool IsSendNotification { get; set; }
        public bool IsReceivedNotification { get; set; }
        public bool IsReadNotification { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
    public class PushNotificationModel
    {
        public int NotificationTypeId { get; set; }
        public NotificationActionType  TypeId { get; set; }
        public NotificationActionSubType SubTypeId { get; set; }
        public string Message { get; set; }
        public int? StaffId { get; set; }
        public int? UserId { get; set; }
        public int? PatientId { get; set; }
        public NotificationType NotificationType { get; set; } 
    }
}
