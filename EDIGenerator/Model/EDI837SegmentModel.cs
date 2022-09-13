using System;
using System.Collections.Generic;
using System.Text;

namespace EDIGenerator.Model
{
    /// <summary>
    /// SUBMITTER EDI CONTACT INFORMATION
    /// </summary>
    public class PER
    {
        public string PER01 { get; set; }
        public string PER02 { get; set; }
        public string PER03 { get; set; }
        public string PER04 { get; set; }
        public string PER05 { get; set; }
        public string PER06 { get; set; }
    }
    /// <summary>
    /// BILLING PROVIDER SPECIALTY INFORMATION
    /// </summary>
    public class PRV
    {
        public string PRV01 { get; set; }
        public string PRV02 { get; set; }
        public string PRV03 { get; set; }
    }

    /// <summary>
    /// SERVICE LINE NUMBER
    /// </summary>
    public class LX
    {
        public string LX01 { get; set; }
    }
    /// <summary>
    ///  CLAIM INFORMATION, max 100 can be sent in one batch
    /// </summary>
    public class CLM
    {
        public string CLM01 { get; set; }
        public string CLM02 { get; set; }
        public string CLM03 { get; set; }
        public string CLM04 { get; set; }
        //public List<CLM05> CLM05List { get; set; }
        public string CLM05 { get; set; }
        public string CLM06 { get; set; }
        public string CLM07 { get; set; }
        public string CLM08 { get; set; }
        public string CLM09 { get; set; }

        public string CLM10 { get; set; }
    }

    /// <summary>
    ///  CLAIM INFORMATION, Facility details
    /// </summary>
    public class CLM05
    {
        public string CLM05_01 { get; set; }
        public string CLM05_02 { get; set; }
        public string CLM05_03 { get; set; }
    }

    /// <summary>
    /// HEALTH CARE DIAGNOSIS CODE
    /// </summary>
    public class HI
    {
        public string HI01 { get; set; } //BK for ICD9 ABK for ICD10 Primary DX rest are Secondary
                                         //public string HI01_1 { get; set; }
                                         //public string HI02 { get; set; }
                                         //public string HI02_1 { get; set; }
                                         //public string HI03 { get; set; }
                                         //public string HI03_1 { get; set; }
                                         //public string HI04 { get; set; }
                                         //public string HI04_1 { get; set; }
    }

    /// <summary>
    /// PROFESSIONAL SERVICE
    /// </summary>
    public class SV1
    {
        public string SV101 { get; set; }
        public string SV102 { get; set; }
        public string SV103 { get; set; }
        public string SV104 { get; set; }
        public string SV105 { get; set; }
        public string SV106 { get; set; }
        public string SV107 { get; set; }
    }

    /// <summary>
    /// DATE - SERVICE DATE
    /// </summary>
    public class DTP
    {
        public string DTP01 { get; set; } // 472
        public string DTP02 { get; set; } // D8
        public string DTP03 { get; set; }
    }

    /// <summary>
    /// OTHER SUBSCRIBER INFORMATION
    /// </summary>
    public class SBR
    {
        public string SBR01 { get; set; }
        public string SBR02 { get; set; }
        public string SBR03 { get; set; }
        public string SBR04 { get; set; }
        public string SBR05 { get; set; }
        public string SBR06 { get; set; }
        public string SBR07 { get; set; }
        public string SBR08 { get; set; }
        public string SBR09 { get; set; }
        public string SBR10 { get; set; }
    }

    /// <summary>
    ///  SERVICE LINE NUMBER
    /// </summary>
    public class ClaimDateTime
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }

    /// <summary>
    ///  Patient Info
    /// </summary>
    public class PAT
    {
        public string PAT01 { get; set; }
        public string PAT02 { get; set; }
        public string PAT03 { get; set; }
        public string PAT04 { get; set; }
        public string PAT05 { get; set; }
        public string PAT06 { get; set; }
        public string PAT07 { get; set; }
        public string PAT08 { get; set; }
        public string PAT09 { get; set; }
    }
    public class AMT
    {
        public string AMT01 { get; set; }
        public string AMT02 { get; set; }
    }

    public class OI
    {
        public string OI01 { get; set; }
        public string OI02 { get; set; }
        public string OI03 { get; set; }
        public string OI04 { get; set; }
        public string OI05 { get; set; }
        public string OI06 { get; set; }
    }

    /// <summary>
    /// Payment Info
    /// </summary>
    public class SVD
    {
        public string SVD01 { get; set; }
        public string SVD02 { get; set; }
        public string SVD03 { get; set; }
        public string SVD04 { get; set; }
        public string SVD05 { get; set; }
    }
    public class CAS
    {
        public string CAS01 { get; set; }
        public string CAS02 { get; set; }
        public string CAS03 { get; set; }
    }
}
