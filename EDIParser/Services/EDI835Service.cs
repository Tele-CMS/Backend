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
    public class EDI835Service : IEDI835Service
    {
        private EDI835SchemaModel ediSchemaModel;
        public EDI835Service()
        {
            ediSchemaModel = new EDI835SchemaModel();
        }
        public EDI835SchemaModel ParseEDI835(string fileText)
        {
            bool payerSeg = false;
            bool payeeSeg = false;
            string claimSegment = string.Empty;
            string segmentValue = string.Empty;
            string[] ediTextArray = fileText.Split("~");
            CLP clp = null;
            SVC svc = null;
            bool findNextCLP = false;
            bool isServiceLineVisited = false;
            foreach (string stringSegment in ediTextArray)
            {
                string[] segmentArray = stringSegment.Split("*");
                if (segmentArray != null)
                {
                    IEnumerable<string> values = Enum.GetValues(typeof(EDI835Segments))
                              .OfType<EDI835Segments>()
                              .Select(s => Convert.ToString(s));

                    if (values.Contains(segmentArray[0].TrimStart()))
                    {
                        if (segmentArray[0].TrimStart() != "CLP" && findNextCLP == true)
                            continue;
                        //else continue;
                        segmentValue = segmentArray[0].TrimStart();
                        if (claimSegment == "CLP")
                            segmentValue = "CLP";
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
                            case "BPR":
                                {
                                    BPR BPR = GetSegmentInfo<BPR>(new BPR(), segmentArray);
                                    ediSchemaModel.BPR = BPR;
                                    break;
                                }
                            case "TRN":
                                {
                                    TRN TRN = GetSegmentInfo<TRN>(new TRN(), segmentArray);
                                    ediSchemaModel.TRN = TRN;
                                    break;
                                }

                            case "DTM":
                                {
                                    DTM DTM = GetSegmentInfo<DTM>(new DTM(), segmentArray);
                                    ediSchemaModel.DTM = DTM;
                                    break;
                                }
                            case "N1":
                                {
                                    if (segmentArray[1] == "PR")
                                    {
                                        N1 N1 = GetSegmentInfo<N1>(new N1(), segmentArray);
                                        ediSchemaModel.PayerInfo = N1;
                                        payerSeg = true;
                                    }
                                    else if (segmentArray[1] == "PE")
                                    {
                                        N1 N1 = GetSegmentInfo<N1>(new N1(), segmentArray);
                                        ediSchemaModel.PayeeInfo = N1;
                                        payeeSeg = true;
                                    }
                                    break;
                                }
                            case "N3":
                                {
                                    if (payerSeg)
                                    {
                                        N3 N3 = GetSegmentInfo<N3>(new N3(), segmentArray);
                                        ediSchemaModel.PayerAddress = N3;
                                    }
                                    else if (payeeSeg)
                                    {
                                        N3 N3 = GetSegmentInfo<N3>(new N3(), segmentArray);
                                        ediSchemaModel.PayeeAddress = N3;
                                    }

                                    break;
                                }
                            case "N4":
                                {
                                    if (payerSeg)
                                    {
                                        N4 N4 = GetSegmentInfo<N4>(new N4(), segmentArray);
                                        ediSchemaModel.PayerCityStateZip = N4;
                                        payerSeg = false;
                                    }
                                    else if (payeeSeg)
                                    {
                                        N4 N4 = GetSegmentInfo<N4>(new N4(), segmentArray);
                                        ediSchemaModel.PayeeCityStateZip = N4;
                                        payeeSeg = false;
                                    }
                                    break;
                                }
                            case "CLP":
                                {
                                    claimSegment = "CLP";
                                    switch (segmentArray[0].TrimStart())
                                    {
                                        case "CLP":
                                            {
                                                findNextCLP = false;
                                                if (clp != null)
                                                {
                                                    ediSchemaModel.CLPList.Add(clp);
                                                    isServiceLineVisited = false;
                                                    clp = null;
                                                }
                                                clp = GetSegmentInfo<CLP>(new CLP(), segmentArray);
                                                break;
                                            }
                                        case "SVC":
                                            {
                                                //if (svc != null)
                                                //{
                                                //    if (clp.SVCList == null) clp.SVCList = new List<SVC>();
                                                //    clp.SVCList.Add(svc);
                                                //    svc = null;
                                                //}
                                                svc = GetSegmentInfo<SVC>(new SVC(), segmentArray);
                                                if (clp != null && clp.SVCList == null)
                                                    clp.SVCList = new List<SVC>();

                                                clp.SVCList.Add(svc);
                                                isServiceLineVisited = true;
                                                break;
                                            }
                                        case "CAS":
                                            {
                                                if (isServiceLineVisited)
                                                {
                                                    CAS CAS = GetSegmentInfo<CAS>(new CAS(), segmentArray);
                                                    if (svc.SVCAdjList == null) svc.SVCAdjList = new List<CAS>();
                                                    svc.SVCAdjList.Add(CAS);
                                                    CAS = null;
                                                }
                                                break;
                                            }
                                        case "REF":
                                            {
                                                if (segmentArray[1].Trim() == "6R" && svc!=null && svc.REF == null) 
                                                {
                                                    svc.REF = new REF();
                                                    svc.REF = GetSegmentInfo<REF>(new REF(), segmentArray);
                                                }
                                                break;
                                            }
                                        case "GE":
                                            {
                                                if (clp != null)
                                                {
                                                    ediSchemaModel.CLPList.Add(clp);
                                                    isServiceLineVisited = false;
                                                    clp = null;
                                                    findNextCLP = true;
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            return ediSchemaModel;
        }
        private enum EDI835Segments
        {
            ISA,
            GS,
            ST,
            BPR,
            TRN,
            DTM,
            N1,
            N3,
            N4,
            CLP,
            SVC,
            CAS,
            REF,
            GE
        }
    }
}
