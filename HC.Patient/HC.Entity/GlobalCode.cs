using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class GlobalCode : BaseEntity
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GlobalCodeID")]
        public int Id { get; set; }     
        public string GlobalCodeName { get; set; }
        
        public string GlobalCodeValue { get; set; }
                
        [ForeignKey("GlobalCodeCategory")]
        public int GlobalCodeCategoryID { get; set; }
                
        public int? DisplayOrder { get; set; }

        [NotMapped]        
        public string value { get { return this.GlobalCodeValue; } set { this.GlobalCodeValue = value; } }

        [Required]        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual GlobalCodeCategory GlobalCodeCategory { get; set; }
        [NotMapped]
        public string SpecialityIcon { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoThumbnailPath { get; set; }
    }
}
