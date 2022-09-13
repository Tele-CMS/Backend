using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Eligibility
{
    public class BlueButtonModel
    {
        public int ClientId { get; set; }
        public string LoginUrl { get; set; }
    }

    public class PatientEligibilityRequestModel
    {
        public int ApiId { get; set; }
        public int ClientId { get; set; }
        public string AuthToken { get; set; }
    }

    public class BlueButtonAuthorizationModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string patient { get; set; }
    }

    public class PatientBBAuthorizationModel
    {
        public string Code { get; set; }
        public string State { get; set; }
        public int ClientId { get; set; }
        public string ReturnURL { get; set; }
    }

}
