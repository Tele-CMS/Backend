using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterRace : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RaceID")]
        public int Id { get; set; }
        
        public string RaceName { get; set; }
        [NotMapped]
        public string value { get { return this.RaceName; } set { this.RaceName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}