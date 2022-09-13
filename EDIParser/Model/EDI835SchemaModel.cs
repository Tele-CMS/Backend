using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.Model
{
    public class EDI835SchemaModel
    {
        public ISA ISA = new ISA();
        public GS GS = new GS();
        public ST ST = new ST();
        public BPR BPR = new BPR();
        public TRN TRN = new TRN();
        public DTM DTM = new DTM();
        public N1 PayerInfo = new N1();
        public N3 PayerAddress = new N3();
        public N4 PayerCityStateZip = new N4();
        public N1 PayeeInfo = new N1();
        public N3 PayeeAddress = new N3();
        public N4 PayeeCityStateZip = new N4();
        public List<CLP> CLPList = new List<CLP>();
    }


    public class CLP
    {
        public string CLP00 { get; set; }
        public string CLP01 { get; set; }
        public string CLP02 { get; set; }
        public string CLP03 { get; set; }
        public string CLP04 { get; set; }
        public string CLP05 { get; set; }
        public string CLP06 { get; set; }
        public string CLP07 { get; set; }
        public string CLP08 { get; set; }
        public string CLP09 { get; set; }
        public string CLP10 { get; set; }
        public string CLP11 { get; set; }
        public string CLP12 { get; set; }
        public string CLP13 { get; set; }
        public string CLP14 { get; set; }
        public List<SVC> SVCList { get; set; }
    }

    public class SVC
    {
        public string SVC00 { get; set; }
        public string SVC01 { get; set; }
        public string SVC02 { get; set; }
        public string SVC03 { get; set; }
        public string SVC04 { get; set; }
        public string SVC05 { get; set; }
        public string SVC06 { get; set; }
        public string SVC07 { get; set; }
        
        public REF REF { get; set; }
        public List<CAS> SVCAdjList { get; set; }
    }

    public class CAS
    {
        public string CAS00 { get; set; }
        public string CAS01 { get; set; }
        public string CAS02 { get; set; }
        public string CAS03 { get; set; }
        public string CAS04 { get; set; }
        public string CAS05 { get; set; }
        public string CAS06 { get; set; }
        public string CAS07 { get; set; }
        public string CAS08 { get; set; }
        public string CAS09 { get; set; }
        public string CAS10 { get; set; }
        public string CAS11 { get; set; }
        public string CAS12 { get; set; }
        public string CAS13 { get; set; }
        public string CAS14 { get; set; }
        public string CAS15 { get; set; }
        public string CAS16 { get; set; }
        public string CAS17 { get; set; }
        public string CAS18 { get; set; }
        public string CAS19 { get; set; }
    }
   
}
