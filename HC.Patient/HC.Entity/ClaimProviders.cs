using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimProviders : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Claims")]
        public int ClaimId { get; set; }

        [ForeignKey("Clinicians")]
        public Nullable<int> ClinicianId { get; set; }
        [ForeignKey("RenderingProviders")]
        public Nullable<int> RenderingProviderId { get; set; }

        public virtual Claims Claims { get;set;}
        public virtual Staffs Clinicians { get; set; }
        public virtual Staffs RenderingProviders { get; set; }

    }
}
