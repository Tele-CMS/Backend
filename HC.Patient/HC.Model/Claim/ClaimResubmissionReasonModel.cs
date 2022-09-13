using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ClaimResubmissionReasonModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ResubmissionCode { get; set; }
        public string ResubmissionReason { get; set; }
        public string Value { get; set; }
    }
}
