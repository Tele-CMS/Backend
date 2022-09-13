using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientEncounters
{
    public class EncounterSignatureModel
    {   
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? StaffId { get; set; }
        public int PatientEncounterId { get; set; }
        public Byte[] PatientSign { get; set; }
        public DateTime? PatientSignDate { get; set; }
        public Byte[] ClinicianSign { get; set; }
        public DateTime? ClinicianSignDate { get; set; }
        public Byte[] GuardianSign { get; set; }
        public DateTime? GuardianSignDate { get; set; }
        public string GuardianName { get; set; }
    }
}
