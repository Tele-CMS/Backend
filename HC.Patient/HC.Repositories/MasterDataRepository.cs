using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;


namespace HC.Patient.Repositories
{
    public class MasterDataRepository : IRepository
    {
        private readonly HCMasterContext _masterContext;
        private readonly HCOrganizationContext _organizationContext;
        public MasterDataRepository(HCMasterContext masterContext, HCOrganizationContext organizationContext)
        {
            this._masterContext = masterContext;
            this._organizationContext = organizationContext;
        }

        /// <summary>
        /// get master's table data by its name
        /// </summary>
        /// <param name="masterDataNames"></param>
        /// <returns></returns>
        public MasterDataModel GetMasterDataByName(List<string> masterDataNames, TokenModel token, string globalCodeId = "")
        {
            try
            {
                MasterDataModel listMasterDataModel = new MasterDataModel();
                GetMasterData(masterDataNames, listMasterDataModel, token, globalCodeId);
                return listMasterDataModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get the state by country id  
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        public List<MasterState> GetStateByCountryID(int countryID)
        {
            try
            {
                return _organizationContext.MasterState.Where(m => m.CountryID == countryID && m.IsActive == true && m.IsDeleted == false).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get master table's data by name
        /// </summary>
        /// <param name="masterDataNames"></param>
        /// <param name="listMasterDataModel"></param>
        private void GetMasterData(List<string> masterDataNames, MasterDataModel listMasterDataModel, TokenModel token, string globalCodeId)
        {
            string flag = string.Empty;
            try
            {


                masterDataNames.ForEach(p =>
                {
                    flag = p;
                    if (Enum.IsDefined(typeof(MasterDataEnum), p.ToUpper()))
                    {
                        switch ((MasterDataEnum)Enum.Parse(typeof(MasterDataEnum), p.ToUpper()))
                        {
                            case MasterDataEnum.MASTERCOUNTRY:
                                listMasterDataModel.MasterCountry = _organizationContext.MasterCountry.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.CountryName).ToList();
                                break;
                            case MasterDataEnum.MASTERETHNICITY:
                                listMasterDataModel.MasterEthnicity = _organizationContext.MasterEthnicity.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.EthnicityName).ToList();
                                break;
                            case MasterDataEnum.MASTERPHONEPREFERENCES:
                                listMasterDataModel.MasterPhonePreferences = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "phonepreference".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            //case MasterDataEnum.MASTEROCCUPATION:
                            //    listMasterDataModel.MasterOccupation = _organizationContext.MasterOccupation.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.OccupationName).ToList();
                            //    break;
                            //case MasterDataEnum.MASTERPREFERREDLANGUAGE:
                            //    listMasterDataModel.MasterPreferredLanguage = _organizationContext.MasterPreferredLanguage.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Language).ToList();
                            //    break;
                            case MasterDataEnum.MASTERRACE:
                                listMasterDataModel.MasterRace = _organizationContext.MasterRace.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.RaceName).ToList();
                                break;
                            case MasterDataEnum.MASTERSTATE:
                                listMasterDataModel.MasterState = _organizationContext.MasterState.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.StateName).ToList();
                                break;
                            case MasterDataEnum.SUFFIX:
                                listMasterDataModel.MasterSuffix = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "suffix".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            //case MasterDataEnum.MASTERPROVIDER:
                            //    listMasterDataModel.Provider = _context.Provider.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();
                            //    break;
                            case MasterDataEnum.MARITALSTATUS:
                                listMasterDataModel.MasterMaritalStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "maritalstatus".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();//_context.MasterStatus.Where(a => a.IsActive == true && a.IsDeleted == false && a.Type.ToUpper() == p.ToUpper()).OrderBy(a => a.StatusName).ToList();
                                break;
                            case MasterDataEnum.MASTERRELATIONSHIP:
                                listMasterDataModel.MasterRelationship = _organizationContext.MasterRelationship.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();//_context.MasterRelationship.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.RelationshipName).ToList();
                                //if (listMasterDataModel.MasterRelationship.Count() > 0)
                                //{
                                //    //Other relation field should be at the end position
                                //    MasterRelationship other = listMasterDataModel.MasterRelationship.Where(a => a.RelationshipName.ToUpper() == "OTHER" && a.IsActive == true && a.IsDeleted == false /*&& a.OrganizationID == token.OrganizationID*/).FirstOrDefault();
                                //    if (other != null)
                                //    {
                                //        listMasterDataModel.MasterRelationship.Remove(other); //remove other relationship from sorted list
                                //        listMasterDataModel.MasterRelationship.Add(other); //added other relationship at the end position
                                //    }
                                //}
                                break;
                            case MasterDataEnum.MASTERPROGRAM:
                                listMasterDataModel.MasterProgram = _organizationContext.MasterProgram.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ProgramName).ToList();
                                break;
                            case MasterDataEnum.PATIENTSTATUS:
                                listMasterDataModel.MasterPatientStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "patientstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();//_context.MasterStatus.Where(a => a.IsActive == true && a.IsDeleted == false && a.Type.ToUpper() == p.ToUpper()).OrderBy(a => a.StatusName).ToList();
                                break;
                            case MasterDataEnum.MASTERINSURANCECOMPANY:
                                listMasterDataModel.InsuranceCompanies = _organizationContext.InsuranceCompanies.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Name).ToList();
                                break;
                            //case MasterDataEnum.MASTERPATIENTCOMMPREFERENCES:
                            //    listMasterDataModel.MasterPatientCommPreferences = _organizationContext.MasterPatientCommPreferences.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.PatientCommPreferences).ToList();//_context.MasterPatientCommPreferences.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.PatientCommPreferences).ToList();
                            //    break;
                            case MasterDataEnum.MASTERREFERRAL:
                                listMasterDataModel.MasterReferral = _organizationContext.MasterReferral.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ReferralName).ToList();
                                break;
                            case MasterDataEnum.MASTERGENDER:
                                listMasterDataModel.MasterGender = _organizationContext.MasterGender.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Gender).ToList();
                                break;
                            case MasterDataEnum.PHONETYPE:
                                listMasterDataModel.MasterPhoneType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "phonetype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();//_context.MasterType.Where(a => a.IsActive == true && a.IsDeleted == false && a.Type.ToUpper() == p.ToUpper()).OrderBy(a => a.TypeName).ToList();
                                break;
                            case MasterDataEnum.INSURANCEPLANTYPE:
                                listMasterDataModel.InsurancePlanType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "insuranceplantype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            //case MasterDataEnum.MASTERCPT:
                            //    listMasterDataModel.MasterCPT = _organizationContext.MasterCPT.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CPTCode).ToList();
                            //    break;                        
                            case MasterDataEnum.MASTERSERVICECODE:
                                listMasterDataModel.MasterServiceCode = _organizationContext.MasterServiceCode.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).Include(x => x.MasterServiceCodeModifiers).OrderBy(a => a.ServiceCode).ToList();
                                break;
                            case MasterDataEnum.MASTERICD:
                                listMasterDataModel.MasterICD = _organizationContext.MasterICD.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Code).ToList();
                                break;
                            //Following are the Master tables for Immunization
                            case MasterDataEnum.MASTERVFC:
                                listMasterDataModel.MasterVFC = _organizationContext.MasterVFCEligibility.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ConceptCode).ToList();
                                break;
                            case MasterDataEnum.MASTERIMMUNIZATION:
                                listMasterDataModel.MasterImmunization = _organizationContext.MasterImmunization.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.CvxCode).ToList();
                                break;
                            case MasterDataEnum.MASTERMANUFACTURE:
                                listMasterDataModel.MasterManufacture = _organizationContext.MasterManufacture.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ManufacturerName).ToList();
                                break;
                            case MasterDataEnum.MASTERADMINISTRATIONSITE:
                                listMasterDataModel.MasterAdministrationSite = _organizationContext.MasterAdministrationSite.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Description).ToList();
                                break;
                            case MasterDataEnum.MASTERROUTEOFADMINISTRATION:
                                listMasterDataModel.MasterRouteOfAdministration = _organizationContext.MasterRouteOfAdministration.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Description).ToList();
                                break;
                            case MasterDataEnum.MASTERIMMUNITYSTATUS:
                                listMasterDataModel.MasterImmunityStatus = _organizationContext.MasterImmunityStatus.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ConceptCode).ToList();
                                break;
                            case MasterDataEnum.MASTERREJECTIONREASON:
                                listMasterDataModel.MasterRejectionReason = _organizationContext.MasterRejectionReason.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.ReasonDesc).ToList();
                                break;
                            case MasterDataEnum.SOCIALHISTORY:
                                listMasterDataModel.MasterSocialHistory = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "socialhistory" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.TRAVELHISTORY:
                                listMasterDataModel.MasterTravelHistory = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "travelhistory" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.ADDRESSTYPE:
                                listMasterDataModel.MasterAddressType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "addresstype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERSTAFF:
                                //listMasterDataModel.Staffs = _organizationContext.Staffs.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.FirstName).ToList();
                                listMasterDataModel.Staffs = _organizationContext.Staffs.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID && (a.UserRoles.UserType.ToLower() == UserTypeEnum.STAFF.ToString().ToLower() || a.UserRoles.UserType.ToLower() == UserTypeEnum.PROVIDER.ToString().ToLower())).OrderBy(a => a.FirstName).ToList();
                                break;
                            case MasterDataEnum.APPOINTMENTTYPE:
                                listMasterDataModel.AppointmentType = _organizationContext.AppointmentType.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Name).ToList();
                                break;
                            case MasterDataEnum.APPOINTMENTSTATUS:
                                listMasterDataModel.AppointmentStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "appointmentstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.APPOINTMENTIMEGRAPHFILTER:
                                listMasterDataModel.AppointmentTimeGraphFilter = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "appointmentgraphfilter" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                }).ToList();
                                break;
                            //Following are the Master tables for Lab
                            case MasterDataEnum.LABTESTTYPE:
                                listMasterDataModel.MasterLabTestType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "labtesttype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERLONIC:
                                listMasterDataModel.MasterLonic = _organizationContext.MasterLonic.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.LonicCode).ToList();
                                break;
                            case MasterDataEnum.MASTERLABS:
                                listMasterDataModel.MasterLabs = _organizationContext.MasterLabs.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.LabName).ToList();
                                break;
                            case MasterDataEnum.TIMETYPE:
                                listMasterDataModel.MasterTimeType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "timetype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.FREQUENCYTYPE:
                                listMasterDataModel.MasterFrequencyType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "frequencytype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.FREQUENCYDURATIONTYPE:
                                listMasterDataModel.MasterFrequencyDurationType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "frequencydurationtype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            //Master Allergy
                            case MasterDataEnum.MASTERALLERGIES:
                                listMasterDataModel.MasterAllergies = _organizationContext.MasterAllergies.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.AllergyType).ToList();
                                break;
                            case MasterDataEnum.MASTERREACTION:
                                listMasterDataModel.MasterReaction = _organizationContext.MasterReaction.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Reaction).ToList();
                                break;
                            case MasterDataEnum.AUTHORIZEDPROCEDURE:
                                listMasterDataModel.MasterAuthorizedProcedure = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "authorizedprocedure" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERCUSTOMLABELTYPE:
                                listMasterDataModel.MasterCustomLabelType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "mastercustomlabels" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERCUSTOMLABELS:
                                listMasterDataModel.MasterCustomLabels = _organizationContext.MasterCustomLabels.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID && (a.RoleTypeID == 1 || a.RoleTypeID == 3)).OrderBy(a => a.Id).Select(t => new MasterCustomLabels { CustomLabelName = t.CustomLabelName, Id = t.Id }).ToList();   //MasterType = t.MasterType }).ToList();                            
                                break;
                            case MasterDataEnum.MASTERPATIENTLOCATION:
                                listMasterDataModel.MasterPatientLocation = _organizationContext.MasterPatientLocation.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Location).ToList();
                                break;
                            case MasterDataEnum.MASTERTAGS:
                                listMasterDataModel.MasterTags = _organizationContext.MasterTags.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Tag).ToList();
                                break;
                            case MasterDataEnum.MASTERCUSTOMLABELSTAFF:
                                listMasterDataModel.MasterCustomLabelStaff = _organizationContext.MasterCustomLabels.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID && (a.RoleTypeID == 1 || a.RoleTypeID == 3)).OrderBy(a => a.Id).Select(t => new MasterCustomLabels { CustomLabelName = t.CustomLabelName, Id = t.Id }).ToList();//, MasterType = t.MasterType }).ToList();
                                break;
                            case MasterDataEnum.MASTERROLES://TO DO need to discuss
                                listMasterDataModel.MasterRoles = _organizationContext.UserRoles.Where(a => a.IsActive == true && a.IsDeleted == false && (a.UserType.ToLower() == UserTypeEnum.STAFF.ToString().ToLower() || a.UserType.ToLower() == UserTypeEnum.PROVIDER.ToString().ToLower()) && a.OrganizationID == token.OrganizationID).OrderBy(a => a.RoleName).ToList();
                                break;
                            case MasterDataEnum.MASTERROLESALL://TO DO need to discuss
                                listMasterDataModel.MasterRoles = _organizationContext.UserRoles.Where(a => a.IsActive == true && a.IsDeleted == false && (a.UserType.ToLower() == UserTypeEnum.PROVIDER.ToString().ToLower() || a.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower()) && a.OrganizationID == token.OrganizationID).OrderBy(a => a.RoleName).ToList();
                                break;
                            case MasterDataEnum.MASTERWEEKDAYS:
                                listMasterDataModel.MasterWeekDays = _organizationContext.MasterWeekDays.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Id).ToList();
                                break;
                            case MasterDataEnum.MASTERUNITTYPE:
                                listMasterDataModel.MasterUnitType = _organizationContext.MasterUnitType.Where(h => h.IsActive == true && h.IsDeleted == false && h.OrganizationID == token.OrganizationID).OrderBy(a => a.UnitTypeName).ToList();
                                break;
                            case MasterDataEnum.MASTERROUNDINGRULES:
                                listMasterDataModel.MasterRoundingRules = _organizationContext.MasterRoundingRules.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.RuleName).ToList();
                                break;
                            case MasterDataEnum.MASTERLOCATION:
                                listMasterDataModel.MasterLocation = _organizationContext.Location.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.LocationName).ToList();
                                break;
                            //case MasterDataEnum.MASTERTEMPLATES:
                            //listMasterDataModel.MasterTemplates = _masterContext.MasterTemplates.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.TemplateName).ToList();
                            //break;
                            case MasterDataEnum.ORGANIZATIONDATABASEDETAIL:
                                List<int> values = _masterContext.MasterOrganization.Select(z => z.DatabaseDetailId).ToList();//get the used database ids
                                listMasterDataModel.OrganizationDatabaseDetail = _masterContext.OrganizationDatabaseDetail.Where(a => !values.Contains(a.Id) || a.IsCentralised == true).ToList();
                                break;
                            case MasterDataEnum.MASTERORGANIZATION:
                                listMasterDataModel.MasterOrganization = _masterContext.MasterOrganization.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.OrganizationName).ToList();
                                break;
                            case MasterDataEnum.USERROLETYPE:
                                listMasterDataModel.UserRoleType = _organizationContext.UserRoleType.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.RoleTypeName).ToList();
                                break;
                            case MasterDataEnum.MASTERINSURANCETYPE://TO DO need to discuss
                                listMasterDataModel.MasterInsuranceType = _organizationContext.MasterInsuranceTypes.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.InsuranceType).ToList();
                                break;
                            case MasterDataEnum.STAFFAVAILABILITY:
                                listMasterDataModel.StaffAvailability = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "availabilitytemplatetype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.ENCOUNTERSTATUS:
                                listMasterDataModel.EncounterStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "encounterstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERCANCELTYPE:
                                listMasterDataModel.MasterCancelType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "appointmentcancellationtype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERRENDERINGPROVIDER:
                                listMasterDataModel.MasterRenderingProvider = _organizationContext.Staffs.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID /*&& a.UserRoles.UserType.ToLower() == UserTypeEnum.STAFF.ToString().ToLower()*/ && a.IsRenderingProvider == true).OrderBy(a => a.FirstName).ToList();
                                break;
                            case MasterDataEnum.MASTERPAYMENTTYPE:
                                listMasterDataModel.MasterPaymentType = _organizationContext.MasterPaymentType.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Key).ToList();
                                break;
                            case MasterDataEnum.MASTERPAYMENTDESCRIPTION:
                                listMasterDataModel.MasterPaymentDescription = _organizationContext.MasterPaymentDescription.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.Key).ToList();
                                break;
                            case MasterDataEnum.EMPLOYEETYPE:
                                listMasterDataModel.EmployeeType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "employeetype".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.REFERRALSOURCE:
                                listMasterDataModel.ReferralSource = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "referralsource".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.EMPLOYEEMENT:
                                listMasterDataModel.Employeement = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "employeement".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERDOCUMENTTYPES:
                                listMasterDataModel.MasterDocumentTypes = _organizationContext.MasterDocumentTypes.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERDOCUMENTTYPESSTAFF:
                                listMasterDataModel.MasterDocumentTypesStaff = _organizationContext.MasterDocumentTypesStaff.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERDEGREE:
                                listMasterDataModel.MasterDegree = _organizationContext.MasterDegree.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DegreeName).ToList();
                                break;
                            case MasterDataEnum.CLAIMRESUBMISSIONREASON:
                                listMasterDataModel.ClaimResubmissionReason = _organizationContext.ClaimResubmissionReason.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.Id).Select(b => new Model.Claim.ClaimResubmissionReasonModel()
                                {
                                    Id = b.Id,
                                    ResubmissionCode = b.ResubmissionCode,
                                    ResubmissionReason = b.ResubmissionReason,
                                    Description = b.Description,
                                    Value = b.Description
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERADJUSTMENTGROUPCODE:
                                listMasterDataModel.AdjustmentGroupCodeModel = _organizationContext.MasterAdjustmentGroupCode.Where(x => x.IsActive == true && x.IsDeleted == false).
                                    OrderBy(y => y.Id).Select(z => new AdjustmentGroupCodeModel()
                                    {
                                        Id = z.Id,
                                        Value = z.Code,
                                        Description = z.CodeDescription
                                    }).ToList();
                                break;
                            case MasterDataEnum.LEAVETYPE:
                                listMasterDataModel.LeaveType = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "leavetype".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.LEAVEREASON:
                                listMasterDataModel.LeaveReason = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "leavereason".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.LEAVESTATUS:
                                listMasterDataModel.LeaveStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "leavestatus".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.CLAIMPAYMENTSTATUS:
                                listMasterDataModel.ClaimPaymentStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "claimpaymentstatus".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.CLAIMPAYMENTSTATUSFORLEDGER:
                                listMasterDataModel.ClaimPaymentStatusForLedger = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "claimpaymentstatusforledger".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            case MasterDataEnum.MASTERTAGSFORSTAFF:
                                listMasterDataModel.MasterTagsforStaff = _organizationContext.MasterTags.Join(_organizationContext.UserRoleType, a => a.RoleTypeID, ur => ur.Id, ((a, ur) => new MasterTags()
                                {
                                    Id = a.Id,
                                    Tag = a.Tag,
                                    value = a.Tag,
                                    IsActive = a.IsActive,
                                    IsDeleted = a.IsDeleted,
                                    OrganizationID = a.OrganizationID,
                                    TypeKey = ur.TypeKey,
                                })).Where(x => (x.TypeKey == "ALL_STAFF" || x.TypeKey == "BOTH") && x.IsActive == true && x.IsDeleted == false && x.OrganizationID == token.OrganizationID).OrderBy(x => x.Tag).ToList();
                                break;
                            case MasterDataEnum.MASTERTAGSFORPATIENT:
                                listMasterDataModel.MasterTagsforPatient = _organizationContext.MasterTags.Join(_organizationContext.UserRoleType, a => a.RoleTypeID, ur => ur.Id, ((a, ur) => new MasterTags()
                                {
                                    Id = a.Id,
                                    Tag = a.Tag,
                                    value = a.Tag,
                                    IsActive = a.IsActive,
                                    IsDeleted = a.IsDeleted,
                                    OrganizationID = a.OrganizationID,
                                    TypeKey = ur.TypeKey,
                                })).Where(x => (x.TypeKey == "ALL_CLIENT" || x.TypeKey == "BOTH") && x.IsActive == true && x.IsDeleted == false && x.OrganizationID == token.OrganizationID).OrderBy(x => x.Tag).ToList();
                                break;

                            case MasterDataEnum.TIMESHEETSTATUS:
                                listMasterDataModel.TimesheetStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "timesheetstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;

                            case MasterDataEnum.PAYPERIOD:
                                listMasterDataModel.PayPeriod = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "payperiod" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;

                            case MasterDataEnum.WORKWEEK:
                                listMasterDataModel.WorkWeek = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "workweek" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();
                                break;
                            //case MasterDataEnum.PAYROLLGROUP:
                            //    listMasterDataModel.PayrollGroup = _organizationContext.PayrollGroup.Where(z => z.IsActive == true && z.IsDeleted == false).Join(_organizationContext.GlobalCode, a => a.Id, gc => gc.Id, ((a, gc) => new MasterDropDown()
                            //    {
                            //        id = a.Id,
                            //        value = a.GroupName,
                            //        key = gc.GlobalCodeName,
                            //    })).OrderBy(a => a.id).ToList();
                            //    break;

                            case MasterDataEnum.PAYROLLGROUP:
                                listMasterDataModel.PayrollGroup = _organizationContext.PayrollGroup.Where(z => z.IsActive == true && z.IsDeleted == false).Join(_organizationContext.GlobalCode, a => a.PayPeriodId, gc => gc.Id, ((a, gc) => new MasterDropDown()
                                {
                                    id = a.Id,
                                    value = a.GroupName,
                                    key = gc.GlobalCodeName,
                                })).OrderBy(a => a.id).ToList();
                                break;

                            case MasterDataEnum.MASTERALLSTAFF:
                                listMasterDataModel.AllStaffs = _organizationContext.Staffs.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderBy(a => a.FirstName).ToList();
                                break;
                            case MasterDataEnum.PAYROLLBREAKTIME:
                                listMasterDataModel.PayrollBreakTime = _organizationContext.PayrollBreakTime.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == token.OrganizationID).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.Name + " (" + x.Duration + " min(s))"
                                }).OrderBy(y => y.value).ToList();
                                break;
                            case MasterDataEnum.CATEGORIES:
                                listMasterDataModel.Categories = _organizationContext.DFA_Category.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderByDescending(f => f.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.CategoryName,
                                    key = x.CategoryName
                                }).ToList();
                                break;
                            case MasterDataEnum.DOCUMENTS:
                                listMasterDataModel.Documents = _organizationContext.DFA_Document.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).OrderByDescending(f => f.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.DocumentName,
                                    key = x.DocumentName
                                }).ToList();
                                break;
                            case MasterDataEnum.DOCUMENTSTATUS:
                                listMasterDataModel.DocumentStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "documentstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERSPECIALITY:
                                listMasterDataModel.MasterSpeciality = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "speciality" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;

                            case MasterDataEnum.MASTERPROVIDERCARECATEGORY:
                                listMasterDataModel.MASTERPROVIDERCARECATEGORY = _organizationContext.ProviderCareCategory.Where(a => a.IsDeleted == false && a.IsActive == true).OrderBy(a => a.CareCategoryName).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.CareCategoryName,
                                    //key = x.GlobalCodeName
                                }).ToList();
                                break;

                            case MasterDataEnum.MASTERPROVIDERSQUESTIONAREQUESTIONS:
                                listMasterDataModel.MASTERPROVIDERSQUESTIONAREQUESTIONS = _organizationContext.ProviderQuestionnaireQuestions.Where(a => a.IsDeleted == false && a.IsActive == true).OrderBy(a => a.QuestionId).Select(x => new MasterDropDown()
                                {
                                    id = x.QuestionId,
                                    value = x.QuestionNameName,
                                    //key = x.GlobalCodeName
                                }).ToList();
                                break;

                            case MasterDataEnum.MASTERTAXONOMY:
                                listMasterDataModel.MasterTaxonomy = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "taxonomy" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeName,
                                    key = x.GlobalCodeValue
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERSTAFFSERVICE:
                                //var lst=new string[] { "359164", "0" };
                                if (globalCodeId != null)
                                {
                                    List<string> selectedSpeciality = new List<string>(Convert.ToString(globalCodeId).Split(','));
                                    listMasterDataModel.MasterStaffServices = _organizationContext.MasterServices.Where(a => a.IsDeleted == false && a.IsActive == true && a.OrganizationId == token.OrganizationID && (globalCodeId == "" || selectedSpeciality.Contains(a.GlobalCodeId.ToString()))).OrderBy(a => a.ServiceName).Select(x => new MasterDropDown()
                                    {
                                        id = x.Id,
                                        value = x.ServiceName,
                                        key = x.Id.ToString()
                                    }).ToList();
                                }
                                break;
                            case MasterDataEnum.GLOBALCODECATEGORY:


                                listMasterDataModel.MasterGlobalCategories = _organizationContext.GlobalCodeCategory.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategoryName.ToLower() == "speciality" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.GlobalCodeCategoryName).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeCategoryName,
                                    key = x.Id.ToString()
                                }).ToList();

                                break;

                            case MasterDataEnum.MASTERTEMPLATETYPE:
                                listMasterDataModel.MasterTemplateTypes = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "templatetype" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeName,
                                    key = x.GlobalCodeValue
                                }).ToList();
                                break;
                            case MasterDataEnum.PRESCRIPTIONTYPE:
                                listMasterDataModel.MasterPrescriptionDrugs = _organizationContext.PrescriptionDrugs.ToList();
                                break;
                            case MasterDataEnum.MASTERCITY:
                                listMasterDataModel.MasterCity = _organizationContext.MasterCity.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CityName).ToList();
                                break;

                            case MasterDataEnum.MASTERPHARMACY:
                                listMasterDataModel.MasterPharmacy = _organizationContext.MasterPharmacy.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.PharmacyName).ToList();
                                break;
                            case MasterDataEnum.MASTERQUESTIONNAIRETYPES:
                                listMasterDataModel.MasterQuestionnaireTypes = _organizationContext.MasterQuestionnaireTypes.Where(a => a.IsActive == true && a.IsDeleted == false && token.OrganizationID == a.OrganizationID).OrderBy(a => a.QuestionnaireType).ToList();
                                break;
                            case MasterDataEnum.MASTERHRACATEGORYRISK:
                                listMasterDataModel.MasterHRACategoryRisk = _organizationContext.MasterHRACategoryRisk.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == token.OrganizationID).OrderBy(a => a.Category).Select(x => new MasterDropDown()
                                {
                                    id = x.ID,
                                    value = x.Category,
                                    key = x.Category
                                }).ToList();
                                break;
                            case MasterDataEnum.GENDERCRITERIAFORHRA:
                                listMasterDataModel.GenderCriteriaForHRA = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "mastergendercriteriaforhra" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERBENCHMARK:
                                listMasterDataModel.MasterBenchmark = _organizationContext.MasterBenchmark.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == token.OrganizationID).ToList();
                                break;
                            case MasterDataEnum.ENCOUNTERTYPES:
                                listMasterDataModel.EncounterTypes = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "encountertypes" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.GlobalCodeValue).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.ENCOUNTERMETHODS:
                                listMasterDataModel.EncounterMethods = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "encountermethods" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERDOCUMENTSTATUS:
                                listMasterDataModel.MasterDocumentStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "documentstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.GlobalCodeValue).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.PATIENTPROGRAMFREQUENCY:
                                listMasterDataModel.PatientProgramFrequency = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "patientprogramfrequency" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.PATIENTPROGRAMSTATUS:
                                listMasterDataModel.PatientProgramStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "patientprogramstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.ALERTSINDICATORFILTER:
                                listMasterDataModel.MasterLoadAlerts = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "alertsindicatorfilter" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                }).ToList();
                                break;

                            case MasterDataEnum.MASTERENROLLMENTTYPEFILTER:
                                listMasterDataModel.MasterEnrollmentTypeFilter = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "masterenrollmenttypefilter" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                }).ToList();
                                break;
                            case MasterDataEnum.MEMBERRELATIONSHIP:
                                listMasterDataModel.MemberRelationship = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "memberrelationship" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.GlobalCodeValue,
                                    key = x.GlobalCodeName
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERPATIENTSTATUS:
                                listMasterDataModel.MasterPatientStatus = _organizationContext.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName == "patientstatus" && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).ToList();//_context.MasterStatus.Where(a => a.IsActive == true && a.IsDeleted == false && a.Type.ToUpper() == p.ToUpper()).OrderBy(a => a.StatusName).ToList();
                                break;
                            case MasterDataEnum.MASTERRISKINDICATOR:
                                listMasterDataModel.MasterRiskIndicator = _organizationContext.MasterRiskIndicatorBenchmark.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == token.OrganizationID).OrderByDescending(a => a.ID).Select(x => new MasterDropDown()
                                {
                                    id = x.ID,
                                    value = x.Risk,
                                    key = x.Risk
                                }).ToList();
                                break;
                            case MasterDataEnum.MASTERDISEASECONDITIONMAPPEDWITHDMP:
                                try
                                {
                                    List<MasterChronicConditionListModel> data = MasterChronicConditionList<MasterChronicConditionListModel>(token).ToList();
                                    if (data != null && data.Count() > 0)
                                    {
                                        listMasterDataModel.MasterDiseaseConditionMappedWithDMP = data.ToList().Select(x => new MasterDropDown()
                                        {
                                            id = x.Id,
                                            value = x.Value,
                                            key = x.Value
                                        }).ToList();
                                    }
                                    else
                                    {
                                        listMasterDataModel.MasterDiseaseConditionMappedWithDMP = new List<MasterDropDown>();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    listMasterDataModel.MasterDiseaseConditionMappedWithDMP = new List<MasterDropDown>();
                                }

                                break;
                            case MasterDataEnum.MASTERCHRONICCONDITION:
                                listMasterDataModel.MasterChronicCondition = _organizationContext.MasterChronicCondition.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == token.OrganizationID).OrderBy(a => a.Condition).Select(x => new MasterDropDown()
                                {
                                    id = x.Id,
                                    value = x.Condition,
                                    key = x.Condition
                                }).ToList();
                                break;
                        }
                    }
                });
            }
            catch (Exception)
            {
                var aaa = flag;
                throw;
            }
        }

        public IQueryable<T> GetAutoComplateSearchingValues<T>(string tableName, string columnName, string searchText, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@TableName", tableName),
                                        new SqlParameter("@ColumnName", columnName),
                                        new SqlParameter("@SearchText", searchText),
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
            };
            return _organizationContext.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_AutoCompleteSearching, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> MasterChronicConditionList<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                        new SqlParameter("@OrganizationID", token.OrganizationID),
            };
            return _organizationContext.ExecStoredProcedureListWithOutput<T>(SQLObjects.AMD_GetMasterChronicConditionList, parameters.Length, parameters).AsQueryable();

        }
    }
}