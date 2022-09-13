using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterCountry : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CountryID")]
        public int Id { get; set; }
        public string CountryName { get; set; }        

        [NotMapped]
        public string value { get { return this.CountryName; } set { this.CountryName = value; } }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        
        public virtual Organization Organization { get; set; }
    }
}