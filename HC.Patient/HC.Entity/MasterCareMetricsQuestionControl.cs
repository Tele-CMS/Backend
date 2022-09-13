using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterCareMetricsQuestionControl : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("MasterCareMetricsQuestion")]
        public int QuestionId { get; set; }
        //[ForeignKey("MasterCareMetricsControlType")]
        //public int ControlId { get; set; }
        //public string ForeignKeyTable { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string OptionValue { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string OptionKey { get; set; }
        public bool IsDateControl { get; set; }
        public int DisplayOrder { get; set; }
        public bool ShowOptionKey { get; set; }
        public bool ShowPlaceHolder { get; set; }
        public virtual MasterCareMetricsQuestion MasterCareMetricsQuestion { get; set; }
        //public virtual MasterCareMetricsControlType MasterCareMetricsControlType { get; set; }
    }
}
