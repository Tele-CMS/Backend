using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MappingHRACategoryRisk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        
        [ForeignKey("DFA_Category")]
        public int HRACategoryId { get; set; }

        [ForeignKey("MasterHRACategoryRisk")]
        public int HRACategoryRiskId { get; set; }

        public virtual MasterHRACategoryRisk MasterHRACategoryRisk { get; set; }
        public virtual DFA_Category DFA_Category { get; set; }
    }
}
