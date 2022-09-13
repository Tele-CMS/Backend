using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class AuthenticationToken : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthenticationTokenID")]        
        public int Id { get; set; }        
        [Required]
        [ForeignKey("User1")]
        
        public int UserID { get; set; }
        
        [StringLength(500)]
        public string Token { get; set; }
        
        [StringLength(500)]
        public string ResetPasswordToken { get; set; }        
        
        
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}
