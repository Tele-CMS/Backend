using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterDegree : BaseEntity
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DegreeID")]
        public int Id { get; set; }
        [MaxLength(50)]
        public string DegreeName { get; set; }        
        [NotMapped]
        public string value { get { return this.DegreeName;  } }
        public string Description { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}