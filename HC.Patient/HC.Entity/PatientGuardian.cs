using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientGuardian : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GuardianId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [StringLength(100)]
        public string GuardianFirstName { get; set; }
        [StringLength(100)]
        public string GuardianLastName { get; set; }
        [StringLength(50)]
        public string GuardianMiddleName { get; set; }
        [StringLength(500)]
        public string GuardianAddress1 { get; set; }
        [StringLength(500)]
        public string GuardianAddress2 { get; set; }
        [StringLength(100)]
        public string GuardianCity { get; set; }
        [ForeignKey("MasterState")]
        public int? GuardianState { get; set; }
        [StringLength(10)]
        public string GuardianZip { get; set; }
        [StringLength(20)]
        public string GuardianWorkPhone { get; set; }
        [StringLength(20)]
        public string GuardianHomePhone { get; set; }
        [StringLength(20)]
        public string GuardianMobile { get; set; }
        [StringLength(256)]
        public string GuardianEmail { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterRelationship")]
        public int RelationshipID { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string OtherRelationshipName { get; set; }
        public bool IsGuarantor { get; set; }
        public virtual Patients Patient { get; set; }        
        public MasterState MasterState { get; set; }
        public virtual MasterRelationship MasterRelationship { get; set; }
    }
}