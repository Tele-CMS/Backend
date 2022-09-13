using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayerAppointmentTypes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public  int Id { get; set; }

        [Required]
        [ForeignKey("InsuranceCompanies")]
        public int PayerId { get; set; }

        [Required]
        [ForeignKey("PayerServiceCodes")]
        public int PayerServiceCodeId { get; set; }


        [ForeignKey("AppointmentType")]
        public int? AppointmentTypeId { get; set; }

        [ForeignKey("PayerServiceCodeModifiers1")]
        public int? Modifier1 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers2")]
        public int? Modifier2 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers3")]
        public int? Modifier3 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers4")]
        public int? Modifier4 { get; set; }
        public decimal? RatePerUnit { get; set; }

        public virtual InsuranceCompanies InsuranceCompanies { get; set; }


        public virtual PayerServiceCodes PayerServiceCodes { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers1 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers2 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers3 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers4 { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }
    }
}
