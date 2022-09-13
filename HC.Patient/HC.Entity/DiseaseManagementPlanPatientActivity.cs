using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class DiseaseManagementPlanPatientActivity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [ForeignKey("PatientDiseaseManagementProgram")]       
        public int PatientDiseaseManagmentPlanID { get; set; }
        [ForeignKey("DiseaseManagementProgramActivities")]
        public int DiseaseManagmentPlanActivityID { get; set; }
        [ForeignKey("MasterFrequencyTypes")]
        public int Frequency { get; set; }
        public int FrequencyValue { get; set; }
        [ForeignKey("MasterMeasureSign")]
        public int? Sign { get; set; }
        public string GoalResultValue { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comment { get; set; }
        [ForeignKey("Staffs")]
        public int CareManagerID { get; set; }
        [ForeignKey("MasterActivityUnitType")]
        public int? ActivityUnitTypeId { get; set; }
        public virtual DiseaseManagementProgramActivities DiseaseManagementProgramActivities { get; set; }
        public virtual PatientDiseaseManagementProgram PatientDiseaseManagementProgram { get; set; }
        public virtual Staffs Staffs { get; set; }
        public virtual MasterFrequencyTypes MasterFrequencyTypes { get; set; }
        public virtual MasterMeasureSign MasterMeasureSign { get; set; }
        public virtual MasterActivityUnitType MasterActivityUnitType { get; set; }
        public virtual List<DiseaseManagementPlanPatientActivityNotifications> DiseaseManagementPlanPatientActivityNotifications { get; set; }
    }
}
