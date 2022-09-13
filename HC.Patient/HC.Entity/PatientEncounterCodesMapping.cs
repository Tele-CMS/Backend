using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounterCodesMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("PatientEncounter")]        
        public int PatientEncounterId { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("PatientEncounterDiagnosisCodes")]
        public int PatientEncounterDiagnosisCodeId { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("PatientEncounterServiceCodes")]
        public int PatientEncounterServiceCodeId { get; set; }
        public bool IsMapped { get; set; }
        public virtual PatientEncounterDiagnosisCodes PatientEncounterDiagnosisCodes { get; set; }
        public virtual PatientEncounterServiceCodes PatientEncounterServiceCodes { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
    }
}