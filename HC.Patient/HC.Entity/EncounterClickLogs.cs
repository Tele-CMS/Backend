using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class EncounterClickLogs : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? ClickDateTime { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        [ForeignKey("PatientEncounter")]
        public int? PatientEncounterId { get; set; }
        public string AddEditAction { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
    }
}
