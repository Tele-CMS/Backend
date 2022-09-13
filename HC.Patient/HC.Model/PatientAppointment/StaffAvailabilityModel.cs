using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class StaffAvailabilityModel
    {
        public int StaffID { get; set; }
        public int? DayID { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
