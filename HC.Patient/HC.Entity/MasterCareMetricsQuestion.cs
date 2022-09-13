using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterCareMetricsQuestion : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string Question { get; set; }
        public int DisplayOrder { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string FrequencyKey { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
