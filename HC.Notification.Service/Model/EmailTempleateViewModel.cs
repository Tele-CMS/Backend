using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service.Model
{
    public class EmailTempleateViewModel
    {
        public string PatientOrStaffName { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }
        public string PatientName { get; set; }
        public string StaffName { get; set; }
        public string ToEmail { get; set; }
        public string EmailTemplate { get; set; }
        public string Address { get; set; }
        public string AppointmentType { get; set; }
        public int MappingId { get; set; }
        public int EmailTypeID { get; set; }
        public string CareManagerDetails { get; set; }
    }
}
