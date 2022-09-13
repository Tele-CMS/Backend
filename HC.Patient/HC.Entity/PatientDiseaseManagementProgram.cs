using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientDiseaseManagementProgram : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [ForeignKey("Patients")]
        public int PatientID { get; set; }
        [ForeignKey("DiseaseManagementProgram")]
        public int DiseaseManagementPlanID { get; set; }
        public string TerminationReason { get; set; }
        public string TerminationComment { get; set; }
        public string CareManager { get; set; }
        [ForeignKey("Staffs")]
        public int? CareManagerId { get; set; }
        public string PrimaryCareLocation { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? DateOfEnrollment { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public DateTime? GraduationDate { get; set; }
        [ForeignKey("GlobalCode")]
        public int? StatusId { get; set; }
        [ForeignKey("GlobalCode1")]
        public int? FrequencyId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string OtherFrequencyDescription { get; set; }
        public virtual Patients Patient {get;set;}
        public virtual DiseaseManagementProgram DiseaseManagementProgram { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual GlobalCode GlobalCode1 { get; set; }
        public virtual List<DiseaseManagementPlanPatientActivity> DiseaseManagementPlanPatientActivity { get; set; }
        public virtual Staffs Staffs { get; set; }
    }
}