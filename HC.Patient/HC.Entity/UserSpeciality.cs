using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserSpeciality : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Speciality { get; set; }
        public int StaffId { get; set; }
        public Staffs Staffs { get; set; }
        public int SpecialityId { get; set; }
    }
}
