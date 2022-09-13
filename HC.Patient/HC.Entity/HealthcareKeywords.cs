using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public  class HealthcareKeywords : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string KeywordName { get; set; }

        [Required]
        [ForeignKey("ProviderCareCategory")]
        public int CareCategoryId { get; set; }

        public virtual ProviderCareCategory ProviderCareCategory { get; set; }
    }
}
