using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterGender : BaseEntity
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GenderID")]
        public int Id { get; set; }
        public string Gender { get; set; }
        [NotMapped]
        public string value { get { return this.Gender; } set { this.Gender = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
