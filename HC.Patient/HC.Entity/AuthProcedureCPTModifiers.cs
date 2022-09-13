using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AuthProcedureCPTModifiers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthProcedureCPTModifierLinkId")]
        public int Id { get; set; }


        [Required]
        [RequiredNumber]
        [ForeignKey("AuthProcedureCPT")]        
        public int AuthProcedureCPTLinkId { get; set; }

        [Required]        
        [StringLength(20)]
        public string Modifier { get; set; }
                
        public virtual AuthProcedureCPT AuthProcedureCPT { get; set; }
    }
}