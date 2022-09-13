using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class SubmittedClaimBatchModel
    {
        public int Id { get; set; }
        public string EDIFileText { get; set; }
        public int NoOfClaims { get; set; }
        public DateTime SentDate { get; set; }
        public string ClearingHouse { get; set; }
        public decimal TotalRecords { get; set; }
        public string SubmittedBy { get; set; }
    }
    public class SubmittedClaimDetailsModel
    {
        public int Id { get; set; }
        public int Claim837BatchId { get; set; }
        public int ClaimId { get; set; }
        public string PayerName { get; set; }
        public string SubmittedPayerPreference { get; set; }
        public string SubmittedAs { get; set; }
    }
    public class SubmittedClaimServiceLineModel
    {
        public int Id { get; set; }
        public string AuthorizationNumber { get; set; }
        public int Claim837ClaimId { get; set; }
        public string ServiceCode { get; set; }
        public decimal Rate { get; set; }
        public int Units { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
    }
}
