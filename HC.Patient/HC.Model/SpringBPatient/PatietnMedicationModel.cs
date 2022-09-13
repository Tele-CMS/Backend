using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class SpringB_PatietnMedication
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; }
        public string NDC { get; set; }
        public string MedicineName { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyNumber { get; set; }
        public int? DaySupply { get; set; }
        public string ServiceDate { get; set; }
        public string AmountPaid { get; set; }
        public decimal? Quantity { get; set; }
        public string DosageFormName { get; set; }
        public string PrescriberNPI { get; set; }
        public int ClaimId { get; set; }
        public string Color { get; set; }

        public string MedicineDetails { get; set; }
        public int TotalRecords { get; set; }

        public string DetectedCondition { get; set; }
        public bool Flag { get; set; }

        public string AlertType { get; set; }
        public string PrescriberName { get; set; }
    }
}