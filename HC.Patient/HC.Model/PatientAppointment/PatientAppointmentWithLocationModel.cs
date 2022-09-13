using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class PatientAppointmentWithLocationModel
    {
        public string StaffId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int? PatientAppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentTypeId { get; set; }
        public decimal CurrentOffset { get; set; }

    }
}
