using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientCurrentAndClaimMedicationModel
    {
        public int ID { get; set; }
        public decimal DaySupply { get; set; }
        public string Medication { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public string ServiceDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Refills { get; set; }
        public string Source { get; set; }
        public int ClaimId { get; set; }
        public string NDC { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyNumber { get; set; }
        public string Condition { get; set; }
        public string ProviderName { get; set; }
        public string ClaimNumber { get; set; }
        public string PrescriberNPI { get; set; }
        public string AmountPaid { get; set; }
        public decimal TotalRecords { get; set; }

        public DateTime? PrescribedDate { get; set; }
    }
}
