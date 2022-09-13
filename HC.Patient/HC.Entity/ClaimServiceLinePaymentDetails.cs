using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimServiceLinePaymentDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("ClaimServiceLine")]
        public int ServiceLineId { get; set; }
        
        public decimal Amount { get; set; }

        [ForeignKey("MasterPaymentDescription")]
        public int DescriptionType  { get; set; }
        

        [ForeignKey("MasterPaymentType")]
        public int PaymentTypeId { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string AdjustmentGroupCode { get; set; }

        [Column(TypeName = "varchar(6)")]
        public string AdjustmentReasonCode { get; set; }
        
        [ForeignKey("PatientInsuranceDetails")]
        public int? PatientInsuranceId { get; set; }

        [ForeignKey("PatientGuardian")]
        public int? GuarantorId { get; set; }

        [ForeignKey("Patients")]
        public int? PatientId { get; set; }

        public DateTime PaymentDate { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string TransactionHandlingCode { get; set; }

        [Column(TypeName = "char(1)")]
        public string CreditDebitFlagCode { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string PaymentMethodCode { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string PaymentFormatCode { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string EDIReferenceNumber { get; set; }
        public DateTime? ResponseProductionDate { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string SenderDFIType { get; set; }  

        [Column(TypeName = "varchar(50)")]
        public string SenderDFINumber { get; set; } 

        [Column(TypeName = "varchar(5)")]
        public string SenderAccountNumberQualifier { get; set; } 

        [Column(TypeName = "varchar(50)")]
        public string SenderAccountNumber { get; set; } 

        [Column(TypeName = "varchar(5)")]
        public string ReceiverDFIType { get; set; } 
        [Column(TypeName = "varchar(50)")]
        public string ReceiverDFINumber { get; set; } 

        [Column(TypeName = "varchar(5)")]
        public string ReceiverAccountNumberQualifier { get; set; } 
        [Column(TypeName = "varchar(50)")]
        public string ReceiverAccountNumber { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string PayerClaimControlNumber { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string ClaimFillingIndicatorCode { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string ClaimStatusCode { get; set; }

        [Column(TypeName ="varchar(100)")]
        public string CustomReferenceNumber { get; set; }

        [ForeignKey("Claim835ServiceLine")]
        public Nullable<int> Claim835ServiceLineId { get; set; }
        [ForeignKey("PaymentCheckDetail")]
        public Nullable<int> PaymentCheckDetailId { get; set; }
        public virtual Claim835ServiceLine Claim835ServiceLine { get; set; }
        public virtual PaymentCheckDetail PaymentCheckDetail { get; set; }
        public virtual ClaimServiceLine ClaimServiceLine { get; set; }
        public virtual MasterPaymentDescription MasterPaymentDescription { get; set; }
        public virtual MasterPaymentType MasterPaymentType { get; set; }
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual PatientGuardian PatientGuardian { get; set; }
    }
}
