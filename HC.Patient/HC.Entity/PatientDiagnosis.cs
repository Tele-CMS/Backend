using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientDiagnosis : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientDiagnosisId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterICD")]
        public int ICDID { get; set; }
        public bool IsPrimary { get; set; }        
        public DateTime DiagnosisDate { get; set; }
        public DateTime? ResolveDate { get; set; }

        //Foreign key's tables        
        public virtual Patients Patient { get; set; }
        public virtual MasterICD MasterICD { get; set; }        
    }
}