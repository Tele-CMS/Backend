using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientGuardianModel
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public string GuardianFirstName { get; set; }
        public string GuardianLastName { get; set; }
        public string GuardianMiddleName { get; set; }
        public string GuardianAddress1 { get; set; }
        public string GuardianAddress2 { get; set; }
        public string GuardianCity { get; set; }
        public int? GuardianState { get; set; }
        public string GuardianZip { get; set; }
        public string GuardianWorkPhone { get; set; }
        public string GuardianHomePhone { get; set; }
        public string GuardianMobile { get; set; }
        public string GuardianEmail { get; set; }
        public int RelationshipID { get; set; }
        public string OtherRelationshipName { get; set; }
        public bool IsGuarantor { get; set; }
        public bool IsActive { get; set; }
        public string RelationshipName { get; set; }        
        public decimal TotalRecords { get; set; }
    }
}
