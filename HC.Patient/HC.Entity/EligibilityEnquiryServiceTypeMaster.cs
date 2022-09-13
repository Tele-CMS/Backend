using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EligibilityEnquiryServiceTypeMaster :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }
        [Column(TypeName ="varchar(5)")]
        public string ServiceTypeCode { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
