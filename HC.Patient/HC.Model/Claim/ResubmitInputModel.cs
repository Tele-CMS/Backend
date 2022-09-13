using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class ResubmitInputModel
    {
        public int ClaimId { get; set; }
        public string ResubmissionReason { get; set; }
        public string PayerControlReferenceNumber { get; set; }
    }
}
