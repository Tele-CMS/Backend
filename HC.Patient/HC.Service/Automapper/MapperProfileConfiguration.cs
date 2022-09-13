using AutoMapper;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Model.Staff;
using HC.Patient.Data.ViewModel;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Chat;
using HC.Patient.Model.Claim;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Model.Message;
using HC.Patient.Model.NotificationSetting;
using HC.Patient.Model.OnboardingDetails;
using HC.Patient.Model.OnboardingHeader;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Model.PatientMedicalFamilyHistory;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Payroll;
using HC.Patient.Model.PayrollBreaktime;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Model.ReviewRatings;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Staff;
using HC.Patient.Model.ThemeSettings;
using HC.Patient.Model.Users;
using System;

namespace HC.Patient.Service.Automapper
{
    public class MapperProfileConfiguration : Profile
    {
        public MapperProfileConfiguration()
        {
            CreateMap<MasterOrganization, MasterOrganizationModel>();
            CreateMap<MasterOrganizationModel, MasterOrganization>();
            CreateMap<RoundingRuleModel, MasterRoundingRules>();
            CreateMap<MasterRoundingRules, RoundingRuleModel>();
            CreateMap<RoundingRuleDetailsModel, RoundingRuleDetails>();
            CreateMap<RoundingRuleDetails, RoundingRuleDetailsModel>();
            CreateMap<RoundingRuleDetails, RoundingRuleModel>();
            CreateMap<RoundingRuleModel, RoundingRuleDetails>();
            CreateMap<PatientEncounterModel, PatientEncounter>();
            CreateMap<PatientEncounter, PatientEncounterModel>();

            CreateMap<PatientEncounterListingModel, PatientEncounter>();
            CreateMap<PatientEncounter, PatientEncounterListingModel>();

            CreateMap<PatientEncounterDiagnosisCodes, PatientEncounterDiagnosisCodesModel>();
            CreateMap<PatientEncounterDiagnosisCodesModel, PatientEncounterDiagnosisCodes>();
            CreateMap<PatientEncounterServiceCodes, PatientEncounterServiceCodesModel>();
            CreateMap<PatientEncounterServiceCodesModel, PatientEncounterServiceCodes>();
            CreateMap<PatientEncounterCodesMapping, PatientEncounterCodesMappingModel>();
            CreateMap<PatientEncounterCodesMappingModel, PatientEncounterCodesMapping>();
            CreateMap<PatientsPrescriptionModel, PatientPrescription>();
            CreateMap<PatientPrescription, PatientsPrescriptionModel>();
            CreateMap<PatientPrescriptionFaxLog, PatientFaxModel>(); 
            CreateMap<PatientFaxModel, PatientPrescriptionFaxLog>(); 
            CreateMap<PatientsPrescriptionEditModel, PatientPrescription>();
            CreateMap<PatientPrescription, PatientsPrescriptionEditModel>();
            CreateMap<SoapNotes, SOAPNotesModel>();
            CreateMap<SOAPNotesModel, SoapNotes>();
            CreateMap<PatientEncounterViewModel, PatientEncounterModel>();
            CreateMap<PatientEncounterServiceCodesViewModel, PatientEncounterServiceCodesModel>();
            CreateMap<PatientEncounterCodesMappingViewModel, PatientEncounterCodesMappingModel>();
            CreateMap<PatientEncounterICDCodesViewModel, PatientEncounterDiagnosisCodesModel>();
            CreateMap<SOAPNotesViewModel, SOAPNotesModel>();
            CreateMap<PatientEncounterICDCodesViewModel, PatientEncounterDiagnosisCodesModel>();
            CreateMap<SOAPNotesViewModel, SOAPNotesModel>();
            CreateMap<Organization, OrganizationModel>();
            CreateMap<OrganizationModel, Organization>();
            CreateMap<MasterOrganization, OrganizationModel>();
            CreateMap<OrganizationModel, MasterOrganization>();
            CreateMap<OrganizationDatabaseDetail, OrganizationDatabaseDetailModel>();
            CreateMap<OrganizationDatabaseDetailModel, OrganizationDatabaseDetail>();
            CreateMap<OrganizationSubscriptionPlan, OrganizationSubscriptionPlanModel>();
            CreateMap<OrganizationSubscriptionPlanModel, OrganizationSubscriptionPlan>();
            CreateMap<OrganizationSMTPDetails, OrganizationSMTPDetailsModel>();
            CreateMap<OrganizationSMTPDetailsModel, OrganizationSMTPDetails>();
            CreateMap<OrganizationSMTPDetails, OrganizationSMTPCommonModel>();
            CreateMap<OrganizationSMTPCommonModel, OrganizationSMTPDetails>();

            CreateMap<ClaimServiceLine, ClaimServiceLineModel>();
            CreateMap<ClaimServiceLineModel, ClaimServiceLine>();
            CreateMap<Claims, ClaimModel>();
            CreateMap<ClaimModel, Claims>();

            CreateMap<PatientMedicalFamilyHistory, Model.PatientMedicalFamilyHistory.PatientMedicalFamilyHistoryModel>();
            CreateMap<Model.PatientMedicalFamilyHistory.PatientMedicalFamilyHistoryModel, PatientMedicalFamilyHistory>();
            CreateMap<PatientMedicalFamilyHistoryDiseases, PatientMedicalFamilyHistoryDiseasesModel>();
            CreateMap<PatientMedicalFamilyHistoryDiseasesModel, PatientMedicalFamilyHistoryDiseases>();
            CreateMap<PatientAppointment, PatientAppointmentModel>();
            CreateMap<PatientAppointmentModel, PatientAppointment>();
            CreateMap<AppConfigurations, MasterAppConfiguration>();
            CreateMap<MasterAppConfiguration, AppConfigurations>();
            CreateMap<SecurityQuestions, MasterSecurityQuestions>();
            CreateMap<MasterSecurityQuestions, SecurityQuestions>();

            CreateMap<AppConfigurations, AppConfigurationsModel>();
            CreateMap<AppConfigurationsModel, AppConfigurations>();

            CreateMap<MessageDetailModel, ForwardMessageDetailModel>();
            CreateMap<ForwardMessageDetailModel, MessageDetailModel>();

            CreateMap<MasterModifierModel, MasterModifiers>();
            CreateMap<MasterModifiers, MasterModifierModel>();

            CreateMap<ModifierModel, MasterServiceCodeModifiers>();
            CreateMap<MasterServiceCodeModifiers, ModifierModel>();

            CreateMap<MasterServiceCodesModel, MasterServiceCode>();
            CreateMap<MasterServiceCode, MasterServiceCodesModel>();


            CreateMap<PayerModifierModel, PayerServiceCodeModifiers>();
            CreateMap<PayerServiceCodeModifiers, PayerModifierModel>();

            CreateMap<PayerServiceCodesModel, PayerServiceCodes>();
            CreateMap<PayerServiceCodes, PayerServiceCodesModel>();

            CreateMap<StaffLeaveModel, StaffLeave>();
            CreateMap<StaffLeave, StaffLeaveModel>();
            CreateMap<PhoneNumbers, Model.Patient.PhoneModel>();
            CreateMap<Model.Patient.PhoneModel, PhoneNumbers>();
            CreateMap<PatientAddress, Model.Patient.AddressModel>();
            CreateMap<Model.Patient.AddressModel, PatientAddress>();

            CreateMap<PatientInsuranceDetails, PatientInsuranceModel>();
            CreateMap<PatientInsuranceModel, PatientInsuranceDetails>();
            CreateMap<InsuredPerson, InsuredPersonModel>();
            CreateMap<InsuredPersonModel, InsuredPerson>();

            CreateMap<CustomLabelModel, PatientCustomLabels>();
            CreateMap<PatientCustomLabels, CustomLabelModel>();
            CreateMap<StaffDetailedTimesheetModel, UserTimesheetByAppointmentType>();
            CreateMap<UserTimesheetByAppointmentType, StaffDetailedTimesheetModel>();

            CreateMap<PayrollGroup, PayrollGroupModel>();
            CreateMap<PayrollGroupModel, PayrollGroup>();
            CreateMap<PatientSocialHistory, PatientSocialHistoryModel>();
            CreateMap<PatientSocialHistoryModel, PatientSocialHistory>();

            CreateMap<PatientImmunization, PatientImmunizationModel>();
            CreateMap<PatientImmunizationModel, PatientImmunization>();

            CreateMap<PatientDiagnosis, PatientDiagnosisModel>();
            CreateMap<PatientDiagnosisModel, PatientDiagnosis>();

            CreateMap<AppointmentType, AppointmentTypesModel>();
            CreateMap<AppointmentTypesModel, AppointmentType>();

            CreateMap<MasterCustomLabels, MasterCustomLabelModel>();
            CreateMap<MasterCustomLabelModel, MasterCustomLabels>();

            CreateMap<MasterSecurityQuestionModel, SecurityQuestions>();
            CreateMap<SecurityQuestions, MasterSecurityQuestionModel>();

            CreateMap<MasterTagModel, MasterTags>();
            CreateMap<MasterTags, MasterTagModel>();

            CreateMap<PayrollBreakTime, PayrollBreaktimeModel>();
            CreateMap<PayrollBreaktimeModel, PayrollBreakTime>();

            CreateMap<PayrollBreakTimeDetails, PayrollBreaktimeDetailsModel>();
            CreateMap<PayrollBreaktimeDetailsModel, PayrollBreakTimeDetails>();

            // Payer 
            CreateMap<InsuranceCompanies, InsuranceCompanyModel>();
            CreateMap<InsuranceCompanyModel, InsuranceCompanies>();

            CreateMap<HealthcareKeywords, KeywordModel>();
            CreateMap<KeywordModel, HealthcareKeywords>();

            CreateMap<HealthcareKeywords, HealthCareCategoryKeywordsModel>();
            CreateMap<HealthCareCategoryKeywordsModel, HealthcareKeywords>();

            CreateMap<ProviderCareCategory, ProviderCareCategoryModel>();
            CreateMap<ProviderCareCategoryModel, ProviderCareCategory>();

            CreateMap<SymptomatePatientReport, SymptomatePatientReportData>();
            CreateMap<SymptomatePatientReportData, SymptomatePatientReport>();

            CreateMap<PatientEncounterNotes, PatientEncounterNotesModel>();
            CreateMap<PatientEncounterNotesModel, PatientEncounterNotes>();

            CreateMap<ProviderQuestionnaireQuestions, ProviderQuestionnaireModel>();
            CreateMap<ProviderQuestionnaireModel, ProviderQuestionnaireQuestions>();

            CreateMap<QuestionnaireOptions, QuestionOptionsModel>();
            CreateMap<QuestionOptionsModel, QuestionnaireOptions>();

            CreateMap<QuestionnaireOptions, QuestionnaireQuestionOptionsModel>();
            CreateMap<QuestionnaireQuestionOptionsModel, QuestionnaireOptions>();

            CreateMap<ProviderQuestionnaires, ManageQuestionnaireModel>();
            CreateMap<ManageQuestionnaireModel, ProviderQuestionnaires>();

            CreateMap<QuestionnaireBenchmarkRange, BenchmarkRangeModel>();
            CreateMap<BenchmarkRangeModel, QuestionnaireBenchmarkRange>();


            // Agency Holidays 
            CreateMap<Holidays, HolidaysModel>();
            CreateMap<HolidaysModel, Holidays>();

            // Allergies
            CreateMap<Entity.PatientAllergies, PatientAllergyModel>();
            CreateMap<PatientAllergyModel, Entity.PatientAllergies>();

            // Medication
            CreateMap<PatientMedication, PatientsMedicationModel>();
            CreateMap<PatientsMedicationModel, PatientMedication>();

            // vitals
            CreateMap<PatientVitals, PatientVitalModel>();
            CreateMap<PatientVitalModel, PatientVitals>();

            //Authorization
            CreateMap<Authorization, AuthModel>();
            CreateMap<AuthModel, Authorization>();
            CreateMap<AuthorizationProcedures, AuthProceduresModel>();
            CreateMap<AuthProceduresModel, AuthorizationProcedures>();
            CreateMap<AuthProcedureCPT, AuthProcedureCPTModel>();
            CreateMap<AuthProcedureCPTModel, AuthProcedureCPT>();

            //EDI
            CreateMap<EDIGateway, EDIModel>();
            CreateMap<EDIModel, EDIGateway>();

            //Staff Custom Label
            CreateMap<StaffCustomLabels, StaffCustomLabelModel>();
            CreateMap<StaffCustomLabelModel, StaffCustomLabels>();


            //MasterICD
            CreateMap<MasterICD, MasterICDModel>();
            CreateMap<MasterICDModel, MasterICD>();

            //Master Insurance type
            CreateMap<MasterInsuranceType, MasterInsuranceTypeModel>();
            CreateMap<MasterInsuranceTypeModel, MasterInsuranceType>();

            //Master Insurance type
            CreateMap<Location, LocationModel>();
            CreateMap<LocationModel, Location>();

            //user roles
            CreateMap<UserRoles, UserRoleModel>();
            CreateMap<UserRoleModel, UserRoles>();
            CreateMap<Patients, PatientDemographicsModel>();
            CreateMap<PatientDemographicsModel, Patients>();

            CreateMap<PayerAppointmentTypes, PayerActivityCodeModel>();
            CreateMap<PayerActivityCodeModel, PayerAppointmentTypes>();

            CreateMap<AuthProcedureCPTModifiers, AuthProcedureCPTModifierModel>();
            CreateMap<AuthProcedureCPTModifierModel, AuthProcedureCPTModifiers>();

            CreateMap<Chat, ChatModel>();
            CreateMap<ChatModel, Chat>();

            //Questionnaire
            CreateMap<CategoryModel, DFA_Category>();
            CreateMap<DFA_Category, CategoryModel>();

            CreateMap<CategoryCodeModel, DFA_CategoryCode>();
            CreateMap<DFA_CategoryCode, CategoryCodeModel>();

            CreateMap<DFA_Document, QuestionnaireDocumentModel>();
            CreateMap<QuestionnaireDocumentModel, DFA_Document>();

            CreateMap<DFA_Section, QuestionnaireSectionModel>();
            CreateMap<QuestionnaireSectionModel, DFA_Section>();

            CreateMap<QuestionnaireSectionItemModel, DFA_SectionItem>();
            CreateMap<DFA_SectionItem, QuestionnaireSectionItemModel>();

            CreateMap<AssignDocumentToPatientModel, DFA_PatientDocuments>();
            CreateMap<DFA_PatientDocuments, AssignDocumentToPatientModel>();

            // Master Template
            CreateMap<MasterTemplatesModel, MasterTemplates>();
            CreateMap<MasterTemplates, MasterTemplatesModel>();

            // Master Template Category
            CreateMap<MasterTemplateCategoryModel, MasterTemplateCategory>();
            CreateMap<MasterTemplateCategory, MasterTemplateCategoryModel>();

            // Master Template Sub Category
            CreateMap<MasterTemplateSubCategory, MasterTemplateSubCategoryModel>();
            CreateMap<MasterTemplateSubCategoryModel, MasterTemplateSubCategory>();

            // Patient Encounter Template
            CreateMap<PatientEncounterTemplateModel, PatientEncounterTemplates>();
            CreateMap<PatientEncounterTemplates, PatientEncounterTemplateModel>();

            //Patient disease
            CreateMap<PatientDiseaseManagementProgram, ProgramModel>();
            CreateMap<ProgramModel, PatientDiseaseManagementProgram>();

            //ML 2.2
            CreateMap<PatientCurrentMedication, PatientsCurrentMedicationModel>();
            CreateMap<PatientsCurrentMedicationModel, PatientCurrentMedication>();
            CreateMap<Chat, CareChatModel>();
            CreateMap<CareChatModel, Chat>();
            CreateMap<CareChat, CareChatModel>();
            CreateMap<CareChatModel, CareChat>();

            CreateMap<ThemeSetting, ThemeSettingsModel>();
            CreateMap<ThemeSettingsModel, ThemeSetting>();

            //Onboarding Module
            CreateMap<OnboardingDetail, OnboardingDetailDto>();
            CreateMap<OnboardingDetailDto, OnboardingDetail>();

            CreateMap<OnboardingDetail, CreateOrEditOnboardingDetailDto>();
            CreateMap<CreateOrEditOnboardingDetailDto, OnboardingDetail>();

            CreateMap<OnboardingHeader, OnboardingHeaderDto>();
            CreateMap<OnboardingHeaderDto, OnboardingHeader>();

            CreateMap<OnboardingHeader, CreateOrEditOnboardingHeaderDto>();
            CreateMap<CreateOrEditOnboardingHeaderDto, OnboardingHeader>();
            




            //User Invitation
            CreateMap<UserInvitationModel, UserInvitation>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(o => o.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(o => o.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(o => o.Phone))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(o => o.LocationId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(o => o.RoleId))
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(o => o.OrganizationId));

            CreateMap<UserInvitation, UserInvitationModel>();

            CreateMap<UserInvitation, UserInvitationResponseModel>()
                .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(o => o.Id))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(o => o.LocationId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(o => o.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(o => o.LastName))
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(o => o.OrganizationId))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(o => o.Phone))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(o => o.RoleId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email));

            //Register New User
            CreateMap<RegisterUserModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(o => o.Username))
                .ForMember(dest => dest.OrganizationID, opt => opt.MapFrom(o => o.OrganizationId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Password)))
                .ForMember(dest => dest.PasswordResetDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(o => o.RoleId));

            CreateMap<RegisterUserModel, Staffs>()
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(o => o.DOB))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(o => o.Username))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Password)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(o => o.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(o => o.LastName))
                .ForMember(dest => dest.OrganizationID, opt => opt.MapFrom(o => o.OrganizationId))
                .ForMember(dest => dest.DOJ, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(o => o.RoleId))
                .ForMember(dest => dest.IsRenderingProvider, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(o => o.Phone))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(o => o.Gender))
                .ForMember(dest => dest.PayRate, opt => opt.MapFrom(o => 0));

            CreateMap<RegisterUserModel, StaffLocation>()
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(o => o.StaffId))
                .ForMember(dest => dest.LocationID, opt => opt.MapFrom(o => o.LocationId))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.OrganizationID, opt => opt.MapFrom(o => o.OrganizationId));

            CreateMap<UserInvitation, UserInvitationRegistrationModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(o => o.Phone))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(o => o.MiddleName))
                .ForMember(dest => dest.InvitationStatus, opt => opt.MapFrom(o => o.InvitationStatus))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(o => o.LastName));

            //Email Log
            CreateMap<EmailModel, EmailLog>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(o => o.EmailBody))
                .ForMember(dest => dest.EmailStatus, opt => opt.MapFrom(o => o.EmailStatus))
                .ForMember(dest => dest.EmailType, opt => opt.MapFrom(o => o.EmailType))
                .ForMember(dest => dest.EmailSubType, opt => opt.MapFrom(o => o.EmailSubType))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(o => o.EmailSubject))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.PrimaryId, opt => opt.MapFrom(o => o.PrimaryId))
                .ForMember(dest => dest.ToEmail, opt => opt.MapFrom(o => o.ToEmail))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy));


            CreateMap<RejectInvitationModel, InvitationRejectLog>()
                .ForMember(dest => dest.RejectRemarks, opt => opt.MapFrom(o => o.Remarks))
                .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(o => Common.CommonMethods.Decrypt(o.InvitationId.Replace(" ", "+"))))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow));

            #region Notification

            CreateMap<NotificationModel, Notifications>()
                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(o => o.ActionTypeId))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(o => o.ChatId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.IsNotificationSend, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.OrganizationID, opt => opt.MapFrom(o => o.OrganizationID))
                .ForMember(dest => dest.PatientAppointmentId, opt => opt.MapFrom(o => o.PatientAppointmentId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(o => o.PatientId))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(o => o.StaffId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true));

            CreateMap<Notifications, NotificationTypeMapping>()
                .ForMember(dest => dest.IsSendNotification, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.IsReadNotification, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.IsReceivedNotification, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(o => o.Id))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true));

            #endregion Notification

            #region Register Client
            CreateMap<RegisterUserModel, PatientDemographicsModel>()
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(o => o.DOB))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(o => o.Gender))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsBlock, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.IsPortalActivate, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsPortalRequired, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(o => o.LastName))
                .ForMember(dest => dest.LocationID, opt => opt.MapFrom(o => o.LocationId))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(o => o.MiddleName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(o => o.Phone))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(o => true))
                //.ForMember(dest => dest.RenderingProviderID, opt => opt.MapFrom(o=>null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(o => o.Username));
            #endregion Register Client

            #region Staff Profile
            CreateMap<StaffExperience, StaffExperienceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Id.ToString())))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(o => o.OrganizationName))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(o => o.StartDate.ToString()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(o => o.EndDate.ToString()))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.StaffId.ToString())))
                .ForMember(dest => dest.TotalExperience, opt => opt.MapFrom(o => Common.CommonMethods.getYearMonthDayBetweenDates(o.StartDate, (DateTime)o.EndDate)));

            CreateMap<StaffExperienceModel, StaffExperience>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => !string.IsNullOrEmpty(o.Id) && o.Id != "0" ? Convert.ToInt16(Common.CommonMethods.Decrypt(o.Id.ToString())) : 0))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(o => o.OrganizationName))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(o => Convert.ToDateTime(o.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(o => Convert.ToDateTime(o.EndDate)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted));


            CreateMap<StaffQualification, StaffQualificationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Id.ToString())))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(o => o.Course))
                .ForMember(dest => dest.University, opt => opt.MapFrom(o => o.University.ToString()))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(o => o.StartDate.ToString()))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.StaffId.ToString())))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(o => o.EndDate.ToString()));

            CreateMap<StaffQualificationModel, StaffQualification>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => !string.IsNullOrEmpty(o.Id) && o.Id != "0" ? Convert.ToInt16(Common.CommonMethods.Decrypt(o.Id.ToString())) : 0))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(o => o.Course))
                .ForMember(dest => dest.University, opt => opt.MapFrom(o => o.University))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(o => Convert.ToDateTime(o.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(o => Convert.ToDateTime(o.EndDate)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted));


            CreateMap<StaffAward, StaffAwardModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Id.ToString())))
               .ForMember(dest => dest.AwardType, opt => opt.MapFrom(o => o.AwardType))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(o => o.Description.ToString()))
               .ForMember(dest => dest.AwardDate, opt => opt.MapFrom(o => o.AwardDate.ToString()))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted))
               .ForMember(dest => dest.StaffId, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.StaffId.ToString())));

            CreateMap<StaffAwardModel, StaffAward>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => !string.IsNullOrEmpty(o.Id) && o.Id != "0" ? Convert.ToInt16(Common.CommonMethods.Decrypt(o.Id.ToString())) : 0))
                .ForMember(dest => dest.AwardType, opt => opt.MapFrom(o => o.AwardType))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(o => o.Description))
                .ForMember(dest => dest.AwardDate, opt => opt.MapFrom(o => Convert.ToDateTime(o.AwardDate)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted));

            #endregion Staff Profile

            #region Master Service
            CreateMap<MasterServices, MasterServicesModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Common.CommonMethods.Encrypt(o.Id.ToString())))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(o => o.ServiceName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted));

            CreateMap<MasterServicesModel, MasterServices>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => !string.IsNullOrEmpty(o.Id) && o.Id != "0" ? Convert.ToInt16(Common.CommonMethods.Decrypt(o.Id.ToString())) : 0))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(o => o.ServiceName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => o.IsDeleted));
            #endregion  Master Service

            #region Appointment Payment
            CreateMap<AppointmentPaymentModel, AppointmentPayments>()
               .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(o => o.AppointmentId))
               .ForMember(dest => dest.PaymentToken, opt => opt.MapFrom(o => o.PaymentToken))
               .ForMember(dest => dest.PaymentMode, opt => opt.MapFrom(o => o.PaymentMode))
               .ForMember(dest => dest.BookingAmount, opt => opt.MapFrom(o => o.Amount))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true));
            #endregion Appointment Payment

            #region Open Tok Settings
            CreateMap<OpenTokSettings, OpenTokSettingModel>()
                .ForMember(dest => dest.APIKey, opt => opt.MapFrom(o => o.APIKey))
                .ForMember(dest => dest.APISecret, opt => opt.MapFrom(o => o.APISecret))
                .ForMember(dest => dest.APIUrl, opt => opt.MapFrom(o => o.APIUrl));
            #endregion Open Tok Settings

            #region Group Sessio Invitation
            CreateMap<GroupSessionInvitationModel, GroupSessionInvitations>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.Name))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(o => o.Email))
               .ForMember(dest => dest.SessionId, opt => opt.MapFrom(o => o.SessionId))
               .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(o => o.AppointmentId))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow));
            #endregion Group Sessio Invitation
            //reviewRating
            CreateMap<ReviewRatings, ReviewRatingsModel>();
            CreateMap<ReviewRatingsModel, ReviewRatings>();

            #region ChatRoom
            CreateMap<ChatRoomModel, ChatRoom>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(o => o.RoomName))
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(o => o.OranizationId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow));

            CreateMap<ChatRoomUserModel, ChatRoomUser>()
               .ForMember(dest => dest.RoomId, opt => opt.MapFrom(o => o.RoomId))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(o => o.UserId))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(o => true))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(o => false))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => System.DateTime.UtcNow));
            #endregion

            //encounter
            CreateMap<PatientEncounterChecklist, PatientEncounterChecklistModel>();
            CreateMap<PatientEncounterChecklistModel, PatientEncounterChecklist>();
        }
    }
}
