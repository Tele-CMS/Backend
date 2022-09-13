using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientReminderAndMessageTypeMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [ForeignKey("MasterMessageType")]
        public int MasterMessageTypeID { get; set; }

        [ForeignKey("PatientReminder")]
        public int PatientReminderId { get; set; }

        public virtual MasterMessageType MasterMessageType { get; set; }
        public virtual PatientReminder PatientReminder { get; set; }

    }
}
