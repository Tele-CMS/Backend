using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EDI999AcknowledgementMaster : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string EDIFileText { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ResponseType { get; set; }
        [ForeignKey("Claim837Batch")]
        public int? Claim837BatchId { get; set; }
        [ForeignKey("EligibilityEnquiry270Master")]
        public int? EligibilityEnquiry270MasterId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [Obsolete]
        public virtual Claim837Batch Claim837Batch { get; set; }
        [Obsolete]
        public virtual EligibilityEnquiry270Master EligibilityEnquiry270Master { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
