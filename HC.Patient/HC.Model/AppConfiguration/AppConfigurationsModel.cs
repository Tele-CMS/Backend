using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.AppConfiguration
{
    public class AppConfigurationsModel
    {   
        public int Id { get; set; }
        public string Key { get; set; }        
        public string Label { get; set; }
        public string Value { get; set; }
        public int ConfigType { get; set; }
    }
}
