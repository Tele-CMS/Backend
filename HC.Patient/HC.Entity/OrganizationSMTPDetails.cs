using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class OrganizationSMTPDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        public string ServerName { get; set; }

        [MaxLength(50)]
        public string Port { get; set; }

        [Required]
        [MaxLength(50)]
        public string ConnectionSecurity { get; set; }

        [Required]
        [StringLength(100)]
        public string SMTPUserName { get; set; }

        [Required]
        [StringLength(100)]
        public string SMTPPassword { get; set; }

        public int OrganizationID { get; set; }
        //relation ship tables        
        public virtual Organization Organization { get; set; }
    }
}

