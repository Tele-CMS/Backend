using CreateClinicalReport.Model;
using CreateClinicalReport.ParserLibrary;
using HL7SDK.Cda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CreateClinicalReport
{
    public class RecordParser
    {
        GetComponents componentSections;
        ComponentDataCollection dataCollection;
        PatientClinicalInformation ptInformation;
        PatientDemographicDetail ptDemographic;
        ClinicInformation ptClinicInformation;
        List<DocumentationOfList> documentationOfInfo;
        List<PatientAllergies> ptAllergies;
        List<PatientProblemes> ptProblemes;
        List<VitalSigns> ptVitalSigns;
        SocialHistoryModel ptSocialHistory;
        List<PatientMedication> ptMedication;
        List<Encounters> ptEncounters;
        List<LabResult> ptLabResults;
        ReasonForVisit ptReason;
        List<Immunization> ptImmunization;
        List<PlanOfCare> ptPlanOfCare;
        List<ProcedureList> ptProcedure;
        List<FunctionalStatus> ptFunctionalStatus;
        Dictionary<string, object> objcollection = new Dictionary<string, object>();
        string reasonforTransfer;
        public PatientClinicalInformation ParseCCDAFile(string path, bool AsString)
        {
            ParseAddress prsadd = new ParseAddress();
            StreamReader sr = new StreamReader(path);
            HL7SDK.Cda.ClinicalDocument doc = new HL7SDK.Cda.ClinicalDocument();
            doc.Load(sr);

            ///Patient Clinic Summary Information
            ptInformation = new PatientClinicalInformation();
            /// Patient Demographic Information
            ptDemographic = new PatientDemographicDetail();
            if (doc.RecordTarget != null)
            {
                IADCollection ptaddess = doc.RecordTarget[0].PatientRole.Addr;
                AddressModel add = prsadd.FillAddress(ptaddess);///Parse Address
                                                                /// Patient Address
                ptDemographic.Street = add.street;
                ptDemographic.City = add.city;
                ptDemographic.State = add.state;
                ptDemographic.Country = add.country;
                ptDemographic.Zip = Convert.ToString(add.pinCode);
                ///// Patient Name
                NameModel ptname = prsadd.FillName(doc.RecordTarget[0].PatientRole.Patient.Name);
                ptDemographic.FirstName = doc.RecordTarget[0].PatientRole.Patient.Name[0].FindENGiven();///Get Patient First Name
                ptDemographic.LastName = doc.RecordTarget[0].PatientRole.Patient.Name[0].FindENFamily();///Get Patient Last Name
                ///// Patient DOB
                ITS ts = doc.RecordTarget[0].PatientRole.Patient.BirthTime;
                ptDemographic.DateofBirth = ts.AsDateTime.ToString();
                ///// Patient Gender
                ptDemographic.gender = doc.RecordTarget[0].PatientRole.Patient.AdministrativeGenderCode.Code == "M" ? "MALE" : "FEMALE";
                ///// Patient SSN
                try
                {
                    var record = doc.RecordTarget[0].PatientRole.Id.Where(k => k.Root == "2.16.840.1.113883.19.5").FirstOrDefault();
                    ptDemographic.SSN = record==null?"":record.Extension;
                }
                catch (Exception)
                {
                    ptDemographic.SSN = doc.RecordTarget[0].PatientRole.Id.FirstOrDefault().Extension.ToString();
                }

                ///// Patient PHNo.
                ptDemographic.ContactNo = Convert.ToString(doc.RecordTarget[0].PatientRole.Telecom[0].Value);
                ///// Patient Race
                ptDemographic.Race = doc.RecordTarget[0].PatientRole.Patient.RaceCode.DisplayName;
                ///// Patient Language
                if (doc.RecordTarget[0].PatientRole.Patient.LanguageCommunication.Count > 0)
                {
                    ptDemographic.PreferredLanguage = doc.RecordTarget[0].PatientRole.Patient.LanguageCommunication[0].LanguageCode.Code;
                    ptDemographic.LanguageCode = doc.RecordTarget[0].PatientRole.Patient.LanguageCommunication[0].LanguageCode.Code;
                }
                ///// Patient Ethencity
                ptDemographic.Ethnicity = doc.RecordTarget[0].PatientRole.Patient.EthnicGroupCode.DisplayName;
                ptInformation.ptDemographicDetail = ptDemographic;
            }
            ///Component OF
            IEncompassingEncounter componentOf = doc.ComponentOf.EncompassingEncounter;
            ptInformation.EncounterNoteDate = componentOf.EffectiveTime.Low != null ? componentOf.EffectiveTime.Low.Value != null ? Convert.ToString(componentOf.EffectiveTime.Low.AsDateTime) : null : null;
            ptInformation.EncounterCode = componentOf.Code.Code == null ? null : componentOf.Code.Code;
            ptInformation.EncounterDescription = componentOf.Code.DisplayName == null ? null : componentOf.Code.DisplayName;
            if (componentOf.ResponsibleParty.AssignedEntity.AssignedPerson.Name.Count > 0)
            {
                ptInformation.EncounterStaffName = componentOf.ResponsibleParty.AssignedEntity.AssignedPerson.Name[0].FindENFamily();///Performer Name (Staff/Clinician Name)
            }
            /// Clinic / Provider Detail
            ptClinicInformation = new ClinicInformation();
            if (doc.Author != null)
            {
                /////Clinic / Provider Address
                IADCollection ptaddess = doc.Author[0].AssignedAuthor.Addr;
                AddressModel add = prsadd.FillAddress(ptaddess);///Parse Address
                ptClinicInformation.ClinicCity = add.city;
                ptClinicInformation.ClinicState = add.state;
                ptClinicInformation.ClinicStreeet = add.street;
                ptClinicInformation.ClinicCountry = add.country;
                ptClinicInformation.ClinicZip = add.pinCode;
                ///// Clinic / Provider PHNo.
                ptClinicInformation.ClinicPhoneNumber = doc.Author[0].AssignedAuthor.Telecom[0].Value;
                ///// Clinic / Provider Name
                try { ptClinicInformation.ClinicName = doc.Author[0].AssignedAuthor.AsPerson.Name[0].FindENGiven(); } catch (Exception) { }

                ptInformation.ptClinicInformation = ptClinicInformation;
            }
            if (doc.DocumentationOf != null)
            {
                documentationOfInfo = new List<DocumentationOfList>();
                for (int i = 0; i < doc.DocumentationOf.Count; i++)
                {
                    DocumentationOfList docof = new DocumentationOfList();
                    IADCollection ptaddess = doc.DocumentationOf[i].ServiceEvent.Performer[0].AssignedEntity.Addr;
                    AddressModel add = prsadd.FillAddress(ptaddess);
                    docof.address = add.street;
                    docof.city = add.city;
                    docof.state = add.state;
                    docof.pinCode = add.pinCode;
                    if (doc.DocumentationOf[i].ServiceEvent.Performer[0].AssignedEntity.AssignedPerson.Name.Count > 0)
                    {
                        docof.staffName = doc.DocumentationOf[i].ServiceEvent.Performer[0].AssignedEntity.AssignedPerson.Name[0].FindENFamily();
                    }
                    documentationOfInfo.Add(docof);
                }

                ptInformation.documentationOfInfo = documentationOfInfo;
            }
            /// Get All Document Component
            if (doc.Component.AsStructuredBody.Component != null)
            {

                IComponent3Collection item = doc.Component.AsStructuredBody.Component;
                IEnumerable<ISection> sections = item.Select(s => s.Section);
                IEnumerable<IStrucDocText> text = sections.Select(t => t.Text);
                IEnumerable<IStrucDocElementCollection> textitems = text.Select(tb => tb.Items);
                foreach (ISection funcststus in sections)
                {
                    string snomdcode = funcststus.Code.Code;
                    componentSections = new GetComponents();
                    dataCollection = new ComponentDataCollection();
                    //Dictionary<string, ArrayList> DataArr = dataCollection.GetDataCollection(funcststus, ptInformation);
                    switch (snomdcode)
                    {
                        case "48765-2":/// Patient Allergies Information
                            // ptAllergies = componentSections.GetAllergies(DataArr);
                            ptAllergies = componentSections.FillAllergies(funcststus.Entry);
                            break;
                        case "11450-4":/// Patient Problems Information
                            //ptProblemes = componentSections.GetProblems(DataArr);
                            ptProblemes = componentSections.FillProblems(funcststus.Entry);
                            break;
                        case "29762-2":/// Patient Social History Information
                            //ptSocialHistory = componentSections.GetSocialHistory(DataArr);
                            ptSocialHistory = componentSections.FillSocialHistory(funcststus.Entry);
                            break;
                        case "8716-3":/// Patient Vital Signs Information
                            //ptVitalSigns = componentSections.GetVitalSigns(DataArr);
                            ptVitalSigns = componentSections.FillVitalSigns(funcststus.Entry);
                            break;
                        case "10160-0":/// Patient Medication Information
                            // ptMedication = componentSections.GetMedication(DataArr);
                            ptMedication = componentSections.FillMedication(funcststus.Entry);
                            break;
                        case "46240-8":/// Patient ENCOUNTERS Information
                            // ptEncounters = componentSections.GetEncounters(DataArr);
                            ptEncounters = componentSections.FillEncounters(funcststus.Entry, ptInformation.EncounterStaffName);
                            break;
                        case "30954-2":/// Patient Lab Results Information
                            //ptLabResults = componentSections.GetLabResults(DataArr);
                            ptLabResults = componentSections.FillLabResults(funcststus.Entry);
                            break;
                        case "46239-0":/// Patient Reason For Visit Information
                            //ptReason = componentSections.GetReason(DataArr);
                            ptReason = componentSections.FillReason(funcststus);
                            break;
                        case "11369-6":/// Patient Immunizations Information
                            //ptImmunization = componentSections.GetImmunization(DataArr);
                            ptImmunization = componentSections.FillImmunization(funcststus.Entry);
                            break;
                        case "18776-5":/// Patient Plan Of Care Information
                            //ptPlanOfCare = componentSections.GetPlanOfCare(DataArr);
                            ptPlanOfCare = componentSections.FillPlanOfCare(funcststus.Entry);
                            break;
                        case "42349-1":/// Patient Reason For Transfer Information
                            //reasonforTransfer = componentSections.GetReasonForTransfer(DataArr);
                            reasonforTransfer = componentSections.FillReasonForTransfer(funcststus);
                            break;
                        case "47519-4":/// Patient Procedure Information
                            //ptProcedure = componentSections.GetProcedure(DataArr);
                            ptProcedure = componentSections.FillProcedure(funcststus.Entry);
                            break;
                        case "47420-5":/// Patient Functional Status Information
                            //ptFunctionalStatus = componentSections.GetFunctionalStatus(DataArr);
                            ptFunctionalStatus = componentSections.FillFunctionalStatus(funcststus.Entry);
                            break;
                    }
                }
                ///Encapsulate Patient Information In A Single Model
                ptInformation.ptAllergies = ptAllergies;
                ptInformation.ptProblemes = ptProblemes;
                ptInformation.ptVitalSigns = ptVitalSigns;
                ptInformation.ptSocialHistory = ptSocialHistory;
                ptInformation.ptMedication = ptMedication;
                ptInformation.ptEncounters = ptEncounters;
                ptInformation.ptLabResults = ptLabResults;
                ptInformation.ptReason = ptReason;
                ptInformation.ptImmunization = ptImmunization;
                ptInformation.ptPlanOfCare = ptPlanOfCare;
                ptInformation.reasonforTransfer = reasonforTransfer;
                ptInformation.ptProcedure = ptProcedure;
                ptInformation.ptFunctionalStatus = ptFunctionalStatus;
                ///END
            }

            return ptInformation;
        }

    }


}
