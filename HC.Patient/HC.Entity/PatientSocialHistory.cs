using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientSocialHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SocialHistoryId")]
        public  int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [Required]
        [RequiredNumber]
        public int AlcohalID { get; set; }

        [Required]
        [RequiredNumber]
        public int TobaccoID { get; set; }

        [Required]
        [RequiredNumber]
        public int DrugID { get; set; }
        [StringLength(300)]
        public string Occupation { get; set; }
        [Required]
        [RequiredNumber]
        public int TravelID { get; set; }


        //Foreign key's tables
        [Obsolete]
        public virtual Patients Patient { get; set; }
    }
}