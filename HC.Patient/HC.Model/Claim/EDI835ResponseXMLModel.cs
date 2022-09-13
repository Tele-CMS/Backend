using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HC.Patient.Model.Claim
{
    public class EDI835ResponseXMLModel
    {
        public XElement EDI835Headers { get; set; }
        public XElement EDI835Claims { get; set; }
        public XElement EDI835ClaimServiceLines { get; set; }
        public XElement EDI835ClaimServiceLineAdjustments { get; set; }
    }
}
