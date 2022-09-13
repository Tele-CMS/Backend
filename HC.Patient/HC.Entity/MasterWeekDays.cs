using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterWeekDays : BaseEntity
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DayId")]
        public int Id { get; set; }
        public string Day { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual List<StaffAvailability> StaffAvailability { get; set; }
    }
}