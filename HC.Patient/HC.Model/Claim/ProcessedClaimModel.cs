using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ProcessedClaimModel
    {
        public int Claim835ClaimId { get; set; }
        public decimal AmountClaimed { get; set; }
        public decimal AmountApproved { get; set; }
        public decimal PatientResponsibilityAmount { get; set; }
        public bool Applied { get; set; }
        public DateTime DOS { get; set; }
        public int ClaimId { get; set; }
        public string PayerName { get; set; }
        public string PatientName { get; set; }
        public string SubmittedAs { get; set; }
        public string ProcessedAs { get; set; }
        public decimal TotalRecords { get; set; }
        public int PatientId { get; set; }
    }
    public class ProcessedClaimServiceLineModel
    {
        public int Claim835ServiceLineId { get; set; }
        public int Claim835ClaimId { get; set; }
        public decimal AmountCharged { get; set; }
        public decimal AmountApproved { get; set; }
        public decimal UnitApproved { get; set; }
        public decimal UnitCharged { get; set; }
        public bool Applied { get; set; }
        public string ServiceCode { get; set; }
    }
    public class ProcessedClaimServiceLineAdjustmentsModel
    {
        public int Claim835ServiceLineAdjustmentId { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public int Claim835ServiceLineId { get; set; }
        public decimal AmountAdjusted { get; set; }
        public decimal UnitApproved { get; set; }
        public bool Applied { get; set; }
    }
}
