using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class DiseaseManagementProgramActivities : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [ForeignKey("DiseaseManagementProgram")]
        public int DiseaseManageProgramID { get; set; }
        [ForeignKey("Description")]
        public int DescriptionID { get; set; }
        [ForeignKey("MasterFrequencyTypes")]
        public int Frequency { get; set; }        
        public int FrequencyValue { get; set; }
        [ForeignKey("MasterMeasureSign")]
        public int? Sign { get; set; }
        public string Value { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comment { get; set; }
        public virtual DiseaseManagementProgram DiseaseManagementProgram { get; set; }
        public virtual Description Description { get; set; }
        public virtual MasterFrequencyTypes MasterFrequencyTypes { get; set; }
        public virtual MasterMeasureSign MasterMeasureSign { get; set; }
    }
}
