using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class StaffCareCategories : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Staffs")]
        public int StaffID { get; set; }

        [Required]
        [ForeignKey("ProviderCareCategory")]
        public int CareCategoryId { get; set; }

        public virtual Staffs Staffs { get; set; }

        public virtual ProviderCareCategory ProviderCareCategory { get; set; }
    }
}
