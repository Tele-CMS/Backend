using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterDescriptionCode : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [ForeignKey("Description")]
        public int DescriptionID { get; set; }
        [ForeignKey("MasterCodeType")]
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public virtual MasterCodeType MasterCodeType { get; set; }
        public virtual Description Description { get; set; }
    }
}   
