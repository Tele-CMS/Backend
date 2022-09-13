using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SymptomChecker
{
    public class result
    {
        public string triage_level { get; set; }
        public List<Serious> serious { get; set; }
        public string description { get; set; }
        public string label { get; set; }

    }
    public class Serious
    {
        public string id { get; set; }
        public string name { get; set; }
        public string common_name { get; set; }
        public string is_emergenc { get; set; }
    }

}

