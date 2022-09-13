using EDIGenerator.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using HC.Patient.Model.Claim;
using EDIGenerator.Model;
using HC.Common;
using System.Reflection;
using System.Linq;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using static EDIGenerator.CommonMethods.CommonEDIGenerationMethods;

namespace EDIGenerator.Services
{
    public class EDI837Service : IEDI837Service
    {       
        public EDI837Service()
        {
       
        }


        public string GenerateEDI837_005010X222A1(EDI837FileModel ediFileModel, string payerpreference, string submissionType,string type)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B,2320,2330A,2330B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);
                        //Get primary payer,subscriber inf for secondary submission

                        if(payerpreference.ToLower()!="primary")
                            GetOtherSubscriber(ediFileModel, ediClaimModel, ediText);
                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information
                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                            //Loop 2430 for payment and adjustments of primary payer in case of secondary submission
                            if (payerpreference.ToLower() != "primary")
                            {
                                if (ediFileModel.EDIOtherPayerInformationModel != null && ediFileModel.EDIOtherPayerInformationModel.Count > 0)
                                {
                                    ediFileModel.EDIOtherPayerInformationModel.ForEach(x => {
                                        GetPaymentsAndAdjusments(x,ediClaimModel, claimServiceLinesList[lxCount], ediFileModel, ediText);
                                    });
                                }
                            }
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        #region Single EDI 837
        public string GenerateSingleEDI837(EDI837FileModel ediFileModel)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);

                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information
                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Single EDI837 Resubmission Case
        public string GenerateSingleEDI837_Resubmit(EDI837FileModel ediFileModel)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);

                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information

                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Batch EDI For Primary Payer
        public string GenerateBatchEDI837(EDI837FileModel ediFileModel) ///Methods are almost same.They are not because we may need other info in future for batch submit
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;
                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    hierarchicalParentIDNumber = 1;
                    ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                    claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                    if (count == 0)    ///just a temperary check
                    {
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                    }

                    #region Loop 2000B and 2010BA Subscriber

                    hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                    #endregion

                    #region Loop 2010BB Payer details
                    //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                    GetPayer(ediText, ediClaimModel);

                    #endregion

                    if (!ediClaimModel.InsurancePersonSameAsPatient)
                    {
                        #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                        hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                        #endregion
                    }

                    #region Loop 2300,2310A,2310B Claim Loops
                    GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);

                    #endregion

                    #region Loop 2400 Service Line(CPTCodes) Information
                    //Loop 2400 -- Claim Service Line
                    for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                    {
                        GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Common Variables For EDI setup
        private void GetEDIVariablesData(EDI837FileModel ediFileModel, ref EDIInterchangeHeaders interchangeHeaders, ref List<EDIClaimModel> ediClaimModelList, ref EDILocationAddressModel serviceAddress, ref EDILocationAddressModel billingAddress, ref EDIGatewayModel ediGateway)
        {
            if (ediFileModel.EDIInterchangeHeaders != null)
                interchangeHeaders = ediFileModel.EDIInterchangeHeaders;
            if (ediFileModel.EDIGateway != null)
                ediGateway = ediFileModel.EDIGateway;
            if (ediFileModel.EDIServiceAddress != null)
                serviceAddress = ediFileModel.EDIServiceAddress;
            if (ediFileModel.EDIBillingAddress != null)
                billingAddress = ediFileModel.EDIBillingAddress;
            if (ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                ediClaimModelList = ediFileModel.EDIClaims;
        }

        #endregion

        #region Methods to get all loops in 837 file
        private void GetInterchangeHeaders(EDIInterchangeHeaders interchangeHeaders, StringBuilder ediText, EDIGatewayModel ediGateway)
        {
            ediText.Append(GetSegment<ISA>(GetISA(EDIConstants.ISA01, string.Empty, EDIConstants.ISA03, string.Empty
                , ediGateway.InterchangeQualId, ediGateway.SenderId, ediGateway.InterchangeQualId
                , ediGateway.ReceiverId, interchangeHeaders.InterChangeDate, interchangeHeaders.InterChangeTime,
                 EDIConstants.RepetitionSeperator, EDIConstants.InterchangeControlVersion
                 , interchangeHeaders.InterChangeControlerNumber, EDIConstants.AcknowledgmentRequired
                 , EDIConstants.TransmissionMode, EDIConstants.ComponentElementSeperator), EDISegments.ISA.ToString()));

            ediText.Append(GetSegment<GS>(GetGS(EDIConstants.FunctionalIdentifierCode, ediGateway.SenderId, ediGateway.ReceiverId
                , interchangeHeaders.CurrentDate, interchangeHeaders.InterChangeTime, interchangeHeaders.BatchId
                , EDIConstants.ResponsibleAgencyCode, EDIConstants.VersionReleaseNo), EDISegments.GS.ToString()));

            ediText.Append(GetSegment<ST>(GetST(EDIConstants.TransactionSetHeader, interchangeHeaders.TranSetContNumber, EDIConstants.VersionReleaseNo), EDISegments.ST.ToString()));
            ediText.Append(GetSegment<BHT>(GetBHT(EDIConstants.HierarchicalStructureCode, EDIConstants.TransactionSetPurposeCode
                , interchangeHeaders.InterChangeControlerNumber, interchangeHeaders.CurrentDate
                , interchangeHeaders.InterChangeTime, EDIConstants.TransactionTypeCode), EDISegments.BHT.ToString()));
        }
        private void GetSubmitter(EDIInterchangeHeaders interchangeHeaders, StringBuilder ediText, EDIGatewayModel ediGateway)
        {
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_Submitter,
                                    EDIConstants.EntityTypeQualifier_Submiiter, interchangeHeaders.SubmitterName, string.Empty, string.Empty, string.Empty, string.Empty,
                                    EDIConstants.IdentitificationCodeQualifier_Submitter, ediGateway.SenderId), EDISegments.NM1.ToString()));
            ediText.Append(GetSegment<PER>(GetContact(interchangeHeaders), EDISegments.PER.ToString()));
        }
        private void GetReceiver(StringBuilder ediText, EDIGatewayModel ediGateway)
        {
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_Receiver,
                    EDIConstants.EntityTypeQualifier_Receiver, ediGateway.ClearingHouseName, string.Empty, string.Empty, string.Empty, string.Empty,
                    EDIConstants.IdentitificationCodeQualifier_Receiver, string.Empty), EDISegments.NM1.ToString()));
        }
        private int GetBillingProvider(EDILocationAddressModel billingAddress, StringBuilder ediText, int hierarchicalIDNumber)
        {
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), string.Empty,
                            EDIConstants.HierarchicalLevelCode_Provider, EDIConstants.HierarchicalChildCode_Provider), EDISegments.HL.ToString()));
            //increment hierarchiacal level counter to get unique value always
            hierarchicalIDNumber = hierarchicalIDNumber + 1;

            //Loop 2010AA // Billing provider name,address,city,state,npi,taxid
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_BillingProvider,
            EDIConstants.EntityTypeQualifier_BillingProviderPractice, billingAddress.BillingProviderLocationName, string.Empty, string.Empty, string.Empty, string.Empty,
            EDIConstants.IdentificationCodeQualifier_BillingProvider, billingAddress.NPINumber), EDISegments.NM1.ToString()));

            ediText.Append(GetSegment<N3>(GetN3(billingAddress.Address1, billingAddress.Address2), EDISegments.N3.ToString()));
            ediText.Append(GetSegment<N4>(GetN4(billingAddress.City, billingAddress.State, billingAddress.Zip), EDISegments.N4.ToString()));
            ediText.Append(GetSegment<REF>(GetREF(EDIConstants.ReferenceIdentificationQualifier_BillingProvider, billingAddress.TaxId), EDISegments.REF.ToString()));
            return hierarchicalIDNumber;
        }
        private int GetSubscriber(StringBuilder ediText, int hierarchicalIDNumber, int hierarchicalParentIDNumber, EDIClaimModel ediClaimModel)
        {
            //Loop 2000B // Subscriber HL segment(Hierrachcial level),SBR
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), hierarchicalParentIDNumber.ToString(),
                               EDIConstants.HierarchicalLevelCode_Subscriber, (!ediClaimModel.InsurancePersonSameAsPatient ? 1 : 0).ToString()), EDISegments.HL.ToString()));

            hierarchicalIDNumber = hierarchicalIDNumber + 1;
            ediText.Append(GetSegment<SBR>(GetSubscriber(ediClaimModel), EDISegments.SBR.ToString()));

            //Loop 2010BA // Subscriber name,address,city,state,date of birth,gender(NM1,N3,N4,DMG)
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_Subscriber,
               EDIConstants.EntityTypeQualifier_Subscriber, ediClaimModel.SubsLastName, ediClaimModel.SubsFirstName, ediClaimModel.SubsMiddleName, string.Empty, string.Empty,
               EDIConstants.MemberIdentification_Subscriber, ediClaimModel.InsuredIDNumber), EDISegments.NM1.ToString()));

            ediText.Append(GetSegment<N3>(GetN3(ediClaimModel.SubsAddress1, ediClaimModel.SubsAddress2), EDISegments.N3.ToString()));
            ediText.Append(GetSegment<N4>(GetN4(ediClaimModel.SubsCity, ediClaimModel.SubsState, ediClaimModel.SubsPostalCode), EDISegments.N4.ToString()));
            ediText.Append(GetSegment<DMG>(GetSubscriberDemographics(ediClaimModel), EDISegments.DMG.ToString()));
            return hierarchicalIDNumber;
        }
        private void GetPayer(StringBuilder ediText, EDIClaimModel ediClaimModel)
        {
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.PayerIdentifierCode,
                                  EDIConstants.PayerEnityType, ediClaimModel.PayerName, string.Empty, string.Empty, string.Empty, string.Empty,
                                  EDIConstants.PayerIdentifierCodeQualifier, ediClaimModel.CarrierPayerId), EDISegments.NM1.ToString()));

            ediText.Append(GetSegment<N3>(GetN3(ediClaimModel.PayerAddress, string.Empty), EDISegments.N3.ToString()));
            ediText.Append(GetSegment<N4>(GetN4(ediClaimModel.PayerCity, ediClaimModel.PayerState, ediClaimModel.PayerZipCode), EDISegments.N4.ToString()));
        }
        private int GetPatient(StringBuilder ediText, int hierarchicalIDNumber, int hierarchicalParentIDNumber, EDIClaimModel ediClaimModel)
        {
            ///LOOP 2000C Patient Hierarchical Level and and LOOP 2010CA – Patient Name  -- Still is in progress
            ediText.Append(GetSegment<HL>(GetHL(hierarchicalIDNumber.ToString(), (hierarchicalParentIDNumber + 1).ToString(),
                            EDIConstants.HierarchicalLevelCode_Patient, "0"), EDISegments.HL.ToString()));

            hierarchicalIDNumber = hierarchicalIDNumber + 1;
            ediText.Append(GetSegment<PAT>(GetPatientRelation(ediClaimModel), EDISegments.PAT.ToString()));

            //Loop 2000CA // Subscriber name,address,city,state,date of birth,gender(NM1,N3,N4,DMG)
            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_Patient,
              EDIConstants.EntityTypeQualifier_Patient, ediClaimModel.PatientLastName, ediClaimModel.PatientFirstName, ediClaimModel.PatientMiddleName, string.Empty, string.Empty,
              EDIConstants.MemberIdentification_Patient, ediClaimModel.SubsIndenCode), EDISegments.NM1.ToString()));

            ediText.Append(GetSegment<N3>(GetN3(ediClaimModel.PatientAddress, string.Empty), EDISegments.N3.ToString()));
            ediText.Append(GetSegment<N4>(GetN4(ediClaimModel.PatientCity, ediClaimModel.PatientState, ediClaimModel.PatientPostalCode), EDISegments.N4.ToString()));
            ediText.Append(GetSegment<DMG>(GetPatientDemographics(ediClaimModel), EDISegments.DMG.ToString()));
            return hierarchicalIDNumber;
        }

        private void GetClaim(List<EDIClaimsServiceLines> claimServiceLinesList, List<EDIDiagnosisCodesModel> ediDiagnosisCodeList, EDILocationAddressModel serviceAddress, EDILocationAddressModel billingAddress, StringBuilder ediText, EDIClaimModel ediClaimModel)
        {
            //- Claim Loop 2300
            ediText.Append(GetSegment<CLM>(GetClaimSegment(ediClaimModel, claimServiceLinesList), EDISegments.CLM.ToString()));
            ediText.Append(GetSegment<DTP>(GetDTPSegment(ediClaimModel.OnSetDate, EDIConstants.OnsetDateQualifier), EDISegments.DTP.ToString()));////Onset date

            //CLIA,Pre-certification and prior authorization,K3 segment pending
            if(ediClaimModel.ClaimFrequencyCode ==7 || ediClaimModel.ClaimFrequencyCode==8)
                ediText.Append(GetSegment<REF>(GetREF(EDIConstants.ClaimREFQualifier_Resubmit.ToString(), ediClaimModel.PayerControlReferenceNumber), EDISegments.REF.ToString()));
            else
                ediText.Append(GetSegment<REF>(GetREF(EDIConstants.ClaimREFQualifier.ToString(), ediClaimModel.ClaimId.ToString()), EDISegments.REF.ToString()));
            ediText.Append(GetClaimDiagnosisCodes(ediDiagnosisCodeList));//HI 2300

            //Loop 2310A  --Referring Provider Info  // Will be added later

            //Loop 2310B  --Rendering Provider Info
            //if (billingAddress.NPINumber.Trim() != ediClaimModel.RenderingProviderNPI.Trim()) 
            //{
                ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_RenderingProvider,
                 EDIConstants.EntityTypeQualifier_RenderingProvider, ediClaimModel.RenderingProviderLastName, ediClaimModel.RenderingProviderFirstName, ediClaimModel.RenderingProviderMiddleName, string.Empty, string.Empty,
                 EDIConstants.IdentificationbCodeQualifier_RenderingProvider, ediClaimModel.RenderingProviderNPI), EDISegments.NM1.ToString()));
                //There are other ref variables
                ediText.Append(GetSegment<REF>(GetREF(EDIConstants.StateLicenseNumberQualifier.ToString(), ediClaimModel.RenderingProviderStateLicenseNumber), EDISegments.REF.ToString()));
            //} 
            //Loop 2310C  -- Service Location

            ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_ServiceLocation,
                EDIConstants.EntityTypeQualifier_ServiceLocation, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
               EDIConstants.IdentificationbCodeQualifier_ServiceLocation, serviceAddress.NPINumber), EDISegments.NM1.ToString()));

            ediText.Append(GetSegment<N3>(GetN3(serviceAddress.Address1, serviceAddress.Address2), EDISegments.N3.ToString()));
            ediText.Append(GetSegment<N4>(GetN4(serviceAddress.City, serviceAddress.State, serviceAddress.Zip), EDISegments.N4.ToString()));
            //optional-Please add later
            //ediText.Append(GetSegment<REF>(GetRenderingProviderRef("ServiceAddressRefNo", EDIConstants.StateLicenseNumberQualifier.ToString()), EDISegments.REF.ToString()));
        }

        private void GetOtherSubscriber(EDI837FileModel edi837FileModel,EDIClaimModel ediClaimModel,StringBuilder ediText)
        {
            if (edi837FileModel != null && edi837FileModel.EDIOtherPayerInformationModel != null && edi837FileModel.EDIOtherPayerInformationModel.Count > 0)
            {
                foreach (EDIOtherPayerInformationModel model in edi837FileModel.EDIOtherPayerInformationModel.Where(x => x.ClaimId == ediClaimModel.ClaimId))
                {
                    ediText.Append(GetSegment<SBR>(GetSBR(model.PayerResponsibilityCode,
                        (!model.InsurancePersonSameAsPatient ? string.Empty : EDIConstants.IndivisualRelationshipCode)
                        , model.InsuranceReferenceNumber, model.InsuranceGroupName, string.Empty, string.Empty, string.Empty, string.Empty,
                        model.InsuranceCode), EDISegments.SBR.ToString()));
                    decimal amount = edi837FileModel.EDIPayerPaymentModel.Where(x => x.ClaimId == ediClaimModel.ClaimId && x.PatientInsuranceId == model.PatientInsuranceId && x.DescriptionType.ToLower() == "payment").Sum(y => y.Amount);
                    ediText.Append(GetSegment<AMT>(GetAMT("D", Convert.ToString(amount)), EDISegments.AMT.ToString()));
                    ediText.Append(GetSegment<OI>(GetOI(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty), EDISegments.OI.ToString()));
                    ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.EntityIdentifierCode_Subscriber,
                     EDIConstants.EntityTypeQualifier_Subscriber, model.SubsLastName, model.SubsFirstName, model.SubsMiddleName, string.Empty, string.Empty,
                     EDIConstants.MemberIdentification_Subscriber, model.SubsIndenCode), EDISegments.NM1.ToString()));

                    ediText.Append(GetSegment<N3>(GetN3(model.SubsAddress, string.Empty), EDISegments.N3.ToString()));
                    ediText.Append(GetSegment<N4>(GetN4(model.SubsCity, model.SubsState, model.SubsPostalCode), EDISegments.N4.ToString()));

                    ediText.Append(GetSegment<NM1>(GetNM1(EDIConstants.PayerIdentifierCode,
                                  EDIConstants.PayerEnityType, model.PayerName, string.Empty, string.Empty, string.Empty, string.Empty,
                                  EDIConstants.PayerIdentifierCodeQualifier, model.CarrierPayerId), EDISegments.NM1.ToString()));

                    //ediText.Append(GetSegment<N3>(GetN3(model.PayerAddress, string.Empty), EDISegments.N3.ToString()));
                    //ediText.Append(GetSegment<N4>(GetN4(model.PayerCity, model.PayerState, model.PayerZipCode), EDISegments.N4.ToString()));
                }
            }
        }

        private void GetPaymentsAndAdjusments(EDIOtherPayerInformationModel ediOtherPayerInformationModel, EDIClaimModel ediClaimModel, EDIClaimsServiceLines ediClaimServiceLine, EDI837FileModel ediFileModel,StringBuilder ediText)
        {
            ediText.Append(GetSegment<SVD>(GetSVD(ediOtherPayerInformationModel.CarrierPayerId, string.Empty,string.Empty,string.Empty,string.Empty, ediClaimServiceLine,ediFileModel), EDISegments.SVD.ToString()));
            List<EDIPayerPaymentModel> list = ediFileModel.EDIPayerPaymentModel.Where(x => x.ClaimServiceLineId == ediClaimServiceLine.ServiceLineId && x.DescriptionType.ToLower()=="adjustment" && x.PatientInsuranceId==ediOtherPayerInformationModel.PatientInsuranceId).ToList();
            list.ForEach(x =>
            {
                ediText.Append(GetSegment<CAS>(GetCAS(x.AdjustmentGroupCode,x.AdjustmentReasonCode,Convert.ToString(x.Amount)), EDISegments.CAS.ToString()));
            });
        }
        private void GetClaimServiceLine(List<EDIClaimsServiceLines> claimServiceLinesList, StringBuilder ediText, EDIClaimModel ediClaimModel, int lxCount)
        {
            ediText.Append(GetSegment<LX>(GetClaimServiceLineLX((lxCount + 1).ToString()), EDISegments.LX.ToString()));
            ediText.Append(GetSegment<SV1>(GetClaimProfessionalService(ediClaimModel, claimServiceLinesList[lxCount]), EDISegments.SV1.ToString()));
            ediText.Append(GetSegment<DTP>(GetDTPSegment(ediClaimModel.OnSetDate, EDIConstants.DateOfServiceQualifier), EDISegments.DTP.ToString()));
            ediText.Append(GetSegment<REF>(GetREF("6R", claimServiceLinesList[lxCount].EDIClaimServiceLineId.ToString()), EDISegments.REF.ToString()));
        }
        private void GetInterchangeTrailer(EDIInterchangeHeaders interchangeHeaders, StringBuilder ediText)
        {
            int totalSegmentsInFile = ediText.ToString().Where(x => x == '~').Count();
            ediText.Append(GetSegment<SE>(GetSE(totalSegmentsInFile.ToString(), interchangeHeaders.TranSetContNumber), EDISegments.SE.ToString()));
            ediText.Append(GetSegment<GE>(GetGE(interchangeHeaders.TranSetContNumber, interchangeHeaders.InterChangeControlerNumber), EDISegments.GE.ToString()));
            ediText.Append(GetSegment<IEA>(GetIEA(interchangeHeaders.InterChangeControlerNumber), EDISegments.IEA.ToString()));
        }
        #endregion

        #region Methods to Get EDI 837 segments


        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for NM1 Submitter segment</Description>
        /// Deprecated as there is common methdo to get all NM1 segment
        ///</Summary>
        private NM1 GetSubmitter(EDIInterchangeHeaders interchangeHeaders, EDIGatewayModel ediGateway)
        {
            NM1 submitter = new NM1();
            submitter.NM101 = EDIConstants.EntityIdentifierCode_Submitter;
            submitter.NM102 = EDIConstants.EntityTypeQualifier_Submiiter;
            submitter.NM103 = CreateString(interchangeHeaders.SubmitterName, (int)SegmentLength.One, (int)SegmentLength.Sixty, ' ', true);
            submitter.NM108 = EDIConstants.IdentitificationCodeQualifier_Submitter;
            submitter.NM109 = CreateString(ediGateway.SenderId, (int)SegmentLength.Two, (int)SegmentLength.Eighty, '0', true); //There can be clinic taxid.Will add later
            return submitter;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for Submitter Contact segment(Practice contact person)</Description>
        ///</Summary>
        private PER GetContact(EDIInterchangeHeaders interchangeHeaders)
        {
            PER contact = new PER();
            contact.PER01 = EDIConstants.ContactFunctionCode;
            contact.PER02 = CreateString(interchangeHeaders.ContactPersonLastName + ' ' + interchangeHeaders.ContactPersonFirstName, (int)SegmentLength.One, (int)SegmentLength.Sixty, ' ', true);
            contact.PER03 = EDIConstants.CommunicationNumberQualifier_Phone;
            contact.PER04 = CreateString(interchangeHeaders.ContactPersonPhoneNumber, (int)SegmentLength.One, (int)SegmentLength.TwoFiftySix, '0', true);
            contact.PER05 = EDIConstants.CommunicationNumberQualifier_Fax;
            contact.PER06 = CreateString(interchangeHeaders.OrganizationFaxNumber, (int)SegmentLength.One, (int)SegmentLength.TwoFiftySix, '0', true);
            return contact;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for SBR Patient segment for patient</Description>
        ///</Summary>
        private SBR GetSubscriber(EDIClaimModel ediclaimModel)
        {
            SBR obj = new SBR();
            obj.SBR01 = ediclaimModel.PayerResponsibilityCode.Trim();
            obj.SBR02 = !ediclaimModel.InsurancePersonSameAsPatient ? string.Empty : EDIConstants.IndivisualRelationshipCode;
            obj.SBR03 = ediclaimModel.InsuranceReferenceNumber;
            obj.SBR04 = ediclaimModel.InsuranceGroupName;
            obj.SBR09 = ediclaimModel.InsuranceCode;
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for subscriber Demographics segment</Description>
        ///</Summary>
        private DMG GetSubscriberDemographics(EDIClaimModel ediClaimModel)
        {
            DMG obj = new DMG();
            obj.DMG01 = EDIConstants.DMG_DateTimeFomrat;
            obj.DMG02 = ediClaimModel.SubsDOB;//CreateString(Convert.ToDateTime(ediClaimModel.SubsDOB).ToString("yyyyMMdd"), (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true);
            obj.DMG03 = CreateString(ediClaimModel.SubsGender, (int)SegmentLength.One, (int)SegmentLength.One, 'U', true);
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for PAT Patient segment for patient</Description>
        ///</Summary>
        private PAT GetPatientRelation(EDIClaimModel ediclaimModel)
        {
            PAT obj = new PAT();
            obj.PAT01 = ediclaimModel.RelationshipCode;
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for subscriber Demographics segment</Description>
        ///</Summary>
        private DMG GetPatientDemographics(EDIClaimModel ediClaimModel)
        {
            DMG obj = new DMG();
            obj.DMG01 = EDIConstants.DMG_DateTimeFomrat;
            obj.DMG02 = CreateString(Convert.ToDateTime(ediClaimModel.PatientDOB).ToString("yyyyMMdd"), (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true);
            obj.DMG03 = CreateString(ediClaimModel.PatientGender, (int)SegmentLength.One, (int)SegmentLength.One, 'U', true);
            return obj;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for claim segment CLM</Description>
        ///</Summary>
        private CLM GetClaimSegment(EDIClaimModel ediclaimModel, List<EDIClaimsServiceLines> claimServiceLines)
        {
            CLM claim = new CLM();
            claim.CLM01 = CreateString(Convert.ToString(ediclaimModel.EDIClaimId), (int)SegmentLength.One, (int)SegmentLength.Twenty, '0', true);
            claim.CLM02 = CreateString(Convert.ToString(claimServiceLines.Sum(x => x.TotalAmount)), (int)SegmentLength.One, (int)SegmentLength.Eighteen, '0', true);
            claim.CLM03 = string.Empty;
            claim.CLM04 = string.Empty;
            claim.CLM05 = ediclaimModel.ServiceFacilityCode + EDIConstants.FacilityCodeQualifier + ediclaimModel.ClaimFrequencyCode.ToString();
            //May need below values in future from db
            claim.CLM06 = "Y";  // Provider Signature -may be needed from db in future
            claim.CLM07 = "A";  //	Provider Accept Assignment Code (A = Assigned,B = Assignment Accepted on Clinical Lab Services Only,C = Not Assigned)
            claim.CLM08 = "Y";  // 	Yes/No Assignments of Benefit
            claim.CLM09 = "Y";  //	Yes/No Release of Information
            claim.CLM10 = "P";  //	Patient signature source code
            return claim;
        }


        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns DTP segment</Description>
        ///</Summary>
        private DTP GetDTPSegment(string date, string qualifier)
        {
            DTP dtp = new DTP();
            dtp.DTP01 = qualifier;
            dtp.DTP02 = EDIConstants.DTPDateFormat;
            dtp.DTP03 = CreateString(date, (int)SegmentLength.One, (int)SegmentLength.ThirtyFive, ' ', true);
            return dtp;
        }

        ///<Summary>
        ///<CreatedOn>20/12/2017</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns Claim diagnosis code segment (HI)</Description>
        ///</Summary>
        private string GetClaimDiagnosisCodes(List<EDIDiagnosisCodesModel> ediDiagnosisCodes)
        {
            StringBuilder dxcode = new StringBuilder();
            dxcode.Append(EDISegments.HI + "*");
            int count = 0;
            string codeQualifier = string.Empty;
            foreach (EDIDiagnosisCodesModel ediDXCode in ediDiagnosisCodes)
            {
                if (ediDXCode.CodeType.ToUpper() == "ICD9" && count == 0)
                    codeQualifier = "BK";
                else if (ediDXCode.CodeType.ToUpper() == "ICD9" && count > 0)
                    codeQualifier = "BF";
                else if (ediDXCode.CodeType.ToUpper() == "ICD10" && count == 0)
                    codeQualifier = "ABK";
                else if (ediDXCode.CodeType.ToUpper() == "ICD10" && count > 0)
                    codeQualifier = "ABF";
                count = count + 1;
                dxcode.Append(CreateString(codeQualifier, (int)SegmentLength.One, (int)SegmentLength.Three, ' ', true) + ":" + CreateString(ediDXCode.DiagnosisCode, (int)SegmentLength.One, (int)SegmentLength.Thirty, ' ', true) + "*");
            }
            dxcode.Replace("*", "", dxcode.ToString().LastIndexOf("*"), 1);
            dxcode.Append("~");
            return dxcode.ToString();
        }

        private LX GetClaimServiceLineLX(string count)
        {
            LX lx = new LX();
            lx.LX01 = CreateString(count, (int)SegmentLength.One, (int)SegmentLength.Six, ' ', true);
            return lx;
        }

        private SV1 GetClaimProfessionalService(EDIClaimModel ediclaimModel, EDIClaimsServiceLines ediClaimServiceLine)
        {
            StringBuilder modifiers = new StringBuilder();
            StringBuilder dxPointers = new StringBuilder();
            modifiers.Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier1) ? ":" + ediClaimServiceLine.Modifier1 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier2) ? ":" + ediClaimServiceLine.Modifier2 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier3) ? ":" + ediClaimServiceLine.Modifier3 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier4) ? ":" + ediClaimServiceLine.Modifier4 : string.Empty);
            dxPointers.Append(!string.IsNullOrEmpty(ediClaimServiceLine.DXPointer1) ? ediClaimServiceLine.DXPointer1 + ":" : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.DXPointer2) ? ediClaimServiceLine.DXPointer2 + ":" : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.DXPointer3) ? ediClaimServiceLine.DXPointer3 + ":" : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.DXPointer4) ? ediClaimServiceLine.DXPointer4 + ":" : string.Empty);
            //if (modifiers.ToString() != string.Empty)
            //    modifiers.Replace(":", "", modifiers.ToString().LastIndexOf(":"), 1);
            if (dxPointers.ToString() != string.Empty)
                dxPointers.Replace(":", "", dxPointers.ToString().LastIndexOf(":"), 1);
            SV1 sv1 = new SV1();
            sv1.SV101 = "HC:" + CreateString(ediClaimServiceLine.ServiceCode, (int)SegmentLength.One, (int)SegmentLength.FortyEight, ' ', true) + modifiers.ToString();
            sv1.SV102 = CreateString(ediClaimServiceLine.TotalAmount.ToString(), (int)SegmentLength.One, (int)SegmentLength.Eighteen, ' ', true);
            sv1.SV103 = "UN";
            sv1.SV104 = CreateString(ediClaimServiceLine.Units.ToString(), (int)SegmentLength.One, (int)SegmentLength.Fifteen, ' ', true);
            sv1.SV105 = CreateString(ediClaimServiceLine.ServiceFacilityCode.ToString(), (int)SegmentLength.One, (int)SegmentLength.Two, ' ', true);
            sv1.SV107 = dxPointers.ToString();
            return sv1;
        }
        



        
        ///<Summary>
        ///<CreatedOn>09/04/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for SBR segment</Description>
        ///</Summary>
        private SBR GetSBR(string SBR01, string SBR02, string SBR03, string SBR04, string SBR05, string SBR06, string SBR07, string SBR08, string SBR09)
        {
            SBR SBR = new SBR();
            SBR.SBR01 = SBR01.Trim();
            SBR.SBR02 = SBR02.Trim();
            SBR.SBR03 = SBR03.Trim();
            SBR.SBR04 = SBR04.Trim();
            SBR.SBR09 = SBR09.Trim();
            return SBR;
        }

        ///<Summary>
        ///<CreatedOn>09/04/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for AMT segment</Description>
        ///</Summary>
        private AMT GetAMT(string AMT01, string AMT02)
        {
            AMT AMT = new AMT();
            AMT.AMT01 = CreateString(AMT01, (int)SegmentLength.One, (int)SegmentLength.Three, ' ', true);
            AMT.AMT02 = CreateString(AMT02, (int)SegmentLength.One, (int)SegmentLength.Eighteen, ' ', true);
            return AMT;
        }

        ///<Summary>
        ///<CreatedOn>09/04/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for OI segment</Description>
        ///</Summary>
        private OI GetOI(string OI01, string OI02,string OI03,string OI04,string OI05,string OI06)
        {
            OI OI = new OI();
            OI.OI03 = "Y";
            OI.OI06 = "Y";
            return OI;
        }


        ///<Summary>
        ///<CreatedOn>09/04/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for SVD segment</Description>
        ///</Summary>
        private SVD GetSVD(string SVD01, string SVD02, string SVD03, string SVD04, string SVD05,EDIClaimsServiceLines ediClaimServiceLine,EDI837FileModel ediFileModel)
        {
            StringBuilder modifiers = new StringBuilder();
            modifiers.Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier1) ? ":" + ediClaimServiceLine.Modifier1 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier2) ? ":" + ediClaimServiceLine.Modifier2 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier3) ? ":" + ediClaimServiceLine.Modifier3 : string.Empty)
                .Append(!string.IsNullOrEmpty(ediClaimServiceLine.Modifier4) ? ":" + ediClaimServiceLine.Modifier4 : string.Empty);
            SVD SVD = new SVD();
            SVD.SVD01 = SVD01;
            SVD.SVD02 = Convert.ToString(ediFileModel.EDIPayerPaymentModel.Where(x => x.ClaimServiceLineId == ediClaimServiceLine.ServiceLineId && x.DescriptionType.ToLower() == "payment").Sum(x => x.Amount));
            SVD.SVD03 = "HC:" + CreateString(ediClaimServiceLine.ServiceCode, (int)SegmentLength.One, (int)SegmentLength.FortyEight, ' ', true) + modifiers.ToString();
            SVD.SVD04 = SVD04;
            SVD.SVD05 = Convert.ToString(ediClaimServiceLine.Units);
            return SVD;
        }

        ///<Summary>
        ///<CreatedOn>09/04/2018</CreatedOn>
        ///<Author>Sunny Bhardwaj</Author>
        ///<Description>Returns object for CAS segment</Description>
        ///</Summary>
        ///
        private CAS GetCAS(string CAS01,string CAS02,string CAS03)
        {
            CAS CAS = new CAS();
            CAS.CAS01 = CAS01;
            CAS.CAS02 = CAS02;
            CAS.CAS03 = CAS03;
            return CAS;
        }

        #endregion



        private enum EDISegments
        {
            ISA,
            GS,
            ST,
            BHT,
            NM1,
            PER,
            HL,
            N3,
            N4,
            REF,
            SBR,
            DMG,
            PAT,
            CLM,
            DTP,
            HI,
            LX,
            SV1,
            SE,
            GE,
            IEA,
            AMT,
            OI,
            SVD,
            CAS
        }

        private static class EDIConstants
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
            public static string VersionReleaseNo = "005010X222A1"; ////Version / Release / Industry Identifier Code fix --005010X222A1


            /// <summary>
            /// ST Constants
            /// </summary>
            public static string TransactionSetHeader = "837";

            /// <summary>
            /// BHT Constants
            /// </summary>
            /// 
            public static string HierarchicalStructureCode = "0019";
            public static string TransactionSetPurposeCode = "00";//00 is used for original and 18 is used for resissue

            // 31	Subrogation Demand
            //CH Chargeable – Use this code when the transaction contains only fee for service claims or claims with at least one chargeable line item. if it is not clear whether a transaction contains claims or encounters, or if the transaction contains a mix of claims and encounters, the developers of this implementation guide recommend using code CH.
            //RP Reporting – Use RP When the entire ST-SE envelope contains encounters.Use RP When the transaction is being sent to an entity.
            public static string TransactionTypeCode = "CH";

            /// <summary>
            /// NM1 Submitter Segment (Loop 1000A)
            /// </summary>
            /// 
            public static string EntityIdentifierCode_Submitter = "41"; // This code works for Submitter
            public static string EntityTypeQualifier_Submiiter = "2";//1 is for person entity and 2 is for non-person entity
            public static string IdentitificationCodeQualifier_Submitter = "46";//Electronic Transmitter identification Number (ETIN) Established by trading partner agreement.

            /// <summary>
            /// PER Contact Info (Loop 1000A)
            /// </summary>
            /// 
            public static string ContactFunctionCode = "IC";
            public static string CommunicationNumberQualifier_Phone = "TE";
            public static string CommunicationNumberQualifier_Fax = "FX";


            /// <summary>
            /// NM1 Receiver Segment (Loop 1000B)
            /// </summary>
            /// 
            public static string EntityIdentifierCode_Receiver = "40"; // This code works for receiver
            public static string EntityTypeQualifier_Receiver = "2";//1 is for person entity and 2 is for non-person entity
            public static string IdentitificationCodeQualifier_Receiver = "46";//Electronic Transmitter identification Number (ETIN) Established by trading partner agreement.

            /// <summary>
            /// Billing Provider Hierarchical Level (Loop 2000A)
            /// </summary>
            /// 
            public static string HierarchicalLevelCode_Provider = "20"; //This code is for billing provider hierarchy
            public static string HierarchicalChildCode_Provider = "1";// 1 means another HL segment in this section

            /// <summary>
            /// Billing Providerinfo (Loop 2010AA)
            /// </summary>
            /// 
            public static string EntityIdentifierCode_BillingProvider = "85";
            public static string EntityTypeQualifier_BillingProvider = "1";
            public static string EntityTypeQualifier_BillingProviderPractice = "2";
            public static string IdentificationCodeQualifier_BillingProvider = "XX";
            public static string ReferenceIdentificationQualifier_BillingProvider = "EI";


            /// <summary>
            /// Subscriber HL segment (2000B)
            /// </summary>
            /// 

            public static string HierarchicalLevelCode_Subscriber = "22"; //This code is for billing provider hierarchy
                                                                          //public static string HierarchicalChildCode_Subscriber = "0";// 1 means another HL segment in this section



            /// <summary>
            /// Subscriber SBR segment(subscriber information)
            /// </summary>
            /// 
            public static string PayerResponsibilityCode = "P";//P ffro primary,S for secondary.There many more codes
            public static string IndivisualRelationshipCode = "18";//18 for self .There are other codes also


            /// <summary>
            /// Subscriber Name  (Loop 2010B)
            /// </summary>
            /// 
            public static string MemberIdentification_Subscriber = "MI";
            public static string EntityIdentifierCode_Subscriber = "IL";
            public static string EntityTypeQualifier_Subscriber = "1";


            /// <summary>
            /// Subscriber Name  (Loop 2010BA)
            /// </summary>
            /// 
            public static string DMG_DateTimeFomrat = "D8";//yyyy/mm/dd



            /// <summary>
            /// Payer Name  (Loop 2010BB)
            /// </summary>
            /// 
            public static string PayerIdentifierCode = "PR";
            public static string PayerEnityType = "2";
            public static string PayerIdentifierCodeQualifier = "PI";


            /// <summary>
            /// Patient Name  (Loop 2000C)
            /// </summary>
            /// 
            public static string HierarchicalLevelCode_Patient = "23";
            public static string EntityIdentifierCode_Patient = "QC";
            public static string EntityTypeQualifier_Patient = "1";
            public static string MemberIdentification_Patient = "MI";


            /// <summary>
            /// Claim Loop  (Loop 2300)
            /// </summary>
            /// 
            public static string ClaimFrequenctCode = "1";//	1 = Original; 7 = Replacement ;8 = Void
            public static string FacilityCodeQualifier = ":B:";
            public static string ClaimREFQualifier = "D9";
            public static string ClaimREFQualifier_Resubmit = "F8";


            /// <summary>
            /// DTP segment  (Loop 2300)
            /// </summary>
            /// 
            public static string DTPDateFormat = "D8";
            public static string OnsetDateQualifier = "431";
            public static string InitialTreatmentDateQualifier = "454";
            public static string DateOfServiceQualifier = "472";


            /// <summary>
            /// NM1 Rendering Provider  (Loop 2310B)
            /// </summary>
            /// 
            public static string EntityIdentifierCode_RenderingProvider = "82";
            public static string EntityTypeQualifier_RenderingProvider = "1";
            public static string IdentificationbCodeQualifier_RenderingProvider = "XX";

            //Comon for 2310C loop
            public static string StateLicenseNumberQualifier = "0B";
            public static string UPINNumberQualifier = "1G";
            public static string CommercialNumberQualifier = "G2";
            public static string LocationNumberQualifier = "LU";



            /// <summary>
            /// Service Location 2310C loop
            /// </summary>
            /// 
            public static string EntityIdentifierCode_ServiceLocation = "77";
            public static string EntityTypeQualifier_ServiceLocation = "2";
            public static string IdentificationbCodeQualifier_ServiceLocation = "XX";

        }
 
        #region Single EDI 837 Secondary Payer
        public string GenerateSingleEDI837_Secondary(EDI837FileModel ediFileModel)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B,2320,2330A,2330B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);
                        //Get primary payer,subscriber inf for secondary submission

                        GetOtherSubscriber(ediFileModel, ediClaimModel, ediText);

                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information
                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                            //Loop 2430 for payment and adjsutments of primary payer in case of secondary submission
                            if (ediFileModel.EDIOtherPayerInformationModel != null && ediFileModel.EDIOtherPayerInformationModel.Count > 0)
                            {
                                ediFileModel.EDIOtherPayerInformationModel.ForEach(x => {
                                    GetPaymentsAndAdjusments(x, ediClaimModel, claimServiceLinesList[lxCount], ediFileModel, ediText);
                                });
                            }
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Batch EDI 837 Secondary Payer
        public string GenerateBatchEDI837_Secondary(EDI837FileModel ediFileModel)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B,2320,2330A,2330B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);
                        //Get primary payer,subscriber inf for secondary submission

                        GetOtherSubscriber(ediFileModel, ediClaimModel, ediText);

                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information
                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                            //Loop 2430 for payment and adjsutments of primary payer in case of secondary submission
                            if (ediFileModel.EDIOtherPayerInformationModel != null && ediFileModel.EDIOtherPayerInformationModel.Count > 0)
                            {
                                ediFileModel.EDIOtherPayerInformationModel.ForEach(x => {
                                    GetPaymentsAndAdjusments(x, ediClaimModel, claimServiceLinesList[lxCount], ediFileModel, ediText);
                                });
                            }
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GenerateSingleEDI837_Tertiary(EDI837FileModel ediFileModel)
        {
            try
            {
                #region Set UP EDI variables
                EDIInterchangeHeaders interchangeHeaders = null;
                List<EDIClaimModel> ediClaimModelList = new List<EDIClaimModel>();
                List<EDIClaimsServiceLines> claimServiceLinesList = null;
                List<EDIDiagnosisCodesModel> ediDiagnosisCodeList = null;
                EDILocationAddressModel serviceAddress = null;
                EDILocationAddressModel billingAddress = null;
                StringBuilder ediText = new StringBuilder();
                EDIGatewayModel ediGateway = null;
                int hierarchicalIDNumber = 1;
                int hierarchicalParentIDNumber = 1;
                int count = 0;

                GetEDIVariablesData(ediFileModel, ref interchangeHeaders, ref ediClaimModelList, ref serviceAddress, ref billingAddress, ref ediGateway);
                #endregion

                #region Interchange Headers
                GetInterchangeHeaders(interchangeHeaders, ediText, ediGateway);
                #endregion

                #region Loop 1000A and 1000B Submitter and Receiver Information(Practice Details and Clearing House Details)
                //Loop 1000A  // Submitter Info
                GetSubmitter(interchangeHeaders, ediText, ediGateway);

                // Loop 1000B // Receiver Info
                GetReceiver(ediText, ediGateway);
                #endregion

                foreach (EDIClaimModel ediClaimModel in ediClaimModelList)
                {
                    if (count == 0)
                    {
                        hierarchicalParentIDNumber = 1;
                        ediDiagnosisCodeList = ediFileModel.EDIDiagnosisCodes.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();
                        claimServiceLinesList = ediFileModel.EDIClaimServiceLines.FindAll(x => x.ClaimId == ediClaimModel.ClaimId).ToList();

                        //if (count == 0)    ///just a temperary check
                        //{
                        #region Loop 2000A and 2010AA Billing Provider.It may be any practice(Non-Person Entity) or provider/doctor(Person Entity)

                        //Loop 2000A // Billing provider HL segment
                        hierarchicalIDNumber = GetBillingProvider(billingAddress, ediText, hierarchicalIDNumber);
                        count = count + 1;

                        #endregion
                        //}

                        #region Loop 2000B and 2010BA Subscriber

                        hierarchicalIDNumber = GetSubscriber(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);

                        #endregion

                        #region Loop 2010BB Payer details
                        //Loop 2010BB // Payer(insurance) name,address,city,state,date of birth,gender(NM1,N3,N4)
                        GetPayer(ediText, ediClaimModel);

                        #endregion

                        if (!ediClaimModel.InsurancePersonSameAsPatient)
                        {
                            #region 2000C and 2000CA Patient Info when Subscriber will not be same as patient
                            hierarchicalIDNumber = GetPatient(ediText, hierarchicalIDNumber, hierarchicalParentIDNumber, ediClaimModel);
                            #endregion
                        }

                        #region Loop 2300,2310A,2310B,2320,2330A,2330B Claim Loops
                        GetClaim(claimServiceLinesList, ediDiagnosisCodeList, serviceAddress, billingAddress, ediText, ediClaimModel);
                        //Get primary payer,subscriber inf for secondary submission

                        GetOtherSubscriber(ediFileModel, ediClaimModel, ediText);

                        #endregion
                    }

                    #region Loop 2400 Service Line(CPTCodes) Information
                    if (claimServiceLinesList != null)
                    {
                        //Loop 2400 -- Claim Service Line
                        for (int lxCount = 0; lxCount < claimServiceLinesList.Count; lxCount++)
                        {
                            GetClaimServiceLine(claimServiceLinesList, ediText, ediClaimModel, lxCount);
                            //Loop 2430 for payment and adjsutments of primary payer in case of secondary submission
                            if (ediFileModel.EDIOtherPayerInformationModel != null && ediFileModel.EDIOtherPayerInformationModel.Count > 0)
                            {
                                ediFileModel.EDIOtherPayerInformationModel.ForEach(x => {
                                    GetPaymentsAndAdjusments(x, ediClaimModel, claimServiceLinesList[lxCount], ediFileModel, ediText);
                                });
                            }
                        }
                    }
                    #endregion
                }

                #region Interchange Trailers
                GetInterchangeTrailer(interchangeHeaders, ediText);
                #endregion

                return ediText.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GenerateBatchEDI837_Tertiary(EDI837FileModel ediFileModel)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
