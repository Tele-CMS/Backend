using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterMessageType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MessageType { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string MessageTypeValue { get; set; }

        public int DisplayOrder { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
