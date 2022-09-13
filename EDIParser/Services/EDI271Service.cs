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
    public class EDI271Service : IEDI271Service
    {
        private EDI271SchemaModel ediSchemaModel;
        public EDI271Service()
        {
            ediSchemaModel = new EDI271SchemaModel();
        }
        public EDI271SchemaModel ParseEDI271(string fileText)
        {
            string claimSegment = string.Empty;
            bool payerLoopVisited = false;
            bool submitterLoopVisited = false;
            bool subscriberLoopVisited = false;
            bool ebSegmentVisited = false;
            bool dtpSegmentVisited = true;
            EB EB = null;
            List<EB> ebList = null;
            string segmentValue = string.Empty;
            string[] ediTextArray = fileText.Split("~");
            foreach (string stringSegment in ediTextArray)
            {
                string[] segmentArray = stringSegment.Split("*");
                if (segmentArray != null)
                {
                    IEnumerable<string> values = Enum.GetValues(typeof(EDI271Segments))
                              .OfType<EDI271Segments>()
                              .Select(s => Convert.ToString(s));

                    if (values.Contains(segmentArray[0].TrimStart()))
                    {
                        segmentValue = segmentArray[0].TrimStart();
                        switch (segmentValue)
                        {
                            case "ISA":
                                {
                                    ISA ISA = GetSegmentInfo<ISA>(new ISA(), segmentArray);
                                    ediSchemaModel.ISA = ISA;
                                    break;
                                }
                            case "GS":
                                {
                                    GS GS = GetSegmentInfo<GS>(new GS(), segmentArray);
                                    ediSchemaModel.GS = GS;
                                    break;
                                }
                            case "ST":
                                {
                                    ST ST = GetSegmentInfo<ST>(new ST(), segmentArray);
                                    ediSchemaModel.ST = ST;
                                    break;
                                }

                            case "AAA":
                                {
                                    AAA AAA = GetSegmentInfo<AAA>(new AAA(), segmentArray);
                                    ediSchemaModel.AAA = AAA;
                                    break;
                                }
                            case "NM1":
                                {
                                    NM1 NM1 = GetSegmentInfo<NM1>(new NM1(), segmentArray);
                                    if (NM1 != null && NM1.NM101 == "PR" && payerLoopVisited == false)
                                    {
                                        ediSchemaModel.PayerDetails = NM1;
                                        payerLoopVisited = true;
                                    }
                                    else if (NM1 != null && NM1.NM101 == "1P" && submitterLoopVisited == false)
                                    {
                                        ediSchemaModel.SubmitterDetails = NM1;
                                        submitterLoopVisited = true;
                                    }
                                    else if (NM1 != null && NM1.NM101 == "IL" && subscriberLoopVisited == false)
                                    {
                                        ediSchemaModel.SubscriberDetails = NM1;
                                        subscriberLoopVisited = true;
                                    }
                                    break;
                                }
                            case "EB":
                                {
                                    if (EB != null)
                                    {
                                        if(EB.EB01=="1" || EB.EB01=="6" || EB.EB01=="I")
                                        {
                                            ebList = new List<EB>();
                                            ebList = EB.EB03.Split("^").Select(x => new EB()
                                            {
                                                EB01 = EB.EB01,
                                                EB02 = EB.EB02,
                                                EB03 = x,
                                                EB04 = EB.EB04,
                                                EB05 = EB.EB05,
                                                EB06 = EB.EB06,
                                                EB07 = EB.EB07,
                                                EB08 = EB.EB08,
                                                EB09 = EB.EB09,
                                                EB10 = EB.EB10,
                                                EB11 = EB.EB11,
                                                EB12 = EB.EB12,
                                                EB13 = EB.EB13,
                                                EB14 = EB.EB14

                                            }).ToList();
                                        }
                                            
                                        if (EB.EB01 == "1")
                                        {   
                                            ediSchemaModel.ActivePlans.AddRange(ebList);
                                        }
                                        else if (EB.EB01 == "6")
                                        {
                                            ediSchemaModel.InactivePlans.AddRange(ebList);
                                        }
                                        else if (EB.EB01 == "I")
                                        {
                                            ediSchemaModel.NonCoveredPlan.AddRange(ebList);
                                        }
                                        else if (EB.EB01 == "C" && (EB.EB06 == "" || EB.EB06=="26"))
                                        {
                                            ediSchemaModel.BaseDeductiblesAllPlans.Add(EB);
                                        }
                                        else if (EB.EB01 == "C" && EB.EB06 == "29")
                                        {
                                            ediSchemaModel.RemainingDeductiblesAllPlans.Add(EB);
                                        }
                                        else if (EB.EB01 == "A")
                                        {
                                            ediSchemaModel.CoinsuranceAllPlans.Add(EB);
                                        }
                                        else if (EB.EB01 == "B")
                                        {
                                            ediSchemaModel.CopayAllPlans.Add(EB);
                                        }
                                    }
                                    EB = GetSegmentInfo<EB>(new EB(), segmentArray);
                                    ebSegmentVisited = true;
                                    break;
                                }
                            case "DTP":
                                {
                                    if (EB != null && ebSegmentVisited == true)
                                        EB.DTP = GetSegmentInfo<DTP>(new DTP(), segmentArray);
                                    //EB = null;
                                    ebSegmentVisited = false;
                                    break;
                                }
                        }
                    }
                }
            }
            return ediSchemaModel;
        }
        private enum EDI271Segments
        {
            ISA,
            GS,
            ST,
            EB,
            NM1
        }
    }
}
