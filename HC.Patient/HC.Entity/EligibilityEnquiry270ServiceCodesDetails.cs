using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EligibilityEnquiry270ServiceCodesDetails :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("EligibilityEnquiry270Master")]
        public int EligibilityEnquiry270MasterId { get; set; }
        [ForeignKey("MasterServiceCode")]
        public int ServiceCodeId { get; set; }
        public virtual EligibilityEnquiry270Master EligibilityEnquiry270Master { get; set; }
        public virtual MasterServiceCode MasterServiceCode { get; set; }
    }
}
