using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientVitalModel
    {   
        public int Id { get; set; }        
        public int PatientID { get; set; }
        public int? BPDiastolic { get; set; }
        public int? BPSystolic { get; set; }
        public double? HeightIn { get; set; }
        public double? WeightLbs { get; set; }
        public int? HeartRate { get; set; }
        public int? Pulse { get; set; }
        public int? Respiration { get; set; }
        public double? BMI { get; set; }
        public double? Temperature { get; set; }
        public DateTime VitalDate { get; set; }
        public int LinkedEncounterId { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
