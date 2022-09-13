using EDIParser.IServices;
using EDIParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static EDIParser.CommonMethods.CommonEDIParserMethods;

namespace EDIParser.Services
{
   public class EDI999Service : IEDI999Service
    {
        private EDI999SchemaModel edi999SchemaModel;
        public EDI999Service()
        {
            edi999SchemaModel = new EDI999SchemaModel();
        }
        public EDI999SchemaModel ParseEDI999(string fileText)
        {
            string segmentValue = string.Empty;
            string[] ediTextArray = fileText.Split("~");
            foreach (string stringSegment in ediTextArray)
            {
                string[] segmentArray = stringSegment.Split("*");
                if (segmentArray != null)
                {
                    IEnumerable<string> values = Enum.GetValues(typeof(EDI999Segments))
                              .OfType<EDI999Segments>()
                              .Select(s => Convert.ToString(s));

                    if (values.Contains(segmentArray[0].TrimStart()))
                    {
                        segmentValue = segmentArray[0].TrimStart();
                        switch (segmentValue)
                        {
                            case "ISA":
                                {
                                    ISA ISA = GetSegmentInfo<ISA>(new ISA(), segmentArray);
                                    edi999SchemaModel.ISA = ISA;
                                    break;
                                }
                            case "GS":
                                {
                                    GS GS = GetSegmentInfo<GS>(new GS(), segmentArray);
                                    edi999SchemaModel.GS = GS;
                                    break;
                                }
                            case "ST":
                                {
                                    ST ST = GetSegmentInfo<ST>(new ST(), segmentArray);
                                    edi999SchemaModel.ST = ST;
                                    break;
                                }
                            case "AK1":
                                {
                                    AK1 AK1 = GetSegmentInfo<AK1>(new AK1(), segmentArray);
                                    edi999SchemaModel.AK1 = AK1;
                                    break;
                                }

                            case "AK2":
                                {
                                    AK2 AK2 = GetSegmentInfo<AK2>(new AK2(), segmentArray);
                                    edi999SchemaModel.AK2 = AK2;
                                    break;
                                }
                            case "IK3":
                                {
                                    IK3 IK3 = GetSegmentInfo<IK3>(new IK3(), segmentArray);
                                    edi999SchemaModel.IK3 = IK3;
                                    break;
                                }
                            case "IK4":
                                {
                                    IK4 IK4 = GetSegmentInfo<IK4>(new IK4(), segmentArray);
                                    edi999SchemaModel.IK4 = IK4;
                                    break;
                                }
                            case "IK5":
                                {
                                    IK5 IK5 = GetSegmentInfo<IK5>(new IK5(), segmentArray);
                                    edi999SchemaModel.IK5 = IK5;
                                    break;
                                }
                            case "AK9":
                                {
                                    AK9 AK9 = GetSegmentInfo<AK9>(new AK9(), segmentArray);
                                    edi999SchemaModel.AK9 = AK9;
                                    break;
                                }
                        }
                    }
                }
            }
            return edi999SchemaModel;
        }
        private enum EDI999Segments
        {
            ISA,
            GS,
            ST,
            AK1,
            AK2,
            IK3,
            IK4,
            IK5,
            AK9
        }
    }
}
