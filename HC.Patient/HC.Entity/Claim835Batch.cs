using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim835Batch :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column(TypeName ="text")]
        public string FileText { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string TransactionHandlingCode { get; set; } 
        public decimal MonetoryAmount { get; set; }

        [Column(TypeName = "char(1)")]
        public string CreditDebitFlagCode { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string PaymentMethodCode { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string PaymentFormatCode { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string EDIReferenceNumber { get; set; }
        public DateTime ProductionDate { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string SenderDFIType { get; set; }  //BPR06

        [Column(TypeName = "varchar(50)")]
        public string SenderDFINumber { get; set; } //BPR07

        [Column(TypeName = "varchar(5)")]
        public string SenderAccountNumberQualifier { get; set; } //BPR08

        [Column(TypeName = "varchar(50)")]
        public string SenderAccountNumber { get; set; } //BPR09

        [Column(TypeName = "varchar(5)")]
        public string ReceiverDFIType { get; set; } //BPR12
        [Column(TypeName = "varchar(50)")]
        public string ReceiverDFINumber { get; set; } //BPR13

        [Column(TypeName = "varchar(5)")]
        public string ReceiverAccountNumberQualifier { get; set; } //BPR14
        [Column(TypeName = "varchar(50)")]

        public string ReceiverAccountNumber { get; set; } //BPR15
    }
}
