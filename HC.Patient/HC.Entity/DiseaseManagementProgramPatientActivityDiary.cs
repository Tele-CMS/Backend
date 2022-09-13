using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class DiseaseManagementProgramPatientActivityDiary : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("DiseaseManagementPlanPatientActivity")]
        public int DiseaseManagementPlanPatientActivityId { get; set; }
        public string Value { get; set; }
        public DateTime LoggedDate { get; set; }

        public virtual DiseaseManagementPlanPatientActivity DiseaseManagementPlanPatientActivity { get; set; }

    }
}
