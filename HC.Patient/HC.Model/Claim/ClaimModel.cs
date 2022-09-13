using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ClaimModel
    {
        public int ClaimId { get; set; }
        public DateTime DOS { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int ClinicianId { get; set; }
        public string Clinician { get; set; }
        public string RenderingProvider { get; set; }
        public int RenderingProviderId { get; set; }
        public string Payer { get; set; }
        public int PatientInsuranceId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalRecords { get; set; }
        public string AdditionalClaimInfo { get; set; }
        public int ServiceLocationId { get; set; }
        public string XmlString { get; set; }  //read xml fro claim encounters
        public List<ClaimEncountersModel> ClaimEncounters { get; set; }

        public List<ClaimServiceLineModel> ClaimServiceLines { get; set; }
        public decimal Balance { get; set; }
        public int SubmissionType { get; set; }
        public bool IsEDIPayer { get; set; }
        public int PayerId { get; set; }
        public int? ClaimPaymentStatusId { get; set; }
    }
    public class ClaimsFullDetailModel
    {
        public List<ClaimModel> Claims { get; set; }
        public List<ClaimServiceLineModel> ClaimServiceLines { get; set; }
    }

    public class ClaimBalanceModel
    {
        public int ClaimId { get; set; }      
        public decimal BalanceAmount { get; set; }
    }
}
