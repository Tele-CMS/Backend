using HC.Patient.Model.Claim;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HC.Patient.Model.Payment
{
    public class PaymentApplyModel
    {
        public int? PayerId { get; set; }
        public int? GuarantorId { get; set; }
        public int? PatientId { get; set; }
        public string CustomReferenceNumber { get; set; }
        public int PaymentTypeId { get; set; }
        public int DescriptionTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public List<PaymentClaimModel> Claims { get; set; }

        public XElement ClaimsXml { get; set; }
        public XElement ClaimServiceLineAdjustment { get; set; }
        public XElement ClaimServiceLineXml { get; set; }
    }
    public class PaymentClaimModel
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
        public int ServiceLocationId { get; set; }
        public string XmlString { get; set; }  //read xml fro claim encounters
        public List<PaymentClaimEncountersModel> ClaimEncounters { get; set; }
        public List<PaymentClaimServiceLineModel> ClaimServiceLines { get; set; }
        public bool MarkSettled { get; set; }
        public int? ClaimPaymentStatusId { get; set; }
        public DateTime? ClaimSettledDate { get; set; }
    }
    public class PaymentClaimEncountersModel
    {
        //public int Id { get; set; }
        public int PatientEncounterId { get; set; }
        public DateTime DOS { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
    public class PaymentClaimServiceLineModel
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public string ServiceCode { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal Balance { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public List<PayementClaimServiceLineAdjustmentModel> ClaimServiceLineAdjustment { get; set; }
    }
    public class PayementClaimServiceLineAdjustmentModel
    {
        public int ServiceLineId { get; set; }
        public decimal AmountAdjusted { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public int PatientInsuranceId { get; set; }
    }
}