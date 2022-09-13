using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class GetNotificationListModel
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
}
