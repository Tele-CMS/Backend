using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ReviewRatings:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ReviewRatingId")]
        public int Id { get; set; }
        [ForeignKey("PatientAppointment")]
        public int PatientAppointmentId  { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }

    }
}
