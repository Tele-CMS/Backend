using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class SubcriptionPlan 
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SubcriptionPlanID")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; }
        
        [Required]
        [RequiredDate]
        public DateTime StartDate { get; set; }

        [Required]
        [RequiredDate]
        public DateTime PlanType { get; set; }

        [Required]
        [RequiredDate]
        public Decimal AmountPerClient { get; set; }

        [Required]
        public int OrganizationID { get; set; }
    }
}
