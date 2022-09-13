using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterReminderType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReminderType { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string ReminderTypeValue { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public int DisplayOrdeer { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
