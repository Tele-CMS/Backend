using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientCustomLabels :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientCustomLabelID")]
        public int Id { get; set; }

        [RequiredNumber]
        [ForeignKey("Patients")]
        public int? PatientID { get; set; }

        [RequiredNumber]
        [ForeignKey("MasterCustomLabels")]
        public int CustomLabelID { get; set; }

        [Required]
        public string CustomLabelValue { get; set; }
        [Required]
        public string CustomLabelDataType { get; set; }
        
        public virtual MasterCustomLabels MasterCustomLabels { get; set; }
        public virtual Patients Patients { get; set; }        
    }
}
