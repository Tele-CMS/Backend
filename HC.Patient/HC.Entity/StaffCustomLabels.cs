using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffCustomLabels :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StaffCustomLabelID")]
        public int Id { get; set; }

        [RequiredNumber]
        [ForeignKey("Staffs")]
        public int? StaffID { get; set; }

        [RequiredNumber]
        [ForeignKey("MasterCustomLabels")]
        public int CustomLabelID { get; set; }

        [Required]
        public string CustomLabelValue { get; set; }
        [Required]
        public string CustomLabelDataType { get; set; }

        public virtual MasterCustomLabels MasterCustomLabels { get; set; }
        public virtual Staffs Staffs { get; set; }
    }
}
