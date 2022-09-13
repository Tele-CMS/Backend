using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("Staffs")]
        public int StaffId { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("Location")]
        public int LocationID { get; set; }

        public bool IsDefault { get; set; }

        [NotMapped]
        public string LocationName
        {
            get
            {
                return Location != null ? Location.LocationName : null;
            }
        }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        public Staffs Staffs { get; set; }
        public Location Location { get; set; }

        public virtual Organization Organization { get; set; }
    }
}