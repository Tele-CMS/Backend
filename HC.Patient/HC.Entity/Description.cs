using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class Description : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [Column(TypeName ="varchar(250)")]
        public string Descriptions { get; set; }
        [ForeignKey("MasterFrequencyTypes")]
        public int Frequency { get; set; }
        public int FrequencyValue { get; set; }
        public decimal Value { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comment { get; set; }
        [ForeignKey("MasterDescriptionType")]
        public int Type { get; set; }
        [ForeignKey("MasterMeasureSign")]
        public int? Sign { get; set; }
        public bool? MannualAdd { get; set; }
        public virtual MasterFrequencyTypes MasterFrequencyTypes { get; set; }
        public virtual MasterMeasureSign MasterMeasureSign { get; set; }
        public MasterDescriptionType MasterDescriptionType { get; set; }
    }
}
