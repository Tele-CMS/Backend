using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientRisk : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientRiskId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
		public int PatientId { get; set; }
		public int AgePoints { get; set; }
		public int DiseaseConditionPoints { get; set; }
		public int EVPoints { get; set; }
		public int HZPoints { get; set; }
		public int MasterRiskIndicatorBenchmarkId { get; set; }
        public virtual Patients Patient { get; set; }
    }
}
