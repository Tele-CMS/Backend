using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    [Table("OnboardingDetails")] 
    public class OnboardingDetail : BaseEntity
    { 
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string ShortDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual int? OrganizationId { get; set; }

        public virtual int? Order { get; set; }

        public virtual int? OnboardingHeaderId { get; set; }

        [ForeignKey("OnboardingHeaderId")]
        public OnboardingHeader OnboardingHeaderFk { get; set; }

    }
}
