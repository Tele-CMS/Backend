using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientHealtheScore:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        [ForeignKey("GlobalCode")]
        public int Status { get; set; }
        [ForeignKey("Staffs")]
        public int AssignedBy { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual Staffs Staffs { get; set; }
    }
}
