using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SymptomChecker
{
   public class CovidQuestion
    {

        public string sex { get; set; }
        public int age { get; set; }

        public List<CovidEvidence> evidence { get; set; }
    }
    public class CovidEvidence
    {
        public string id { get; set; }
        public string choice_id { get; set; }
    }
}
