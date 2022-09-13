using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientEncounterProgram : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }
        [ForeignKey("DiseaseManagementProgram")]
        public int ProgramId { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual DiseaseManagementProgram DiseaseManagementProgram { get; set; }
    }
}
