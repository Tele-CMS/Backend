using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterEthnicity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EthnicityID")]
        public int Id { get; set; }
        public string EthnicityName { get; set; }
        [NotMapped]
        public string value { get { return this.EthnicityName; } set { this.EthnicityName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}