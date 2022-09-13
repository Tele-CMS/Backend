using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    [Table("OnboardingHeaders")]
    public class OnboardingHeader : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public virtual string Header { get; set; }

        public virtual string HeaderDescription { get; set; }

        public virtual string HeaderImage { get; set; }

        public virtual string HeaderVideo { get; set; }

        public virtual bool ActiveStatus { get; set; }

        public virtual int? OrganizationId { get; set; }

        public virtual string Category { get; set; }

        public virtual int? TotalSteps { get; set; }

        public virtual int? IsImage { get; set; }

        public virtual int? Duration { get; set; }
    }
}
