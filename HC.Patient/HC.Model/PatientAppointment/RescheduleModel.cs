using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
  public  class RescheduleModel
    {


        public int AppointmentId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
