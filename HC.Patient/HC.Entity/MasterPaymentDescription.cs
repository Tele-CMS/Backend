using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPaymentDescription : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public string DescriptionType { get; set; }

        [NotMapped]
        public string value { get { return this.DescriptionType; } set { this.DescriptionType = value; } }

        [StringLength(50)]
        public string Key { get; set; }

        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}