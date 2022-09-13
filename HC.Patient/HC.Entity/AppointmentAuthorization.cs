using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AppointmentAuthorization : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]        
        public int Id { get; set; }
        [ForeignKey("PatientAppointment")]
        public int AppointmentId { get; set; }
        [ForeignKey("AuthProcedureCPT")]
        public int AuthProcedureCPTLinkId { get; set; }
        [ForeignKey("MasterServiceCode")]
        public int ServiceCodeId { get; set; }
        public int UnitsBlocked { get; set; }

        public int? UnitsConsumed { get; set; }

        public bool IsBlocked { get; set; }

        public DateTime AuthScheduledDate { get; set; }

        public virtual PatientAppointment PatientAppointment { get; set; }
        public virtual AuthProcedureCPT AuthProcedureCPT { get; set; }
        public virtual MasterServiceCode MasterServiceCode { get; set; }
    }
}
