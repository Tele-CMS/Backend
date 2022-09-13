using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientLabTest : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TestId")]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [StringLength(100)]
        [Required]
        public string TestName { get; set; }
        [Required(ErrorMessage = "Please select one Test Type")]
        public int TestTypeID { get; set; }
        [ForeignKey("MasterLonic")]
        public int? LoincCodeID { get; set; }
        [Required(ErrorMessage = "Please select ScheduledDate")]
        public DateTime ScheduledDate { get; set; }
        public string LabName { get; set; }
        public bool Ordered { get; set; }
        public int? OrderBy { get; set; }
        public DateTime OrderDate { get; set; }
        [StringLength(200)]
        public string TestSpecimenSource { get; set; }
        [StringLength(200)]
        public string ConditionOfSpecimen { get; set; }        
        public int? TimeTypeID { get; set; }                
        public int? FrequencyID { get; set; }
        [StringLength(100)]
        public string FrequencyDuration { get; set; }
        public int? FrequencyDurationID { get; set; }
        public string Notes { get; set; }
        public string Hl7Url { get; set; }
        public string Hl7 { get; set; }
        [StringLength(100)]
        public string OrderNumber { get; set; }
        [StringLength(100)]
        public string FillerOrderNumber { get; set; }
        public string HL7Result { get; set; }

        //Foreign key's tables
        public virtual Patients Patient { get; set; }
        public virtual MasterLonic MasterLonic { get; set; }        
    }
}