using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterReaction : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ReactionID")]
        public int Id { get; set; }
        public string Reaction { get; set; }
        [NotMapped]
        public string value { get { return this.Reaction; } set { this.Reaction = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
