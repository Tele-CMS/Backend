using HC.Common.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AuthProcedureCPT : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthProcedureCPTLinkId")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("authorizationProcedure")]
        public int AuthorizationProceduresId { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("MasterCPT")]
        public int CPTID { get; set; }
        public MasterServiceCode MasterCPT { get; set; }
        public int? UnitConsumed { get; set; }
        public int? BlockedUnit { get; set; }
        public virtual AuthorizationProcedures AuthorizationProcedure { get; set; }
        public virtual List<AuthProcedureCPTModifiers> AuthProcedureCPTModifiers { get; set; }
    }
}