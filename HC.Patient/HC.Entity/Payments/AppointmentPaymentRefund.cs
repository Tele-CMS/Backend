using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
  public  class AppointmentPaymentRefund : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PatientAppointment")]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentPayment")]
        [Required]
        public string PaymentToken { get; set; }
        [Required]
        public string RefundToken { get; set; }

    }
}
