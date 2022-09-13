using System;
using System.Collections.Generic;
using System.Text;

namespace EDIParser.Model
{
   public class EDI999SchemaModel
    {
        public ISA ISA = new ISA();
        public GS GS = new GS();
        public ST ST = new ST();
        public AK1 AK1 = new AK1();
        public AK2 AK2 = new AK2();
        public IK3 IK3 = new IK3();
        public IK4 IK4 = new IK4();
        public IK5 IK5 = new IK5();
        public AK9 AK9 = new AK9();
    }
    public class AK1
    {
        public string AK100 { get; set; }
        public string AK101 { get; set; }
        public string AK102 { get; set; }
        public string AK103 { get; set; }
    }
    public class AK2
    {
        public string AK200 { get; set; }
        public string AK201 { get; set; }
        public string AK202 { get; set; }
        public string AK203 { get; set; }
    }
    public class IK3
    {
        public string IK300 { get; set; }
        public string IK301 { get; set; }
        public string IK302 { get; set; }
        public string IK303 { get; set; }
        public string IK304 { get; set; }
    }
    public class IK4
    {
        public string IK400 { get; set; }
        public string IK401 { get; set; }
        public string IK402 { get; set; }
        public string IK403 { get; set; }
        public string IK404 { get; set; }
    }
    public class IK5
    {
        public string IK500 { get; set; }
        public string IK501 { get; set; }
    }
    public class AK9
    {
        public string AK900 { get; set; }
        public string AK901 { get; set; }
        public string AK902 { get; set; }
        public string AK903 { get; set; }
        public string AK904 { get; set; }
        public string AK905 { get; set; }
    }
}
