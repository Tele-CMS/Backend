using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SymptomChecker
{
    public class question
    {
        public extra extras { get; set; }
        public string sex { get; set; }
        public int age { get; set; }

        public List<evidence> evidence { get; set; }

    }


    public class extra
    {
        public bool disable_groups { get; set; }
    }
    public class evidence
    {
        public string source { get; set; }
        public string id { get; set; }
        public string choice_id { get; set; }
    }
}
