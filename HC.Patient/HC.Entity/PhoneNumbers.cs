using HC.Common.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PhoneNumbers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PhoneNumberId")]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int? PatientID { get; set; }
        [ForeignKey("Staff")]
        public int? StaffID { get; set; }
        public byte[] PhoneNumber { get; set; }
        [Required]
        [RequiredNumber]
        public int PhoneNumberTypeId { get; set; }
        
        [ForeignKey("Preferences")]
        public int PreferenceID { get; set; }

        [StringLength(50)]
        public string OtherPhoneNumberType { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual Staffs Staff { get; set; }
        public virtual GlobalCode Preferences { get; set; }
    }
}
