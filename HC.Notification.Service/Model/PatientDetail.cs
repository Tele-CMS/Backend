using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service
{
    public class PatientDetail
    {
        public int MappingId { get; set; }
        public int NotificationId { get; set; }
        public int StaffId { get; set; }
        public int PatientId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationAction { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string PatientName { get; set; }
        public string StaffName { get; set; }
        public string AppointmentType { get; set; }
        public string DeviceToken { get; set; }
        public int DeviceTypeId { get; set; }
        public string StaffEmail { get; set; }
        public string PatientEmail { get; set; }
        public string TimeZoneName { get; set; }
        public int OrganizationId { get; set; }
        public int LocationId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public decimal StandardTime { get; set; }
        public decimal DaylightSavingTime { get; set; }
        public string CareManagerName { get; set; }
        public string CareManagerPhoneNumber { get; set; }
        public string CareManagerEmail { get; set; }
        public decimal DaylightOffset
        {
            get
            {
                return CommonMethods.GetMin(this.DaylightSavingTime);
            }
        }
        public decimal StandardOffset
        {
            get
            {
                return CommonMethods.GetMin(this.StandardTime);
            }
        }
        public string Address { get; set; }
        public string SetPushNotificationMessage
        {
            get
            {
                if (this.NotificationType.ToLower() == "pushnotification")
                {
                    string message = String.Empty;
                    switch (this.NotificationAction.ToLower())
                    {
                        case "creatappointment":
                            message = "Your appointment has been scheduled with " + this.StaffName + " On " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName);
                            break;
                        case "deleteappointment":
                            message = "Your appointment for " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName) + " has been removed by " + this.StaffName;
                            break;
                        case "cancelappointment":
                            message = "Your appointment on " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName) + " has been cancelled by " + this.StaffName;
                            break;
                        case "taskassign":
                            break;
                        case "taskcompleted":
                            break;
                        case "updateappointment":
                            break;
                        case "updaterequestappointment":
                            break;
                        case "approvedappointment":
                            message = "Your appointment request for " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName) + " has been approved by " + this.StaffName;
                            break;
                        case "chatmessage":
                            message = "You have a message from " + this.StaffName + " Message :" + CommonMethods.Decrypt(this.Message);
                            break;
                        case "activateappointment":
                            message = "Your appointment on " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName) + " has been restored by " + this.StaffName;
                            break;
                        case "tentativeappointment":
                            message = "Your tentative appointment has been scheduled with " + this.StaffName + " On " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName); ;
                            break;
                             case "autoconfirmtentativeappointment":
                            message = "Your tentative appointment is auto confirm, That's is scheduled with " + this.StaffName + " On " + CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName); ;
                            break;
                    }
                    return message;

                }
                return "";
            }
        }

        public DateTime StartDateTimeUtc
        {
            get
            {
                if (this.StartDateTime != null)
                {

                    return CommonMethods.ConvertFromUtcTimeWithOffset(this.StartDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName);
                }
                return DateTime.UtcNow;
            }
        }

        public DateTime EndDateTimeUtc
        {
            get
            {
                if (this.EndDateTime != null)
                {
                    return CommonMethods.ConvertFromUtcTimeWithOffset(this.EndDateTime.Value, this.DaylightOffset, this.StandardOffset, this.TimeZoneName);
                }
                return DateTime.UtcNow;
            }
        }
    }
    public class TemplateModel
    {
        public int EmailTypeId { get; set; }
        public string Template { get; set; }
        public string EmailType { get; set; }
    }
}
