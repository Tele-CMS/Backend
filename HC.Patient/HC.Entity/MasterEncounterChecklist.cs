using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterEncounterChecklist: BaseEntity
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName ="varchar(max)" )]
        public string Name { get; set; }
        [ForeignKey("Screens")]
        public int? ScreenId { get; set; }
        public int SortOrder { get; set; }
        public bool IsAdministrativeType { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Obsolete]
        public virtual Organization Organization { get; set; }
        public virtual Screens Screens { get; set; }
    }
}
