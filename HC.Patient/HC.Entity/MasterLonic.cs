using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterLonic : BaseEntity
    {  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("LonicID")]
        public int Id { get; set; }     
        public string LonicCode { get; set; }
        [NotMapped]
        public string value { get { return this.LonicCode; } set { this.LonicCode = value; } }
        public string Description { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}