using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientEncounterTemplates : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("PatientEncounter")]
        public int? PatientEncounterId { get; set; }
        [ForeignKey("MasterTemplates")]
        public int? MasterTemplateId { get; set; }
        [Column(TypeName = "text")]
        public string TemplateData { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual MasterTemplates MasterTemplates { get; set; }
        public virtual Organization Organization { get; set; }

    }
}
