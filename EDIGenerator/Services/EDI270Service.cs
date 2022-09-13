using EDIGenerator.IServices;
using EDIGenerator.Model;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static EDIGenerator.CommonMethods.CommonEDIGenerationMethods;

namespace EDIGenerator.Services
{
    public class EDI270Service : IEDI270Service
    {
        StringBuilder ediText = new StringBuilder();
        public string GenerateEDI270(EDI270FileModel ediFileModel)
        {
            int hierarchicalIDNumber = 1;
            int hierarchicalParentIDNumber = 1;
            #region Interchange Headers
            GetInterchangeHeaders(ediFileModel.EDI270InterchangeHeaders, ediFileModel.EDIGateway);
            #endregion

            #region Loop 2000A  -- Hierarchical Level Node
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), string.Empty, EDI270Constants.HierarchicalLevelCode_Source, "1"), EDISegments.HL.ToString()));
            hierarchicalIDNumber = hierarchicalIDNumber + 1;
            #endregion

            #region Loop 2100A Information Source Name --Payer Details
            ediText.Append(GetSegment<NM1>(GetNM1(EDI270Constants.Payer_EntityIdentifierQualifier, EDI270Constants.Payer_EntityTypeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.PayerName, string.Empty, string.Empty, string.Empty, string.Empty, EDI270Constants.Payer_IdentificationCodeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.CarrierPayerId), EDISegments.NM1.ToString()));
            #endregion

            #region 2000B Information Receiver Level and Name
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), hierarchicalParentIDNumber.ToString(), EDI270Constants.HierarchicalLevelCode_Receiver, "1"), EDISegments.HL.ToString()));
            hierarchicalIDNumber = hierarchicalIDNumber + 1;
            hierarchicalParentIDNumber = hierarchicalParentIDNumber + 1;
            ediText.Append(GetSegment<NM1>(GetNM1(EDI270Constants.Practice_EntityIdentifierQualifier, EDI270Constants.Practice_EntityTypeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.SubmitterName, string.Empty, string.Empty, string.Empty, string.Empty, EDI270Constants.Practice_IdentificationCodeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.NPINumber), EDISegments.NM1.ToString()));
            #endregion

            #region Loop 2000C Suscriber Level and Name
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), hierarchicalParentIDNumber.ToString(), EDI270Constants.HierarchicalLevelCode_Subscriber, "0"), EDISegments.HL.ToString()));
            ediText.Append(GetSegment<NM1>(GetNM1(EDI270Constants.Subscriber_EntityIdentifierQualifier, EDI270Constants.Subscriber_EntityTypeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.SubsLastName, ediFileModel.EDI270EligibilityEnquiryDetails.SubsFirstName, ediFileModel.EDI270EligibilityEnquiryDetails.SubsMiddleName, string.Empty, string.Empty, EDI270Constants.Subscriber_IdentificationCodeQualifier, ediFileModel.EDI270EligibilityEnquiryDetails.InsuredIDNumber), EDISegments.NM1.ToString()));
            ediText.Append(GetSegment<DMG>(GetDMG(EDI270Constants.Subscriber_DOBFormat, ediFileModel.EDI270EligibilityEnquiryDetails.SubsDOB, string.Empty), EDISegments.DMG.ToString()));
            ediText.Append(GetSegment<DTP>(GetDTP(EDI270Constants.Subscriber_VisitDateQualifier, EDI270Constants.Subscriber_VisitDateFormat, ediFileModel.EDI270InterchangeHeaders.CurrentDate), EDISegments.DTP.ToString()));
            #endregion

            #region Enquiry Details
            string serviceTypeIds = string.Join("^", ediFileModel.EDI270EligibilityEnquiryServiceTypesList.Select(x => x.ServiceTypeCode));
            ediText.Append(GetSegment<EQ>(GetEQ(serviceTypeIds, "","",""), EDISegments.EQ.ToString()));
            string serviceCodes = string.Empty;
            if (ediFileModel.EDI270EligibilityEnquiryServiceCodesList != null && ediFileModel.EDI270EligibilityEnquiryServiceCodesList.Count > 0)
            {
                ediFileModel.EDI270EligibilityEnquiryServiceCodesList.ForEach(x =>
                {
                    ediText.Append(GetSegment<EQ>(GetEQ("", EDI270Constants.EnquiryConstant_ServiceCode + x.ServiceCode, "", ""), EDISegments.EQ.ToString()));
                });
            }
            #endregion
            int totalSegmentsInFile = ediText.ToString().Where(x => x == '~').Count();
            #region Interchange Trailors
            GetInterchangeTrailer(totalSegmentsInFile, ediFileModel.EDI270InterchangeHeaders.TranSetContNumber, ediFileModel.EDI270InterchangeHeaders.InterChangeControlerNumber);
            #endregion

            return ediText.ToString();
        }
        private EQ GetEQ(string EQ01,string EQ02,string EQ03,string EQ04)
        {
            EQ obj = new EQ();
            obj.EQ01 = EQ01;
            obj.EQ02 = EQ02;
            //obj.EQ03 = EQ03;
            //obj.EQ04 = EQ04;
            return obj;
        }
  
        private static class EDI270Constants
        {
            /// <summary>
            /// ISA Constants
            /// </summary>
            public static string ISA01 = "00";
            public static string ISA03 = "00";
            public static string RepetitionSeperator = "^";
            public static string InterchangeControlVersion = "00501";    //ISA12  --This is the version number of edi file.00501 is latest stable /// //Interchange Control Version Number---00501 Standards Approved for Publication by ASC X12 Procedures Review Board through October 2003
            public static string AcknowledgmentRequired = "1";  // ISA 14  -- This is set to be 1 if we need any acknowledgement for edi837 file
            public static string TransmissionMode = "T"; // T stands for edi test file and P is used when we will use files on production.There can be other ways for specifying files as test files depending on clearing house 
            public static string ComponentElementSeperator = ":";

            /// <summary>
            /// GS Constants
            /// </summary>
            public static string FunctionalIdentifierCode = "HC";//It denotes it is a health care claim
            public static string ResponsibleAgencyCode = "X"; //It is responsible Agency Code
            public static string VersionReleaseNo = "005010X279A1"; ////Version / Release / Industry Identifier Code fix --005010X222A1


            /// <summary>
            /// ST Constants
            /// </summary>
            public static string TransactionSetHeader = "270";

            /// <summary>
            /// BHT Constants
            /// </summary>
            /// 
            public static string HierarchicalStructureCode = "0022";
            public static string TransactionSetPurposeCode = "13";

            /// <summary>
            /// HLConstants
            /// </summary>
            /// 
            public static string HierarchicalLevelCode_Source = "20";
            public static string HierarchicalLevelCode_Receiver = "21";
            public static string HierarchicalLevelCode_Subscriber = "22";


            /// <summary>
            /// NM1 Constants
            /// </summary>
            /// 
            public static string Payer_EntityIdentifierQualifier = "PR";
            public static string Payer_EntityTypeQualifier = "2";//1 or 2 Person and No-Person Entity
            public static string Payer_IdentificationCodeQualifier = "PI";

            public static string Practice_EntityIdentifierQualifier = "1P";
            public static string Practice_EntityTypeQualifier = "2";//1 or 2 Person and No-Person Entity
            public static string Practice_IdentificationCodeQualifier = "XX";

            public static string Subscriber_EntityIdentifierQualifier = "IL";
            public static string Subscriber_EntityTypeQualifier = "1";//1 or 2 Person and No-Person Entity
            public static string Subscriber_IdentificationCodeQualifier = "MI";

            /// <summary>
            /// DMG Constants
            /// </summary>
            ///
            public static string Subscriber_DOBFormat = "D8";

            /// <summary>
            /// DTP Constants
            /// </summary>
            ///
            public static string Subscriber_VisitDateQualifier = "291";
            public static string Subscriber_VisitDateFormat = "RD8";

            /// <summary>
            /// EQ Constants
            /// </summary>
            ///
            public static string EnquiryConstant_ServiceCode = "HC|";

        }

        private enum EDISegments
        {
            ISA,
            GS,
            ST,
            BHT,
            HL,
            NM1,
            DMG,
            DTP,
            EQ,
            TRN,
            SE,
            GE,
            IEA
        }
        ///<Summary>
        ///<CreatedOn>08/08/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Return a valid length string for edi segment </Description>
        ///</Summary>
     
        private void GetInterchangeHeaders(EDI270InterchangeHeaders interchangeHeaders,EDIGatewayModel ediGateway)
        {
            ediText.Append(GetSegment<ISA>(GetISA(EDI270Constants.ISA01, string.Empty, EDI270Constants.ISA03
                , ediGateway.InterchangeQualId, ediGateway.SenderId, ediGateway.InterchangeQualId, ediGateway.InterchangeQualId
                , ediGateway.ReceiverId, interchangeHeaders.InterChangeDate, interchangeHeaders.InterChangeTime
                , EDI270Constants.RepetitionSeperator, EDI270Constants.InterchangeControlVersion
                , interchangeHeaders.InterChangeControlerNumber, EDI270Constants.AcknowledgmentRequired
                , EDI270Constants.TransmissionMode, EDI270Constants.ComponentElementSeperator), EDISegments.ISA.ToString()));

            ediText.Append(GetSegment<GS>(GetGS(EDI270Constants.FunctionalIdentifierCode, ediGateway.SenderId, ediGateway.ReceiverId
                , interchangeHeaders.CurrentDate, interchangeHeaders.InterChangeTime, interchangeHeaders.EligibilityEnquiry270MasterId
                , EDI270Constants.ResponsibleAgencyCode, EDI270Constants.VersionReleaseNo), EDISegments.GS.ToString()));

            ediText.Append(GetSegment<ST>(GetST(EDI270Constants.TransactionSetHeader, interchangeHeaders.TranSetContNumber, EDI270Constants.VersionReleaseNo), EDISegments.ST.ToString()));

            ediText.Append(GetSegment<BHT>(GetBHT(EDI270Constants.HierarchicalStructureCode, EDI270Constants.TransactionSetPurposeCode
                , interchangeHeaders.InterChangeControlerNumber, interchangeHeaders.CurrentDate
                , interchangeHeaders.InterChangeTime, string.Empty), EDISegments.BHT.ToString()));
        }

        private void GetInterchangeTrailer(int totalSegmentsInFile,string tranSetContNumber,string interChangeControlerNumber)
        {
            ediText.Append(GetSegment<SE>(GetSE(totalSegmentsInFile.ToString(), tranSetContNumber), EDISegments.SE.ToString()));
            ediText.Append(GetSegment<GE>(GetGE(tranSetContNumber, interChangeControlerNumber), EDISegments.GE.ToString()));
            ediText.Append(GetSegment<IEA>(GetIEA(interChangeControlerNumber), EDISegments.IEA.ToString()));
        }
    }
}
