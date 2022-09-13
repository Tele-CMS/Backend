using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
  public class PatientAndCareTeamMapping:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientAndCareTeamMachingId")]
        public int ID { get; set; }
        [ForeignKey("Staffs")]
        public int CareTeamMemberID { get; set; }
        [ForeignKey("Patients")]
        public int PatientID { get; set; }
        public virtual Staffs Staffs { get; set; }
        public virtual Patients Patients { get; set; }
    }
}
