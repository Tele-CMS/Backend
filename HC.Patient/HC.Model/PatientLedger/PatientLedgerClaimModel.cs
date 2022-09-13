using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientLedger
{
    public class PatientLedgerClaimModel
    {
        public int ClaimId { get; set; }
        public DateTime DOS { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Clinician { get; set; }
        public string RenderingProvider { get; set; }
        public string Payer { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InsuranceOwes { get; set; }
        public decimal PatientOwes { get; set; }
        public decimal InsurancePayments { get; set; }
        public decimal InsuranceAdjustments { get; set; }
        public decimal PatientPayments { get; set; }
        public decimal PatientAdjustments { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalRecords { get; set; }
        public string XmlString { get; set; }
        public int? ClaimPaymentStatusId { get; set; }
    }
}
