using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HC.Patient.Entity;

namespace HC.Patient.Entity
{
    //[Table("clinical_datagrid_system_category")]
    public class ClinicalDatagridSystemCategory : BaseEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        [ForeignKey("patient")]
        public int patient_empi { get; set; }
        [Required]
        [ForeignKey("encounters")]
        public int encounter_id { get; set; }
        [Required]
        [ForeignKey("masterSystem")]
        public int system_id { get; set; }
        public bool active { get; set; }
        [MaxLength(255)]
        public string comments { get; set; }
        public MasterSystem masterSystem { get; set; }
        public PatientEncounter encounters { get; set; }
        public Patients patient { get; set; }
    }
}
