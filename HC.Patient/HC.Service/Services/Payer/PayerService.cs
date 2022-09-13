using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.Payer;
using HC.Patient.Service.Services.Notification;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payer
{
    public class PayerService : BaseService, IPayerService
    {
        private readonly IPayerRepository _payerRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly IkeywordRepository _keywordRepository;
        private readonly IProviderCareCategoryRepository _providercarecategoryRepository;
        private readonly ISymptomatePatientReportRepository _symptomatepatientreportRepository;
        private HCOrganizationContext _context;
        private readonly IPatientService _patientService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IProviderQuestionnaireQuestionsRepository _providerquestionnairequestionsRepository;
        private readonly IQuestionnaireOptionsRepository _providerquestionnaireoptionsRepository;
        private readonly IProviderQuestionnairesRepository _questionnaireRepository;
        JsonModel response = null;
        public PayerService(IPayerRepository payerRepository, IUserCommonRepository userCommonRepository, IkeywordRepository keywordRepository, HCOrganizationContext context, IProviderCareCategoryRepository providercarecategoryRepository, ISymptomatePatientReportRepository symptomatepatientreportRepository, IPatientService patientService,INotificationRepository notificationRepository,IStaffRepository staffRepository, IProviderQuestionnaireQuestionsRepository providerquestionnairequestionsRepository, IQuestionnaireOptionsRepository providerquestionnaireoptionsRepository, IProviderQuestionnairesRepository questionnaireRepository)
        {
            _payerRepository = payerRepository;
            _userCommonRepository = userCommonRepository;
            _keywordRepository = keywordRepository;
            _context = context;
            _providercarecategoryRepository = providercarecategoryRepository;
            _symptomatepatientreportRepository = symptomatepatientreportRepository;
            _patientService = patientService;
            _notificationRepository= notificationRepository;
            _staffRepository = staffRepository;
            _providerquestionnairequestionsRepository = providerquestionnairequestionsRepository;
            _providerquestionnaireoptionsRepository = providerquestionnaireoptionsRepository;
            _questionnaireRepository = questionnaireRepository;
        response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        }


        public JsonModel DeletePayerData(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterICD"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            else
            {
                InsuranceCompanies insuranceCompanies = _payerRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (insuranceCompanies != null)
                {
                    insuranceCompanies.IsDeleted = true;
                    insuranceCompanies.DeletedBy = tokenModel.UserID;
                    insuranceCompanies.DeletedDate = DateTime.UtcNow;
                    _payerRepository.Update(insuranceCompanies);
                    _payerRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.PayerDeleted, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.PayerDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        public JsonModel GetPayerDataById(int id, TokenModel tokenModel)
        {
            InsuranceCompanies insuranceCompanies = _payerRepository.GetByID(id);
            if (insuranceCompanies != null)
            {
                InsuranceCompanyModel insuranceCompanyModel = new InsuranceCompanyModel();
                AutoMapper.Mapper.Map(insuranceCompanies, insuranceCompanyModel);
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel GetPayerList(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<InsuranceCompanyModel> insuranceCompanyModel = _payerRepository.GetPayerList<InsuranceCompanyModel>(searchFilterModel, tokenModel).ToList();
            if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
            {
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel SavePayerData(InsuranceCompanyModel insuranceCompanyModel, TokenModel tokenModel)
        {
            InsuranceCompanies insuranceCompanies = null;
            if (insuranceCompanyModel.Id == 0)
            {
                insuranceCompanies = new InsuranceCompanies();
                AutoMapper.Mapper.Map(insuranceCompanyModel, insuranceCompanies);
                insuranceCompanies.CreatedBy = tokenModel.UserID;
                insuranceCompanies.CreatedDate = DateTime.UtcNow;
                insuranceCompanies.IsDeleted = false;
                insuranceCompanies.IsActive = true;
                insuranceCompanies.OrganizationID = tokenModel.OrganizationID;
                _payerRepository.Create(insuranceCompanies);
                _payerRepository.SaveChanges();
                response = new JsonModel(insuranceCompanies, StatusMessage.PayerCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                insuranceCompanies = _payerRepository.Get(a => a.Id == insuranceCompanyModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (insuranceCompanies != null)
                {
                    AutoMapper.Mapper.Map(insuranceCompanyModel, insuranceCompanies);
                    insuranceCompanies.UpdatedBy = tokenModel.UserID;
                    insuranceCompanies.UpdatedDate = DateTime.UtcNow;
                    _payerRepository.Update(insuranceCompanies);
                    _payerRepository.SaveChanges();
                    response = new JsonModel(insuranceCompanies, StatusMessage.PayerUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.PayerDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        public JsonModel SaveKeyword(KeywordModel keywordmodel, TokenModel tokenModel)
        {
            HealthcareKeywords healthcareKeywords = null;
            foreach(var item in keywordmodel.healthCareCategoryKeywords)
            {
                if (item.Id > 0)
                {
                    healthcareKeywords = _keywordRepository.Get(a => a.Id == item.Id && a.IsActive == true && a.IsDeleted == false);
                    if (healthcareKeywords != null)
                    {
                        AutoMapper.Mapper.Map(keywordmodel, healthcareKeywords);
                        healthcareKeywords.UpdatedBy = tokenModel.UserID;
                        healthcareKeywords.UpdatedDate = DateTime.UtcNow;
                        healthcareKeywords.KeywordName = item.KeywordName;
                        healthcareKeywords.Id = item.Id;
                        _keywordRepository.Update(healthcareKeywords);
                        _keywordRepository.SaveChanges();
                        response = new JsonModel(healthcareKeywords, StatusMessage.KeywordUpdated, (int)HttpStatusCode.OK);
                    }
                    else
                    {
                        response = new JsonModel(null, StatusMessage.KeywordDoesNotExist, (int)HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    healthcareKeywords = new HealthcareKeywords();
                    AutoMapper.Mapper.Map(keywordmodel, healthcareKeywords);
                    healthcareKeywords.CreatedBy = tokenModel.UserID;
                    healthcareKeywords.CreatedDate = DateTime.UtcNow;
                    healthcareKeywords.IsDeleted = false;
                    healthcareKeywords.IsActive = true;
                    //healthcareKeywords.ProviderCareCategory = keywordmodel.Id;
                    healthcareKeywords.KeywordName= item.KeywordName;
                    healthcareKeywords.Id = item.Id;
                    //healthcareKeywords.OrganizationID = tokenModel.OrganizationID;
                    _keywordRepository.Create(healthcareKeywords);
                    _keywordRepository.SaveChanges();
                    response = new JsonModel(healthcareKeywords, StatusMessage.KeywordCreated, (int)HttpStatusCode.OK);
                }
            }
           
            return response;
        }


        public JsonModel SaveCareCategory(ProviderCareCategoryModel carecategorymodel, TokenModel tokenModel)
        {
            ProviderCareCategory providercarecategory = null;
            if (carecategorymodel.Id == 0)
            {
                providercarecategory = new ProviderCareCategory();
                AutoMapper.Mapper.Map(carecategorymodel, providercarecategory);
                providercarecategory.CreatedBy = tokenModel.UserID;
                providercarecategory.CreatedDate = DateTime.UtcNow;
                providercarecategory.IsDeleted = false;
                providercarecategory.IsActive = true;
                providercarecategory.OrganizationId = tokenModel.OrganizationID;
                _providercarecategoryRepository.Create(providercarecategory);
                _providercarecategoryRepository.SaveChanges();
                response = new JsonModel(providercarecategory, StatusMessage.CareCategoryCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                providercarecategory = _providercarecategoryRepository.Get(a => a.Id == carecategorymodel.Id && a.IsActive == true && a.IsDeleted == false);
                if (providercarecategory != null)
                {
                    AutoMapper.Mapper.Map(carecategorymodel, providercarecategory);
                    providercarecategory.UpdatedBy = tokenModel.UserID;
                    providercarecategory.UpdatedDate = DateTime.UtcNow;
                    _providercarecategoryRepository.Update(providercarecategory);
                    _providercarecategoryRepository.SaveChanges();
                    response = new JsonModel(providercarecategory, StatusMessage.CareCategoryUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.CareCategoryDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        public JsonModel GetKeywordList(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<KeywordpaginationModel> insuranceCompanyModel = _keywordRepository.GetKeywordList<KeywordpaginationModel>(searchFilterModel, tokenModel).ToList();
            if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
            {
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel ProviderAvailableListForMobile(AppointmentSearchModelForMobile appointmentSearchModel, TokenModel tokenModel)
        {
            List<string> presIds = appointmentSearchModel.keywords.Split(',').ToList();
            foreach (string keyword in presIds)
            {
                var categoryid = _context.HealthcareKeywords.Where(p => p.KeywordName == keyword).Select(x => x.CareCategoryId).FirstOrDefault();
                if (categoryid != 0)
                {
                    var providerList = _keywordRepository.GetProviderListToMakeAppointmentForMobile(tokenModel, null, appointmentSearchModel, Convert.ToInt32(categoryid));
                    if (providerList != null && providerList.Count > 0)
                    {
                        response.data = providerList;
                        response.Message = StatusMessage.FetchMessage;
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.meta = new Meta(providerList, appointmentSearchModel);
                    }
                    else
                    {
                        response.data = new object();
                        response.Message = StatusMessage.NotFound;
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    return response;
                }
            }
            return response;
        }


        public JsonModel GetKeywordDataById(int id, TokenModel tokenModel)
        {
            KeywordModel responsekeywordObj = new KeywordModel();
            List<KeywordModel> responsekeywordObjList = new List<KeywordModel>();
            responsekeywordObj.healthCareCategoryKeywords = new List<HealthCareCategoryKeywordsModel>();

            if (id > 0)
            {
                HealthcareKeywords keyHisObj = _keywordRepository.Get(a => a.CareCategoryId == id && a.IsActive == true && a.IsDeleted == false);
               
               
                if (keyHisObj != null)
                {
                    keyHisObj.CareCategoryId = id;
                    List<HealthcareKeywords> patMedFamHisDBObj = _keywordRepository.GetAll(a => a.CareCategoryId == id && a.IsActive == true && a.IsDeleted == false).ToList();
                    AutoMapper.Mapper.Map(keyHisObj, responsekeywordObj);
                    AutoMapper.Mapper.Map(patMedFamHisDBObj, responsekeywordObj.healthCareCategoryKeywords);
                    return new JsonModel()
                    {
                        data = responsekeywordObj,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                else
                {
                    responsekeywordObj.CareCategoryId = id;
                    return new JsonModel()
                    {

                        data = responsekeywordObj,
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound//Success
                    };
                }
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//Success
                };
            }
        }

        //public JsonModel GetKeywordDataById(int id, TokenModel tokenModel)
        //{
        //    HealthcareKeywords keywords = _keywordRepository.GetByID(id);
        //    if (keywords != null)
        //    {
        //        KeywordModel keyword = new KeywordModel();
        //        AutoMapper.Mapper.Map(keywords, keyword);
        //        response = new JsonModel(keyword, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
        //    }
        //    return response;
        //}

        public JsonModel DeleteKeywordData(int id, TokenModel tokenModel)
        {

            HealthcareKeywords keywords = _keywordRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (keywords != null)
                {
                keywords.IsDeleted = true;
                keywords.DeletedBy = tokenModel.UserID;
                keywords.DeletedDate = DateTime.UtcNow;
                _keywordRepository.Update(keywords);
                _keywordRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.KeywordDeleted, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.KeywordDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            return response;
        }


        public JsonModel GetCareCategoryById(int id, TokenModel tokenModel)
        {
            ProviderCareCategory carecategory = _providercarecategoryRepository.GetByID(id);
            if (carecategory != null)
            {
                ProviderCareCategoryModel carecategorymodel = new ProviderCareCategoryModel();
                AutoMapper.Mapper.Map(carecategory, carecategorymodel);
                response = new JsonModel(carecategorymodel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }


        public JsonModel DeleteCareCategory(int id, TokenModel tokenModel)
        {

            ProviderCareCategory carecategory = _providercarecategoryRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (carecategory != null)
            {
                carecategory.IsDeleted = true;
                carecategory.DeletedBy = tokenModel.UserID;
                carecategory.DeletedDate = DateTime.UtcNow;
                _providercarecategoryRepository.Update(carecategory);
                _providercarecategoryRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CareCategoryDeleted, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.CareCategoryDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }

        public JsonModel SaveSymptomatePatientReport(SymptomatePatientReportData symptomatepatientreportdata, TokenModel tokenModel)
        {
            string patreportedsymptoms;
            string patpresentsymptoms;
            string patabsentsymptoms;
            string patunknownsymptoms;
            string patfinalconditionsymptoms;

            bool IsSymptomateReportExist = false;
            int ReportId = 0;

            if (symptomatepatientreportdata.PatientReportedSymptoms != null)
            {
                 patreportedsymptoms = string.Join(",", symptomatepatientreportdata.PatientReportedSymptoms);
            }
            else
            {
                 patreportedsymptoms = "";
            }

            if (symptomatepatientreportdata.PatientPresentSymptoms != null)
            {
                 patpresentsymptoms = string.Join(",", symptomatepatientreportdata.PatientPresentSymptoms);
            }
            else
            {
                patpresentsymptoms = "";
            }

            if (symptomatepatientreportdata.PatientAbsentSymptoms != null)
            {
                patabsentsymptoms = string.Join(",", symptomatepatientreportdata.PatientAbsentSymptoms);
            }
            else
            {
                patabsentsymptoms = "";
            }


            if (symptomatepatientreportdata.PatientUnknownSymptoms != null)
            {
                patunknownsymptoms = string.Join(",", symptomatepatientreportdata.PatientUnknownSymptoms);
            }
            else
            {
                patunknownsymptoms = "";
            }


            if (symptomatepatientreportdata.PatientFinalConditions != null)
            {
                patfinalconditionsymptoms = string.Join(",", symptomatepatientreportdata.PatientFinalConditions);
            }
            else
            {
                patfinalconditionsymptoms = "";
            }


            SymptomatePatientReport symptomatepatientreport = null;
            if (symptomatepatientreportdata.Id == 0)
            {
                symptomatepatientreport = new SymptomatePatientReport();
                AutoMapper.Mapper.Map(symptomatepatientreportdata, symptomatepatientreport);
                symptomatepatientreport.ReportedSymptoms = patreportedsymptoms;
                symptomatepatientreport.PresentSymptoms = patpresentsymptoms;
                symptomatepatientreport.AbsentSymptoms = patabsentsymptoms;
                symptomatepatientreport.UnknownSymptoms = patunknownsymptoms;
                symptomatepatientreport.FinalConditions = patfinalconditionsymptoms;
                symptomatepatientreport.CreatedDate = DateTime.UtcNow;
                _symptomatepatientreportRepository.Create(symptomatepatientreport);
                _symptomatepatientreportRepository.SaveChanges();
                //response = new JsonModel(symptomatepatientreport, StatusMessage.SymptomateReportSaved, (int)HttpStatusCode.OK);
                response = new JsonModel("", StatusMessage.SymptomateReportSaved, (int)HttpStatusCode.OK);
            }

            //Push notifications for mobile device

            PatientAppointment appointmentdetails = new PatientAppointment();
            appointmentdetails = _context.PatientAppointment.Where(x => x.Id == symptomatepatientreportdata.PatientId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            var patientResponse = _patientService.GetPatientByIdForPushNotificatons(appointmentdetails.PatientID, tokenModel);
            PatientDemographicsModel patient;
            if (patientResponse.StatusCode == (int)HttpStatusCodes.OK)
                patient = (PatientDemographicsModel)patientResponse.data;
            else
                patient = null;
            string fullName = patient.FirstName + " " + patient.LastName;

            AppointmentStaff appointmenstaffdetails = new AppointmentStaff();
            appointmenstaffdetails = _context.AppointmentStaff.Where(x => x.PatientAppointmentID == symptomatepatientreportdata.PatientId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            
            string message = _notificationRepository.GetNotificationMessage(fullName, null, symptomatepatientreportdata.PatientId, "requested");

            var result = _context.SymptomatePatientReport.Where(s => s.PatientID == symptomatepatientreportdata.PatientId).Select(s => s.Id).FirstOrDefault();
            if (result != 0)
            {
                IsSymptomateReportExist = true;
                ReportId = result;
            }

            int userId = _context.Staffs.Where(x => x.Id == appointmenstaffdetails.StaffID && x.IsActive == true && x.IsDeleted == false).Select(x => x.UserID).FirstOrDefault();
            string deviceToken = _context.User.Where(x => x.Id == userId).Select(x => x.DeviceToken).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(deviceToken))
            {
                PushMobileNotificationModel pushMobileNotification = new PushMobileNotificationModel();
                pushMobileNotification.DeviceToken = deviceToken;
                pushMobileNotification.Message = message;
                pushMobileNotification.NotificationPriority = PushNotificationPriority.High;
                pushMobileNotification.NotificationType = CommonEnum.NotificationActionType.RequestAppointment.ToString();
                PushNotificationsUserDetailsModel model = new PushNotificationsUserDetailsModel()
                {
                    ProviderID = appointmenstaffdetails.StaffID,
                    PatientID = (int)appointmentdetails.PatientID,
                    AppointmentId = appointmentdetails.Id,
                    ImageThumbnail = "",
                    Name = fullName,
                    Address = "",
                    StartDate = appointmentdetails.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    EndDate = appointmentdetails.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    StatusName = "Pending",
                    IsSymptomateReportExist = IsSymptomateReportExist,
                    ReportId = ReportId
                };
                pushMobileNotification.Data = model;
                PushNotificationsService.SendPushNotificationForMobile(pushMobileNotification);
            }


            return response;
        }

        public JsonModel GetSymptomateReportById(int id, TokenModel tokenModel)
        {
            SymptomatePatientReport symptomatereport = _symptomatepatientreportRepository.GetByID(id);
            //string t = symptomatereport.AbsentSymptoms;
            //List<string> PattAbsentSymptoms= t.Split(',');
            if (symptomatereport != null)
            {
                SymptomatePatientReportData symptomatereportmodel = new SymptomatePatientReportData();
                AutoMapper.Mapper.Map(symptomatereport, symptomatereportmodel);
                symptomatereportmodel.PatientReportedSymptoms = symptomatereport.ReportedSymptoms.Split(',').ToList();
                symptomatereportmodel.PatientPresentSymptoms = symptomatereport.PresentSymptoms.Split(',').ToList();
                symptomatereportmodel.PatientAbsentSymptoms = symptomatereport.AbsentSymptoms.Split(',').ToList();
                symptomatereportmodel.PatientUnknownSymptoms = symptomatereport.UnknownSymptoms.Split(',').ToList();
                symptomatereportmodel.PatientFinalConditions = symptomatereport.FinalConditions.Split(',').ToList();
                response = new JsonModel(symptomatereportmodel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel GetEncounterListingById(int id, TokenModel tokenModel)
        {
            if (id > 0)
            {
                List<PatientEncounterListingModel> patientencountermodel = new List<PatientEncounterListingModel>();
                List<PatientEncounterNotes> patientencounternotes = new List<PatientEncounterNotes>();
                List<PatientEncounter> patientencounter = new List<PatientEncounter>();
                List<SymptomatePatientReport> symptomatepatientreport = new List<SymptomatePatientReport>();
                patientencounter = _context.PatientEncounter.Where(x => x.PatientID == id && x.IsDeleted == false).OrderByDescending(x=>x.CreatedDate).ToList();
                patientencounternotes = _context.PatientEncounterNotes.Where(x => x.PatientId == id).ToList();
                symptomatepatientreport= _context.SymptomatePatientReport.Where(x => x.PrePatientID == id).ToList();
                AutoMapper.Mapper.Map(patientencounter, patientencountermodel);
                //patientencountersdsd = patientencounter.OfType<string>();
                foreach (var i in patientencountermodel)
                {
                    Staffs staffs = _staffRepository.Get(a => a.Id == i.StaffID && a.IsDeleted == false && a.IsActive == true);
                    i.StaffName = staffs.FirstName + " " + staffs.LastName;
                    foreach (var j in patientencounternotes)
                    {
                        if (i.PatientAppointmentId == j.AppointmentId)
                        {
                            i.IsNotesExist=true;
                            i.EncounterNotes = j.EncounterNotes;
                        }
                    }

                    foreach (var k in symptomatepatientreport)
                    {
                        if (i.PatientAppointmentId == k.PatientID)
                        {
                            i.IsSymptomateReportExist = true;
                            i.ReportId = k.Id;
                        }
                    }
                }
                if(patientencounter != null && patientencounter.Count > 0)
                {
                    response = new JsonModel(patientencountermodel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                }

                else
                {
                    response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
                }
               
            }
            else
            {
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }


        public JsonModel AddSymptomateReport(SymptomatePatientReportData symptomatepatientreportdata, TokenModel tokenModel)
        {
            string patreportedsymptoms;
            string patpresentsymptoms;
            string patabsentsymptoms;
            string patunknownsymptoms;
            string patfinalconditionsymptoms;

           

            if (symptomatepatientreportdata.PatientReportedSymptoms != null)
            {
                patreportedsymptoms = string.Join(",", symptomatepatientreportdata.PatientReportedSymptoms);
            }
            else
            {
                patreportedsymptoms = "";
            }

            if (symptomatepatientreportdata.PatientPresentSymptoms != null)
            {
                patpresentsymptoms = string.Join(",", symptomatepatientreportdata.PatientPresentSymptoms);
            }
            else
            {
                patpresentsymptoms = "";
            }

            if (symptomatepatientreportdata.PatientAbsentSymptoms != null)
            {
                patabsentsymptoms = string.Join(",", symptomatepatientreportdata.PatientAbsentSymptoms);
            }
            else
            {
                patabsentsymptoms = "";
            }


            if (symptomatepatientreportdata.PatientUnknownSymptoms != null)
            {
                patunknownsymptoms = string.Join(",", symptomatepatientreportdata.PatientUnknownSymptoms);
            }
            else
            {
                patunknownsymptoms = "";
            }


            if (symptomatepatientreportdata.PatientFinalConditions != null)
            {
                patfinalconditionsymptoms = string.Join(",", symptomatepatientreportdata.PatientFinalConditions);
            }
            else
            {
                patfinalconditionsymptoms = "";
            }


            SymptomatePatientReport symptomatepatientreport = null;
            if (symptomatepatientreportdata.Id == 0)
            {
                symptomatepatientreport = new SymptomatePatientReport();
                AutoMapper.Mapper.Map(symptomatepatientreportdata, symptomatepatientreport);
                symptomatepatientreport.ReportedSymptoms = patreportedsymptoms;
                symptomatepatientreport.PresentSymptoms = patpresentsymptoms;
                symptomatepatientreport.AbsentSymptoms = patabsentsymptoms;
                symptomatepatientreport.UnknownSymptoms = patunknownsymptoms;
                symptomatepatientreport.FinalConditions = patfinalconditionsymptoms;
                symptomatepatientreport.CreatedDate = DateTime.UtcNow;
                _symptomatepatientreportRepository.Create(symptomatepatientreport);
                _symptomatepatientreportRepository.SaveChanges();
                //response = new JsonModel(symptomatepatientreport, StatusMessage.SymptomateReportSaved, (int)HttpStatusCode.OK);
                response = new JsonModel("", StatusMessage.SymptomateReportSaved, (int)HttpStatusCode.OK);
            }

            
            return response;
        }

        public JsonModel GetSymptomateReportListing(PatientSymptomateFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientsSymptomaticReportModel> patientPrescription = _symptomatepatientreportRepository.GetSymptomateReportListing<PatientsSymptomaticReportModel>(patientFilterModel, tokenModel).ToList();
            if (patientPrescription != null && patientPrescription.Count > 0)
            {
                response = new JsonModel(patientPrescription, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientPrescription, patientFilterModel);
            }
            return response;
        }



        public JsonModel SaveProviderQuestionnaireQuestions(ProviderQuestionnaireModel providerquestionnairemodel, TokenModel tokenModel)
        {
            ProviderQuestionnaireQuestions providerquestionnairequestions = null;
            if (providerquestionnairemodel.QuestionId == 0)
            {
                providerquestionnairequestions = new ProviderQuestionnaireQuestions();
                AutoMapper.Mapper.Map(providerquestionnairemodel, providerquestionnairequestions);
                providerquestionnairequestions.CreatedBy = tokenModel.UserID;
                providerquestionnairequestions.CreatedDate = DateTime.UtcNow;
                providerquestionnairequestions.IsDeleted = false;
                providerquestionnairequestions.IsActive = true;
                providerquestionnairequestions.OrganizationId = tokenModel.OrganizationID;
                providerquestionnairequestions.QuestionnareId = providerquestionnairemodel.QuestionnaireId;
                _providerquestionnairequestionsRepository.Create(providerquestionnairequestions);
                _providercarecategoryRepository.SaveChanges();
                response = new JsonModel(providerquestionnairequestions, StatusMessage.ProviderQuestionnaireQuestionCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                providerquestionnairequestions = _providerquestionnairequestionsRepository.Get(a => a.QuestionId == providerquestionnairemodel.QuestionId && a.IsActive == true && a.IsDeleted == false);
                if (providerquestionnairequestions != null)
                {
                    AutoMapper.Mapper.Map(providerquestionnairemodel, providerquestionnairequestions);
                    providerquestionnairequestions.UpdatedBy = tokenModel.UserID;
                    providerquestionnairequestions.UpdatedDate = DateTime.UtcNow;
                    _providerquestionnairequestionsRepository.Update(providerquestionnairequestions);
                    _providerquestionnairequestionsRepository.SaveChanges();
                    response = new JsonModel(providerquestionnairequestions, StatusMessage.ProviderQuestionnaireQuestionUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.CareCategoryDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        //public JsonModel GetQuestionnaireQuestionsList(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        //{
        //    List<QuestionpaginationModel> insuranceCompanyModel = _providerquestionnairequestionsRepository.GetQuestionnaireQuestionsList<QuestionpaginationModel>(searchFilterModel, tokenModel).ToList();
        //    if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
        //    {
        //        response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
        //        response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
        //    }
        //    return response;
        //}

        public JsonModel GetQuestionnaireQuestionsList(ProviderQuestionnaireFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<QuestionpaginationModel> insuranceCompanyModel = _providerquestionnairequestionsRepository.GetQuestionnaireQuestionsList<QuestionpaginationModel>(searchFilterModel, tokenModel).ToList();
            if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
            {
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetQuestionById(int id, TokenModel tokenModel)
        {
            ProviderQuestionnaireQuestions questionnaireQuestions = _providerquestionnairequestionsRepository.GetByID(id);
            if (questionnaireQuestions != null)
            {
                ProviderQuestionnaireModel questionnaireQuestionsmodel = new ProviderQuestionnaireModel();
                AutoMapper.Mapper.Map(questionnaireQuestions, questionnaireQuestionsmodel);
                response = new JsonModel(questionnaireQuestionsmodel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel DeleteQuestion(int id, TokenModel tokenModel)
        {

            ProviderQuestionnaireQuestions questionnairequestions = _providerquestionnairequestionsRepository.Get(a => a.QuestionId == id && a.IsActive == true && a.IsDeleted == false);
            if (questionnairequestions != null)
            {
                questionnairequestions.IsDeleted = true;
                questionnairequestions.DeletedBy = tokenModel.UserID;
                questionnairequestions.DeletedDate = DateTime.UtcNow;
                _providerquestionnairequestionsRepository.Update(questionnairequestions);
                _providerquestionnairequestionsRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CareCategoryDeleted, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.CareCategoryDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }


        public JsonModel SaveQuestionOptions(QuestionOptionsModel questionoptionsmodel, TokenModel tokenModel)
        {
            QuestionnaireOptions questionnaireOptions = null;
            foreach (var item in questionoptionsmodel.questionnaireoptions)
            {
                if (item.OptionId > 0)
                {
                    questionnaireOptions = _providerquestionnaireoptionsRepository.Get(a => a.OptionId == item.OptionId && a.IsActive == true && a.IsDeleted == false);
                    if (questionnaireOptions != null)
                    {
                        AutoMapper.Mapper.Map(questionoptionsmodel, questionnaireOptions);
                        questionnaireOptions.UpdatedBy = tokenModel.UserID;
                        questionnaireOptions.UpdatedDate = DateTime.UtcNow;
                        questionnaireOptions.QuestionId = item.QuestionId;
                        questionnaireOptions.QuestionOptionName = item.QuestionOptionName;
                        questionnaireOptions.OptionId = item.OptionId;
                        _providerquestionnaireoptionsRepository.Update(questionnaireOptions);
                        _providerquestionnaireoptionsRepository.SaveChanges();
                        response = new JsonModel(questionnaireOptions, StatusMessage.ProviderQuestionnaireQuestionoptionUpdated, (int)HttpStatusCode.OK);
                    }
                    else
                    {
                        response = new JsonModel(null, StatusMessage.KeywordDoesNotExist, (int)HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    questionnaireOptions = new QuestionnaireOptions();
                    AutoMapper.Mapper.Map(questionoptionsmodel, questionnaireOptions);
                    questionnaireOptions.CreatedBy = tokenModel.UserID;
                    questionnaireOptions.CreatedDate = DateTime.UtcNow;
                    questionnaireOptions.IsDeleted = false;
                    questionnaireOptions.IsActive = true;
                    //healthcareKeywords.ProviderCareCategory = keywordmodel.Id;
                    questionnaireOptions.QuestionOptionName = item.QuestionOptionName;
                    questionnaireOptions.QuestionId = item.QuestionId;
                    questionnaireOptions.OptionId = item.OptionId;
                    //healthcareKeywords.OrganizationID = tokenModel.OrganizationID;
                    _providerquestionnaireoptionsRepository.Create(questionnaireOptions);
                    _providerquestionnaireoptionsRepository.SaveChanges();
                    response = new JsonModel(questionnaireOptions, StatusMessage.ProviderQuestionnaireQuestionoptionCreated, (int)HttpStatusCode.OK);
                }
            }

            return response;
        }

        public JsonModel GetQuestionOptionDataById(int id, TokenModel tokenModel)
        {
            QuestionOptionsModel responsekeywordObj = new QuestionOptionsModel();
            List<QuestionOptionsModel> responsekeywordObjList = new List<QuestionOptionsModel>();
            responsekeywordObj.questionnaireoptions = new List<QuestionnaireQuestionOptionsModel>();

            if (id > 0)
            {
                QuestionnaireOptions keyHisObj = _providerquestionnaireoptionsRepository.Get(a => a.QuestionId == id && a.IsActive == true && a.IsDeleted == false);


                if (keyHisObj != null)
                {
                    keyHisObj.QuestionId = id;
                    List<QuestionnaireOptions> patMedFamHisDBObj = _providerquestionnaireoptionsRepository.GetAll(a => a.QuestionId == id && a.IsActive == true && a.IsDeleted == false).ToList();
                    AutoMapper.Mapper.Map(keyHisObj, responsekeywordObj);
                    AutoMapper.Mapper.Map(patMedFamHisDBObj, responsekeywordObj.questionnaireoptions);
                    return new JsonModel()
                    {
                        data = responsekeywordObj,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                else
                {
                    responsekeywordObj.QuestionId = id;
                    return new JsonModel()
                    {

                        data = responsekeywordObj,
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound//Success
                    };
                }
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//Success
                };
            }
        }


        public JsonModel AddQuestionnaire(ManageQuestionnaireModel managequestionnairemodel, TokenModel tokenModel)
        {
            ProviderQuestionnaires providerquestionnaires = null;
            if (managequestionnairemodel.QuestionnareId == 0)
            {
                providerquestionnaires = new ProviderQuestionnaires();
                AutoMapper.Mapper.Map(managequestionnairemodel, providerquestionnaires);
                providerquestionnaires.CreatedBy = tokenModel.UserID;
                providerquestionnaires.CreatedDate = DateTime.UtcNow;
                providerquestionnaires.IsDeleted = false;
                providerquestionnaires.IsActive = true;
                providerquestionnaires.OrganizationId = tokenModel.OrganizationID;
                //providerquestionnairequestions.QuestionNameName = providerquestionnairemodel.QuestionName;
                _questionnaireRepository.Create(providerquestionnaires);
                _questionnaireRepository.SaveChanges();
                response = new JsonModel(providerquestionnaires, StatusMessage.ProviderQuestionnaireCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                providerquestionnaires = _questionnaireRepository.Get(a => a.QuestionnareId == managequestionnairemodel.QuestionnareId && a.IsActive == true && a.IsDeleted == false);
                if (providerquestionnaires != null)
                {
                    AutoMapper.Mapper.Map(managequestionnairemodel, providerquestionnaires);
                    providerquestionnaires.UpdatedBy = tokenModel.UserID;
                    providerquestionnaires.UpdatedDate = DateTime.UtcNow;
                    _questionnaireRepository.Update(providerquestionnaires);
                    _questionnaireRepository.SaveChanges();
                    response = new JsonModel(providerquestionnaires, StatusMessage.ProviderQuestionnaireUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.ProviderQuestionnaireDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }


        public JsonModel GetQuestionnaireList(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<QuestionnairepaginationModel> insuranceCompanyModel = _questionnaireRepository.GetQuestionnaireList<QuestionnairepaginationModel>(searchFilterModel, tokenModel).ToList();
            if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
            {
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetQuestionnaireById(int id, TokenModel tokenModel)
        {
            ProviderQuestionnaires questionnaires = _questionnaireRepository.GetByID(id);
            if (questionnaires != null)
            {
                ManageQuestionnaireModel managequestionnairesmodel = new ManageQuestionnaireModel();
                AutoMapper.Mapper.Map(questionnaires, managequestionnairesmodel);
                response = new JsonModel(managequestionnairesmodel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel DeleteQuestionnaire(int id, TokenModel tokenModel)
        {

            ProviderQuestionnaires questionnaires = _questionnaireRepository.Get(a => a.QuestionnareId == id && a.IsActive == true && a.IsDeleted == false);
            if (questionnaires != null)
            {
                questionnaires.IsDeleted = true;
                questionnaires.DeletedBy = tokenModel.UserID;
                questionnaires.DeletedDate = DateTime.UtcNow;
                _questionnaireRepository.Update(questionnaires);
                _questionnaireRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.ProviderQuestionnaireDeleted, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.ProviderQuestionnaireDoesNotExist, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }
    }

   
}
