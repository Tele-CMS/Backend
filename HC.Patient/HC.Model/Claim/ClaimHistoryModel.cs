using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
  public class ClaimHistoryModel
    {
        public int ClaimId { get; set; }
        public DateTime DOS { get; set; }
        public string PatientName { get; set; }
        public string Payer { get; set; }
    }
    public class ClaimHistoryDetailModel
    {
        public int ClaimId { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ColumnName { get; set; }
        public string PatientName { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TotalRecords { get; set; }
        public int ServiceLocationId { get; set; }
    }
}
