using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.EDI
{
    public class EDI999FileModel
    {
        public int Id { get; set; }
        public string AcknowledgementType { get; set; }
        public int ControlNumber { get; set; }
        public string Status { get; set; }
        public string EDIFileText { get; set; }
    }
}
