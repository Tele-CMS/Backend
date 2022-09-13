using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientReminderDetailWithReminderMessageMapping: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("PatientReminder")]
        public int ConfigurationId { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public DateTime? LastReminderSentDateTime { get; set; }
        public bool IsMarkToSend { get; set; }
        public string Source { get; set; }
        public int ModuleId { get; set; }
        [ForeignKey("MasterMessageType")]
        public int? MessageTypeId { get; set; }
        [ForeignKey("MasterReminderFrequencyType")]
        public int? ReminderFrequencyTypeId { get; set; }
        [ForeignKey("MasterReminderType")]
        public int? ReminderTypeId { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string EnrolledProgramName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string EnrolledProgramIds { get; set; }

        public virtual PatientReminder PatientReminder { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual MasterMessageType MasterMessageType { get; set; }
        public virtual MasterReminderFrequencyType MasterReminderFrequencyType { get; set; }
        public virtual MasterReminderType MasterReminderType { get; set; }

    }
}
