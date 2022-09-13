using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class GroupSessionInvitations : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? InvitaionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [ForeignKey("TelehealthSessionDetails")]
        public int? SessionId { get; set; }
        public string SessionMode { get; set; }
        [ForeignKey("PatientAppointment")]
        public int? AppointmentId { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public PatientAppointment PatientAppointment { get; set; }
        public TelehealthSessionDetails TelehealthSessionDetails { get; set; }
        [ForeignKey("InvitedPatientAppointment")]
        public int? InvitedAppointmentId { get; set; }
        public PatientAppointment InvitedPatientAppointment { get; set; }
        [ForeignKey("InvitedUser")]
        public int? UserID { get; set; }
        public User InvitedUser { get; set; }
    }
}
