using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HC.Patient.Model.Patient
{
    public class PHIEncryptedModel
    {
        public byte[] FirstName { get; set; }
        public byte[] MiddleName { get; set; }
        public byte[] LastName { get; set; }
        public byte[] DateOfBirth { get; set; }
        public byte[] EmailAddress { get; set; }
        public byte[] SSN { get; set; }
        public byte[] MRN { get; set; }
        public byte[] Aptnumber { get; set; }
        public byte[] Address1 { get; set; }
        public byte[] Address2 { get; set; }
        public byte[] City { get; set; }
        public byte[] ZipCode { get; set; }
        public byte[] Phonenumber { get; set; }
        public byte[] HealthPlanBeneficiaryNumber { get; set; }
    }
    public class PHIDecryptedModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }        
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string SSN { get; set; }
        public string MRN { get; set; }
        public string Aptnumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phonenumber { get; set; }
        public string HealthPlanBeneficiaryNumber { get; set; }
        public string SecondaryEmailAddress { get; set; }
    }



    public class PHIMultipleEncryptedModel
    {
        public int Index { get; set; }
        public byte[] Value { get; set; }
    }

}
