using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientVitals : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientVitalId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
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
        public virtual Patients Patient { get; set; }
    }
}
