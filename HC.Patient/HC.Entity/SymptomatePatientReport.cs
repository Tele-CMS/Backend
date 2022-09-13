using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public  class SymptomatePatientReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public int PatientID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string ReportedSymptoms { get; set;}
        public string PresentSymptoms { get; set; }
        public string AbsentSymptoms { get; set; }
        public string UnknownSymptoms { get; set; }
        public string FinalConditions { get; set; }

        public int PrePatientID { get; set; }
    }
}
