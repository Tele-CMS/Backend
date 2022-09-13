using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterRiskIndicatorBenchmark : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Risk { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LowValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HighValue { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Color { get; set; }

        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
