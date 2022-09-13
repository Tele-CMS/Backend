using System;
using System.Collections.Generic;
using System.Text;

namespace EDIGenerator.Model
{
    /// <summary>
    /// INTERCHANGE CONTROL HEADER
    /// </summary>
    public class ISA
    {
        public string ISA01 { get; set; }
        public string ISA02 { get; set; }
        public string ISA03 { get; set; }
        public string ISA04 { get; set; }
        public string ISA05 { get; set; }
        public string ISA06 { get; set; }
        public string ISA07 { get; set; }
        public string ISA08 { get; set; }
        public string ISA09 { get; set; }
        public string ISA10 { get; set; }
        public string ISA11 { get; set; }
        public string ISA12 { get; set; }
        public string ISA13 { get; set; }
        public string ISA14 { get; set; }
        public string ISA15 { get; set; }
        public string ISA16 { get; set; }
    }

    /// <summary>
    /// INTERCHANGE CONTROL TRAILER
    /// </summary>
    public class IEA
    {
        public string IEA01 { get; set; }
        public string IEA02 { get; set; }
    }


    /// <summary>
    /// FUNCTIONAL GROUP HEADER
    /// </summary>
    public class GS
    {
        public string GS01 { get; set; }
        public string GS02 { get; set; }
        public string GS03 { get; set; }
        public string GS04 { get; set; }
        public string GS05 { get; set; }
        public string GS06 { get; set; }
        public string GS07 { get; set; }
        public string GS08 { get; set; }
    }

    /// <summary>
    /// FUNCTIONAL GROUP TRAILER
    /// </summary>
    public class GE
    {
        public string GE01 { get; set; }
        public string GE02 { get; set; }
    }

    /// <summary>
    /// TRANSACTION SET HEADER
    /// </summary>
    public class ST
    {
        public string ST01 { get; set; }
        public string ST02 { get; set; }
        public string ST03 { get; set; }
    }

    /// <summary>
    /// TRANSACTION SET TRAILER
    /// </summary>
    public class SE
    {
        public string SE01 { get; set; }
        public string SE02 { get; set; }
    }

    /// <summary>
    /// BEGINNING OF HIERARCHICAL TRANSACTION
    /// </summary>
    public class BHT
    {
        public string BHT01 { get; set; }
        public string BHT02 { get; set; }
        public string BHT03 { get; set; }
        public string BHT04 { get; set; }
        public string BHT05 { get; set; }
        public string BHT06 { get; set; }
    }

    /// <summary>
    /// SUBMITTER NAME
    /// </summary>
    public class NM1
    {
        public string NM101 { get; set; }
        public string NM102 { get; set; }
        public string NM103 { get; set; }
        public string NM104 { get; set; }
        public string NM105 { get; set; }
        public string NM106 { get; set; }
        public string NM107 { get; set; }
        public string NM108 { get; set; }
        public string NM109 { get; set; }
    }
    /// <summary>
    /// BILLING PROVIDER HIERARCHICAL LEVEL
    /// </summary>
    public class HL
    {
        public string HL01 { get; set; }
        public string HL02 { get; set; }
        public string HL03 { get; set; }
        public string HL04 { get; set; }
    }
    /// <summary>
    /// BILLING PROVIDER ADDRESS
    /// </summary>
    public class N3
    {
        public string N301 { get; set; }
        public string N302 { get; set; }
    }

    /// <summary>
    /// BILLING PROVIDER CITY, STATE, ZIP CODE
    /// </summary>
    public class N4
    {
        public string N401 { get; set; }
        public string N402 { get; set; }
        public string N403 { get; set; }
        //public string N404 { get; set; }
        //public string N405 { get; set; }
        //public string N406 { get; set; }
        //public string N407 { get; set; }
    }

    /// <summary>
    /// PATIENT DEMOGRAPHIC INFORMATION
    /// </summary>
    public class DMG
    {
        public string DMG01 { get; set; }
        public string DMG02 { get; set; }
        public string DMG03 { get; set; }
        //public string DMG04 { get; set; }
        //public string DMG05 { get; set; }
        //public string DMG06 { get; set; }
        //public string DMG07 { get; set; }
        //public string DMG08 { get; set; }
        //public string DMG09 { get; set; }
        //public string DMG10 { get; set; }
        //public string DMG11 { get; set; }

    }

    /// <summary>
    /// PROPERTY AND CASUALTY CLAIM NUMBER
    /// </summary>
    public class REF
    {
        public string REF01 { get; set; }
        public string REF02 { get; set; }
        //public string REF03 { get; set; }
        //public string REF04 { get; set; }

    }
}
