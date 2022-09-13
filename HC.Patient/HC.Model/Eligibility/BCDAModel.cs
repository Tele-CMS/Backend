using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Eligibility
{
    public class Output
    {
        public string type { get; set; }
        public string url { get; set; }
        public List<ExtensionBCDA> extension { get; set; }
    }

    public class BCDAModel
    {
        public DateTime transactionTime { get; set; }
        public string request { get; set; }
        public bool requiresAccessToken { get; set; }
        public List<Output> output { get; set; }
        public List<object> error { get; set; }
        public int JobID { get; set; }
    }

    public class ExtensionBCDA
    {
        public string url { get; set; }
        public string valueString { get; set; }
        public int? valueDecimal { get; set; }
    }

}
