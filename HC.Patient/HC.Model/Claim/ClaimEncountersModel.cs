using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ClaimEncountersModel
    {
        //public int Id { get; set; }
        public int PatientEncounterId { get; set; }

        public DateTime DOS { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string AppointmentType { get; set; }
    }
}
