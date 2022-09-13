using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterPatientEncLinkedColumn : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        [ForeignKey("MasterPatientEncLinkedEntity")]
        public int MasterPatientEncLinkedEntityId { get; set; }

        public virtual MasterPatientEncLinkedEntity MasterPatientEncLinkedEntity { get; set; }
    }
}
