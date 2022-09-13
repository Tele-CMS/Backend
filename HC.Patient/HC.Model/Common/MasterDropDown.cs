using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Common
{
    public class MasterDropDown
    {
        public int id { get; set; }
        public string value { get; set; }
        public string key { get; set; }
    }

    public class MasterDropDownForMedication : MasterDropDown
    {
        public string form { get; set; }
        public string Unit { get; set; }
    } 
}
