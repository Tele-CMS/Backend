using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientReminder : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [ForeignKey("MasterReminderFrequencyType")]
        public int MasterReminderFrequencyTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int? EnrollmentId { get; set; }

        public bool? IsSendReminderToCareManager { get; set; }

        public string CareManagerMessage { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Message { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Notes { get; set; }

        [Column(TypeName = "int")]
        public int FrequencyValue { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual MasterReminderFrequencyType MasterReminderFrequencyType { get; set; }
    }
}
