using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounterDiagnosisCodes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        public int ICDID { get; set; }
        [Obsolete]
        public MasterICD MasterICD { get; set; }

        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }

        [Required]
        [ForeignKey("MasterICD")]
        public int ICDCodeId { get; set; }

        public PatientEncounter PatientEncounter { get; set; }
    }
}