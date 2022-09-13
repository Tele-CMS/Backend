using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class SpringBVitalMobileView
    {
        public DateTime LastDatePerformed { get; set; }
        public string Vital { get; set; }
        public string VitalValue { get; set; }
        public decimal? HighValue { get; set; }
        public decimal? LowValue { get; set; }
        public string LOINCCode { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public string Goal { get; set; }
        public int? DecimalPrecision { get; set; }
        public int? SortOrder { get; set; }
        public string DeviceType { get; set; }
        public DateTime SyncDateTime { get; set; }
        public bool IsManuallyAdded { get; set; }
        public bool IsItalic { get; set; }
        public int TotalRecords { get; set; }
        public DateTime? UTCDateTimeForMobile { get; set; }
    }
}
