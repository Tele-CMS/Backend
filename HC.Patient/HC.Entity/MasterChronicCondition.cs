using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterChronicCondition : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Color { get; set; }
        public Nullable<bool> IsObserved { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
