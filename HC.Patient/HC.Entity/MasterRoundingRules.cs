using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterRoundingRules : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RuleID")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string RuleName { get; set; }

        [NotMapped]
        public string value { get { return this.RuleName; } set { this.RuleName = value; } }
        
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}