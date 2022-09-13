using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class AppointmentPayments : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PatientAppointment")]
        public int AppointmentId { get; set; }
        [Required]
        public string PaymentToken { get; set; }
        [Required]
        public decimal BookingAmount { get; set; }
        [Required]
        public decimal CommissionPercentage { get; set; }
        public string Email { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        public PatientAppointment PatientAppointment { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        //public string RefundToken { get; set; }

    }
}
