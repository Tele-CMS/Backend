using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class EDI835ResponseModel
    {
        public string TransactionHandlingCode { get; set; }  //BPR01
        public decimal MonetoryAmount { get; set; } //BPR02
        public string CreditDebitFlagCode { get; set; }  //BPR03
        public string PaymentMethodCode { get; set; } //BPR04
        public string PaymentFormatCode { get; set; } //BPR05
        public string SenderDFIType { get; set; }  //BPR06
        public string SenderDFINumber { get; set; } //BPR07
        public string SenderAccountNumberQualifier { get; set; } //BPR08
        public string SenderAccountNumber { get; set; } //BPR09
        public string ReceiverDFIType { get; set; } //BPR12
        public string ReceiverDFINumber{ get; set; } //BPR13

        public string ReceiverAccountNumberQualifier { get; set; } //BPR14
        public string ReceiverAccountNumber { get; set; } //BPR15
        public string EDIReferenceNumber { get; set; }
        public DateTime ProductionDate { get; set; }
        public List<EDI835ResponseClaims> EDI835ClaimsList { get; set; }
        public List<EDI835ResponseClaimServiceLine> EDI835ResponseClaimServiceLineList { get; set; }
        public List<EDI835ResponseClaimServiceLineAdjustments> EDI835ResponseClaimServiceLineAdjustmentsList { get; set; }
    }
    public class EDI835ResponseClaims
    {
        public int Claim837ClaimId { get; set; } //CLP01
        public string ClaimStatusCode { get; set; } //CLP02
        public decimal AmountClaimed { get; set; } //CLP03
        public decimal AmountApproved { get; set; } //CLP04
        public decimal PatientResponsibilityAmount { get; set; } //CLP05
        public string ClaimFillingIndicatorCode { get; set; } //CLP06
        public string PayerClaimControlNumber { get; set; } //CLP07
    }
    public class EDI835ResponseClaimServiceLine
    {
        public int Claim837ClaimId { get; set; } //CLP01
        public int Claim837ServiceLineId { get; set; }

        public string ServiceCode { get; set; } //SVC01
        public decimal AmountCharged { get; set; } //SVC02
        public decimal AmountApproved { get; set; } //SVC03
        public decimal UnitApproved { get; set; } //SVC05
        public decimal UnitCharged { get; set; } //SVC07
    }
    public class EDI835ResponseClaimServiceLineAdjustments
    {
        public int Claim837ServiceLineId { get; set; }
        public decimal AmountAdjusted { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public decimal UnitApproved { get; set; }
    }
}
