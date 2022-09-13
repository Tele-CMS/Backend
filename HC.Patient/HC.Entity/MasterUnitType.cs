using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterUnitType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UnitTypeID")]
        public int Id { get; set; }
        public string UnitTypeName { get; set; }

        [NotMapped]
        public string value { get { return this.UnitTypeName; } set { this.UnitTypeName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}