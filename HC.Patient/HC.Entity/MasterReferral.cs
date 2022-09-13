using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterReferral : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ReferralID")]
        public int Id { get; set; }
        public string ReferralName { get; set; }
        [NotMapped]
        public string value { get { return this.ReferralName; } set { this.ReferralName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }

}


