using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientsMedicationModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Medicine { get; set; }
        public string Dose { get; set; }
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        public string Frequency { get; set; }
        public decimal TotalRecords { get; set; }
        public bool IsActive { get; set; }
        public int LinkedEncounterId { get; set; }
    }

    public class PatientsMedicationModelForPrint : PatientsMedicationModel
    {
  
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
     
    }
}
