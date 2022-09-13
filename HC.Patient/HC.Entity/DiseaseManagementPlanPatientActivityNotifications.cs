using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class DiseaseManagementPlanPatientActivityNotifications : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [ForeignKey("DiseaseManagementPlanPatientActivity")]
        public int DiseaseManagmentPlanPatientActivityID { get; set; }
        [ForeignKey("MasterNotificationTypes")]
        public int NotificationTypeID { get; set; }
        [ForeignKey("MasterFrequencyTypes")]
        public int NotificationFrequency { get; set; }
        public int NotificationFrequencyValue { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Message { get; set; }
        [ForeignKey("MasterMeasureSign")]
        public int? Sign { get; set; }
        public virtual DiseaseManagementPlanPatientActivity DiseaseManagementPlanPatientActivity { get; set; }
        public virtual MasterNotificationType MasterNotificationTypes { get; set; }
        public virtual MasterFrequencyTypes MasterFrequencyTypes { get; set; }
        public virtual MasterMeasureSign MasterMeasureSign { get; set; }
    }
}
