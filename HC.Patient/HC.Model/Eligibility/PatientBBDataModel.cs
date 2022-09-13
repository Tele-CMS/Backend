using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Eligibility
{
    public class PatientBBDataModel
    {
            public string resourceType { get; set; }
            public string id { get; set; }
            //public Meta meta { get; set; }
            public List<Extension> extension { get; set; }
            public List<Identifier> identifier { get; set; }
            public List<Name> name { get; set; }
            public List<Telecom> telecom { get; set; }
            public string gender { get; set; }
            public string birthDate { get; set; }
            public string deceasedDateTime { get; set; }
            public List<Address> address { get; set; }
        public string type { get; set; }
        public int total { get; set; }
        public List<Entry> entry { get; set; }
    }

    //public class Meta
    //{
    //    public DateTime lastUpdated { get; set; }
    //}

    public class ValueCoding
    {
        public string system { get; set; }
        public string code { get; set; }
        public string display { get; set; }
    }

    public class Extension
    {
        public string url { get; set; }
        public ValueCoding valueCoding { get; set; }
        public string valueDate { get; set; }
        public ValueIdentifier valueIdentifier { get; set; }
        public ValueQuantity valueQuantity { get; set; }
    }

    public class Identifier
    {
        public string system { get; set; }
        public string value { get; set; }
        public Period period { get; set; }
    }

    public class Name
    {
        public string use { get; set; }
        public string family { get; set; }
        public List<string> given { get; set; }
    }

    public class Telecom
    {
        public string system { get; set; }
        public string value { get; set; }
        public string use { get; set; }
    }

    public class Address
    {
        public List<string> line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }

    public class PatientEOBBlueButtonModel
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        //public Meta meta { get; set; }
        public string type { get; set; }
        public int total { get; set; }
        public List<Link> link { get; set; }
        public List<Entry> entry { get; set; }
    }

    public class Link
    {
        public string relation { get; set; }
        public string url { get; set; }
    }

    public class Coding
    {
        public string system { get; set; }
        public string code { get; set; }
        public string display { get; set; }
        public string version { get; set; }
    }

    public class Type
    {
        public List<Coding> coding { get; set; }
    }

    public class Patient
    {
        public string reference { get; set; }
    }

    public class BillablePeriod
    {
        public string start { get; set; }
        public string end { get; set; }
        public List<Extension> extension { get; set; }
    }

    public class DiagnosisCodeableConcept
    {
        public List<Coding> coding { get; set; }
    }

    public class PackageCode
    {
        public List<Coding> coding { get; set; }
    }

    public class Organization
    {
        public Identifier identifier { get; set; }
    }

    public class Facility
    {
        public List<Extension> extension { get; set; }
        public Identifier identifier { get; set; }
    }

    public class Category
    {
        public List<Coding> coding { get; set; }
    }

    public class Code
    {
        public List<Coding> coding { get; set; }
    }

    public class Information
    {
        public int sequence { get; set; }
        public Category category { get; set; }
        public Code code { get; set; }
    }

    public class Provider
    {
        public Identifier identifier { get; set; }
    }

    public class Role
    {
        public List<Coding> coding { get; set; }
    }

    public class CareTeam
    {
        public int sequence { get; set; }
        public Provider provider { get; set; }
        public Role role { get; set; }
    }

    public class ValueIdentifier
    {
        public List<Extension> extension { get; set; }
        public string system { get; set; }
        public string value { get; set; }
    }

    public class Coverage
    {
        public List<Extension> extension { get; set; }
        public string reference { get; set; }
    }

    public class Insurance
    {
        public Coverage coverage { get; set; }
    }

    public class Service
    {
        public List<Coding> coding { get; set; }
    }

    public class ValueQuantity
    {
        public int value { get; set; }
    }

    public class Quantity
    {
        public List<Extension> extension { get; set; }
        public double value { get; set; }
        //public int value { get; set; }
    }

    public class Reason
    {
        public List<Coding> coding { get; set; }
    }

    public class Amount
    {
        public double value { get; set; }
        public string system { get; set; }
        public string code { get; set; }
    }

    public class Adjudication
    {
        public Category category { get; set; }
        public Reason reason { get; set; }
        public Amount amount { get; set; }
    }

    public class Detail
    {
        public Type type { get; set; }
    }

    //public class Item
    //{
    //    public int sequence { get; set; }
    //    public List<int> careTeamLinkId { get; set; }
    //    public Service service { get; set; }
    //    public string servicedDate { get; set; }
    //    public Quantity quantity { get; set; }
    //    public List<Adjudication> adjudication { get; set; }
    //    public List<Detail> detail { get; set; }
    //}

    public class Payment
    {
        public string date { get; set; }
    }

    //public class Resource
    //{
    //    public string resourceType { get; set; }
    //    public string id { get; set; }
    //    //public Meta meta { get; set; }
    //    public List<Identifier> identifier { get; set; }
    //    public string status { get; set; }
    //    public Type type { get; set; }
    //    public Patient patient { get; set; }
    //    public BillablePeriod billablePeriod { get; set; }
    //    public Organization organization { get; set; }
    //    public Facility facility { get; set; }
    //    public List<Information> information { get; set; }
    //    public List<CareTeam> careTeam { get; set; }
    //    public Insurance insurance { get; set; }
    //    public List<Item> item { get; set; }
    //    public Payment payment { get; set; }
    //}

    public class Entry
    {
        public Resource resource { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Meta
    //{
    //    public DateTime lastUpdated { get; set; }
    //}

    public class Beneficiary
    {
        public string reference { get; set; }
    }


    public class PolicyHolder
    {
        public string reference { get; set; }
    }

    public class Grouping
    {
        public string subGroup { get; set; }
        public string subPlan { get; set; }
        public string group { get; set; }
        public string groupDisplay { get; set; }
        public string subGroupDisplay { get; set; }
        public string plan { get; set; }
        public string planDisplay { get; set; }
        public string subPlanDisplay { get; set; }
        public string @class { get; set; }
        public string classDisplay { get; set; }
        public string subClass { get; set; }
        public string subClassDisplay { get; set; }
}

    public class Contract
    {
        public string id { get; set; }
        public string reference { get; set; }
    }

    public class Resource
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public List<Extension> extension { get; set; }
        public string status { get; set; }
        public Type type { get; set; }
        public Beneficiary beneficiary { get; set; }
        public Period period { get; set; }
        public Grouping grouping { get; set; }
        public List<Contract> contract { get; set; }
    }

    public class Subscriber
    {
        public string reference { get; set; }
    }

    public class Relationship
    {
        public List<Coding> coding { get; set; }
    }

    public class Period
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Payor
    {
        public string reference { get; set; }
    }

    public class Class
    {
        public Type type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
    }

    public class PatientBBCoverageData
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        //public Meta meta { get; set; }
        public string type { get; set; }
        public int total { get; set; }
        public List<Link> link { get; set; }
        public List<Entry> entry { get; set; }
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        //public Type type { get; set; }
        public Subscriber subscriber { get; set; }
        public string subscriberId { get; set; }
        public Beneficiary beneficiary { get; set; }
        public string dependent { get; set; }
        public Relationship relationship { get; set; }
        public Period period { get; set; }
        public List<Payor> payor { get; set; }
        public List<Class> @class { get; set; }
        public PolicyHolder policyHolder { get; set; }
        public string sequence { get; set; }
        
        public string network { get; set; }
        public int order { get; set; }
        public Contract contract { get; set; }
    }

    public class Diagnosi
    {
        public int sequence { get; set; }
        public DiagnosisCodeableConcept diagnosisCodeableConcept { get; set; }
        public List<Type> type { get; set; }
        public PackageCode packageCode { get; set; }
    }

    public class ServicedPeriod
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class LocationCodeableConcept
    {
        public List<Extension> extension { get; set; }
        public List<Coding> coding { get; set; }
    }

    public class LocationAddress
    {
        public string state { get; set; }
    }

    public class Item
    {
        public int sequence { get; set; }
        public Service service { get; set; }
        public ServicedPeriod servicedPeriod { get; set; }
        public LocationCodeableConcept locationCodeableConcept { get; set; }
        public Quantity quantity { get; set; }
        public List<int> careTeamLinkId { get; set; }
        public LocationAddress locationAddress { get; set; }
        public string servicedDate { get; set; }
        public List<Adjudication> adjudication { get; set; }
        public List<Detail> detail { get; set; }
    }

    public class ProcedureCodeableConcept
    {
        public List<Coding> coding { get; set; }
    }

    public class Procedure
    {
        public int sequence { get; set; }
        public ProcedureCodeableConcept procedureCodeableConcept { get; set; }
        public DateTime? date { get; set; }
    }

    public class AB2DEOBModel
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        //public Meta meta { get; set; }
        public List<Extension> extension { get; set; }
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public Type type { get; set; }
        public Patient patient { get; set; }
        public BillablePeriod billablePeriod { get; set; }
        public List<Diagnosi> diagnosis { get; set; }
        public int precedence { get; set; }
        public List<Item> item { get; set; }
        public Provider provider { get; set; }
        public Organization organization { get; set; }
        public Facility facility { get; set; }
        public List<CareTeam> careTeam { get; set; }
        public List<Procedure> procedure { get; set; }
    }




}
