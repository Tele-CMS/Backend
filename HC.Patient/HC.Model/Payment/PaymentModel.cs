using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payment
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public decimal Amount { get; set; }
        public int DescriptionTypeId { get; set; }
        public string Notes { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentTypeId { get; set; }
        public int? PatientInsuranceId { get; set; }
        public string ChequeNo { get; set; }
        public int ServiceLineId { get; set; }
        public int? PatientId { get; set; }
        public int? GuarantorId { get; set; }
    }
    public class ExpiryCardNotification
    {
        public string LastFourNumber { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
    }
}
