using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientMedication : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientMedicationId")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }    
        [StringLength(200)]
        public string Medicine { get; set; }   
        public string Dose { get; set; }

        [ForeignKey("Frequency")]
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual GlobalCode Frequency { get; set; }
    }
}
