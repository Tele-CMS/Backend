using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PaymentCheckDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("InsuranceCompanies")]
        public int? PayerId { get; set; }

        [ForeignKey("Patients")]
        public int? PatientId { get; set; }

        [ForeignKey("PatientGuardian")]
        public int? GuarantorId { get; set; }

        [ForeignKey("MasterPaymentType")]
        public int PaymentTypeId { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }

        [StringLength(20)]
        public string RefrenceNo { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        public virtual InsuranceCompanies InsuranceCompanies { get; set; }
        public virtual Staffs Staff { get; set; }
        public virtual MasterPaymentType MasterPaymentType { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual PatientGuardian PatientGuardian { get; set; }


    }
}
