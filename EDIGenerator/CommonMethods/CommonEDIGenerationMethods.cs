using EDIGenerator.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EDIGenerator.CommonMethods
{
    public static class CommonEDIGenerationMethods
    {
        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Return a valid length string for edi segment </Description>
        ///</Summary>

        public static string CreateString(string Value, int MinLength, int MaxLength, char PaddingChar, params bool[] isRequiredAndIsPadRight)
        {
            bool isRequired = false, isPadRight = false;
            if (isRequiredAndIsPadRight.Length == 2)
            {
                isRequired = isRequiredAndIsPadRight[0];
                isPadRight = isRequiredAndIsPadRight[1];
            }
            else
            {
                isRequired = isRequiredAndIsPadRight[0];
            }
            string tempVar = string.Empty;
            tempVar = !isRequired && String.IsNullOrEmpty(Value) ? "" :
                        (String.IsNullOrEmpty(Value) ?
                        (isPadRight ? tempVar.PadRight(MinLength, PaddingChar) : tempVar.PadLeft(MinLength, PaddingChar)) :
                            (
                                (MinLength <= Value.Length) && (Value.Length <= MaxLength) ?
                                Value :
                                (
                                    Value.Length < MinLength ?
                                    (isPadRight ? Value.PadRight(MinLength, PaddingChar) : Value.PadLeft(MinLength, PaddingChar)) :
                                    Value.Substring(0, MaxLength)
                                 )
                            )
                          );
            return tempVar;
        }

        public static string GetSegment<T>(T instance, string segment)
        {
            StringBuilder sbSeg = new StringBuilder();
            sbSeg.Append(segment);
            sbSeg.Append("*");
            Type type = instance.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                sbSeg.Append(property.GetValue(instance, null));
                sbSeg.Append("*");
            }
            sbSeg.Replace("*", "", sbSeg.ToString().LastIndexOf("*"), 1);
            sbSeg.Append("~");
            return sbSeg.ToString();
        }

        #region EDI Header generation methods
        ///<Summary>
        ///<CreatedOn>08/08/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for ISA segments(Interchange Contoller Header)</Description>
        ///</Summary>
        public static ISA GetISA(string ISA01,string ISA02,string ISA03, string ISA04, string ISA05,
            string ISA06, string ISA07, string ISA08, string ISA09, string ISA10, string ISA11,
            string ISA12, string ISA13, string ISA14, string ISA15, string ISA16)
        {
            ISA obj = new ISA();
            obj.ISA01 = ISA01;    // ISA01 and ISA03 are used in case of security purpose otherwise set to 00
            obj.ISA02 = string.Empty.PadRight((int)SegmentLength.Ten);  // Requred ten empty spaces in this segment element as per edi standard
            obj.ISA03 = ISA03;
            obj.ISA04 = string.Empty.PadRight((int)SegmentLength.Ten);// Requred ten empty spaces in this segment element as per edi standard
            obj.ISA05 = ISA05;
            obj.ISA06 = CreateString(ISA06, (int)SegmentLength.Fifteen, (int)SegmentLength.Fifteen, ' ', true, true); //Sender Id will be of 15 chararcters
            obj.ISA07 = ISA07;
            obj.ISA08 = CreateString(ISA08, (int)SegmentLength.Fifteen, (int)SegmentLength.Fifteen, ' ', true, true); //Receiver Id will be of 15 characters
            obj.ISA09 = ISA09;
            obj.ISA10 = ISA10;
            obj.ISA11 = ISA11;
            obj.ISA12 = ISA12;
            /////Batch Id is the Control No.All the claims will be under this control number
            obj.ISA13 = ISA13;
            obj.ISA14 = ISA14;
            obj.ISA15 = ISA15;
            obj.ISA16 = ISA16;
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>08/08/2018<</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for GS segments(Functional Group Header)</Description>
        ///</Summary>
        public static GS GetGS(string GS01,string GS02, string GS03, string GS04, string GS05, string GS06
                , string GS07, string GS08)
        {
            GS obj = new GS();
            obj.GS01 = GS01;
            obj.GS02 = CreateString(GS02, (int)SegmentLength.Two, (int)SegmentLength.Fifteen, ' ', true, true);//Sender Id from clearing house
            obj.GS03 = CreateString(GS03, (int)SegmentLength.Two, (int)SegmentLength.Fifteen, ' ', true, true);//Receiver Id from Clearing house
            obj.GS04 = GS04; //Current date
            obj.GS05 = GS05;//Time
            obj.GS06 = CreateString(GS06, (int)SegmentLength.One, (int)SegmentLength.Nine, '0', true, true); //We can use Interchange Controller number here too
            obj.GS07 = GS07;
            obj.GS08 = GS08;
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>08/08/2018<</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for ST segments(Transaction Set Header)</Description>
        ///</Summary>
        public static ST GetST(string ST01,string ST02,string ST03) //Transaction Header
        {
            ST obj = new ST();
            obj.ST01 = ST01;
            obj.ST02 = ST02;
            obj.ST03 = ST03;
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for BHT segments(Beginning of Hierarchical Transaction)</Description>
        ///</Summary>
        public static BHT GetBHT(string BHT01, string BHT02, string BHT03, string BHT04, string BHT05, string BHT06)
        {
            BHT obj = new BHT();
            obj.BHT01 = BHT01;
            obj.BHT02 = BHT02; //"18 for resubmision"
            obj.BHT03 = BHT03;//It length can be 1 to 50 
            obj.BHT04 = BHT04;
            obj.BHT05 = BHT05;
            obj.BHT06 = BHT06;
            return obj;
        }
        #endregion


        #region Common segment generation Methods for all EDI Files
        ///<Summary>
        ///<CreatedOn>03/01/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Get HL segment for All</Description>
        ///</Summary>
        ///

        public static HL GetHL(string HL01, string HL02, string HL03, string HL04)
        {
            HL HL = new HL();
            HL.HL01 = CreateString(HL01.ToString(), (int)SegmentLength.One, (int)SegmentLength.Twelve, '0', true);
            HL.HL02 = !string.IsNullOrEmpty(HL02) ? CreateString(HL02.ToString(), (int)SegmentLength.One, (int)SegmentLength.Twelve, '0', true) : string.Empty;
            HL.HL03 = HL03;
            HL.HL04 = HL04;
            return HL;
        }
        ///<Summary>
        ///<CreatedOn>03/01/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Get NM1 segment for All</Description>
        ///</Summary>
        ///
        public static NM1 GetNM1(string NM101, string NM102, string NM103, string NM104, string NM105, string NM106, string NM107, string NM108, string NM109)
        {
            NM1 NM1 = new NM1();
            NM1.NM101 = NM101;
            NM1.NM102 = NM102;
            NM1.NM103 = !string.IsNullOrEmpty(NM103) ? CreateString(NM103, (int)SegmentLength.One, (int)SegmentLength.Sixty, ' ', true) : string.Empty;
            NM1.NM104 = !string.IsNullOrEmpty(NM104) ? CreateString(NM104, (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true) : string.Empty;
            NM1.NM105 = !string.IsNullOrEmpty(NM105) ? CreateString(NM105, (int)SegmentLength.One, (int)SegmentLength.TwentyFive, ' ', true) : string.Empty;
            NM1.NM106 = !string.IsNullOrEmpty(NM106) ? NM106 : string.Empty;
            NM1.NM107 = !string.IsNullOrEmpty(NM107) ? NM107 : string.Empty;
            NM1.NM108 = !string.IsNullOrEmpty(NM108) ? NM108 : string.Empty;
            NM1.NM109 = CreateString(NM109, (int)SegmentLength.Two, (int)SegmentLength.Eighty, '0', true);
            return NM1;
        }

        ///<Summary>
        ///<CreatedOn>03/01/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Get N3 segment for All</Description>
        ///</Summary>
        ///
        public static N3 GetN3(string N301, string N302)
        {
            N3 N3 = new N3();
            N3.N301 = CreateString(N301, (int)SegmentLength.One, (int)SegmentLength.FiftyFive, ' ', true);//Address1
            N3.N302 = !string.IsNullOrEmpty(N302) ? CreateString(N302, (int)SegmentLength.One, (int)SegmentLength.FiftyFive, ' ', true) : string.Empty;//Address2
            return N3;
        }

        ///<Summary>
        ///<CreatedOn>03/01/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Get N4 segment for All</Description>
        ///</Summary>
        ///
        public static N4 GetN4(string N401, string N402, string N403)
        {
            N4 N4 = new N4();
            N4.N401 = CreateString(N401, (int)SegmentLength.Two, (int)SegmentLength.Thirty, ' ', true);//City
            N4.N402 = CreateString(N402, (int)SegmentLength.Two, (int)SegmentLength.Two, ' ', true);//State
            N4.N403 = CreateString(N403, (int)SegmentLength.Three, (int)SegmentLength.Fifteen, ' ', true);//ZipCode
            return N4;
        }

        ///<Summary>
        ///<CreatedOn>03/01/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Get REF segment for All</Description>
        ///</Summary>
        ///
        public static REF GetREF(string REF01, string REF02)
        {
            REF REF = new REF();
            REF.REF01 = CreateString(REF01, (int)SegmentLength.Two, (int)SegmentLength.Three, ' ', true);
            REF.REF02 = CreateString(REF02, (int)SegmentLength.One, (int)SegmentLength.Fifty, ' ', true);
            return REF;
        }

        ///<Summary>
        ///<CreatedOn>08/09/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for subscriber Demographics segment</Description>
        ///</Summary>
        public static DMG GetDMG(string DMG01, string DMG02, string DMG03)
        {
            DMG obj = new DMG();
            obj.DMG01 = DMG01;
            obj.DMG02 = DMG02; //CreateString(Convert.ToDateTime(DMG02).ToString("yyyyMMdd"), (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true);
            //obj.DMG03 = CreateString(DMG02, (int)SegmentLength.One, (int)SegmentLength.One, 'U', true);
            return obj;
        }
        ///<Summary>
        ///<CreatedOn>08/09/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for DTP segment</Description>
        ///</Summary>
        public static DTP GetDTP(string DTP01, string DTP02, string DTP03)
        {
            DTP dtp = new DTP();
            dtp.DTP01 = DTP01;
            dtp.DTP02 = DTP02;
            dtp.DTP03 = DTP03;// CreateString(DTP03, (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true); //CreateString(Convert.ToDateTime(DTP03).ToString("yyyyMMdd"), (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true);
            return dtp;
        }

        #endregion

        #region EDI Trailor Generation Methods
        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for SE segment</Description>
        ///</Summary>
        public static SE GetSE(string segmentCount, string transSetControlNo) //Transaction Trailer
        {
            SE obj = new SE();
            obj.SE01 = CreateString(segmentCount, (int)SegmentLength.One, (int)SegmentLength.Ten, '0', true);//segmentCount;
            obj.SE02 = CreateString(transSetControlNo, (int)SegmentLength.Four, (int)SegmentLength.Nine, '0', true);//transSetControlNo.PadLeft(9, '0');
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for GE segment</Description>
        ///</Summary>
        public static GE GetGE(string tranSetControlNo, string controlNo)
        {
            GE obj = new GE();
            obj.GE01 = CreateString(tranSetControlNo, (int)SegmentLength.One, (int)SegmentLength.Six, '0', true);
            obj.GE02 = CreateString(controlNo, (int)SegmentLength.One, (int)SegmentLength.Nine, '0', true);
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for IEA segment</Description>
        ///</Summary>
        public static IEA GetIEA(string controlNo)
        {
            IEA obj = new IEA();
            obj.IEA01 = "1";
            obj.IEA02 = CreateString(controlNo, (int)SegmentLength.Nine, (int)SegmentLength.Nine, '0', true);//controlNo.ToString().PadLeft(9, '0');
            return obj;
        }
        #endregion

        public enum SegmentLength
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Eleven = 11,
            Twelve = 12,
            Thirteen = 13,
            Fourteen = 14,
            Fifteen = 15,
            Eighteen = 18,
            Twenty = 20,
            TwentyFive = 25,
            FortyEight = 48,
            Sixty = 60,
            Eighty = 80,
            TwoFiftySix = 256,
            FiftyFive = 55,
            Thirty = 30,
            Fifty = 50,
            ThirtyFive = 35
        }
    }
}
