using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class DiagnosticLabResultValueModel
    {
        public string LastResultValue { get; set; }
        public string LastDatePerformed { get; set; }
        public int DescriptionSeq { get; set; }
    }
}
