using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ProviderQuestionnaireAsnwers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProviderQuestionnaireAsnwerId")]
        public int Id { get; set; }

        [Required]
        //[ForeignKey("ProviderQuestionnaireControls")]
        public int ProviderQuestionnaireControlId { get; set; }
        public virtual ProviderQuestionnaireControls ProviderQuestionnaireControls { get; set; }


        [Required]
        [ForeignKey("PatientAppointment")]
        public int PatientAppointmentId { get; set; }
        public virtual PatientAppointment PatientAppointment { get; set; }

        public string Answer { get; set; }

    }
}
