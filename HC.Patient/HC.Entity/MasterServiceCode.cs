using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterServiceCode : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ServiceCodeID")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string ServiceCode { get; set; }

        [NotMapped]
        public string value { get { return this.ServiceCode; } set { this.ServiceCode = value; } }

        [MaxLength(30)]
        public string Type { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int UnitDuration { get; set; }

        [Required]      
        [ForeignKey("MasterUnitType")]
        public int UnitTypeID { get; set; }

        [NotMapped]
        public string UnitTypeName
        {
            get
            {
                    return this.MasterUnitType == null ? null : this.MasterUnitType.UnitTypeName;               
            }            
        }
        public decimal RatePerUnit { get; set; }
        public bool IsBillable { get; set; }

        [Required]
        [ForeignKey("MasterRoundingRules")]
        public int RuleID { get; set; }

        [NotMapped]
        public string RuleName
        {
            get
            {
                return this.MasterRoundingRules == null ? null : this.MasterRoundingRules.RuleName;
            }
        }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public bool IsRequiredAuthorization { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual MasterUnitType MasterUnitType { get; set; }
        public virtual MasterRoundingRules MasterRoundingRules { get; set; }        
        public virtual List<AuthProcedureCPT> AuthProcedureCPT { get; set; }
        public virtual List<MasterServiceCodeModifiers> MasterServiceCodeModifiers { get; set; }
    }
}