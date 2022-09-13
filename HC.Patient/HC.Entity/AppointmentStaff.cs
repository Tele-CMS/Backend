using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{

    public class AppointmentStaff : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("PatientAppointment")]
        public int PatientAppointmentID { get; set; }

        [ForeignKey("Staffs")]

        public int StaffID { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                try
                {
                    return Staffs == null ? "" : Staffs.FirstName + " " + Staffs.MiddleName + " " + Staffs.LastName;
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            }
        }
        public virtual Staffs Staffs { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }
    }
}
