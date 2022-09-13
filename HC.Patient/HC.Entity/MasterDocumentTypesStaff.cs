using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterDocumentTypesStaff : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public string Type { get; set; }

        [NotMapped]
        public string value { get { return this.Type; } set { this.Type = value; } }

        public int? DisplayOrder { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        
        public virtual Organization Organization { get; set; }
    }
}
