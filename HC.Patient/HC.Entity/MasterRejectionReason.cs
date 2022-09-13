using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterRejectionReason : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RejectionReasonID")]
        public int Id { get; set; }
        [StringLength(5)]
        public string ReasonCode { get; set; }
        [StringLength(100)]
        public string ReasonDesc { get; set; }        
        [NotMapped]
        public string value { get { return this.ReasonDesc; } set { this.ReasonDesc = value; } }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}