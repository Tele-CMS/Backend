using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EncounterSignature
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int? PatientId { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }

        [ForeignKey("PatientEncounter")]
        public int PatientEncounterId { get; set; }

        public Byte[] PatientSign { get; set; }

        public DateTime? PatientSignDate { get; set; }

        public Byte[] ClinicianSign { get; set; }

        public DateTime? ClinicianSignDate { get; set; }

        public Byte[] GuardianSign { get; set; }

        public DateTime? GuardianSignDate { get; set; }

        public string GuardianName { get; set; }

        public virtual Staffs Staff { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
    }
}
