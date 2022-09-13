using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientsCurrentMedicationModel
    {
        public int Id { get; set; }
        public int MedicationId { get; set; }
        public string Medication { get; set; }
        public string Dose { get; set; }
        public int? FrequencyId { get; set; }
        public string Frequency { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DaySupply { get; set; }
        public decimal? Refills { get; set; }
        public int? ConditionId { get; set; }
        public string Condition { get; set; }
        public int? ProviderId { get; set; }
        public int PatientId { get; set; }
        public decimal TotalRecords { get; set; }
        public string ProviderName { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public int LinkedEncounterId { get; set; }
        public int? SourceId { get; set; }
        public string Source { get; set; }
        public string Notes { get; set; }
        public string Color { get; set; }
        public string DosageForm { get; set; }
        public string Unit { get; set; }
        public bool IsManualEntry { get; set; }
    }
}
