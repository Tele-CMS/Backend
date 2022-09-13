using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class GlobalCodeCategory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GlobalCodeCategoryID")]
        public int Id { get; set; }
        
        public string GlobalCodeCategoryName { get; set; }

        [NotMapped]        
        public string value { get { return this.GlobalCodeCategoryName; } set { this.GlobalCodeCategoryName = value; } }

        [Required]        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
