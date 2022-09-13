using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayerServiceCodes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("InsuranceCompanies")]
        public int PayerId { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int UnitDuration { get; set; }

        [Required]
        [ForeignKey("MasterUnitType")]
        public int UnitType { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal? NewRatePerUnit { get; set; }
        public bool IsBillable { get; set; }
        public bool IsRequiredAuthorization { get; set; }

        [Required]
        [ForeignKey("MasterServiceCode")]
        public int ServiceCodeId { get; set; }

        [ForeignKey("MasterRoundingRules")]
        public int RuleID { get; set; }
        public DateTime? EffectiveDate { get; set; }

        [NotMapped]
        public string value
        {
            get{return MasterServiceCode == null ? null : MasterServiceCode.ServiceCode;}
        }

       
        public virtual InsuranceCompanies InsuranceCompanies { get; set; }
        public virtual MasterUnitType MasterUnitType { get; set; }
        public virtual MasterServiceCode MasterServiceCode { get; set; }
        public virtual MasterRoundingRules MasterRoundingRules { get; set; }
        public virtual List<PayerServiceCodeModifiers> PayerServiceCodeModifiers { get; set; }
    }
}
