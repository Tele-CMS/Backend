using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SymptomChecker
{
    public class QuestionResponse
    {
       public QuestionItem question { get; set; }
        public List<Condition> conditions { get; set; }

        public bool should_stop { get; set; }
    }
    public class QuestionItem
    {
        public string type { get; set; }
        public string text { get; set; }
        public List<Item> items { get; set; }
        
    }
    public class Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<symptom> choices { get; set; }
        public string type { get; set; }
    }
    public class Condition
    {
        public string id { get; set; }
        public string name { get; set; }
        public string common_name { get; set; }
        public string probability { get; set; }


    }
        


}
