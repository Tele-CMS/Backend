using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class EmailModel
    {
        public string ToEmail { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public bool EmailStatus { get; set; }
        public int EmailType { get; set; }
        public int EmailSubType { get; set; }
        public int PrimaryId { get; set; }
        public int CreatedBy { get; set; }
    }
}
