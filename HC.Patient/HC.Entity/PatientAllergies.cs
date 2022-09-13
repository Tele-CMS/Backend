using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientAllergies : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientAllergyId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]        
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [ForeignKey("MasterAllergies")]
        public int AllergyTypeID { get; set; }
        [StringLength(100)]
        public string Allergen { get; set; }
        [StringLength(2000)]
        public string Note { get; set; }
        [ForeignKey("MasterReaction")]
        public int ReactionID { get; set; }        
        [Obsolete]
        public virtual Patients Patient { get; set; }
        [Obsolete]
        public MasterAllergies MasterAllergies { get; set; }
        [Obsolete]
        public MasterReaction MasterReaction { get; set; }
    }
}