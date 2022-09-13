using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserTimesheet : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public decimal ServiceDuration { get; set; }

        [ForeignKey("PatientEncounter")]
        public Nullable<int> PatientEncounterId { get; set; }

        [ForeignKey("UserTimesheetByAppointmentType")]
        public Nullable<int> UserTimesheetByAppointmentTypeId { get; set; }

        [ForeignKey("GlobalCode")]
        public int StatusId { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual PatientEncounter PatientEncounter { get;set;}
        public virtual UserTimesheetByAppointmentType UserTimesheetByAppointmentType { get; set; }
    }
}
