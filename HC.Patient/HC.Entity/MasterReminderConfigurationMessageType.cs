using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterReminderConfigurationAndMessageTypeMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("MasterReminderConfiguration")]
        public int MasterReminderConfigurationId { get; set; }
        [ForeignKey("MasterMessageType")]
        public int MasterMessageTypeID { get; set; }
        public virtual MasterMessageType MasterMessageType { get; set; }
        public virtual MasterReminderConfiguration MasterReminderConfiguration { get; set; }
    }
}
