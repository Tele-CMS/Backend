using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.Model
{
    public class EDI271SchemaModel
    {
        public ISA ISA = new ISA();
        public GS GS = new GS();
        public ST ST = new ST();
        public NM1 PayerDetails = new NM1();
        public NM1 SubmitterDetails = new NM1();
        public NM1 SubscriberDetails = new NM1();
        public AAA AAA = new AAA();
        public List<EB> EBList = new List<EB>();
        public List<EB> ActivePlans = new List<EB>();
        public List<EB> InactivePlans = new List<EB>();
        public List<EB> NonCoveredPlan = new List<EB>();
        public List<EB> BaseDeductiblesAllPlans = new List<EB>();
        public List<EB> RemainingDeductiblesAllPlans = new List<EB>();
        public List<EB> CoinsuranceAllPlans = new List<EB>();
        public List<EB> CopayAllPlans = new List<EB>();
    }

    public class AAA
    {
        public string AAA00 { get; set; }
        public string AAA01 { get; set; }
        public string AAA02 { get; set; }
        public string AAA03 { get; set; }
        public string AAA04 { get; set; }

    }
    public class EB
    {
        public string EB00 { get; set; }
        public string EB01 { get; set; }
        public string EB02 { get; set; }
        public string EB03 { get; set; }
        public string EB04 { get; set; }
        public string EB05 { get; set; }
        public string EB06 { get; set; }
        public string EB07 { get; set; }
        public string EB08 { get; set; }
        public string EB09 { get; set; }
        public string EB10 { get; set; }
        public string EB11 { get; set; }
        public string EB12 { get; set; }
        public string EB13 { get; set; }
        public string EB14 { get; set; }

        public DTP DTP=new DTP();
    }
}
