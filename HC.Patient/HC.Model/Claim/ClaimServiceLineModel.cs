using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ClaimServiceLineModel
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }
        public string ServiceCode { get; set; }

        public decimal Rate { get; set; }

        public int Quantity { get; set; }
        public string Value { get; set; }
        public decimal TotalAmount { get; set; }
        public int PatientInsuranceId { get; set; }
        public decimal Balance { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public int? ClinicianId { get; set; }
        public int? RenderingProviderId { get; set; }
        public int? PatientAddressID { get; set; }
        public int? OfficeAddressID { get; set; }
        public int? CustomAddressID { get; set; }
        public string CustomAddress { get; set; }
        public string Clinician { get; set; }
        public string RenderingProvider { get; set; }

        public decimal? RateModifier1 { get; set; }
        public decimal? RateModifier2 { get; set; }
        public decimal? RateModifier3 { get; set; }
        public decimal? RateModifier4 { get; set; }
        public int ServiceFacilityCode { get; set; }
        public bool IsMultiplePractitioner { get; set; }
        public string StaffNames { get; set; }
    }

   
}
