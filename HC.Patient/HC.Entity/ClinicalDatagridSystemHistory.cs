using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HC.Patient.Entity;

namespace HC.Patient.Entity
{
    //[Table("clinical_datagrid_system_history")]

    public class ClinicalDatagridSystemHistory : BaseEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        [ForeignKey("ClinicalDatagridSystemCategory")]
        public int clinical_system_id { get; set; }
        [Required]
        [ForeignKey("master_history")]
        public int history_id { get; set; }
        public bool active { get; set; }
        public ClinicalDatagridSystemCategory ClinicalDatagridSystemCategory { get; set; }
        public MasterHistory master_history { get; set; }

    }
}
