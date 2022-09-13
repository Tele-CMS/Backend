using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class PatientLabResultsModel
    {
        public DateTime Date { get; set; }
        public string Test { get; set; }
        public string TestValue { get; set; }
        public decimal? HighValue { get; set; }
        public decimal? LowValue { get; set; }
        public string LOINCCode { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public string TextResult { get; set; }
        public bool IsManuallyAdded { get; set; }
        public string Goal { get; set; }
        public int? DecimalPrecision { get; set; }
        public int? SortOrder { get; set; }
        public string ReferenceRange { get; set; }
        public int TotalRecords { get; set; }
        public string OperandValue { get; set; }
        public string DeviceType { get; set; }
        public DateTime SyncDateTime { get; set; }
        public bool IsItalic { get; set; }
        public DateTime? UTCDateTimeForMobile { get; set; }
    }

    public class LoincCodeDetailModel
    {
        public int Id { get; set; }
        public DateTime LabTestDate { get; set; }
        public string Analyte { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal HighValue { get; set; }
        public decimal LowValue { get; set; }
        public string Sign { get; set; }
        public bool IsDeleted { get; set; }
        public string ControlType { get; set; }
        public string TextResult { get; set; }
    }
    public class VitalDateModel
    {
        public DateTime VitalDate { get; set; }
    }

    #region Medication
    public class PatMedicationModel
    {
        public int Id { get; set; }
        public string  DrugName { get; set; }
        public int? Quantity { get; set; }
        public int? SupplyDays { get; set; }
        public string NDCCode { get; set; }
        public string PrescriberNPI { get; set; }
        public string   Pharmacy { get; set; }
        public Nullable<DateTime> FillDate { get; set; }
        public int? MasterMedicationId { get; set; }
        public int PatientID { get; set; }
        public int LinkedEncounterId { get; set; }
    }
    #endregion
}
