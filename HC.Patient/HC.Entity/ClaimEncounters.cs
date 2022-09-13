using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimEncounters : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }

        [ForeignKey("Claims")]
        public int ClaimId { get; set; }
        [Obsolete]
        public virtual Claims Claims { get; set; }
        [Obsolete]
        public virtual PatientEncounter PatientEncounter { get; set; }        
    }
}
