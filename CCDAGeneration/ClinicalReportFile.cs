using CreateClinicalReport.Actions;
using CreateClinicalReport.Model;
using HC.Common.HC.Common;
using HL7SDK.Cda;
using System;
using System.IO;
using System.Xml;

namespace CreateClinicalReport
{
    public class ClinicalReportFile : IDisposable
    {
        //public ClinicalReportFile()
        //{
        //}
        private ClinicalDocument clreport;

        private III hl7III;
       
        private IAD hl7IADInterface;
        private IADXP hl7IADXPInterface;
        private ICD icdObjects;
        private Factory hl7Factory = new Factory();
        private ICS realmCode;
      
        GenerateHeader rpHeader;
        GenerateRecordTarget rpTarget;
        GetAuthorInformation rpClinicInfo;
        GenerateDataEnterer rpClinicDataEntererInfo;
        GenerateInformant rpClinicInformant;
        GenerateCustodian rpClinicCustodian;
        GenerateRecipientInfo rpRecipientInfo;
        GenerateLegalAuthenticator rpLegalAuthenticator;
        GenerateAuthenticator rpAuthenticator;
        GenerateParticipantInfo rpParticipantInfo;
        GenerateDocumentationOf rpDocumentationOf;
        GenerateComponent rpGenerateComponent;
        GenerateAllergies rpAllergies;
        GeneratePatientProblem rpProblemes;
        GenerateSocialHistory rpSocialHistory;
        GenerateVitalSigns rpVitalSigns;
        GenerateMedication rpMedication;
        GenerateFunctionalStatus rpFunctionalStatus;
        GenerateEncounters rpEncounters;
        GenerateLabResults rpLabResult;
        GenerateReasonForVisit rpReasonforVisit;
        GenerateImmunization rpImmunizationt;
        GeneratePlanOfCare rpPlanOfCare;
        GenerateReasonforTransfer rpReasonforTransfer;
        GenerateProcedure rpProcedure;
        XmlDocument doc;
        MemoryStream xmlStream;

