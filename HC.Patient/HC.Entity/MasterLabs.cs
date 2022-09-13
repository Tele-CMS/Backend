using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterLabs : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("LabID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string LabName { get; set; }
        [NotMapped]
        public string value { get { return this.LabName; } set { this.LabName = value; } }
        
        [StringLength(500)]
        public string Address1 { get; set; }
        
        [StringLength(500)]
        public string Address2 { get; set; }
        
        [StringLength(100)]
        public string City { get; set; }
        
        [ForeignKey("MasterState")]
        public int StateID { get; set; }
        
        [StringLength(20)]
        public string Zip { get; set; }
        
        [StringLength(20)]
        public string LabPhone { get; set; }

        [Required]        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
        public MasterState MasterState { get; set; }
    }
}