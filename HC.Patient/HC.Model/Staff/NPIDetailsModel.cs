using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class Taxonomy
    {
        public string state { get; set; }
        public string code { get; set; }
        public bool primary { get; set; }
        public string license { get; set; }
        public string desc { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string address_2 { get; set; }
        public string telephone_number { get; set; }
        public string fax_number { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string address_1 { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string address_type { get; set; }
        public string address_purpose { get; set; }
    }

    public class Identifier
    {
        public string code { get; set; }
        public string issuer { get; set; }
        public string state { get; set; }
        public string identifier { get; set; }
        public string desc { get; set; }
    }

    public class Basic
    {
        public string status { get; set; }
        public string credential { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string name { get; set; }
        public string sole_proprietor { get; set; }
        public string gender { get; set; }
        public string last_updated { get; set; }
        public string name_prefix { get; set; }
        public string enumeration_date { get; set; }
    }

    public class Result
    {
        public List<Taxonomy> taxonomies { get; set; }
        public List<Address> addresses { get; set; }
        public int created_epoch { get; set; }
        public List<Identifier> identifiers { get; set; }
        public List<object> other_names { get; set; }
        public int number { get; set; }
        public int last_updated_epoch { get; set; }
        public Basic basic { get; set; }
        public string enumeration_type { get; set; }
    }

    public class NPIDetailsRootObject
    {
        public int result_count { get; set; }
        public List<Result> results { get; set; }
    }
}
