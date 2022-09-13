using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientPrescription : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientPrescriptionId")]
        public int Id { get; set; }
        [ForeignKey("Prescription")]
        public int DrugID { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public string Dose { get; set; }
        [ForeignKey("Frequency")]
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public string Duration { get; set; }
        public string Notes { get; set; }
        public string Directions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual GlobalCode Frequency { get; set; }
        public virtual PrescriptionDrugs Prescription { get; set; }
    }
}

