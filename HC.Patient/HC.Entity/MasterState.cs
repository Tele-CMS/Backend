using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterState : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StateID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string StateName { get; set; }
        [NotMapped]
        public string value { get { return this.StateName; } set { this.StateName = value; } }
        [StringLength(2)]
        public string StateAbbr { get; set; }
        [ForeignKey("MasterCountry")]
        public int? CountryID { get; set; }
        public decimal? StandardTime { get; set; }
        public decimal? DaylightSavingTime { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}