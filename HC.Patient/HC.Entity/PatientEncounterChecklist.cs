using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientEncounterChecklist: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }
        [ForeignKey("MasterEncounterChecklist")]
        public int MasterEncounterChecklistId { get; set; }
        [Column(TypeName = "text")]
        public string Notes { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual MasterEncounterChecklist MasterEncounterChecklist { get; set; }

    }
}
