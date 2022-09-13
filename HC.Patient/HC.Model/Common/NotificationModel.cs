using HC.Patient.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Common
{
    public class NotificationModel
    {
       // public List<MessageNotificationModel> MessageNotification { get; set; }
       // public List<UserDocumentNotificationModel> UserDocumentNotification { get; set; }
        public UnReadNotificationCount UnReadNotificationCount { get; set; }
        public List<UserNotificationModel> UserDocumentNotification { get; set; }
    }
    public class MessageNotificationModel
    {
        public int MessageId { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public DateTime MessageDate { get; set; }
        public int ParentMessageId { get; set; }
        public string Thumbnail { get; set; }
        public bool IsPatient { get; set; }
    }
    public class UserDocumentNotificationModel
    {
        public int DocumentId { get; set; }
        public string Message { get; set; }
        public string DocumentName { get; set; }
        public string Expiration { get; set; }
    }
    public class ChatAndNotificationCountModel
    {
        public int ChateNotificationCount { get; set; }
        public int TotalNotificationCount { get; set; }

    }
    public class UnReadNotificationCount
    {
        public int TotalUnReadNotification { get; set; }
    }

 
    public class ChatAndNotificaitonModel
    {
        public ChatAndNotificationCountModel ChatAndNotificationCountModel { get; set; }
        public List<UnReadMessageCount> UnReadMessageCount { get; set; }
    }
    public class PatientNotificationListModel
    {
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public string NotificationAction { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string StaffName { get; set; }
        public int AppointmentType { get; set; }
        public int LocationId { get; set; }
        public decimal DaylightSavingTime { get; set; }
        public decimal StandardTime { get; set; }
        public string TimeZoneName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalRecords { get; set; }
    }

    public class NotificationGroupItem<T>
    {
        /// <summary>
        /// Specific Day for header.
        /// </summary>
        public string SpecificDay { get; set; }

        public string SpecificDayStr { get; set; }
        /// <summary>
        /// NotificationGroup List Data for each day.
        /// </summary>
        public List<T> NotificationGroup { get; set; }
    }

    public class AlertItemModel
    {
        /// <summary>
        /// AlertTypeId
        /// </summary>
        public int AlertTypeId { get; set; }

        /// <summary>
        /// AlertId of the object(chase/pend/address)
        /// </summary>
        public int AlertId { get; set; }

        /// <summary>
        /// Messsage
        /// </summary>
        public string Messsage { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// AlertURL
        /// </summary>
        public string AlertURL { get; set; }

        /// <summary>
        /// AlertTypeName
        /// </summary>
        public string Name { get; set; }

        public AlertItemModel()
        {
            this.CreatedDate = DateTime.UtcNow.ToString();
        }
    }

    public class UnReadMessageCount
    {
        public int UserID { get; set; }
        public int StaffID { get; set; }
        public int TotalUnreadMessage { get; set; }
    }
    public class UserNotificationModel
    {
        public int MappintId { get; set; }
        public int NotificationId { get; set; }
        public int? StaffId { get; set; }
        public int? PatientId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationAction { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string PatientName { get; set; }
        public string StaffName { get; set; }
        public string CreatedDate { get; set; }
        public int TotalRecords { get; set; }
        public int? LocationId { get; set; }
        public DateTime CreatedDateWitOutFormat { get; set; }
        public int? UserId { get; set; }
        public int? NotificationTypeId { get; set; }
        public int? NotificationSubTypeId { get; set; }
        public string Type { get; set; } 
        public string SubType { get; set; }

    }
    public class PushMobileNotificationModel
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public int StaffId { get; set; }
        public int OrganizationId { get; set; }
        public string DeviceToken { get; set; }
        public string NotificationPriority { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public bool IsSent { get; set; }
        public string StatusName { get; set; }
    }
    public class PushNotificationsUserDetailsModel
    {
        public int AppointmentId { get; set; }
        public int ProviderID { get; set; }
        public int PatientID { get; set; }
        public string ImageThumbnail { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StatusName { get; set; }
        public bool IsSymptomateReportExist { get; set; }
        public int ReportId { get; set; }
        public bool? IsTelehealthAppointment { get; set; }
        public string AppointmentTypeName { get; set; }
    }
    public class PushNotificationChatMessageModel
    {
        public int AppointmentId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string ChatDate { get; set; }
        public bool IsReceived { get; set; }
        public int RoomId { get; set; }
        public string Image { get; set; }
        public string StatusName { get; set; }
    }
}
