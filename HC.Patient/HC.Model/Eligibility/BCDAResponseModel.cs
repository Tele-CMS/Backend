using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Eligibility
{
    public class BCDAResponseModel
    {
        public List<Address> address { get; set; }
        public string birthDate { get; set; }
        public string deceasedDateTime { get; set; }
        public List<Extension> extension { get; set; }
        public string gender { get; set; }
        public string id { get; set; }
        public List<Identifier> identifier { get; set; }
        //public Meta meta { get; set; }
        public List<Name> name { get; set; }
        public string resourceType { get; set; }
    }
        //public class Address
        //{
        //    public string city { get; set; }
        //    public List<string> line { get; set; }
        //    public string postalCode { get; set; }
        //    public string state { get; set; }
        //}

        //public class ValueCoding
        //{
        //    public string code { get; set; }
        //    public string display { get; set; }
        //    public string system { get; set; }
        //}

        //public class Extension
        //{
        //    public string url { get; set; }
        //    public ValueCoding valueCoding { get; set; }
        //    public string valueDate { get; set; }
        //}

        //public class Period
        //{
        //    public string start { get; set; }
        //}

        //public class Identifier
        //{
        //    public string system { get; set; }
        //    public string value { get; set; }
        //    public Period period { get; set; }
        //    public List<Extension> extension { get; set; }
        //}

        //public class Meta
        //{
        //    public DateTime lastUpdated { get; set; }
        //}

        //public class Name
        //{
        //    public string family { get; set; }
        //    public List<string> given { get; set; }
        //    public string use { get; set; }
        //}

}
