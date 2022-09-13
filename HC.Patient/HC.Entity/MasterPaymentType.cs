using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPaymentType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public string PaymentType { get; set; }

        [NotMapped]
        public string value { get { return this.PaymentType; } set { this.PaymentType = value; } }

        [StringLength(50)]
        public string Key { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        [Column(TypeName ="varchar(30)")]
        public string AssociatedEntity { get; set; }

        public virtual Organization Organization { get; set; }
    }
}