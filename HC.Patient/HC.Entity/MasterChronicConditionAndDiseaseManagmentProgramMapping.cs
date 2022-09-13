using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterChronicConditionAndDiseaseManagmentProgramMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("MasterChronicCondition")]
        public int? MasterChronicConditionId { get; set; }
        [ForeignKey("DiseaseManagementProgram")]
        public int? DiseaseManagmentProgramId { get; set; }
        public virtual MasterChronicCondition MasterChronicCondition { get; set; }
        public virtual DiseaseManagementProgram DiseaseManagementProgram { get; set; }
    }
}
