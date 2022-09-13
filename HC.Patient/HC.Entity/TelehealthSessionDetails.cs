using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class TelehealthSessionDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("Staffs")]
        public int? StaffId { get; set; }
        [ForeignKey("Patients")]
        public int? PatientID { get; set; }

        public string SessionID { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [NotMapped]
        public Exception exception { get; set; }

        [NotMapped]
        public int result { get; set; }


        public virtual Staffs Staffs { get; set; }
        public virtual Patients Patients { get; set; }
        [ForeignKey("PatientAppointment")]
        public int? AppointmentId { get; set; }
        public PatientAppointment PatientAppointment;
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
