using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterReminderConfiguration:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MasterReminderFrequencyType")]
        public int MasterReminderFrequencyTypeId { get; set; }
        public int? FrequencyMinValue { get; set; }
        public int FrequencyMaxValue { get; set; }
        [ForeignKey("MasterReminderType")]
        public int MasterReminderTypeID { get; set; }

        [ForeignKey("MasterPriorTimeForReminder")]
        public int? PriorTimeForReminderId { get; set; }

        public int? PriorTimeValue { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public bool? SendReminderUntilTaskClosed { get; set; }     
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Notes { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Message { get; set; }
        [ForeignKey("MasterTaskTypes")]
        public int? TaskTypeId { get; set; }
        public int? EnrollmentId { get; set; }
        public bool? IsSendReminderToCareManager { get; set; }
        public string CareManagerMessage { get; set; }        
        public virtual MasterReminderType MasterReminderType { get; set; }
        public virtual MasterReminderFrequencyType MasterReminderFrequencyType { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual MasterPriorTimeForReminder MasterPriorTimeForReminder { get; set; }
        public virtual MasterTaskTypes MasterTaskTypes { get; set; }
        //public virtual MasterTimeBetweenConsecutiveReminders MasterTimeBetweenConsecutiveReminders { get; set; }

    }
}
