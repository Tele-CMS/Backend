using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
   public class PatientGuarantorModel
    {
        public int GuardianId { get; set; }
        public string GuarantorName { get; set; }
        public string RelationshipName { get; set; }
        public string GuardianHomePhone { get; set; }
    }
}
