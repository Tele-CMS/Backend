using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffPayrollRateForActivity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int AppointmentTypeId { get; set; }
        public decimal PayRate { get; set; }
        
        public virtual Staffs Staffs { get; set; }        
        public virtual AppointmentType AppointmentType { get; set; }
    }
}
