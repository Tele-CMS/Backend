namespace HC.Common.Enums
{
    public class CommonEnum
    {
        /// <summary>
        /// Enumeration for master data for getting data from database on the basis on master table name
        /// </summary>
        public enum MasterDataEnum
        {
            MASTERCOUNTRY, MASTERDOCUMENTTYPE, MASTERETHNICITY, MASTERINSURANCEPCP, MASTERINSURANCETYPE, /*MASTEROCCUPATION,*/ /*MASTERPREFERREDLANGUAGE,*/
            MASTERRACE, MASTERSTATE, MASTERSTATUS, SUFFIX, MASTERPROVIDER, MARITALSTATUS, MASTERRELATIONSHIP, MASTERPROGRAM, MASTERSERVICE, PATIENTSTATUS, MASTERINSURANCECOMPANY,
            /*MASTERPATIENTCOMMPREFERENCES*/
            MASTERREFERRAL, MASTERGENDER, PHONETYPE, INSURANCEPLANTYPE, MASTERSERVICECODE, MASTERICD,
            MASTERVFC, MASTERIMMUNIZATION, MASTERMANUFACTURE, MASTERADMINISTRATIONSITE, MASTERROUTEOFADMINISTRATION, MASTERIMMUNITYSTATUS, MASTERREJECTIONREASON,
            SOCIALHISTORY, TRAVELHISTORY, ADDRESSTYPE, MASTERSTAFF, APPOINTMENTTYPE, LABTESTTYPE, MASTERLONIC, MASTERLABS, TIMETYPE, FREQUENCYTYPE, FREQUENCYDURATIONTYPE,
            MASTERALLERGIES, MASTERREACTION, AUTHORIZEDPROCEDURE, MASTERCUSTOMLABELS, MASTERCUSTOMLABELTYPE, MASTERPATIENTLOCATION, MASTERTAGS,
            MASTERCUSTOMLABELSTAFF, MASTERROLES, MASTERWEEKDAYS, MASTERUNITTYPE, MASTERROUNDINGRULES
            , MASTERLOCATION, MASTERTEMPLATES, ORGANIZATIONDATABASEDETAIL, MASTERORGANIZATION, MASTERRENDERINGPROVIDER,
            USERROLETYPE, MASTERPHONEPREFERENCES, MASTERCANCELTYPE, MASTERPAYMENTTYPE, MASTERPAYMENTDESCRIPTION,
            STAFFAVAILABILITY, ENCOUNTERSTATUS, EMPLOYEETYPE, REFERRALSOURCE, EMPLOYEEMENT, MASTERDOCUMENTTYPES, MASTERDOCUMENTTYPESSTAFF, MASTERDEGREE, CLAIMRESUBMISSIONREASON, MASTERADJUSTMENTGROUPCODE
            , LEAVETYPE, LEAVEREASON, LEAVESTATUS, CLAIMPAYMENTSTATUS, CLAIMPAYMENTSTATUSFORLEDGER, MASTERTAGSFORSTAFF, MASTERTAGSFORPATIENT, TIMESHEETSTATUS, PAYPERIOD, WORKWEEK, PAYROLLGROUP, MASTERALLSTAFF, PAYROLLBREAKTIME, CATEGORIES,
            DOCUMENTS, DOCUMENTSTATUS, MASTERSPECIALITY, MASTERTAXONOMY, MASTERROLESALL, MASTERSTAFFSERVICE, MASTERTEMPLATETYPE, APPOINTMENTSTATUS, APPOINTMENTIMEGRAPHFILTER,
            PRESCRIPTIONTYPE, MASTERCITY, MASTERPHARMACY, MASTERPROVIDERCARECATEGORY, MASTERPROVIDERSQUESTIONAREQUESTIONS,GLOBALCODECATEGORY, MASTERQUESTIONNAIRETYPES, MASTERHRACATEGORYRISK, GENDERCRITERIAFORHRA, MASTERBENCHMARK, ENCOUNTERTYPES, ENCOUNTERMETHODS, MASTERDOCUMENTSTATUS
            ,PATIENTPROGRAMFREQUENCY, PATIENTPROGRAMSTATUS, ALERTSINDICATORFILTER, MASTERENROLLMENTTYPEFILTER, MEMBERRELATIONSHIP, MASTERPATIENTSTATUS, MASTERRISKINDICATOR, MASTERDISEASECONDITIONMAPPEDWITHDMP, MASTERCHRONICCONDITION
        }
        /// <summary>
        /// Enumeration for all data types
        /// </summary>
        public enum DataType
        {
            System_Boolean = 0,
            System_Int32 = 1,
            System_Int64 = 2,
            System_Double = 3,
            System_DateTime = 4,
            System_String = 5

        }
        public enum AccountConfiguration
        {
            LockDays = 60,
            LockNotification = 60
        }
        public enum HttpStatusCodes
        {
            // http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1.1

            Continue = 100,        // Section 10.1.1: Continue
            SwitchingProtocols = 101,        // Section 10.1.2: Switching Protocols

            OK = 200,                // Section 10.2.1: OK
            Created = 201,        // Section 10.2.2: Created
            Accepted = 202,        // Section 10.2.3: Accepted
            NonAuthoritativeInformation = 203,    // Section 10.2.4: Non-Authoritative Information
            NoContent = 204,        // Section 10.2.5: No Content
            ResetContent = 205,    // Section 10.2.6: Reset Content
            PartialContent = 206,    // Section 10.2.7: Partial Content

            MultipleChoices = 300,        // Section 10.3.1: Multiple Choices
            MovedPermanently = 301,        // Section 10.3.2: Moved Permanently
            Found = 302,            // Section 10.3.3: Found
            SeeOther = 303,        // Section 10.3.4: See Other
            NotModified = 304,    // Section 10.3.5: Not Modified
            UseProxy = 305,        // Section 10.3.6: Use Proxy
            TemporaryRedirect = 307,        // Section 10.3.8: Temporary Redirect

            BadRequest = 400,        // Section 10.4.1: Bad Request
            Unauthorized = 401,    // Section 10.4.2: Unauthorized
            PaymentRequired = 402,        // Section 10.4.3: Payment Required
            Forbidden = 403,        // Section 10.4.4: Forbidden
            NotFound = 404,        // Section 10.4.5: Not Found
            MethodNotAllowed = 405,        // Section 10.4.6: Method Not Allowed
            NotAcceptable = 406,    // Section 10.4.7: Not Acceptable
            ProxyAuthenticationRequired = 407,    // Section 10.4.8: Proxy Authentication Required
            RequestTimeout = 408,    // Section 10.4.9: Request Time-out
            Conflict = 409,        // Section 10.4.10: Conflict
            Gone = 410,            // Section 10.4.11: Gone
            LengthRequired = 411,    // Section 10.4.12: Length Required
            PreconditionFailed = 412,    // Section 10.4.13: Precondition Failed
            RequestEntityTooLarge = 413,    // Section 10.4.14: Request Entity Too Large
            RequestUriTooLarge = 414,    // Section 10.4.15: Request-URI Too Large
            UnsupportedMediaType = 415,    // Section 10.4.16: Unsupported Media Type
            RequestedRangeNotSatisfiable = 416,    // Section 10.4.17: Requested range not satisfiable
            ExpectationFailed = 417,        // Section 10.4.18: Expectation Failed
            UnprocessedEntity = 422,

            InternalServerError = 500,    // Section 10.5.1: Internal Server Error
            NotImplemented = 501,    // Section 10.5.2: Not Implemented
            BadGateway = 502,        // Section 10.5.3: Bad Gateway
            ServiceUnavailable = 503,    // Section 10.5.4: Service Unavailable
            GatewayTimeout = 504,        // Section 10.5.5: Gateway Time-out
            HttpVersionNotSupported = 505,        // Section 10.5.6: HTTP Ve
        }

        public enum InsurancePlanType
        {
            Primary, Secondary, Tertiary
        }
        public enum ClaimSubmissionType
        {
            PaperClaim,
            EDI,
            NonEDI
        }
        public enum EDISubmissionType
        {
            Original, Edited
        }
        public enum MasterStatusClaim
        {
            ReadyToBill = 1,
            Submitted = 2,
            EDIResponse = 3,
            Settled = 4
        }
        public static class SQLObjects
        {
            //StaffCheck 
            public readonly static string STF_CheckStaffProfile = "STF_CheckIfUserProfile";

            //Staff
            public readonly static string APPOINTMENT_GetStaffAvailability = "APPOINTMENT_GetStaffAvailability";
            public readonly static string STF_GetStaffCustomLabels = "STF_GetStaffCustomLabels";
            public readonly static string STF_GetAssignedLocationsById = "STF_GetAssignedLocationsById";
            public readonly static string STF_GetHeaderData = "STF_GetHeaderData";
            public readonly static string STF_GetStaffUsersByName = "STF_GetStaffUsersByName";

            //Staff Leave
            public readonly static string STF_GetStaffLeaveList = "STF_GetStaffLeaveList";
            public readonly static string STF_GetStaffUsers = "STF_GetStaffUsers";
            public readonly static string STF_GetAllDatesForLeaveDateRange = "STF_GetAllDatesForLeaveDateRange";
            public readonly static string STF_GetStaffByTags = "STF_GetStaffByTags";
            public readonly static string STF_GetProfileData = "STF_GetProfileData";
            public readonly static string STF_GetStaffTimesheetDataSheetView = "STF_GetStaffTimesheetDataSheetView";
            public readonly static string STF_GetStaffTimesheetDataTabularView = "STF_GetStaffTimesheetDataTabularView";
            public readonly static string STF_UpdateTimesheetStatus = "STF_UpdateTimesheetStatus";

            //Appointment
            public readonly static string APT_CheckIsValidAppointment = "APT_CheckIsValidAppointment";
            public readonly static string APT_GetAddressListOnScheduler = "APT_GetAddressListOnScheduler";
            public readonly static string PAT_GetAuthDataForPatientAppointment = "PAT_GetAuthDataForPatientAppointment";
            public readonly static string APT_DeleteAppointment = "APT_DeleteAppointment";
            public readonly static string APT_DeleteAppointment_v1 = "APT_DeleteAppointment_v1";
            public readonly static string APT_PastAndUpcomingAppointmentsList = "APT_PastAndUpcomingAppointmentsList";
            public readonly static string APT_GetApprovedAppointments = "APT_GetApprovedAppointments";
            public readonly static string APT_UpdateApptStatus = "APT_UpdateApptStatus";

            //Encounter
            public readonly static string RPT_DownloadEncounter = "RPT_DownloadEncounter";

            //Claims
            public readonly static string CLM_CreateClaim = "CLM_CreateClaim";
            public readonly static string CLM_GetAllClaimsWithServiceLines = "CLM_GetAllClaimsWithServiceLines";
            public readonly static string CLM_GetAllClaimsWithServiceLinesForPayer = "CLM_GetAllClaimsWithServiceLinesForPayer";
            public readonly static string CLM_ApplyPayments = "CLM_ApplyPayments";
            public readonly static string CLM_ApplyEOBPayments = "CLM_ApplyEOBPayments";
            public readonly static string CLM_GetClaimDetailsById = "CLM_GetClaimDetailsById";
            public readonly static string CLM_GetClaims = "CLM_GetClaims";
            public readonly static string CLM_GetClaimServiceLines = "CLM_GetClaimServiceLines";
            public readonly static string CLM_GetOpenChargesForPatient = "CLM_GetOpenChargesForPatient";
            public readonly static string CLM_GetPaperClaimInfo = "CLM_GetPaperClaimInfo";
            public readonly static string CLM_GetBatchPaperClaimInfo = "CLM_GetBatchPaperClaimInfo";
            public readonly static string CLM_GetBatchPaperClaimInfo_Clubbed = "CLM_GetBatchPaperClaimInfo_Clubbed";
            public readonly static string CLM_GetEDIInfo = "CLM_GetEDIInfo";
            public readonly static string CLM_GetBatchEDIInfo = "CLM_GetBatchEDIInfo";
            public readonly static string CLM_GetEDIInfoResubmit = "CLM_GetEDIInfoResubmit";
            public readonly static string CLM_GetBatchEDIInfoResubmit = "CLM_GetBatchEDIInfoResubmit";
            public readonly static string CLM_GenerateClaim837BatchIdForBatchSubmit = "CLM_GenerateClaim837BatchIdForBatchSubmit";
            public readonly static string CLM_GenerateClaim837BatchIdForSingleSubmit = "CLM_GenerateClaim837BatchIdForSingleSubmit";

            public readonly static string CLM_GetEDIInfo_SecondaryPayer = "CLM_GetEDIInfo_SecondaryPayer";
            public readonly static string CLM_GetBatchEDIInfo_SecondaryPayer = "CLM_GetBatchEDIInfo_SecondaryPayer";
            public readonly static string CLM_GetEDIInfo_TertiaryPayer = "CLM_GetEDIInfo_TertiaryPayer";
            public readonly static string CLM_GetBatchEDIInfo_TertiaryPayer = "CLM_GetBatchEDIInfo_TertiaryPayer";
            public readonly static string CLM_UpdateBatchRequestStatus = "CLM_UpdateBatchRequestStatus";
            public readonly static string CLM_SaveEDI835ResponseDetails = "CLM_SaveEDI835ResponseDetails";
            public readonly static string CLM_Apply835PaymentsToPatientAccount = "CLM_Apply835PaymentsToPatientAccount";
            public readonly static string CLM_GetProcessedClaims = "CLM_GetProcessedClaims";
            public readonly static string CLM_GetClaimsForPatientLedger = "CLM_GetClaimsForPatientLedger";
            public readonly static string CLM_GetClaimServiceLinesForPatientLedger = "CLM_GetClaimServiceLinesForPatientLedger";
            public readonly static string CLM_GetClaimHistory = "CLM_GetClaimHistory";
            public readonly static string CLM_InsertClaimHistory = "CLM_InsertClaimHistory";
            public readonly static string CLM_GetClaimsForLedger = "CLM_GetClaimsForLedger";
            public readonly static string CLM_GetClaimServiceLinesForLedger = "CLM_GetClaimServiceLinesForLedger";
            public readonly static string CLM_GetPaperClaimInfo_Secondary = "CLM_GetPaperClaimInfo_Secondary";
            public readonly static string CLM_DeleteClaim = "CLM_DeleteClaim";
            public readonly static string CLM_GetClaimBalance = "CLM_GetClaimBalance";

            //Payments
            public readonly static string CLM_GetServiceLinePaymentDetailsForPatientLedger = "CLM_GetServiceLinePaymentDetailsForPatientLedger";
            public readonly static string CLM_GetServiceLinePaymentDetailsForLedger = "CLM_GetServiceLinePaymentDetailsForLedger";
            public readonly static string CLM_GetSubmittedClaimsHistory = "CLM_GetSubmittedClaimsHistory";
            public readonly static string CLM_GetSubmittedClaimsHistoryDetails = "CLM_GetSubmittedClaimsHistoryDetails";

            //Role Permissions
            public readonly static string USR_GetUserRolePermissions = "USR_GetUserRolePermissions";
            public readonly static string USR_SaveUserRolePermissions = "USR_SaveUserRolePermissions";
            public readonly static string USR_GetApnToken = "USR_GetApnToken";

            // Patient 
            public readonly static string PAT_GetPatientInfoForStripe = "PAT_GetPatientInfoForStripe";
            public readonly static string GetPatientDiagnosisCodes = "GetPatientDiagnosisCodes";
            public readonly static string GetPatientByTags = "GetPatientByTags";
            public readonly static string PAT_GetPatientDetails = "PAT_GetPatientDetails";
            public readonly static string PAT_GetPatientDashbaordDetails = "PAT_GetPatientDashboardDetails";
            public readonly static string PAT_GetPatientHeaderInfo = "PAT_GetPatientHeaderInfo";
            public readonly static string PAT_GetActivitiesForPatientPayer = "PAT_GetActivitiesForPatientPayer";
            public readonly static string PAT_GetPatientActiveAuthorizationData = "PAT_GetPatientActiveAuthorizationData";
            public readonly static string PAT_GetPatientAddressList = "PAT_GetPatientAddressList";
            public readonly static string PAT_GetPatientPayerServiceCodes = "PAT_GetPatientPayerServiceCodes";
            public readonly static string PAT_GetPatientPayerServiceCodesAndModifiers = "PAT_GetPatientPayerServiceCodesAndModifiers";
            public readonly static string PAT_GetAuthorizedServiceCodesForPatientAppointmentType = "PAT_GetAuthorizedServiceCodesForPatientAppointmentType";
            public readonly static string PAT_CheckServiceCodesAuthorizationForPatient = "PAT_CheckServiceCodesAuthorizationForPatient";
            public readonly static string PAT_GetPatientGuarantor = "PAT_GetPatientGuarantor";
            public readonly static string PAT_GetPatientGuardian = "PAT_GetPatientGuardian";
            public readonly static string PAT_GetPatientPhoneAddress = "PAT_GetPatientPhoneAddress";
            public readonly static string PAT_GetPatientInsuranceInsuredPerson = "PAT_GetPatientInsuranceInsuredPerson";
            public readonly static string PAT_GetPatientCustomLabels = "PAT_GetPatientCustomLabels";
            public readonly static string PAT_GetAuthorizationsForPatientPayer = "PAT_GetAuthorizationsForPatientPayer";
            public readonly static string PAT_GetImmunization = "PAT_GetImmunization";
            public readonly static string PAT_GetDiagnosis = "PAT_GetDiagnosis";
            public readonly static string PAT_GetAllergies = "PAT_GetAllergies";
            public readonly static string PAT_GetAllAuthorizationsForPatient = "PAT_GetAllAuthorizationsForPatient";
            public readonly static string PAT_GetMedication = "PAT_GetMedication";
            public readonly static string PAT_GetMedicationForPrint = "PAT_GetMedicationForPrint";
            public readonly static string PAT_GetVitals = "PAT_GetVitals";
            public readonly static string PAT_CheckExistingPatient = "PAT_CheckExistingPatient";
            public readonly static string PAT_SaveEligibilityEnquiryRequestData = "PAT_SaveEligibilityEnquiryRequestData";
            public readonly static string PAT_UpdateEligibilityRequest = "PAT_UpdateEligibilityRequest";
            public readonly static string PAT_GetEDI270RequestInfo = "PAT_GetEDI270RequestInfo";
            public readonly static string MTR_DecryptPHIData = "MTR_DecryptPHIData";
            public readonly static string MTR_EncryptPHIData = "MTR_EncryptPHIData";
            public readonly static string PAT_SavePatientAddressAndPhoneNumber = "PAT_SavePatientAddressAndPhoneNumber";
            public readonly static string MTR_EncryptMultipleValues = "MTR_EncryptMultipleValues";
            public readonly static string EDI_SaveEDI999ResponseDetails = "EDI_SaveEDI999ResponseDetails";
            public readonly static string PAT_GetEligibilityEnquiryServiceCodes = "PAT_GetEligibilityEnquiryServiceCodes";
            //Patient Prescription
            public readonly static string PAT_GetPrescription = "PAT_GetPrescription";
            public readonly static string PAT_MasterprescriptionDrugs = "PAT_MasterprescriptionDrugs";
            public readonly static string PAT_Masterpharmacy = "PAT_Masterpharmacy";
            public readonly static string PAT_GetSentPrescription = "PAT_GetSentPrescription";
            public readonly static string PAT_GetSYmptomateReportListing = "PAT_GetSYmptomateReportListing";

            //Common
            public readonly static string MTR_CheckRecordDepedencies = "MTR_CheckRecordDepedencies";
            public readonly static string MTR_AutoCompleteSearching = "MTR_AutoCompleteSearching";
            public readonly static string MTR_GetEDIGateways = "MTR_GetEDIGateways";
            public readonly static string MTR_GetInsuranceType = "MTR_GetInsuranceType";
            public readonly static string MTR_GetLocations = "MTR_GetLocations";
            public readonly static string MTR_GetRoles = "MTR_GetRoles";
            public readonly static string MTR_GetMasterServiceCodeExcludedFromPayerServiceCodes = "MTR_GetMasterServiceCodeExcludedFromPayerServiceCodes";
            public readonly static string MTR_GetAuthorizationById = "MTR_GetAuthorizationById";
            public readonly static string LOC_GetLocationDetail = "LOC_GetLocationDetail";
            public readonly static string MTR_GetAllLocationsByOrganizationId = "MTR_GetAllLocationsByOrganizationId";

            //Payer
            public readonly static string PAY_PayerInformation = "PAY_PayerInformation";
            public readonly static string PAY_GetPayerServiceCodeDetail = "PAY_GetPayerServiceCodeDetail";
            public readonly static string PAY_GetMasterAppointmentTypeForPayer = "PAY_GetMasterAppointmentTypeForPayer";
            public readonly static string PAY_GetPayerServiceCodeModifiers = "PAY_GetPayerServiceCodeModifiers";
            public readonly static string PAY_SaveUpdatePayerServiceCode = "PAY_SaveUpdatePayerServiceCode";
            public readonly static string PAY_GetPayerOrMasterServiceCodes = "PAY_GetPayerOrMasterServiceCodes";
            public readonly static string PAY_GetActivities = "PAY_GetActivities";
            public readonly static string PAY_ActivityServiceCodes = "PAY_ActivityServiceCodes";
            public readonly static string PAY_GetExcludedServiceCodesFromActivity = "PAY_GetExcludedServiceCodesFromActivity";
            //public readonly static string GetKeywordList = "GetKeywordList";
            public readonly static string GetKeywordList = "GetKeywordList_v1";
            //Payroll
            public readonly static string PAR_SaveUpdateStaffPayrollRateForActivity = "PAR_SaveUpdateStaffPayrollRateForActivity";
            public readonly static string PAR_GetStaffPayRateOfActivity = "PAR_GetStaffPayRateOfActivity";



            // Dashboard
            public readonly static string ADM_GetTotalRevenue = "ADM_GetTotalRevenue";
            public readonly static string ADM_GetOrganizationAuthorization = "ADM_GetOrganizationAuthorization";
            public readonly static string ADM_GetOrganizationEncounter = "ADM_GetOrganizationEncounter";
            public readonly static string ADM_GetCurrentYearRegisteredClientCount = "ADM_GetCurrentYearRegisteredClientCount";
            public readonly static string ADM_GetStaffEncounters = "ADM_GetStaffEncounters";
            public readonly static string ADM_GetClientsStatusChart = "ADM_GetClientsStatusChart";
            public readonly static string ADM_GetNotification = "ADM_GetNotification";
            public readonly static string ADM_GetTotalAppointmentRevenue = "GetAppointmentRevenue";
            public readonly static string ADM_GetTotalAppointmentRefund = "GetAppointmentRefund";
            public readonly static string APT_GetDataForAppointmentsGraph = "APT_GetDataForAppointmentsGraph";
            public readonly static string ADM_GetNotificationDashboard = "ADM_GetNotificationForDashboard";

            //Message
            public readonly static string MSG_GetInboxData = "MSG_GetInboxData";
            public readonly static string MSG_SentMessageData = "MSG_SentMessageData";
            public readonly static string MSG_GetMessageDetail = "MSG_GetMessageDetail";
            public readonly static string MSG_UsersData = "MSG_UsersData";
            public readonly static string MSG_GetFavouriteMessage = "MSG_GetFavouriteMessage";
            public readonly static string MSG_GetDeletedMessage = "MSG_GetDeletedMessage";
            public readonly static string MSG_GetMessageCounts = "MSG_GetMessageCounts";
            public readonly static string MSG_GetAllMessagesOfThread = "MSG_GetAllMessagesOfThread";

            //Master
            public readonly static string MTR_GetMasterServiceCodes = "MTR_GetMasterServiceCodes";
            public readonly static string MTR_GetAppointmentType = "MTR_GetAppointmentType";
            public readonly static string MTR_GetCustomLabels = "MTR_GetCustomLabels";
            public readonly static string MTR_GetMasterTags = "MTR_GetMasterTags";
            public readonly static string MTR_GetMasterICDCodes = "MTR_GetMasterICDCodes";
            public readonly static string MTR_GetSecurityQuestions = "MTR_GetSecurityQuestions";
            public readonly static string MTR_GetMasterTemplates = "MTR_GetMasterTemplates";

            //Payroll
            public readonly static string PRL_GetPayrollGroupList = "PRL_GetPayrollGroupList";
            public readonly static string PRL_GetPayrollGroupDropdown = "PRL_GetPayrollGroupDropdown";


            // Organization
            public readonly static string ORG_GetOrganizationData = "ORG_GetOrganizationData";

            //Chat
            public readonly static string CHT_GetChatHistory = "CHT_GetChatHistory";
            public readonly static string CHT_GetUserConnectedIdInRoom = "Chat_GetConnectUserInRoom";

            //Questionnaire
            public readonly static string DFA_GetCategories = "DFA_GetCategories";
            public readonly static string DFA_GetCategoryCodes = "DFA_GetCategoryCodes";
            public readonly static string DFA_GetDocuments = "DFA_GetDocuments";
            public readonly static string DFA_GetSections = "DFA_GetSections";
            public readonly static string DFA_GetSectionItems = "DFA_GetSectionItems";
            public readonly static string DFA_GetSectionItemDDValues = "DFA_GetSectionItemDDValues";
            public readonly static string DFA_GetSectionItemsByID = "DFA_GetSectionItemsByID";
            public readonly static string DFA_SaveAnswers = "DFA_SaveAnswers";
            public readonly static string DFA_GetPatientDocumentAnswer = "DFA_GetPatientDocumentAnswer";
            public readonly static string DFA_GetPatientDocuments = "DFA_GetPatientDocuments";
            //public readonly static string GetQuestionnaireQuestionsList = "GetQuestionnaireQuestionList";
            //public readonly static string GetQuestionnaireQuestionsList = "GetQuestionnaireQuestionList_v1";
            public readonly static string GetQuestionnaireQuestionsList = "GetQuestionnaireQuestionList_v2";
            public readonly static string GetQuestionnaireList = "GetQuestionnaireList";

            //Patient Encounter Templates
            public readonly static string ENC_GetPatientEncounterTemplateData = "ENC_GetPatientEncounterTemplateData";

            // notifications
            public readonly static string ADM_NotificationDetailsById = "ADM_NotificationDetailsById";
            public readonly static string ADM_GetNotificationList = "ADM_GetNotificationList";
            public readonly static string NS_GetAllUnReadNotification = "NS_GetAllUnReadNotification";

            public readonly static string PAT_GetNotificationList = "PAT_GetNotificationList";
            public readonly static string GetAllUnReadNotificationForPatient = "GetAllUnReadNotificationForPatient";
            public readonly static string PAT_TotalNotificationAndChatNotificationCount = "PAT_TotalNotificationAndChatNotificationCount";

            //OHC
            public readonly static string MTR_GetDiseaseManagementProgramList = "MTR_GetDiseaseManagementProgramList";
            public readonly static string MTR_GetDiseaseProgramsListWithEnrollments = "MTR_GetDiseaseProgramsListWithEnrollments";
            public readonly static string MTR_GetDiseaseConditionsFromProgramIds = "MTR_GetDiseaseConditionsFromProgramIds";
            public readonly static string MTR_GetDiseaseManagementProgramActivitiesList = "MTR_GetDiseaseManagementProgramActivitiesList";
            public readonly static string DFA_GetPatientDocumentDetails = "DFA_GetPatientDocumentDetails";
            public readonly static string DFA_GetSectionItemsForForm = "DFA_GetSectionItemsForForm";
            public readonly static string DFA_GetQuestionType = "DFA_GetQuestionType";
            public readonly static string DFA_GetQuesDMP_GetAllPatientDiseaseManagementProgramsListtionType = "DMP_GetAllPatientDiseaseManagementProgramsList";
            public readonly static string DMP_GetPatientDiseaseManagementProgramList = "DMP_GetPatientDiseaseManagementProgramList";
            public readonly static string DMP_GetAllPatientDiseaseManagementProgramsList = "DMP_GetAllPatientDiseaseManagementProgramsList";
            public readonly static string DMP_GetProgramsEnrollPatientsForBulkMessage = "DMP_GetProgramsEnrollPatientsForBulkMessage";
            public readonly static string DMP_ExportPatientDiseaseManagementProgramsData = "DMP_ExportPatientDiseaseManagementProgramsData";
            public readonly static string DMP_PrintPatientDiseaseManagementProgramsData = "DMP_PrintPatientDiseaseManagementProgramsData";
            public readonly static string ADM_PrintEncounterPDF = "ADM_PrintEncounterPDF";
            public readonly static string ADM_GetEncounterExportExcel = "ADM_GetEncounterExportExcel";
            public readonly static string ADM_GetAllPAtientEncounterUsers = "ADM_GetAllPAtientEncounterUsers";
            public readonly static string ADM_GetAllPAtientEncounters = "ADM_GetAllPAtientEncounters";
            public readonly static string PAT_GetPatientAlerts = "PAT_GetPatientAlerts";
            public readonly static string PAT_GetAllPatientAlertsUsers = "PAT_GetAllPatientAlertsUsers";
            public readonly static string DMP_GetDiseaseManagmentProgramActivityDiary = "DMP_GetDiseaseManagmentProgramActivityDiary";
            public readonly static string DMP_GetDiseaseManagementProgramActivitiesByDMPId = "DMP_GetDiseaseManagementProgramActivitiesByDMPId";
            public readonly static string DFA_GetPatientQuestionnnaireAggregatedResponse = "DFA_GetPatientQuestionnnaireAggregatedResponse";
            public readonly static string DMP_GetDiseaseManagementProgramActivitiesForMember = "DMP_GetDiseaseManagementProgramActivitiesForMember";
            public readonly static string RPT_GetFilteredAppointmentsForReporting = "RPT_GetFilteredAppointmentsForReporting";
            public readonly static string MTR_GetLocationByServiceLocationID = "MTR_GetLocationByServiceLocationID";
            public readonly static string ADM_GetRedAlertsInfo = "ADM_GetRedAlertsInfo";
            public readonly static string AMD_GetMasterChronicConditionList = "AMD_GetMasterChronicConditionList";
            public readonly static string PAT_GetMemberHRARequiredData = "PAT_GetMemberHRARequiredData";
            public readonly static string PAT_GetPrintEncounterSummaryDetails = "PAT_GetPrintEncounterSummaryDetails";
            public readonly static string PAT_GetMemberHRAData = "PAT_GetMemberHRAData";

            //ML 2.2
            public readonly static string EHS_GeteHealthScoreDataForPDFReport = "EHS_GeteHealthScoreDataForPDFReport";
            public readonly static string PAT_GetMemberHealtheScoreData = "PAT_GetMemberHealtheScoreData";
            public readonly static string PAT_AssignHealtheScoreToMultipleMember = "PAT_AssignHealtheScoreToMultipleMember";
            public readonly static string PAT_GetHealtheScoreDataForMember = "PAT_GetHealtheScoreDataForMember";
            public readonly static string PAT_UpdateHealtheScoreForMultipleMember = "PAT_UpdateHealtheScoreForMultipleMember";
            public readonly static string PAT_GetMemberHealtheScoreRequiredData = "PAT_GetMemberHealtheScoreRequiredData"; //Not found
            public readonly static string PAT_GetHealtheScoreToExcel = "PAT_GetHealtheScoreToExcel";
            public readonly static string CM_GetCareTeamManagerList = "CM_GetCareTeamManagerList";
            public readonly static string CM_GetTeamManagerList = "CM_GetTeamManagerList";
            public readonly static string PAT_AttachCareTeamToPatient = "PAT_AttachCareTeamToPatient";

            //print assessment
            public readonly static string PAT_GetAssessmentType = "PAT_GetAssessmentType";
            public readonly static string PAT_GetHRADataIndividualReport = "PAT_GetHRADataIndividualReport";
            public readonly static string PAT_GetWHODataIndividualReport = "PAT_GetWHODataIndividualReport";
            public readonly static string PAT_GetAsthmaDataIndividualReport = "PAT_GetAsthmaDataIndividualReport";
            public readonly static string PAT_GetCOPDDataIndividualReport = "PAT_GetCOPDDataIndividualReport";
            public readonly static string PAT_GetDiabetesDataIndividualReport = "PAT_GetDiabetesDataIndividualReport";
            public readonly static string PAT_GetCardiovascularDataIndividualReport = "PAT_GetCardiovascularDataIndividualReport";
            public readonly static string PAT_GetDataForExecutiveReport = "PAT_GetDataForExecutiveReport";



            #region Book Appointment
            public readonly static string GetProviderListToBookAppointment = "getStaffAvailableForProfileListing";
            public readonly static string getStaffAvailableForProfileListingKeySearch = "getStaffAvailableForProfileListingKeySearch";
            public readonly static string GetProviderListToBookAppointmentForMobile = "getStaffAvailableForProfileListing_v1";
            public readonly static string GetProviderListToBookAppointmentForRate = "testgetStaffAvailableForProfileListing";
            public readonly static string GetProviderListToBookAppointmentLessRate = "LesstestgetStaffAvailableForProfileListing";
            public readonly static string GetProviderListToBookAppointmentMoreRate = "MoretestgetStaffAvailableForProfileListing";
            public readonly static string GetProviderListToBookAppointmentRviewRating = "ReviewRatinggetStaffAvailableForProfileListing";
            public readonly static string GetProviderListToBookAppointmentForRate_v1 = "PayrategetStaffAvailableForProfileListing";
            public readonly static string GetProviderListToBookAppointmentForRate_v2 = "PayrategetStaffAvailableForProfileListing_v1";
            public readonly static string GetProviderListToBookAppointmentRviewRating_v1 = "ReviewRatinggetStaffAvailableForProfileListing_v1";
            public readonly static string GetSortedProviderListToBookAppointment = "Sorting_StaffAvailableForProfileListing";
            public readonly static string GetUrgentCareProviderListToBookAppointment = "getUrgentCareStaffListing";
            public readonly static string GetUrgentCareProviderListToBookAppointmentForRate = "Payrate_getUrgentCareStaffAvailable";
            public readonly static string getUrgentCareStaffAvailableListingKeySearch = "getUrgentCareStaffAvailableListingKeySearch";
            public readonly static string GetSortedUrgentCareProviderListToBookAppointment = "Sorting_UrgentCareStaffListing";

            #endregion Book Appointment
            #region Get Appointment Payment Listing

            public readonly static string PAY_GetAppointmentPaymentListing = "PAY_GetAppointmentPayments";
            public readonly static string PAY_GetAppointmentRefundListing = "PAY_GetAppointmentPaymentRefund";
            public readonly static string PAY_GetClientAppointmentPaymentListing = "PAY_GetClientAppointmentPayments";
            public readonly static string PAY_GetClientAppointmentRefundListing = "PAY_GetClientAppointmentPaymentRefund";
            public readonly static string PAY_GetAppointmentPaymentListingForReport = "PAY_GetAppointmentPaymentsForReport";
            public readonly static string PAY_GetClienPayments = "PAY_GetClientPayments";
            public readonly static string APT_GetPatientPastUpcomingAppointment = "APT_GetPatientPastUpcomingAppointment";



            #endregion Get Appointment Payment Listing

            //SpringB
            public readonly static string SpringB_GetSpringBVitals = "GetSpringBVitals";
            public readonly static string SpringB_GetPatientDiagnosis = "SpringB_GetPatientDiagnosis";
            public readonly static string SpringB_GetMedication = "SpringB_GetMedication";
            public readonly static string GetDistinctDateOfVitals = "GetDistinctDateOfVitals";
            public readonly static string PAT_AddClaimsMedToCurrent = "PAT_AddClaimsMedToCurrent";
            public readonly static string PAT_GetLatestPatientVitalDetail = "PAT_GetLatestPatientVitalDetail";

            //Medication
            public readonly static string GetMasterMedicationAutoComplete = "GetMasterMedicationAutoComplete";
            public readonly static string GetPatientMedicationDetail = "GetPatientMedicationDetail";
            public readonly static string PAT_GetCurrentMedicationList = "PAT_GetCurrentMedicationList";
            public readonly static string GetCurrentMedicationByID = "GetCurrentMedicationByID";
            public readonly static string PAT_GetPrintPatientCurrentMedication = "PAT_GetPrintPatientCurrentMedication";
            public readonly static string PAT_GetCurrentAndClaimMedicationList = "PAT_GetCurrentAndClaimMedicationList";
            public readonly static string MTR_GetCurrentMedicationStrengthList = "MTR_GetCurrentMedicationStrengthList";
            public readonly static string MTR_GetCurrentMedicationUnit = "MTR_GetCurrentMedicationUnit";
            public readonly static string MTR_GetCurrentMedicationForm = "MTR_GetCurrentMedicationForm";

            //patient
            public readonly static string PAT_GetPatientPhysicianList = "PAT_GetPatientPhysicianList";
            public readonly static string CHT_GetCareChatHistory = "CHT_GetCareChatHistory";
            public readonly static string PAT_GetDiagnosisWithPagination = "PAT_GetDiagnosisWithPagination";
            public readonly static string Log_GetHraProgramsLogs = "Log_GetHraProgramsLogs";

            //Bluebutton
            public readonly static string PAT_UpdateProfileDetails = "PAT_UpdateProfileDetailsFromApis";
        }

        public enum StaffAvailabilityEnum
        {
            WEEKDAY,
            AVAILABLE,
            UNAVAILABLE
        }
        public enum ImagesFolderEnum
        {
            //Organization images folder name
            Logo,
            Favicon,

            //encounter sign images
            ClinicianSign,
            PatientSign,
            GuardianSign
        }
        public static class ClaimHistoryAction
        {
            public readonly static string AddServiceLine = "Add Service Line";
            public readonly static string UpdateServiceLine = "Update Service Line";
            public readonly static string DeleteServiceLine = "Delete Service Line";
            public readonly static string PrintPaperClaim = "Print Paper Claim";
            public readonly static string PrintPaperClaimForSecondary = "Print Paper Claim For Secondary Payer";
            public readonly static string PrintAndSubmitPaperClaim = "Print and Submit Paper Claim";
            public readonly static string PrintAndSubmitPaperClaimForSecondary = "Print and Submit Paper Claim For Secondary";
            public readonly static string ResubmissionClaim = "Resubmitted Claim";
            public readonly static string PrimaryPayerSubmission = "Submitted To Primary Payer";
            public readonly static string SecondaryPayerSubmission = "Submitted To Secondary Payer";
            public readonly static string TertiaryPayerSubmission = "Submitted To Tertiary Payer";
        }
        public static class EncryptDecryptKey
        {
            public readonly static string Key = "MAKV2SPBNI99212";
            public readonly static string PHIKey = "~!@#$%^*HeaLthC@re$smaRtData~!@!!!=";
        }
        public static class EncryptionDecryptionFlags
        {
            public readonly static bool PHIEncryptionDecryptionFlag = false;
        }
        public enum UserTypeEnum
        {
            CLIENT,
            STAFF,
            ADMIN,
            PROVIDER
        }
        public enum OrganizationRoles
        {
            Admin,
            Patient,
        }
        public enum DocumentUserTypeEnum //only for document user type
        {
            PATIENT
        }

        public static class ColorCode
        {
            public readonly static string Organge = "#fdba2c";
            public readonly static string Red = "#a94442";
            public readonly static string Green = "#3c763d";
        }

        public static class CssClass
        {
            public readonly static string Sucess = "alert-success";
            public readonly static string Warning = "alert-warning";
            public readonly static string Danger = "alert-danger";
        }

        public enum UserInvitationStatus
        {
            Pending = 0,
            Accepted = 1,
            Rejected = 2,
            ReInvited = 3
        }
        public enum EmailType
        {
            InvitationEmail = 1,
            BookAppointment = 2,
            GroupSessionInvitation = 3,
            ResetPassword = 4,
            RegisterAgency = 5,
            PatientBlueButton = 6
        }
        public enum EmailSubType
        {
            none = 0,
            BookAppointmentToClient = 1,
            BookAppointmentToProvider = 2,
            AcceptApointment = 3,
            RejectApointment = 4,
            NewGroupSessionInvitation = 5,
            ResendGroupSessionInvitation = 6,
            AcceptInvitationApointment = 7,
            RejectInvitationApointment = 8,
            AgencyRegistrationCompleted = 9,
            PatientBlueButtonInstructions = 10

        }
        public enum NotificationActionType
        {
            UserInvitation = 1,
            DeleteAppointment = 2,
            CancelAppointment = 3,
            RequestAppointment = 4,
            ChatMessage = 5,
            TaskAssign = 6,
            TaskCompleted = 7,
            UpdateAppointment = 8,
            UpdateRequestAppointment = 9,
            ApprovedAppointment = 10,
            ActivateAppointment = 11,
            TentativeAppointment = 12,
            AcceptAppointment = 13,
            RejectAppointment = 14,
            AutoConfirmTentativeAppointment = 15,
            CreateAppointment = 16,
            GroupSession = 17

        }
        public enum NotificationActionSubType
        {
            SendInvitation = 1,
            ReSendInvitation = 2,
            GroupSessionInvitaion = 3,
            GroupSessionInvitaionAccepted = 4,
            GroupSessionInvitaionRejected = 5,
            RequestAppointment = 6,
            AppointmentAcceptedByProvider = 7,
            AppointmentRejectedByProvider = 8,
        }

        public enum NotificationType
        {
            TextNotification = 1,
            PushNotification = 2,
            EmailNotification = 3,

        }

        public enum PaymentMode
        {
            Stripe = 1,
            Paypal = 2
        }
        public static class NotificationMessage
        {
            public readonly static string Message = "Invitation sent successfully";
            public readonly static string GroupSessionInvitation = "Group session invitation sent successfully";
            public readonly static string GroupSessionInvitationAccepted = "Group session invitation accepted successfully";
            public readonly static string GroupSessionInvitationRejected = "Group session invitation rejected";
            public readonly static string GroupSessionInvitationNotification = "has Invited to join a group session.";

            public readonly static string AppointmentRequested = "You have new appointment, requested by";
            public readonly static string AppointmentAccepted = "Your upcoming appointment has been scheduled with";
            public readonly static string AppointmentCancelled = "Your appointment has been cancelled";
            public readonly static string AppointmentRescheduled = "Your appointment has been rescheduled";
            public readonly static string AppointmentRejected = "Your appointment has been Rejected by";


        }
        public enum MessageType
        {
            Text = 0,
            File = 1,
            Recording = 2
        }
        public static class PushNotificationPriority
        {
            public readonly static string High = "High";
            public readonly static string Medium = "Medium";
            public readonly static string Low = "Low";
        }
        public static class FCMAPIConfig
        {
            public readonly static string PushNotification_APIEndPoint = "https://fcm.googleapis.com/fcm/send";
            public readonly static string PushNotification_APIEndPointToken = "AAAAdF1W9mM:APA91bFTfu9hsf0RXz6rR2_JUXn9Hhf3FdQbZal-WzqBGmV6M_uno7lxkOzIMt2Wsm1eFtQmrA5hPt-kXjtvin1fW1LJveCV_WCRudEeXrzMNbZAEA9mHPlgPj5sqtB2nhExuhg3GYzD";
            public readonly static string PushNotification_SenderKey = "499782186595";
        }
        public static class SymptomConfig
        {
            public readonly static string Symptom_APIEndPoint = "https://api.infermedica.com/v2";
            public readonly static string Symptom_APIEndPointToken = "AAAAdF1W9mM:APA91bFTfu9hsf0RXz6rR2_JUXn9Hhf3FdQbZal-WzqBGmV6M_uno7lxkOzIMt2Wsm1eFtQmrA5hPt-kXjtvin1fW1LJveCV_WCRudEeXrzMNbZAEA9mHPlgPj5sqtB2nhExuhg3GYzD";
            public readonly static string symptom_SenderId = "9c2cba74";
            public readonly static string Symptom_SenderKey = "21503bbf325177d819ea07819ae43406";
        }


        public static class Benchmarks
        {
            public const string Normal_Risk = "Normal Risk";
            public const string Moderate_Risk = "Moderate Risk";
            public const string High_Risk = "High Risk";
            //BMI
            public const string Underweight = "Underweight";
            public const string Normal = "Normal";
            public const string Overweight = "Overweight";
            public const string Obese = "Obese";
        }
        public static class PortalType
        {
            public const string CareManager = "caremanager";
            public const string WebMemberPortal = "memberportal";
        }
        public enum LogReportType
        {
            HRA = 1,
            Programs = 2
        }
    }
}
