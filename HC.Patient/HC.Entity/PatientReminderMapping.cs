using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientReminderMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [ForeignKey("PatientReminder")]
        public int PatientReminderId { get; set; }

        [ForeignKey("Patients")]
        public int PatientId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual PatientReminder PatientReminder { get; set; }
        public virtual Patients Patients { get; set; }
    }
}
