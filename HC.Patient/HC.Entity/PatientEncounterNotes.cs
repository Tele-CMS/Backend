using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounterNotes: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Notes { get; set; }
        [ForeignKey("Users3")]
        public int? UserId { get; set; }
        public int StaffId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public string EncounterNotes { get; set; }
        public DateTime NotesAddedDate { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual User Users3 { get; set; }
    }
}
