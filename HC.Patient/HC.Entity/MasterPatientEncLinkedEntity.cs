using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterPatientEncLinkedEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string DisplayName { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
