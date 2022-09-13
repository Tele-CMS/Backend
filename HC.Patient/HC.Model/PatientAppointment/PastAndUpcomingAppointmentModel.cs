using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class PastAndUpcomingAppointmentModel
    {
        public string StaffName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string StaffImageUrl { get; set; }
        public int ServiceLocationID { get; set; }
        public decimal DaylightSavingTime { get; set; }
        public decimal StandardTime { get; set; }
        public string TimeZoneName { get; set; }
    }
}
