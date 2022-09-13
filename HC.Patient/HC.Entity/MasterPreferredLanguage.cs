using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPreferredLanguage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("LanguageID")]
        public int Id { get; set; }
        
        public string Language { get; set; }
        [NotMapped]
        public string value { get { return this.Language; } set { this.Language = value; } }
        public string Code { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}