        #region"Generate CCDA (Clinical Document Architecture)"
        public MemoryStream GenerateCCDA(PatientClinicalInformation patientinfo, string title = "smartData")
        {


            string result = string.Empty;
            clreport = new ClinicalDocument();
            hl7Factory = new Factory();

            rpHeader = new GenerateHeader();
            rpTarget = new GenerateRecordTarget();
            rpClinicInfo = new GetAuthorInformation();
            rpClinicDataEntererInfo = new GenerateDataEnterer();
            rpClinicInformant = new GenerateInformant();
            rpClinicCustodian = new GenerateCustodian();
            rpRecipientInfo = new GenerateRecipientInfo();
            rpLegalAuthenticator = new GenerateLegalAuthenticator();
            rpAuthenticator = new GenerateAuthenticator();
            rpParticipantInfo = new GenerateParticipantInfo();
            rpDocumentationOf = new GenerateDocumentationOf();
            rpGenerateComponent = new GenerateComponent();
            rpAllergies = new GenerateAllergies();
            rpProblemes = new GeneratePatientProblem();
            rpSocialHistory = new GenerateSocialHistory();
            rpVitalSigns = new GenerateVitalSigns();
            rpMedication = new GenerateMedication();
            rpFunctionalStatus = new GenerateFunctionalStatus();
            rpEncounters = new GenerateEncounters();
            rpLabResult =new GenerateLabResults();
            rpReasonforVisit = new GenerateReasonForVisit();
            rpImmunizationt = new GenerateImmunization();
            rpPlanOfCare = new GeneratePlanOfCare();
            rpReasonforTransfer = new GenerateReasonforTransfer();
            rpProcedure = new GenerateProcedure();
            xmlStream = new MemoryStream();

            doc = new XmlDocument();
            try
            {
                //Bind Report Header
                result = rpHeader.BindHeader(title, clreport, hl7Factory, realmCode);
                //END
                /*********************Bind Report Suumary***************************************/
                //Bind Client RecordTarget
                if (patientinfo.ptDemographicDetail != null)
                {
                    result = rpTarget.BindRecordTarget(title, clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Clinic Author Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpClinicInfo.FillAuthorInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Clinic Data Enterer Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpClinicDataEntererInfo.FillDataEnterer(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Clinic Informant Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpClinicInformant.FillInformantInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Clinic Custodian Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpClinicCustodian.FillCustodianInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Recipient Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpRecipientInfo.FillRecipientInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Legal Authenticator Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpLegalAuthenticator.FillLegalAuthenticatorInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Authenticator Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpAuthenticator.FillLegalAuthenticatorInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Participant Information
                if (patientinfo.ptClinicInformation != null)
                {
                    result = rpParticipantInfo.FillParticipantInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Documentation Of Information
                if (patientinfo.ptClinicInformation != null && patientinfo.ptDemographicDetail != null)
                {
                    result = rpDocumentationOf.FillDocumentationOf(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Component Of Information
                if (patientinfo.EncounterStaffName != null && patientinfo.EncounterNoteDate != null)
                {
                    result = rpGenerateComponent.FillComponentInfo(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Allergies Information
                if (patientinfo.ptAllergies != null && patientinfo.ptAllergies.Count > 0)
                {
                    result = rpAllergies.FillPatientAllergies(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Problemes Information
                if (patientinfo.ptProblemes != null && patientinfo.ptProblemes.Count > 0)
                {
                    result = rpProblemes.FillPatientProblemes(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind SocialHistory Information
                if (patientinfo.ptSocialHistory != null)
                {
                    result = rpSocialHistory.FillSocialHistory(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Vital Signs Information
                if (patientinfo.ptVitalSigns != null && patientinfo.ptVitalSigns.Count > 0)
                {
                    result = rpVitalSigns.FillVitalSigns(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Medication Information
                if (patientinfo.ptMedication != null && patientinfo.ptMedication.Count > 0)
                {
                    result = rpMedication.FillPatientMedication(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Functional Status Information
                if (patientinfo.ptFunctionalStatus != null && patientinfo.ptFunctionalStatus.Count > 0)
                {
                    result = rpFunctionalStatus.FillFunctionalStatus(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Encounter Information
                if (patientinfo.ptEncounters != null && patientinfo.ptEncounters.Count > 0)
                {
                    result = rpEncounters.FillEncounters(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Lab Result Information
                if (patientinfo.ptLabResults != null && patientinfo.ptLabResults.Count > 0)
                {
                    result = rpLabResult.FillPatientLabResult(clreport, hl7Factory, hl7III, patientinfo);
                }
                //END
                //Bind Reason For Visit Information
                string reasonforVisitInfo = rpReasonforVisit.FillReasonForVisit(clreport, hl7Factory, hl7III, patientinfo);

                //END
                //Bind Immunization Information
                if (patientinfo.ptImmunization != null && patientinfo.ptImmunization.Count > 0)
                {
                    result = rpImmunizationt.FillPatientImmunization(clreport, hl7Factory, hl7III, patientinfo);
                }

                //END
                //Bind Plan Of Care Information
                if (patientinfo.ptPlanOfCare != null && patientinfo.ptPlanOfCare.Count > 0)
                {
                    result = rpPlanOfCare.FillPlanOfCare(clreport, hl7Factory, hl7III, patientinfo);
                }

                //END
                // Bind Reason For Referral Information
                if (patientinfo.ptReason != null)
                {
                    result = rpReasonforTransfer.FillReasonForReferral(clreport, hl7Factory, hl7III, patientinfo);
                }

                // END
                // Bind Procedures Information
                if (patientinfo.ptProcedure != null && patientinfo.ptProcedure.Count > 0)
                {
                    result = rpProcedure.FillPatientProcedure(clreport, hl7Factory, hl7III, patientinfo);
                }
                // END
                /*********************END*******************************************************/
                //result = ptProcedure;

  
                string resulttext = "<?xml-stylesheet type='text/xsl' href ='CDA.xsl'?>" + result.Trim();
                doc.LoadXml(resulttext.ToString().Trim());

                string webRootPath = "";

                //get root path
                webRootPath = Directory.GetCurrentDirectory();

                if (!Directory.Exists(webRootPath + CCDAPath.CCDAXmlPath))
                {
                    Directory.CreateDirectory(webRootPath + CCDAPath.CCDAXmlPath);
                }

                doc.Save(webRootPath + CCDAPath.CCDAXmlPath + "CDA.xml");

                XmlDocument xmlDoc = new XmlDocument();

                doc.Save(xmlStream);

                xmlStream.Flush();//Adjust this if you want read your data 
                xmlStream.Position = 0;


            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            //return new HttpResponseMessage()
            //{
            //    Content = new StringContent(doc.ToString(), Encoding.UTF8, "application/xml")
            //};
            return xmlStream;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ClinicalReportFile() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        #endregion

    }
}
