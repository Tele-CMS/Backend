using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim837Claims : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Claim837Batch")]
        public int Claim837BatchId { get; set; }

        [ForeignKey("Claims")]         
        public int ClaimId { get; set; }

        [ForeignKey("PatientInsuranceDetails")]
        public int PatientInsuranceId { get; set; }

        [ForeignKey("GlobalCode")]
        public int PayerPreference { get; set; }

        [ForeignKey("GlobalCode1")]
        public int? SubmissionType { get; set; }
                
        public virtual Claim837Batch Claim837Batch { get; set; }
                
        public virtual Claims Claims { get; set; }

        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
                
        public virtual GlobalCode GlobalCode { get; set; }
                
        public virtual GlobalCode GlobalCode1 { get; set; }
    }
}
