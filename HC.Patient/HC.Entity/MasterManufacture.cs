using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterManufacture : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ManufactureID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string MVXCode { get; set; }
        [StringLength(500)]
        public string ManufacturerName { get; set; }
        [NotMapped]
        public string value { get { return this.ManufacturerName; } set { this.ManufacturerName = value; } }
        [Required]
        public int Notes { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        
        public virtual Organization Organization { get; set; }
    }
}
