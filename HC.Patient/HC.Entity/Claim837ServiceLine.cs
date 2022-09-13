using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HC.Patient.Entity
{
    public class Claim837ServiceLine : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]        
        public int Id { get; set; }

        [ForeignKey("Claim837Claims")]
        public int Claim837ClaimId { get; set; }

        [ForeignKey("ClaimServiceLine")]
        public int ServiceLineId { get; set; }

        public string AuthorizationNumber { get; set; }

        public decimal Rate { get; set; }

        public int Units { get; set; }

        public Nullable<int> Pointer1 { get; set; }
        public Nullable<int> Pointer2 { get; set; }
        public Nullable<int> Pointer3 { get; set; }
        public Nullable<int> Pointer4 { get; set; }

        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }

        
        public virtual Claim837Claims Claim837Claims { get; set; }
        public virtual ClaimServiceLine ClaimServiceLine { get; set; }
    }
}
