using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounterServiceCodes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("PatientEncounter")]        
        public int PatientEncounterId { get; set; }
        
        [Required]
        [ForeignKey("MasterServiceCode")]
        public int ServiceCodeId { get; set; }

        [ForeignKey("PayerServiceCodeModifiers1")]
        public int? Modifier1 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers2")]
        public int? Modifier2 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers3")]
        public int? Modifier3 { get; set; }

        [ForeignKey("PayerServiceCodeModifiers4")]
        public int? Modifier4 { get; set; }


        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers1 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers2 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers3 { get; set; }
        public virtual PayerServiceCodeModifiers PayerServiceCodeModifiers4 { get; set; }

        [ForeignKey("AuthProcedureCPT")]
        public Nullable<int> AuthProcedureCPTLinkId { get; set; }
        public virtual AuthProcedureCPT AuthProcedureCPT { get; set; }

        public string AuthorizationNumber
        {
            get; set;
        }

            
        public PatientEncounter PatientEncounter { get; set; }
        
        public virtual MasterServiceCode MasterServiceCode { get; set; }
    }
}