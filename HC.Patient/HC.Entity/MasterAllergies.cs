using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterAllergies : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AllergyID")]
        public int Id { get; set; }        
        public string AllergyType { get; set; }
        [NotMapped]
        public string value { get { return this.AllergyType; } set { this.AllergyType = value; } }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }

}
