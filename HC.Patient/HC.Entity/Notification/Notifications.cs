using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class Notifications : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsNotificationSend { get; set; }
        [ForeignKey("Staffs")]
        public int? StaffId { get; set; }
        [ForeignKey("MasterNotificationActionType")]
        public int ActionTypeId { get; set; }
        [ForeignKey("Patients")]
        public int? PatientId { get; set; }
        [ForeignKey("PatientAppointment")]
        public int? PatientAppointmentId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        [ForeignKey("Chat")]
        public int? ChatId { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }
        public virtual MasterNotificationActionType MasterNotificationActionType { get; set; }
        public virtual Staffs Staffs { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Chat Chat { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }

    }
}
