using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserTimesheetByAppointmentType:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id{ get; set; }
        [ForeignKey("AppointmentType")]
        public Nullable<int> AppointmentTypeId { get; set; }
        [ForeignKey("Staffs")]
        public int StaffId { get; set; }
        [ForeignKey("PatientEncounter")]
        public Nullable<int> PatientEncounterId { get; set; }

        public DateTime DateOfService { get; set; }
        public decimal ExpectedTimeDuration { get; set; }
        public decimal ActualTimeDuration { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public decimal Distance { get; set; }

        public bool IsTravelTime { get; set; }

        [ForeignKey("TimesheetStatus")]
        public int StatusId { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [Column(TypeName ="varchar(200)")]
        public string Notes { get; set; }
        public GlobalCode TimesheetStatus { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }
        public virtual Staffs Staffs { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Location Location { get; set; }
    }
}
