using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EligibilityEnquiry270Master :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        [ForeignKey("PatientInsuranceDetails")]
        public int PatientInsuranceId { get; set; }

        [Column(TypeName = "text")]
        public string EDIText { get; set; }

        [ForeignKey("MasterEligibilityEnquiryStatus")]
        public int StatusId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual MasterEligibilityEnquiryStatus MasterEligibilityEnquiryStatus { get; set; }
    }
}
