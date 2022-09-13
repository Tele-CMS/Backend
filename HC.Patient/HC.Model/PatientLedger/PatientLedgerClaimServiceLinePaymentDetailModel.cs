using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientLedger
{
    public class PatientLedgerClaimServiceLinePaymentDetailModel
    {
        public int Id { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public decimal Amount { get; set; }
        public int DescriptionTypeId { get; set; }
        public string Notes { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentTypeId { get; set; }
        public int? PatientInsuranceId { get; set; }
        public int? PatientId { get; set; }
        public int? GuarantorId { get; set; }
        public string PayerName { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public string PaymentType { get; set; }
        public string DescriptionType { get; set; }
        public int ServiceLineId { get; set; }
    }
}
