using HC.Common;
using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.Eligibility;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientEncounters;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;
using System.Text;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Organizations;
using System.Threading.Tasks;

namespace HC.Patient.Service.Services.PatientEncounters
{
    public class PatientEncounterService : BaseService, IPatientEncounterService
    {
        private HCOrganizationContext _context;
        private IPatientEncounterRepository _patientEncounterRepository;
        private ISoapNoteRepository _soapNoteRepository;
        private IPatientEncounterServiceCodesRepository _patientEncounterServiceCodesRepository;
        private IPatientEncounterICDCodesRepository _patientEncounterICDCodesRepository;
        private IPatientEncounterCodesMappingsRepository _patientEncounterCodesMappingsRepository;
        private IAppointmentRepository _appointmentRepository;
        private IPatientRepository _patientRepository;
        private IAuditLogRepository _auditLogRepository;
        private IPatientAuthorizationProcedureCPTLinkRepository _patientAuthorizationProcedureCPTLinkRepository;
        private IUserDriveTimeRepository _userDriveTimeRepository;
        private IUserDetailedDriveTimeRepository _userDetailedDriveTimeRepository;
        private IImageService _imageService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly ILocationService _locationService;
        private IPatientEncounterTemplateRepository _patientEncounterTemplateRepository;
        private IPatientEncounterChecklistRepository _patientEncounterChecklistRepository;
        private IMasterEncounterChecklistRepository _masterEncounterChecklistRepository;
        private IMasterEncChecklistReviewItemsRepository _masterEncChecklistReviewItemsRepository;
        private IEncounterProgramRepository _encounterProgramRepository;
        private IPatientEncounterCurrentMedicationDetailsRepository _patientEncounterCurrentMedicationDetailsRepository;
        private IEmailService _emailService;
        private readonly IHostingEnvironment _env;
        private readonly IChatRepository _chatRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITrackEncounterAddUpdateClickLogsRepository _trackEncounterAddUpdateClickLogsRepository;
        private Nullable<int> pId;
        private readonly IPatientService _IPatientService;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private IPatientEncounterNotesRepository _patientEncounterNotesRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailSender;
        private readonly IPatientInsuranceRepository _patientInsuranceRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IOrganizationService _organizationService;
        public PatientEncounterService(IPatientService patientService, IHostingEnvironment env, HCOrganizationContext context, IPatientEncounterRepository patientEncounterRepository, ISoapNoteRepository soapNoteRepository,
            IPatientEncounterServiceCodesRepository patientEncounterServiceCodesRepository, IPatientEncounterICDCodesRepository patientEncounterICDCodesRepository,
            IPatientEncounterCodesMappingsRepository patientEncounterCodesMappingsRepository, IPatientRepository patientRepository, IAppointmentRepository appointmentRepository
            , IAuditLogRepository auditLogRepository, IPatientAuthorizationProcedureCPTLinkRepository patientAuthorizationProcedureCPTLinkRepository, IImageService imageService, IUserDriveTimeRepository userDriveTimeRepository, IUserDetailedDriveTimeRepository userDetailedDriveTimeRepository, ILocationService locationService,
            IPatientEncounterTemplateRepository patientEncounterTemplateRepository, IPatientEncounterChecklistRepository patientEncounterChecklistRepository,
            IMasterEncounterChecklistRepository masterEncounterChecklistRepository, IOrganizationSMTPRepository organizationSMTPRepository, IMasterEncChecklistReviewItemsRepository masterEncChecklistReviewItemsRepository, IEncounterProgramRepository encounterProgramRepository, IPatientEncounterCurrentMedicationDetailsRepository patientEncounterCurrentMedicationDetailsRepository, IEmailService emailService,
            IChatRepository chatRepository,
            ITokenRepository tokenRepository
            , ITrackEncounterAddUpdateClickLogsRepository trackEncounterAddUpdateClickLogsRepository
            , IPatientEncounterNotesRepository patientEncounterNotesRepository
            , IEmailService emailSender, IEmailWriteService emailWriteService, IConfiguration configuration,
            ITokenService tokenService, IPatientInsuranceRepository patientInsuranceRepository,
            IGlobalCodeService globalCodeService, IOrganizationService organizationService
          )
        {
            _context = context;
            _patientEncounterRepository = patientEncounterRepository;
            _soapNoteRepository = soapNoteRepository;
            _patientEncounterServiceCodesRepository = patientEncounterServiceCodesRepository;
            _patientEncounterICDCodesRepository = patientEncounterICDCodesRepository;
            _patientEncounterCodesMappingsRepository = patientEncounterCodesMappingsRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _auditLogRepository = auditLogRepository;
            _patientAuthorizationProcedureCPTLinkRepository = patientAuthorizationProcedureCPTLinkRepository;
            _imageService = imageService;
            _userDriveTimeRepository = userDriveTimeRepository;
            _userDetailedDriveTimeRepository = userDetailedDriveTimeRepository;
            _locationService = locationService;
            _patientEncounterTemplateRepository = patientEncounterTemplateRepository;
            _patientEncounterChecklistRepository = patientEncounterChecklistRepository;
            _masterEncounterChecklistRepository = masterEncounterChecklistRepository;
            _masterEncChecklistReviewItemsRepository = masterEncChecklistReviewItemsRepository;
            _encounterProgramRepository = encounterProgramRepository;
            _patientEncounterCurrentMedicationDetailsRepository = patientEncounterCurrentMedicationDetailsRepository;
            _organizationSMTPRepository = organizationSMTPRepository;
            _emailService = emailService;
            _env = env;
            _chatRepository = chatRepository;
            _tokenRepository = tokenRepository;
            _trackEncounterAddUpdateClickLogsRepository = trackEncounterAddUpdateClickLogsRepository;
            _IPatientService = patientService;
            _patientEncounterNotesRepository = patientEncounterNotesRepository;
            _emailSender = emailSender;
            _configuration = configuration;
            _emailWriteService = emailWriteService;
            _tokenService = tokenService;
            _patientInsuranceRepository = patientInsuranceRepository;
            _globalCodeService = globalCodeService;
            _organizationService = organizationService;
        }

        public JsonModel GetPatientNonBillableEncounterDetails(int appointmentId, int encounterId, bool isAdmin, TokenModel token)
        {
            PatientEncounterModel response = new PatientEncounterModel();
            PatientAppointment pat = new PatientAppointment();
            LocationModel locationModal = new LocationModel();

            if (appointmentId > 0)
            {
                pat = _appointmentRepository.GetByID(appointmentId);
                locationModal = _locationService.GetLocationOffsets(pat.ServiceLocationID, token);
            }
            else
            {
                PatientEncounter pe = _patientEncounterRepository.GetByID(encounterId);
                locationModal = _locationService.GetLocationOffsets(pe.ServiceLocationID, token);
            }

            //PatientAppointment pat = _appointmentRepository.GetByID(appointmentId);
            //LocationModel locationModal = GetLocationOffsets(pat.ServiceLocationID);
            if (isAdmin && encounterId > 0 && appointmentId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, token);
                GetAppointmentDetail(locationModal, response, appointmentId, token);
            }
            else if (encounterId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, token);
                GetAppointmentDetail(locationModal, response, appointmentId, token);
            }
            else
            {
                if (appointmentId > 0)
                    GetAppointmentDetail(locationModal, response, appointmentId, token);
            }

            if (response != null && response.StartDateTime != null && response.EndDateTime != null)
            {
                response.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                response.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
            }

            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
            };
        }

        private void GetAppointmentDetail(LocationModel locationModal, PatientEncounterModel response, int appointmentId, TokenModel token)
        {
            response.PatientAppointment = _appointmentRepository.GetAppointmentDetails<PatientAppointmentModel>(appointmentId).FirstOrDefault();
            if (response.PatientAppointment != null)
            {
                response.PatientAppointment.AppointmentStaffs = !string.IsNullOrEmpty(response.PatientAppointment.XmlString) ? XDocument.Parse(response.PatientAppointment.XmlString).Descendants("Child").Select(y => new AppointmentStaffs()
                {
                    StaffId = Convert.ToInt32(y.Element("StaffId").Value),
                    StaffName = y.Element("StaffName").Value,
                }).ToList() : new List<AppointmentStaffs>(); response.PatientAppointment.XmlString = null;

                // for appointment programs
                //response.PatientAppointment.ProgramTypeIds = !string.IsNullOrEmpty(response.PatientAppointment.XmlProgramString) ? XDocument.Parse(response.PatientAppointment.XmlProgramString).Descendants("Child").Select(y => new AppointmentProgramsModel()
                //{
                //    ProgramTypeId = Convert.ToInt32(y.Element("ProgramId").Value),
                //    ProgramName = y.Element("Description").Value,
                //}).ToList() : new List<AppointmentProgramsModel>(); response.PatientAppointment.XmlProgramString = null;

                response.PatientAppointment.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.PatientAppointment.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                response.PatientAppointment.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.PatientAppointment.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
            }
        }

        public JsonModel GetPatientEncounterDetails(int appointmentId, int encounterId, bool isAdmin, TokenModel token)
        {
            PatientEncounterModel response = new PatientEncounterModel();
            PatientAppointment pat = new PatientAppointment();
            LocationModel locationModal = new LocationModel();

            //bool isAuthMandatory = _patientRepository.CheckAuthorizationSetting();

            if (appointmentId > 0)
            {
                pat = _appointmentRepository.GetByID(appointmentId);
                locationModal = _locationService.GetLocationOffsets(pat.ServiceLocationID, token);
            }
            //else
            //{
            //    PatientEncounter pe = _patientEncounterRepository.GetByID(encounterId);
            //    locationModal = GetLocationOffsets(pe.ServiceLocationID);
            //}

            if (isAdmin && encounterId > 0 && appointmentId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, token);
                response.PatientEncounterServiceCodes = new List<PatientEncounterServiceCodesModel>();
                //response.PatientEncounterServiceCodes = _patientEncounterRepository.GetServiceCodeForEncounterByAppointmentType<PatientEncounterServiceCodesModel>(appointmentId).ToList();
            }
            else if (encounterId > 0)
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, token);

            else
            {
                response.PatientEncounterServiceCodes = new List<PatientEncounterServiceCodesModel>();
                response.PatientEncounterChecklistModel = _masterEncounterChecklistRepository.GetAll()
                    .Where(x => x.OrganizationID == token.OrganizationID && x.IsActive == true && x.IsDeleted == false)
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new PatientEncounterChecklistModel
                    {
                        Id = 0,
                        PatientEncounterId = 0,
                        MasterEncounterChecklistId = x.ID,
                        Name = x.Name,
                        NavigationLink = x.Screens.NavigationLink,
                        IsAdministrativeType = x.IsAdministrativeType
                    }).ToList();
                response.EncounterChecklistReviewItems = _masterEncChecklistReviewItemsRepository.GetAll().Where(y => y.IsActive == true && y.IsDeleted == false)
                    .Select(x => new EncounterChecklistReviewItems
                    {
                        Id = x.Id,
                        MasterEncounterChecklistId = x.MasterEncounterChecklistId,
                        ItemName = x.ItemName,
                    }).ToList();
                response.EncounterChangeHistory = new List<EncounterChangeHistory>();
                //response.PatientEncounterServiceCodes = _patientEncounterRepository.GetServiceCodeForEncounterByAppointmentType<PatientEncounterServiceCodesModel>(appointmentId).ToList();
                //int? patientId = pat.PatientID;// _appointmentRepository.GetByID(appointmentId).PatientID;
                //if (patientId != null && patientId > 0)
                //    response.PatientEncounterDiagnosisCodes = _patientRepository.GetPatientDiagnosisCodes<PatientEncounterDiagnosisCodesModel>(Convert.ToInt32(patientId)).ToList();
            }
            if (appointmentId > 0)
                GetAppointmentDetail(locationModal, response, appointmentId, token);
            if (response.StartDateTime != null && response.EndDateTime != null && response.ServiceLocationID != null && (!response.IsImported || response.IsImportUpdated))
            {
                locationModal = _locationService.GetLocationOffsets(response.ServiceLocationID, token);
                response.DateOfService = CommonMethods.ConvertFromUtcTimeWithOffset(response.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                response.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                response.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
            }
            
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
            };
        }
        public JsonModel SavePatientNonBillableEncounter(PatientEncounterModel requestObj, bool isAdmin, TokenModel token)
        {
            pId = requestObj.PatientID;
            PatientEncounter patientEncounter = null;
            int? appointmentTypeId = _appointmentRepository.GetByID(requestObj.PatientAppointmentId).AppointmentTypeID;
            LocationModel locationModal = _locationService.GetLocationOffsets(requestObj.ServiceLocationID, token);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (requestObj.Id == 0)
                    {
                        patientEncounter = new PatientEncounter();
                        AutoMapper.Mapper.Map(requestObj, patientEncounter);
                        patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        patientEncounter.CreatedBy = token.UserID;
                        patientEncounter.OrganizationID = token.OrganizationID;
                        patientEncounter.CreatedDate = DateTime.UtcNow;
                        patientEncounter.IsActive = true;
                        patientEncounter.IsDeleted = false;
                        patientEncounter.IsBillableEncounter = Convert.ToBoolean(requestObj.IsBillableEncounter);
                        patientEncounter.NonBillableNotes = requestObj.NonBillableNotes;
                        //mark encounter rendered
                        patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                        //
                        _patientEncounterRepository.Create(patientEncounter);
                        //  _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Create, pId, token.UserID, null, token, null, null);


                        _patientEncounterRepository.SaveChanges();

                        requestObj.EncounterSignature.ForEach(a => { a.PatientEncounterId = patientEncounter.Id; });
                        SaveSignature(requestObj.EncounterSignature);

                    }
                    else
                    {
                        patientEncounter = _patientEncounterRepository.Get(x => x.Id == requestObj.Id && x.IsActive == true && x.IsDeleted == false);
                        if (!ReferenceEquals(patientEncounter, null))
                        {
                            //Update patient encounter
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            //patientEncounter.ClinicianSign = requestObj.ClinicianSign;
                            //patientEncounter.ClinicianSignDate = requestObj.ClinicianSignDate;
                            patientEncounter.IsBillableEncounter = Convert.ToBoolean(requestObj.IsBillableEncounter);
                            patientEncounter.NonBillableNotes = requestObj.NonBillableNotes;
                            patientEncounter.AppointmentTypeId = appointmentTypeId;
                            patientEncounter.CustomAddressID = requestObj.CustomAddressID;
                            patientEncounter.CustomAddress = requestObj.CustomAddress;
                            patientEncounter.PatientAddressID = requestObj.PatientAddressID;
                            patientEncounter.OfficeAddressID = requestObj.OfficeAddressID;
                            patientEncounter.ServiceLocationID = requestObj.ServiceLocationID;
                            patientEncounter.StaffID = requestObj.StaffID;
                            //TODO
                            //_imageService.SaveImages(requestObj.ClinicianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.ClinicianSign.ToString());
                            //_imageService.SaveImages(requestObj.GuardianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.GuardianSign.ToString());
                            //_imageService.SaveImages(requestObj.PatientSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.PatientSign.ToString());


                            ////mark encounter rendered
                            //patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                            //
                            patientEncounter.UpdatedBy = token.UserID;
                            patientEncounter.UpdatedDate = DateTime.UtcNow;
                            //   _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token, null, null);
                            _patientRepository.SaveChanges();
                            SaveSignature(requestObj.EncounterSignature);
                        }
                    }
                    SaveTimesheetData(token, patientEncounter);
                    SaveDriveTimeData(token, patientEncounter);
                    transaction.Commit();
                    return new JsonModel()
                    {
                        data = patientEncounter,
                        Message = Common.HC.Common.StatusMessage.SoapSuccess,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }

        }
        public JsonModel SavePatientEncounter(PatientEncounterModel requestObj, bool isAdmin, TokenModel token)
        {
            pId = requestObj.PatientID;
            PatientEncounter patientEncounter = null;
            int? appointmentTypeId = null;
            if (requestObj.PatientAppointmentId != null && requestObj.PatientAppointmentId > 0)
            {
                appointmentTypeId = _appointmentRepository.GetByID(requestObj.PatientAppointmentId).AppointmentTypeID;
            }
            List<PatientEncounterChecklist> patientEncounterChecklists;
            List<PatientEncounterProgram> patientEncounterPrograms = new List<PatientEncounterProgram>();
            LocationModel locationModal = _locationService.GetLocationOffsets(requestObj.ServiceLocationID, token);
            List<PatientEncounterCurrentMedicationDetails> patientEncounterCurrentMedicationDetails = new List<PatientEncounterCurrentMedicationDetails>();
            List<PatientEncounterServiceCodes> serviceCodesList = new List<PatientEncounterServiceCodes>();
            List<PatientEncounterCodesMapping> codesMappingList = new List<PatientEncounterCodesMapping>();
            List<PatientEncounterServiceCodes> serviceCodesInsertList = null;
            PatientEncounterServiceCodes serviceCodeObj = new PatientEncounterServiceCodes();
            PatientEncounterCodesMapping CodesMapping = new PatientEncounterCodesMapping();

            //  List<PatientCurrentMedication> patientCurrentMedicationlist = new List<PatientCurrentMedication>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    patientEncounterChecklists = new List<PatientEncounterChecklist>();
                    AutoMapper.Mapper.Map(requestObj.PatientEncounterChecklistModel, patientEncounterChecklists);
                    AutoMapper.Mapper.Map(requestObj.PatientEncounterServiceCodes, serviceCodesList);
                    if (requestObj.Id == 0)
                    {
                        patientEncounter = new PatientEncounter();
                        AutoMapper.Mapper.Map(requestObj, patientEncounter);
                        if (!(requestObj.PatientAppointmentId > 0))
                        {
                            patientEncounter.DateOfService = CommonMethods.ConvertUtcTime(patientEncounter.DateOfService, token);
                            patientEncounter.StartDateTime = CommonMethods.ConvertUtcTime(patientEncounter.StartDateTime, token);
                            patientEncounter.EndDateTime = CommonMethods.ConvertUtcTime(patientEncounter.EndDateTime, token);
                        }
                        else
                        {
                            patientEncounter.DateOfService = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.DateOfService, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        }
                        patientEncounter.CreatedBy = token.UserID;
                        patientEncounter.OrganizationID = token.OrganizationID;
                        patientEncounter.CreatedDate = DateTime.UtcNow;
                        patientEncounter.IsActive = requestObj.IsActive != null ? (bool)requestObj.IsActive : true;
                        patientEncounter.IsDeleted = false;
                        //mark encounter rendered
                        patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "PENDING" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                        //
                        _patientEncounterRepository.Create(patientEncounter);
                        _context.SaveChanges();
                        // _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Attempt, pId, token.UserID, null, token, null, null);
                        if (patientEncounter.Id > 0)
                        {
                            // save Encounter checklists
                            if (patientEncounterChecklists.Count > 0)
                            {
                                patientEncounterChecklists.ForEach(x =>
                                {
                                    x.PatientEncounterId = patientEncounter.Id;
                                    x.CreatedBy = token.UserID;
                                    x.CreatedDate = DateTime.UtcNow;
                                    x.IsActive = true;
                                    x.IsDeleted = false;
                                });
                                _patientEncounterChecklistRepository.Create(patientEncounterChecklists.ToArray());
                                _patientEncounterChecklistRepository.SaveChanges();
                            }

                            if (requestObj.ProgramTypeIds != null && requestObj.ProgramTypeIds.Count > 0)
                            {
                                patientEncounterPrograms = requestObj.ProgramTypeIds.Select(x => new PatientEncounterProgram() { ProgramId = x.ProgramTypeId, PatientEncounterId = patientEncounter.Id, CreatedBy = token.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList();
                                _encounterProgramRepository.Create(patientEncounterPrograms.ToArray());
                                _encounterProgramRepository.SaveChanges();
                            }

                            //Save Encounter Service Codes
                            if (serviceCodesList != null && serviceCodesList.Count > 0)
                            {
                                serviceCodesList.ForEach(x =>
                                {
                                    x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.PatientEncounterId = patientEncounter.Id; x.IsActive = true; x.IsDeleted = false;
                                    if (!_patientRepository.CheckAuthorizationSetting())
                                    { x.AuthProcedureCPTLinkId = null; x.AuthorizationNumber = null; }
                                });
                                _patientEncounterServiceCodesRepository.Create(serviceCodesList.ToArray());
                                _patientEncounterServiceCodesRepository.SaveChanges();
                                if (_patientRepository.CheckAuthorizationSetting())
                                    BlockServiceCodeUnits(token, serviceCodesList);
                            }

                            //adding PatientCurrentMedication

                            //    patientCurrentMedicationlist = _patientCurrentMedicationRepository.GetAll(x => x.PatientId == patientEncounter.PatientID && x.IsDeleted == false).ToList();
                            //List<PatientEncounterCurrentMedicationDetails> PatientEncounterCurrentMedicationDetailsList_check = new List<PatientEncounterCurrentMedicationDetails>();
                            //PatientEncounterCurrentMedicationDetailsList_check = _patientEncounterCurrentMedicationDetailsRepository.GetAll(x => x.EncounterId == patientEncounter.Id && x.IsDeleted == false).ToList();
                            //if (PatientEncounterCurrentMedicationDetailsList_check.Count == 0)
                            //{
                            //    patientEncounterCurrentMedicationDetails = patientCurrentMedicationlist.Select(x => new PatientEncounterCurrentMedicationDetails()
                            //    { EncounterId = patientEncounter.Id, PatientMedicationId = x.Id, CreatedBy = token.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList();

                            //    _patientEncounterCurrentMedicationDetailsRepository.Create(patientEncounterCurrentMedicationDetails.ToArray());
                            //    _patientEncounterCurrentMedicationDetailsRepository.SaveChanges();
                            //}
                            //else
                            //{
                            //    foreach (PatientCurrentMedication medicationDetailsobj in patientCurrentMedicationlist)
                            //    {
                            //        var check = PatientEncounterCurrentMedicationDetailsList_check.Where(x => x.PatientMedicationId == medicationDetailsobj.Id).ToList();
                            //        if (check.Count == 0)
                            //        {
                            //            PatientEncounterCurrentMedicationDetailsList_check.Add(new PatientEncounterCurrentMedicationDetails()
                            //            {
                            //                EncounterId = patientEncounter.Id,
                            //                PatientMedicationId = medicationDetailsobj.Id,
                            //                CreatedBy = token.UserID,
                            //                CreatedDate = DateTime.UtcNow,
                            //                IsActive = true,
                            //                IsDeleted = false
                            //            });
                            //        }
                            //    }
                            //    if (PatientEncounterCurrentMedicationDetailsList_check.Any(x => x.Id == 0))
                            //    {
                            //        _patientEncounterCurrentMedicationDetailsRepository.Create(PatientEncounterCurrentMedicationDetailsList_check.Where(x => x.Id == 0).ToArray());
                            //    }

                            //    _patientEncounterCurrentMedicationDetailsRepository.SaveChanges();
                            //}

                        }
                    }
                    else
                    {
                        patientEncounter = _patientEncounterRepository.Get(x => x.Id == requestObj.Id && x.IsDeleted == false);
                        if (!ReferenceEquals(patientEncounter, null))
                        {
                            //Update patient encounter
                            patientEncounter.DateOfService = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            //patientEncounter.ClinicianSign = requestObj.ClinicianSign;
                            //patientEncounter.ClinicianSignDate = requestObj.ClinicianSignDate;
                            //patientEncounter.PatientSign = requestObj.PatientSign;
                            //patientEncounter.PatientSignDate = requestObj.PatientSignDate;
                            //patientEncounter.GuardianSign = requestObj.GuardianSign;
                            //patientEncounter.GuardianSignDate = requestObj.GuardianSignDate;
                            //patientEncounter.GuardianName = requestObj.GuardianName;
                            if (appointmentTypeId > 0)
                                patientEncounter.AppointmentTypeId = appointmentTypeId;
                            patientEncounter.CustomAddressID = requestObj.CustomAddressID;
                            patientEncounter.CustomAddress = requestObj.CustomAddress;
                            patientEncounter.PatientAddressID = requestObj.PatientAddressID;
                            patientEncounter.OfficeAddressID = requestObj.OfficeAddressID;
                            patientEncounter.ServiceLocationID = requestObj.ServiceLocationID;
                            patientEncounter.StaffID = requestObj.StaffID;
                            patientEncounter.Notes = requestObj.Notes;
                            patientEncounter.ManualChiefComplaint = requestObj.ManualChiefComplaint;
                            patientEncounter.EncounterTypeId = requestObj.EncounterTypeId;
                            patientEncounter.EncounterMethodId = requestObj.EncounterMethodId;
                            patientEncounter.MemberNotes = requestObj.MemberNotes;
                            patientEncounter.IsPatientEligible = requestObj.IsPatientEligible;

                            //TODO
                            //_imageService.SaveImages(requestObj.ClinicianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.ClinicianSign.ToString());
                            //_imageService.SaveImages(requestObj.GuardianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.GuardianSign.ToString());
                            //_imageService.SaveImages(requestObj.PatientSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.PatientSign.ToString());


                            //mark encounter rendered if isCompleted = true
                            if (requestObj.isCompleted)
                                patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                            //
                            patientEncounter.IsActive = true;
                            patientEncounter.UpdatedBy = token.UserID;
                            patientEncounter.UpdatedDate = DateTime.UtcNow;
                            if (patientEncounter.IsImported)
                                patientEncounter.IsImportUpdated = true;
                            _patientEncounterRepository.Update(patientEncounter);
                            _patientEncounterRepository.SaveChanges();
                            // _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token, null, null);

                            // update Encounter checklists
                            List<PatientEncounterChecklist> savedPatientEncounterChecklist = _patientEncounterChecklistRepository.GetAll().Where(x => x.PatientEncounterId == patientEncounter.Id && x.IsActive == true && x.IsDeleted == false).ToList();
                            //savedPatientEncounterChecklist = savedPatientEncounterChecklist.Where(x => !patientEncounterChecklists.Any(p => p.ID == x.ID)).ToList();
                            if (savedPatientEncounterChecklist.Count > 0)
                            {
                                savedPatientEncounterChecklist.ForEach(x =>
                                {
                                    PatientEncounterChecklist encounterChecklist = patientEncounterChecklists.Find(y => y.ID == x.ID);
                                    if (ReferenceEquals(encounterChecklist, null))
                                    {
                                        x.DeletedBy = token.UserID;
                                        x.DeletedDate = DateTime.UtcNow;
                                        x.IsActive = false;
                                        x.IsDeleted = true;
                                    }
                                    else
                                    {
                                        x.Notes = encounterChecklist.Notes;
                                    }
                                });
                                _patientEncounterChecklistRepository.Update(savedPatientEncounterChecklist.ToArray());
                            }
                            patientEncounterChecklists = patientEncounterChecklists.Where(x => x.ID == 0).ToList();
                            if (patientEncounterChecklists.Count > 0)
                            {
                                patientEncounterChecklists.ForEach(x =>
                                {
                                    x.PatientEncounterId = patientEncounter.Id;
                                    x.CreatedBy = token.UserID;
                                    x.CreatedDate = DateTime.UtcNow;
                                    x.IsActive = true;
                                    x.IsDeleted = false;
                                });
                                _patientEncounterChecklistRepository.Create(patientEncounterChecklists.ToArray());
                            }
                            if (savedPatientEncounterChecklist.Count > 0 || patientEncounterChecklists.Count > 0)
                                _patientEncounterChecklistRepository.SaveChanges();


                            // Encounter programs --
                            patientEncounterPrograms = _encounterProgramRepository.GetAll().Where(x => x.PatientEncounterId == patientEncounter.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                            patientEncounterPrograms.ForEach(x => x.IsDeleted = true);
                            foreach (EncounterProgramsModel programObj in requestObj.ProgramTypeIds)
                            {
                                if (patientEncounterPrograms.Any(x => x.ProgramId == programObj.ProgramTypeId))
                                {
                                    patientEncounterPrograms.Find(x => x.ProgramId == programObj.ProgramTypeId).IsDeleted = false;
                                }
                                else
                                {
                                    if (programObj.ProgramTypeId > 0)
                                        patientEncounterPrograms.Add(new PatientEncounterProgram() { ProgramId = programObj.ProgramTypeId, PatientEncounterId = patientEncounter.Id, CreatedBy = token.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false });
                                }
                            }
                            if (patientEncounterPrograms.Count() > 0)
                            {
                                if (patientEncounterPrograms.Any(x => x.Id == 0))
                                {
                                    _encounterProgramRepository.Create(patientEncounterPrograms.Where(x => x.Id == 0).ToArray());
                                }
                                else
                                {
                                    _encounterProgramRepository.Update(patientEncounterPrograms.Where(x => x.Id > 0).ToArray());
                                }
                                _encounterProgramRepository.SaveChanges();
                            }

                            //Update Service codes
                            if (requestObj.PatientEncounterServiceCodes != null)
                            {
                                if (isAdmin)
                                {
                                    UpdatePreviousServiceCodesAndMappings(requestObj, token);
                                }
                                //List to insert new cptcodes
                                serviceCodesInsertList = new List<PatientEncounterServiceCodes>();
                                serviceCodesList = _patientEncounterServiceCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterServiceCodesModel serviceCodeModel in requestObj.PatientEncounterServiceCodes)
                                {
                                    if (serviceCodeModel.Id > 0)
                                    {
                                        //Update service codes if exist in Soap
                                        serviceCodeObj = serviceCodesList.Find(x => x.Id == serviceCodeModel.Id);
                                        if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == true)
                                        {
                                            serviceCodeObj.IsDeleted = serviceCodeModel.IsDeleted;
                                            serviceCodeObj.DeletedDate = DateTime.UtcNow;
                                            serviceCodeObj.DeletedBy = token.UserID;
                                        }
                                        else if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == false)
                                        {
                                            serviceCodeObj.Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1;
                                            serviceCodeObj.Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2;
                                            serviceCodeObj.Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3;
                                            serviceCodeObj.Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4;
                                            serviceCodeObj.UpdatedBy = token.UserID;
                                            serviceCodeObj.UpdatedDate = DateTime.UtcNow;
                                        }
                                    }
                                    else
                                    {
                                        //Insert new cptcodes to soap
                                        serviceCodeObj = new PatientEncounterServiceCodes()
                                        {
                                            ServiceCodeId = serviceCodeModel.ServiceCodeId,
                                            Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1,
                                            Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2,
                                            Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3,
                                            Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4,
                                            AuthorizationNumber = (_patientRepository.CheckAuthorizationSetting() == false || string.IsNullOrEmpty(serviceCodeModel.AuthorizationNumber)) ? null : serviceCodeModel.AuthorizationNumber,
                                            AuthProcedureCPTLinkId = (_patientRepository.CheckAuthorizationSetting() == false || serviceCodeModel.AuthProcedureCPTLinkId == null || serviceCodeModel.AuthProcedureCPTLinkId == 0) ? null : serviceCodeModel.AuthProcedureCPTLinkId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        serviceCodesInsertList.Add(serviceCodeObj);
                                    }
                                }
                                if (serviceCodesList != null && serviceCodesList.Count() > 0)
                                {
                                    _patientEncounterServiceCodesRepository.Update(serviceCodesList.ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (serviceCodesInsertList != null && serviceCodesInsertList.Count > 0)
                                {
                                    List<PatientEncounterServiceCodes> serviceCodesFinalInsertList = new List<PatientEncounterServiceCodes>();
                                    PatientEncounterServiceCodes serviceCodesExisting = new PatientEncounterServiceCodes();
                                    serviceCodesInsertList.ForEach(s =>
                                    {
                                        serviceCodesExisting = _patientEncounterServiceCodesRepository.Get(x => x.ServiceCodeId == s.ServiceCodeId && x.PatientEncounterId == s.PatientEncounterId && x.IsDeleted == false && x.IsActive == true);
                                        if(serviceCodesExisting == null)
                                        serviceCodesFinalInsertList.Add(s);
                                    });
                                    if(serviceCodesFinalInsertList.Count() > 0)
                                    {
                                        _patientEncounterServiceCodesRepository.Create(serviceCodesFinalInsertList.ToArray());
                                        _patientEncounterServiceCodesRepository.SaveChanges();
                                        //_patientEncounterServiceCodesRepository.Create(serviceCodesInsertList.ToArray());
                                    }
                                    
                                    if (_patientRepository.CheckAuthorizationSetting())
                                        BlockServiceCodeUnits(token, serviceCodesInsertList);
                                }
                            }

                            //adding PatientCurrentMedication
                            if (requestObj.PatientCurrentMedicationModel != null)
                            {
                                if (requestObj.PatientCurrentMedicationModel.Count > 0)
                                {
                                    //    patientCurrentMedicationlist = _patientCurrentMedicationRepository.GetAll(x => x.PatientId == patientEncounter.PatientID && x.IsDeleted == false).ToList();
                                    //List<PatientEncounterCurrentMedicationDetails> PatientEncounterCurrentMedicationDetailsList_check = new List<PatientEncounterCurrentMedicationDetails>();
                                    //PatientEncounterCurrentMedicationDetailsList_check = _patientEncounterCurrentMedicationDetailsRepository.GetAll(x => x.EncounterId == patientEncounter.Id && x.IsDeleted == false).ToList();
                                    //if (PatientEncounterCurrentMedicationDetailsList_check.Count == 0)
                                    //{
                                    //    patientEncounterCurrentMedicationDetails = requestObj.PatientCurrentMedicationModel.Select(x => new PatientEncounterCurrentMedicationDetails()
                                    //    { EncounterId = patientEncounter.Id, PatientMedicationId = x.Id, CreatedBy = token.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList();

                                    //    _patientEncounterCurrentMedicationDetailsRepository.Create(patientEncounterCurrentMedicationDetails.ToArray());
                                    //    _patientEncounterCurrentMedicationDetailsRepository.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    foreach (PatientCurrentMedication medicationDetailsobj in patientCurrentMedicationlist)
                                    //    {
                                    //        var check = PatientEncounterCurrentMedicationDetailsList_check.Where(x => x.PatientMedicationId == medicationDetailsobj.Id).ToList();
                                    //        if (check.Count == 0)
                                    //        {
                                    //            PatientEncounterCurrentMedicationDetailsList_check.Add(new PatientEncounterCurrentMedicationDetails()
                                    //            {
                                    //                EncounterId = patientEncounter.Id,
                                    //                PatientMedicationId = medicationDetailsobj.Id,
                                    //                CreatedBy = token.UserID,
                                    //                CreatedDate = DateTime.UtcNow,
                                    //                IsActive = true,
                                    //                IsDeleted = false
                                    //            });
                                    //        }
                                    //    }
                                    //    if (PatientEncounterCurrentMedicationDetailsList_check.Any(x => x.Id == 0))
                                    //    {
                                    //        _patientEncounterCurrentMedicationDetailsRepository.Create(PatientEncounterCurrentMedicationDetailsList_check.Where(x => x.Id == 0).ToArray());
                                    //    }

                                    //    _patientEncounterCurrentMedicationDetailsRepository.SaveChanges();
                                    //}

                                }
                            }
                        }
                    }
                    transaction.Commit();
                    return new JsonModel()
                    {
                        data = patientEncounter,
                        Message = Common.HC.Common.StatusMessage.SoapSuccess,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }

        private int GetTimesheetStatusId(string status, TokenModel token)
        {
            return _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "timesheetstatus" && a.OrganizationID == token.OrganizationID && a.GlobalCodeName.ToLower() == status.ToLower()).OrderBy(a => a.DisplayOrder).FirstOrDefault().Id;

        }
        private void SaveTimesheetData(TokenModel token, PatientEncounter patientEncounter)
        {
            int? appoinmentTypeId = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new { enc, apt }).Where(x => x.enc.Id == patientEncounter.Id).FirstOrDefault().apt.AppointmentTypeID;
            decimal duration = Convert.ToDecimal(patientEncounter.EndDateTime.Subtract(patientEncounter.StartDateTime).TotalMinutes / 60);
        }
        private void SaveDriveTimeData(TokenModel token, PatientEncounter patientEncounter)
        {
            int? appoinmentTypeId = _context.AppointmentType.Where(x => x.Name.ToLower() == "travel" && x.OrganizationID == token.OrganizationID).First().Id;
            double? lat1 = 0;
            double? long1 = 0;
            double? lat2 = 0;
            double? long2 = 0;
            decimal distance = 0;
            decimal driveTime = 0;

            PatientAppointmentModel apt1 = null, apt2 = null;

            apt2 = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new
            {
                enc,
                apt
            }).Where(y => y.enc.Id == patientEncounter.Id && y.enc.IsDeleted == false && y.apt.IsDeleted == false).Select(z => new PatientAppointmentModel()
            {
                PatientAppointmentId = z.apt.Id,
                StartDateTime = z.apt.StartDateTime,
                EndDateTime = z.apt.EndDateTime,
                Latitude = z.apt.Latitude,
                Longitude = z.apt.Longitude
            }).FirstOrDefault();

            apt1 = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new
            {
                enc,
                apt
            }).Where(y => y.enc.StaffID == patientEncounter.StaffID
                && y.enc.DateOfService.Date == patientEncounter.DateOfService.Date
                && y.enc.IsDeleted == false
                && y.apt.IsDeleted == false
                && y.apt.IsExcludedFromMileage == false
                && y.apt.StartDateTime < apt2.StartDateTime)
            .OrderByDescending(k => k.apt.StartDateTime)
            .Select(z => new PatientAppointmentModel()
            {
                PatientAppointmentId = z.apt.Id,
                StartDateTime = z.apt.StartDateTime,
                EndDateTime = z.apt.EndDateTime,
                Latitude = z.apt.Latitude,
                Longitude = z.apt.Longitude
            }).FirstOrDefault();
            if (apt1 != null && apt1.Latitude != 0 && apt1.Longitude != 0 && apt2 != null && apt2.Latitude != 0 && apt2.Longitude != 0)
            {
                lat2 = apt2.Latitude;
                long2 = apt2.Longitude;
                lat1 = apt1.Latitude;
                long1 = apt1.Longitude;
                GetDriveTimeAndDistance(lat1, long1, lat2, long2, "driving", "en-EN", "metric", ref distance, ref driveTime);
            }
        }

        private void UpdatePreviousServiceCodesAndMappings(PatientEncounterModel requestObj, TokenModel token)
        {
            List<PatientEncounterServiceCodes> deleteList = _patientEncounterServiceCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id).ToList();
            deleteList.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = DateTime.UtcNow; });
            _patientEncounterServiceCodesRepository.Update(deleteList.ToArray());
            _patientEncounterServiceCodesRepository.SaveChanges();
            List<PatientEncounterCodesMapping> deleteMappings = _patientEncounterCodesMappingsRepository.GetAll(x => x.PatientEncounterId == requestObj.Id).ToList();
            deleteMappings.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = DateTime.UtcNow; });
            _patientEncounterCodesMappingsRepository.Update(deleteMappings.ToArray());
            _patientEncounterCodesMappingsRepository.SaveChanges();
        }

        private void BlockServiceCodeUnits(TokenModel token, List<PatientEncounterServiceCodes> serviceCodesList)
        {
            //May be needed in future
            //int[] authds = serviceCodesList.Where(x=>(x.AuthProcedureCPTLinkId!=null && x.AuthProcedureCPTLinkId>0)).Select(y => (int)y.AuthProcedureCPTLinkId).ToArray();
            //List<AuthProcedureCPT> AuthCPT = _patientAuthorizationProcedureCPTLinkRepository.GetAll().Where(c => authds.Contains(c.Id)).ToList();
            //if (AuthCPT != null && AuthCPT.Count > 0)
            //{
            //    AuthCPT.ForEach(z => { z.BlockedUnit = (z.BlockedUnit == null ? 0 : z.BlockedUnit) + 1; z.UpdatedBy = token.UserID; z.UpdatedDate = DateTime.UtcNow; });
            //    _patientAuthorizationProcedureCPTLinkRepository.Update(AuthCPT.ToArray());
            //    _patientAuthorizationProcedureCPTLinkRepository.SaveChanges();
            //}
        }

        private void CreateEncounterCodesMappings(TokenModel token, PatientEncounter patientEncounter, List<PatientEncounterDiagnosisCodes> ICDCodesList, List<PatientEncounterServiceCodes> serviceCodesList, List<PatientEncounterCodesMapping> codesMappingList)
        {
            if (serviceCodesList != null && serviceCodesList.Count > 0 && ICDCodesList != null && ICDCodesList.Count > 0)
            {
                codesMappingList = serviceCodesList.Join(ICDCodesList, SC => SC.PatientEncounter, ICD => ICD.PatientEncounter, (SC, DXCode) => new PatientEncounterCodesMapping
                {
                    PatientEncounterId = patientEncounter.Id,
                    PatientEncounterServiceCodeId = SC.Id,
                    PatientEncounterDiagnosisCodeId = DXCode.Id,
                    IsMapped = true,
                    CreatedBy = token.UserID,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    IsActive = true
                }).Where(x => x.Id == 0).ToList();

                _patientEncounterCodesMappingsRepository.Create(codesMappingList.ToArray());
                _patientEncounterCodesMappingsRepository.SaveChanges();
            }
        }

        public List<PatientEncounterModel> GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", TokenModel token = null)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(token, (int)patientID))
            //{
            //    return new List<PatientEncounterModel>();
            //}
            List<PatientEncounterModel> response = _patientEncounterRepository.GetAllEncounters<PatientEncounterModel>(patientID, appointmentType, staffName, status, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
            if (response != null && response.Count > 0)
                response.ForEach(x =>
                {
                    if (!x.IsImported || x.IsImportUpdated)
                    {
                        LocationModel locationModal = _locationService.GetLocationOffsets(x.ServiceLocationID, token);
                        x.DateOfService = CommonMethods.ConvertFromUtcTimeWithOffset(x.DateOfService, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                    }
                });
            return response;
        }


        public List<PatientEncounterModel> GetAllPatientEncounter(EncounterFilterModel filtermodel, TokenModel token)
        {
            List<PatientEncounterModel> response = _patientEncounterRepository.GetAllPatientEncounters<PatientEncounterModel>(filtermodel, token).ToList();
            if (response != null && response.Count > 0)
                response.ForEach(x =>
                {
                    if (!x.IsImported || x.IsImportUpdated)
                    {
                        LocationModel locationModal = _locationService.GetLocationOffsets(x.ServiceLocationID, token);
                        x.DateOfService = CommonMethods.ConvertFromUtcTimeWithOffset(x.DateOfService, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                    }
                });
            return response;
        }
        public JsonModel TrackEncounterAddUpdateClicks(EncounterClickLogsModel encounterClickLogsModel, TokenModel tokenModel)
        {
            EncounterClickLogs encounterClickLogs = new EncounterClickLogs();
            encounterClickLogs.ClickDateTime = encounterClickLogsModel.ClickDateTime;
            encounterClickLogs.AddEditAction = encounterClickLogsModel.AddEditAction;
            encounterClickLogs.PatientId = encounterClickLogsModel.PatientId;
            encounterClickLogs.PatientEncounterId = encounterClickLogsModel.PatientEncounterId;
            encounterClickLogs.UserId = tokenModel.UserID;
            encounterClickLogs.LocationId = tokenModel.LocationID;
            encounterClickLogs.OrganizationId = tokenModel.OrganizationID;
            encounterClickLogs.CreatedBy = tokenModel.UserID;
            encounterClickLogs.IsDeleted = false;
            encounterClickLogs.CreatedDate = DateTime.UtcNow;
            _trackEncounterAddUpdateClickLogsRepository.Create(encounterClickLogs);
            _trackEncounterAddUpdateClickLogsRepository.SaveChanges();
            return new JsonModel(encounterClickLogs, StatusMessage.EncounterLogOnAddCreated, (int)HttpStatusCode.OK);
        }

        private void CreateAppointment(PatientEncounterModel requestObj, TokenModel token)
        {
            PatientAppointment appointment = _appointmentRepository.Get(x => x.Id == requestObj.ParentAppointmentId && x.IsDeleted == false && x.IsActive == true);
            if (appointment != null)
            {
                PatientAppointment apptReqObj = new PatientAppointment()
                {
                    AppointmentTypeID = appointment.AppointmentTypeID,
                    PatientID = appointment.PatientID,
                    PatientAddressID = appointment.PatientAddressID,
                    StartDateTime = requestObj.AppointmentStartDateTime,
                    EndDateTime = requestObj.AppointmentEndDateTime,
                    CreatedBy = token.UserID,
                    CreatedDate = DateTime.UtcNow,
                    ServiceLocationID = appointment.ServiceLocationID,
                    IsTelehealthAppointment = appointment.IsTelehealthAppointment,
                    IsActive = true,
                    IsDeleted = false,
                    ParentAppointmentID = requestObj.ParentAppointmentId
                };
                _appointmentRepository.Create(apptReqObj);
                _appointmentRepository.SaveChanges();
                requestObj.PatientAppointmentId = apptReqObj.Id;
            }
        }


        public int? GetPatientIdFromEncounterId(int patientEncounterId)
        {
            return _patientEncounterRepository.Get(x => x.Id == patientEncounterId && x.IsActive == true && x.IsDeleted == false).PatientID;
        }

        public JsonModel SavePatientSignForPatientEncounter(int patientEncounterId, PatientEncounterModel patientEncounterModel)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel SaveEncounterSignature(EncounterSignatureModel encounterSignatureModel)
        {
            List<EncounterSignatureModel> encounterSignatureModels = new List<EncounterSignatureModel>();
            encounterSignatureModels.Add(encounterSignatureModel);
            List<EncounterSignature> encounterSignature = null;
            encounterSignature = SaveSignature(encounterSignatureModels);
            if (encounterSignatureModel.Id > 0)
            {
                response = new JsonModel(encounterSignature.FirstOrDefault(), StatusMessage.SignatureUpdated, (int)HttpStatusCode.OK);
            }
            else
            {
                response = new JsonModel(encounterSignature.FirstOrDefault(), StatusMessage.SignatureCreated, (int)HttpStatusCode.OK);
            }
            return response;
        }

        private List<EncounterSignature> SaveSignature(List<EncounterSignatureModel> encounterSignatureModels)
        {
            List<EncounterSignature> encounterSignatures = new List<EncounterSignature>();
            foreach (var encounterSignatureModel in encounterSignatureModels)
            {
                EncounterSignature encounterSignature = null;
                if (encounterSignatureModel.Id > 0)
                {
                    encounterSignature = _context.EncounterSignature.Where(a => a.PatientEncounterId == encounterSignatureModel.PatientEncounterId && a.Id == encounterSignatureModel.Id).FirstOrDefault();
                    encounterSignature.ClinicianSign = encounterSignatureModel.ClinicianSign;
                    encounterSignature.ClinicianSignDate = encounterSignatureModel.ClinicianSignDate;
                    encounterSignature.PatientSign = encounterSignatureModel.PatientSign;
                    encounterSignature.PatientSignDate = encounterSignatureModel.PatientSignDate;
                    encounterSignature.GuardianSign = encounterSignatureModel.GuardianSign;
                    encounterSignature.GuardianSignDate = encounterSignatureModel.GuardianSignDate;
                    encounterSignature.GuardianName = encounterSignatureModel.GuardianName;
                    encounterSignatures.Add(encounterSignature);
                }
                else
                {
                    encounterSignature = new EncounterSignature();
                    encounterSignature.ClinicianSign = encounterSignatureModel.ClinicianSign;
                    encounterSignature.ClinicianSignDate = encounterSignatureModel.ClinicianSignDate;
                    encounterSignature.PatientSign = encounterSignatureModel.PatientSign;
                    encounterSignature.PatientSignDate = encounterSignatureModel.PatientSignDate;
                    encounterSignature.GuardianSign = encounterSignatureModel.GuardianSign;
                    encounterSignature.GuardianSignDate = encounterSignatureModel.GuardianSignDate;
                    encounterSignature.GuardianName = encounterSignatureModel.GuardianName;
                    encounterSignature.PatientId = encounterSignatureModel.PatientId;
                    encounterSignature.StaffId = encounterSignatureModel.StaffId;
                    encounterSignature.PatientEncounterId = encounterSignatureModel.PatientEncounterId;
                    encounterSignatures.Add(encounterSignature);
                }
            }
            _context.UpdateRange(encounterSignatures);
            _context.SaveChanges();

            return encounterSignatures;
        }

        public MemoryStream DownloadEncounter(int encounterId, TokenModel token)
        {
            //PdfDocument document = new PdfDocument();

            //// Create an empty page
            //PdfPage page = document.AddPage();

            //// Get an XGraphics object for drawing
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //// Create a font
            //XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            //// Draw the text
            //gfx.DrawString("Hello, World!", font, XBrushes.Black,
            //  new XRect(0, 0, page.Width, page.Height),
            //  XStringFormat.Center);

            //// Save the document...
            ////string filename = "HelloWorld.pdf";
            //MemoryStream memoryStream = new MemoryStream();
            //document.Save(memoryStream, false);
            //return memoryStream;
            return null;
        }

        private void GetDriveTimeAndDistance(double? lat1, double? long1, double? lat2, double? long2, string mode, string language, string units, ref decimal distance, ref decimal driveTime)
        {
            string URL = "http://maps.googleapis.com/maps/api/distancematrix/json?origins=" + lat1 + "," + long1 + "&destinations=" + lat2 + "," + long2 + "&mode=driving&language=en-EN&units=metric";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        RootObject root = JsonConvert.DeserializeObject<RootObject>(response);
                        if (root != null && root.rows != null && root.rows.Count > 0 && root.rows.FirstOrDefault().elements != null && root.rows.FirstOrDefault().elements.Count > 0 && root.rows.FirstOrDefault().elements.FirstOrDefault().distance != null && root.rows.FirstOrDefault().elements.FirstOrDefault().duration != null)
                        {
                            distance = Math.Round(Convert.ToDecimal((root.rows.FirstOrDefault().elements.FirstOrDefault().distance.value) / 1000.0), 2);
                            driveTime = Math.Round(Convert.ToDecimal((root.rows.FirstOrDefault().elements.FirstOrDefault().duration.value) / 3600.0), 2);
                        }
                    }
                }
            }
        }

        public JsonModel SavePatientEncounterTemplateData(PatientEncounterTemplateModel patientEncounterTemplateModel, TokenModel token)
        {
            PatientEncounterTemplates patientEncounterTemplates = null;
            if (patientEncounterTemplateModel.Id == 0)
            {
                patientEncounterTemplates = new PatientEncounterTemplates();
                AutoMapper.Mapper.Map(patientEncounterTemplateModel, patientEncounterTemplates);
                patientEncounterTemplates.OrganizationID = token.OrganizationID;
                patientEncounterTemplates.CreatedBy = token.UserID;
                _patientEncounterTemplateRepository.Create(patientEncounterTemplates);
                _patientEncounterTemplateRepository.SaveChanges();
                response = new JsonModel(patientEncounterTemplates, StatusMessage.PatientEncounterTemplateCreated, (int)HttpStatusCodes.OK);
            }
            else
            {
                patientEncounterTemplates = _patientEncounterTemplateRepository.Get(p => p.Id == patientEncounterTemplateModel.Id && p.IsDeleted == false && p.IsActive == true && p.OrganizationID == token.OrganizationID);
                if (patientEncounterTemplates != null)
                {
                    AutoMapper.Mapper.Map(patientEncounterTemplateModel, patientEncounterTemplates);
                    patientEncounterTemplates.UpdatedBy = token.UserID;
                    patientEncounterTemplates.UpdatedDate = DateTime.UtcNow;
                    _patientEncounterTemplateRepository.Update(patientEncounterTemplates);
                    _patientEncounterTemplateRepository.SaveChanges();
                    response = new JsonModel(patientEncounterTemplates, StatusMessage.PatientEncounterTemplateUpdated, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }

        public JsonModel GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId, TokenModel tokenModel)
        {
            PatientEncounterTemplateModel patientEncounterTemplateModel = _patientEncounterTemplateRepository.GetPatientEncounterTemplateData<PatientEncounterTemplateModel>(patientEncounterId, masterTemplateId, tokenModel).FirstOrDefault();
            if (patientEncounterTemplateModel != null)
            {
                response = new JsonModel(patientEncounterTemplateModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteEncounter(int encounterId, TokenModel token)
        {
            PatientEncounter patientEncounter = _patientEncounterRepository.GetByID(encounterId);
            patientEncounter.IsActive = false;
            patientEncounter.IsDeleted = true;
            patientEncounter.DeletedBy = token.UserID;
            patientEncounter.DeletedDate = DateTime.UtcNow;
            _patientEncounterRepository.Update(patientEncounter);
            _patientEncounterRepository.SaveChanges();
            return new JsonModel(null, StatusMessage.Success, (int)HttpStatusCodes.OK);
        }
        public JsonModel GetEncounterSummaryDetailsForPDF(int encounterId, TokenModel tokenModel)
        {
            PatientEncounterSummaryPDFModel patientEncounterSummaryPDFModel = _patientEncounterRepository.PrintEncounterSummaryDetails(encounterId, "", tokenModel);
            if (patientEncounterSummaryPDFModel != null)
            {
                response = new JsonModel(patientEncounterSummaryPDFModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public MemoryStream PrintEncounterSummaryDetails(int encounterId, string checkListIds, string portalkey, TokenModel token)
        {
            MemoryStream ms = new MemoryStream();
            PatientEncounterSummaryPDFModel patientEncounterSummaryPDFModel = _patientEncounterRepository.PrintEncounterSummaryDetails(encounterId, checkListIds, token);
            MemoryStream memoryStream = GenerateEncounterSummaryPDF(patientEncounterSummaryPDFModel, false, string.Empty, checkListIds, portalkey, token);
            return memoryStream;
        }
        public JsonModel EmailEncounterSummary(int encounterId, string checkListIds, TokenModel token)
        {
            //var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/send-email-help-support.html");
            //PatientEncounterSummaryPDFModel patientEncounterSummaryPDFModel = _patientEncounterRepository.PrintEncounterSummaryDetails(encounterId, checkListIds, token);
            //if (patientEncounterSummaryPDFModel != null && patientEncounterSummaryPDFModel.PatientDetailsModel != null && !string.IsNullOrEmpty(patientEncounterSummaryPDFModel.PatientDetailsModel.Email))
            //{
            //    Random random = new Random();
            //    int randomNumber = random.Next(0, 9999);

            //    MemoryStream memoryStream = GenerateEncounterSummaryPDF(patientEncounterSummaryPDFModel,true, randomNumber.ToString(),checkListIds,"caremanager", token);
            //    if (memoryStream != null)
            //    {
            //        OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == token.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            //        OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
            //        AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
            //        organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);
            //        string vEmailSubject, toEmail,passwordSubject;
            //        passwordSubject= "Password " +"Encounter Summary - " + patientEncounterSummaryPDFModel.PatientEncounterModel.StartDateTime.ToString("MM/dd/yy h:mm tt") + " to " + patientEncounterSummaryPDFModel.PatientEncounterModel.EndDateTime.ToString("h:mm tt");
            //        vEmailSubject = "Encounter Summary - "+ patientEncounterSummaryPDFModel.PatientEncounterModel.StartDateTime.ToString("MM/dd/yy h:mm tt") + " to " + patientEncounterSummaryPDFModel.PatientEncounterModel.EndDateTime.ToString("h:mm tt");
            //        toEmail = patientEncounterSummaryPDFModel.PatientDetailsModel.Email;
            //        var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/encounter.html");
            //        emailHtml = emailHtml.Replace("{{name}}", !ReferenceEquals(patientEncounterSummaryPDFModel.PatientDetailsModel.FirstName, null) ? string.Concat(patientEncounterSummaryPDFModel.PatientDetailsModel.FirstName.Substring(0, 3), "*** ", patientEncounterSummaryPDFModel.PatientDetailsModel.LastName.Substring(0, 3), "***") : string.Empty);
            //        var passwordEmailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/password-generation.html");
            //        passwordEmailHtml = passwordEmailHtml.Replace("{{password}}",randomNumber.ToString());
            //        passwordEmailHtml = passwordEmailHtml.Replace("{{name}}", !ReferenceEquals(patientEncounterSummaryPDFModel.PatientDetailsModel.FirstName, null) ? string.Concat(patientEncounterSummaryPDFModel.PatientDetailsModel.FirstName.Substring(0, 3), "*** ", patientEncounterSummaryPDFModel.PatientDetailsModel.LastName.Substring(0, 3), "***") : string.Empty);
            //        _emailService.SendEmailWithAttachmentAsync(toEmail, vEmailSubject, emailHtml, organizationSMTPDetailModel, memoryStream, "");
            //        _emailService.SendEmailAsync(toEmail, vEmailSubject, passwordEmailHtml, organizationSMTPDetailModel,string.Empty);
            //        response = new JsonModel(null, StatusMessage.EmailAttachmentSent, (int)HttpStatusCodes.OK);
            //    }
            //    else
            //    {
            //        //some issue in PDF generation
            //        response = new JsonModel(null, StatusMessage.IssueInPDF, (int)HttpStatusCodes.InternalServerError);
            //    }
            //}
            //else
            //{
            //    //no email
            //    response = new JsonModel(null, StatusMessage.NoEmail, (int)HttpStatusCodes.NotAcceptable);
            //}
            return null;
        }
        public MemoryStream GenerateEncounterSummaryPDF(PatientEncounterSummaryPDFModel patientEncounterSummaryPDFModel, bool isEncrypt, string encryptionPassword, string checkListIds, string portalkey, TokenModel token)

        {
            List<int> chklistIdsList = new List<int>();
            if (!string.IsNullOrEmpty(checkListIds))
            {
                chklistIdsList = checkListIds.Split(',').Select(str => int.Parse(str)).ToList();
            }
            MemoryStream tempStream = null;
            Document document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;
            document.DefaultPageSetup.TopMargin = 30;
            document.DefaultPageSetup.BottomMargin = 80;

            //Header Section

            Section section = document.AddSection();
            LocationModel locationModal = _locationService.GetLocationOffsets(token.LocationID, token);
            //Document Footer
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            pageNumberFooterTable.BottomPadding = 0;
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            //pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            //pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 350;
            pageNumberFooterTable.Columns[1].Width = 150;

            MigraDoc.DocumentObjectModel.Tables.Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            pageNumberFooterRow.Cells[0].AddParagraph("Page ").AddPageField();
            pageNumberFooterRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            string path = _env.WebRootPath + "\\PDFImages\\logo-4.png";
            pageNumberFooterRow.Cells[1].AddImage(path);
            pageNumberFooterRow.Format.Alignment = ParagraphAlignment.Right;
            pageNumberFooterRow.Format.Font.Size = 10;
            int index = 0;
            if (index == 0)
            {
                Paragraph topHeaderLogoParagraph = section.AddParagraph();
                string logoPath = _env.WebRootPath + "\\PDFImages\\logo-4.png";
                topHeaderLogoParagraph.AddImage(logoPath).Width = 200;
                topHeaderLogoParagraph.Format.Alignment = ParagraphAlignment.Left;
                index++;
            }

            Paragraph topHeaderLogoEmptyParagraph = section.AddParagraph();
            Paragraph topHeaderLogoEmpty1Paragraph = section.AddParagraph();

            Paragraph headerParagraph = section.AddParagraph();
            headerParagraph.AddText("Tele CMS");
            headerParagraph.AddLineBreak();
            headerParagraph.AddText("Encounter Summary");
            headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 18;
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Bold = true;

            Paragraph emptyParagraph = section.AddParagraph();
            // Patient details
            Table patientInfoTable = section.AddTable();


            Column patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoTable.Format.Font.Size = 11;

            patientInfoTable.Columns[0].Width = 190;
            patientInfoTable.Columns[1].Width = 150;
            patientInfoTable.Columns[2].Width = 120;

            MigraDoc.DocumentObjectModel.Tables.Row patientInfoRow = patientInfoTable.AddRow();

            Paragraph patientInfoPara = new Paragraph();

            patientInfoPara = patientInfoRow.Cells[0].AddParagraph();

            patientInfoPara.AddFormattedText("Member Name : ").Font.Bold = true;
            patientInfoPara.AddText(patientEncounterSummaryPDFModel.PatientDetailsModel.FirstName + " " + patientEncounterSummaryPDFModel.PatientDetailsModel.LastName);

            patientInfoPara = patientInfoRow.Cells[1].AddParagraph();

            patientInfoPara.AddFormattedText("Date Of Birth : ").Font.Bold = true;
            patientInfoPara.AddText(patientEncounterSummaryPDFModel.PatientDetailsModel.DOB);

            patientInfoPara = patientInfoRow.Cells[2].AddParagraph();

            patientInfoPara.AddFormattedText("Gender : ").Font.Bold = true;
            patientInfoPara.AddText(patientEncounterSummaryPDFModel.PatientDetailsModel.Gender);


            Paragraph emptyPara = section.AddParagraph();

            //Care Manager Details
            if (patientEncounterSummaryPDFModel.CareManagerDetailsModel != null)
            {
                Table careManagerInfoTable = section.AddTable();


                Column careManagerInfoColumn = careManagerInfoTable.AddColumn();
                careManagerInfoColumn.Format.Alignment = ParagraphAlignment.Left;

                careManagerInfoColumn = careManagerInfoTable.AddColumn();
                careManagerInfoColumn.Format.Alignment = ParagraphAlignment.Left;

                careManagerInfoColumn = careManagerInfoTable.AddColumn();
                careManagerInfoColumn.Format.Alignment = ParagraphAlignment.Left;

                careManagerInfoTable.Format.Font.Size = 11;

                careManagerInfoTable.Columns[0].Width = 190;
                careManagerInfoTable.Columns[1].Width = 170;
                careManagerInfoTable.Columns[2].Width = 130;

                MigraDoc.DocumentObjectModel.Tables.Row careManagerInfoRow = careManagerInfoTable.AddRow();

                Paragraph careManagerInfoPara = new Paragraph();

                careManagerInfoPara = careManagerInfoRow.Cells[0].AddParagraph();

                careManagerInfoPara.AddFormattedText("Care Manager: ").Font.Bold = true;
                careManagerInfoPara.AddText(patientEncounterSummaryPDFModel.CareManagerDetailsModel.CareManagerName);

                careManagerInfoPara = careManagerInfoRow.Cells[1].AddParagraph();

                careManagerInfoPara.AddFormattedText("Email: ").Font.Bold = true;
                careManagerInfoPara.AddText(patientEncounterSummaryPDFModel.CareManagerDetailsModel.Email);

                careManagerInfoPara = careManagerInfoRow.Cells[2].AddParagraph();

                careManagerInfoPara.AddFormattedText("Phone: ").Font.Bold = true;
                careManagerInfoPara.AddText(patientEncounterSummaryPDFModel.CareManagerDetailsModel.PhoneNumber);

                Paragraph emptyCMDetailsPara = section.AddParagraph();
            }
            // Encounter details
            Table encounterDetailsTable = section.AddTable();
            encounterDetailsTable.TopPadding = 10;
            Column encounterDetailsColumn = encounterDetailsTable.AddColumn();
            encounterDetailsColumn.Format.Alignment = ParagraphAlignment.Left;

            //encounterDetailsColumn = encounterDetailsTable.AddColumn();
            //encounterDetailsColumn.Format.Alignment = ParagraphAlignment.Left;

            //encounterDetailsColumn = encounterDetailsTable.AddColumn();
            //encounterDetailsColumn.Format.Alignment = ParagraphAlignment.Left;

            encounterDetailsTable.Format.Font.Size = 11;

            encounterDetailsTable.Columns[0].Width = 450;
            //encounterDetailsTable.Columns[1].Width = 150;
            //encounterDetailsTable.Columns[2].Width = 120;

            MigraDoc.DocumentObjectModel.Tables.Row encounterDetailsRow = encounterDetailsTable.AddRow();

            encounterDetailsRow.Cells[0].AddParagraph("Encounter Details").Format.Font.Bold = true;
            encounterDetailsRow.Cells[0].Format.Font.Size = 12;
            //encounterDetailsRow.Cells[0].MergeRight = 2;

            #region get location time
            if (token.LocationID > 0)
            {
                if (patientEncounterSummaryPDFModel.PatientEncounterModel != null)
                {
                    patientEncounterSummaryPDFModel.PatientEncounterModel.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientEncounterSummaryPDFModel.PatientEncounterModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                    patientEncounterSummaryPDFModel.PatientEncounterModel.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientEncounterSummaryPDFModel.PatientEncounterModel.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                }
                if (patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel != null)
                {
                    patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                    patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                }
            }
            #endregion
            if(patientEncounterSummaryPDFModel.PatientEncounterModel != null)
            {
                Paragraph encounterDetailsPara = new Paragraph();
                encounterDetailsRow = encounterDetailsTable.AddRow();
                encounterDetailsPara = encounterDetailsRow.Cells[0].AddParagraph();
                encounterDetailsPara.AddFormattedText("Encounter Time : ").Font.Bold = true;
                encounterDetailsRow.Cells[0].Format.Font.Size = 10;
                encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.StartDateTime.ToString("MM/dd/yy h:mm tt") + " - " + patientEncounterSummaryPDFModel.PatientEncounterModel.EndDateTime.ToString("h:mm tt"));
                //encounterDetailsRow.Cells[0].MergeRight = 2;

                Paragraph encounterPrintTimePara = new Paragraph();
                encounterDetailsRow = encounterDetailsTable.AddRow();
                encounterDetailsPara = encounterDetailsRow.Cells[0].AddParagraph();
                encounterDetailsPara.AddFormattedText("Encounter Print Time : ").Font.Bold = true;
                encounterDetailsRow.Cells[0].Format.Font.Size = 10;
                DateTime eastern = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, locationModal.TimeZoneName);
                encounterDetailsPara.AddText(eastern.ToString("MM/dd/yy h:mm tt"));

                if (!string.IsNullOrEmpty(patientEncounterSummaryPDFModel.PatientEncounterModel.ManualChiefComplaint))
                {
                    encounterDetailsRow = encounterDetailsTable.AddRow();
                    encounterDetailsPara = encounterDetailsRow.Cells[0].AddParagraph();
                    encounterDetailsPara.AddFormattedText("Chief Complaint: ").Font.Bold = true;
                    encounterDetailsRow.Cells[0].Format.Font.Size = 10;
                    encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.ManualChiefComplaint);
                }
                if (!string.IsNullOrEmpty(patientEncounterSummaryPDFModel.PatientEncounterModel.ReferringProviderName))
                {
                    encounterDetailsRow = encounterDetailsTable.AddRow();
                    encounterDetailsPara = encounterDetailsRow.Cells[0].AddParagraph();
                    encounterDetailsPara.AddFormattedText("Referring Provider: ").Font.Bold = true;
                    encounterDetailsRow.Cells[0].Format.Font.Size = 10;
                    encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.ReferringProviderName.Replace("<br/>", "\u000A\t\t\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0"));
                    // addDescpara.AddText(obj.CategoryDescription
                    //.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"))

                }
                if (portalkey == PortalType.CareManager)
                {
                    encounterDetailsRow = encounterDetailsTable.AddRow();
                    encounterDetailsPara = encounterDetailsRow[0].AddParagraph();

                    encounterDetailsPara.AddFormattedText("Notes: ").Font.Bold = true;
                    encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.Notes != null ? patientEncounterSummaryPDFModel.PatientEncounterModel.Notes.Replace("\n", string.Empty).Replace("\\n", string.Empty) : string.Empty);

                    //encounterDetailsRow = encounterDetailsTable.AddRow();
                    //encounterDetailsPara = encounterDetailsRow[0].AddParagraph();

                    //encounterDetailsPara.AddFormattedText("Member Notes: ").Font.Bold = true;
                    //encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.MemberNotes != null ? patientEncounterSummaryPDFModel.PatientEncounterModel.MemberNotes : string.Empty);

                }
                else if (portalkey == PortalType.WebMemberPortal)
                {
                    //encounterDetailsRow = encounterDetailsTable.AddRow();
                    //encounterDetailsPara = encounterDetailsRow[0].AddParagraph();

                    //encounterDetailsPara.AddFormattedText("Member Notes: ").Font.Bold = true;
                    //encounterDetailsPara.AddText(patientEncounterSummaryPDFModel.PatientEncounterModel.MemberNotes != null ? patientEncounterSummaryPDFModel.PatientEncounterModel.MemberNotes : string.Empty);
                }
            }


           

           
            //Check List details
            Table checkListTable = section.AddTable();
            checkListTable.TopPadding = 10;
            Column checkListColumn = checkListTable.AddColumn();
            checkListColumn.Format.Alignment = ParagraphAlignment.Left;

            checkListTable.Columns[0].Width = 450;

            MigraDoc.DocumentObjectModel.Tables.Row checkListRow = checkListTable.AddRow();

            checkListRow.Cells[0].AddParagraph("Summary Description").Format.Font.Bold = true;
            checkListRow.Cells[0].Format.Font.Size = 12;
            checkListRow = checkListTable.AddRow();
            Paragraph checkListParagraph = section.AddParagraph();
            bool showMedications = false;

            if (patientEncounterSummaryPDFModel.PatientEncounterChecklistModel != null)
            {
                foreach (var checklist in patientEncounterSummaryPDFModel.PatientEncounterChecklistModel)
                {
                    if (chklistIdsList != null && chklistIdsList.Count > 0 && chklistIdsList.Contains(checklist.Id))
                    {
                        if (checklist.Name == "Pharmacy Claims") showMedications = true;
                        if (checklist.PatientEncounterId > 0)
                        {
                            checkListRow.Cells[0].AddParagraph(checklist.Name).Format.Font.Bold = true;

                            //foreach (var childCheckListItems in patientEncounterSummaryPDFModel.EncounterChecklistReviewItems)
                            //{
                            //    if (childCheckListItems.MasterEncounterChecklistId == checklist.MasterEncounterChecklistId)
                            //    {
                            //        checkListParagraph = checkListRow.Cells[0].AddParagraph();
                            //        ListInfo listinfo = new ListInfo();
                            //        listinfo.NumberPosition = Unit.FromCentimeter(1);
                            //        //listinfo.ListType = ListType.BulletList1;
                            //        checkListParagraph.Format.ListInfo = listinfo;
                            //        checkListParagraph.AddFormattedText(childCheckListItems.ItemName);
                            //        checkListParagraph.AddLineBreak();
                            //    }
                            //}
                            //checkListRow = checkListTable.AddRow();
                            if (portalkey == PortalType.CareManager)
                            {
                                checkListParagraph = checkListRow.Cells[0].AddParagraph();
                                checkListParagraph.AddFormattedText("Notes: ").Font.Bold = true;
                                checkListParagraph.AddText(checklist.Notes != null ? checklist.Notes.Replace("\n", string.Empty) : string.Empty);
                            }
                            checkListRow.Cells[0].AddParagraph("Changes").Format.Font.Bold = true;
                            foreach (var changesHistoryItem in patientEncounterSummaryPDFModel.EncounterChangeHistory)
                            {
                                if (changesHistoryItem.MasterEncounterChecklistId == checklist.MasterEncounterChecklistId)
                                {
                                    //checkListRow = checkListTable.AddRow();
                                    //checkListRow.Cells[0].AddParagraph("Changes").Format.Font.Bold = true;
                                    checkListParagraph = checkListRow.Cells[0].AddParagraph();
                                    ListInfo listinfo = new ListInfo();
                                    listinfo.NumberPosition = Unit.FromCentimeter(1);
                                    listinfo.ListType = ListType.BulletList1;

                                    checkListParagraph.Format.ListInfo = listinfo;
                                    checkListParagraph.Format.LeftIndent = "1.5cm";
                                    checkListParagraph.Format.FirstLineIndent = "-0.5cm";
                                    checkListParagraph.AddFormattedText(changesHistoryItem.Changes);
                                    checkListParagraph.AddLineBreak();
                                }
                            }
                            checkListRow = checkListTable.AddRow();
                            checkListParagraph = checkListRow.Cells[0].AddParagraph();
                            checkListParagraph.AddFormattedText("____________________________________________________________________________________").Font.Bold = true;
                            checkListParagraph.AddLineBreak();
                            checkListRow.TopPadding = 5;
                            checkListRow.BottomPadding = 15;
                        }
                    }

                }


            }
            if (patientEncounterSummaryPDFModel.TaskDetailsModel != null && patientEncounterSummaryPDFModel.TaskDetailsModel.Count > 0)
            {
                //Task details
                Table taskListTable = section.AddTable();
                taskListTable.TopPadding = 10;
                Column taskListColumn = taskListTable.AddColumn();
                taskListColumn.Format.Alignment = ParagraphAlignment.Left;

                taskListColumn = taskListTable.AddColumn();
                taskListColumn.Format.Alignment = ParagraphAlignment.Left;

                taskListTable.Format.Font.Size = 11;

                taskListTable.Columns[0].Width = 225;
                taskListTable.Columns[1].Width = 225;


                MigraDoc.DocumentObjectModel.Tables.Row taskListRow = taskListTable.AddRow();
                Paragraph taskListData = new Paragraph();
                taskListRow.Cells[0].AddParagraph("Task Details : ").Format.Font.Bold = true;
                taskListRow.Cells[0].Format.Font.Size = 12;
                //taskListRow.Cells[0].MergeRight = 1;       
                taskListRow = taskListTable.AddRow();

                foreach (var taskList in patientEncounterSummaryPDFModel.TaskDetailsModel)
                {
                    //Paragraph taskListData = new Paragraph();
                    taskListData = taskListRow.Cells[0].AddParagraph();

                    taskListData.AddFormattedText("Task Description : ").Font.Bold = true;
                    taskListRow.Cells[0].Format.Font.Size = 10;
                    taskListData.AddText(taskList.Description);

                    taskListData = taskListRow.Cells[1].AddParagraph();

                    taskListData.AddFormattedText("Due date : ").Font.Bold = true;
                    taskListRow.Cells[1].Format.Font.Size = 10;
                    taskListData.AddText(taskList.DueDate);
                }

            }

            Paragraph spacePara = section.AddParagraph();

            if (patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel != null)
            {
                //Appointment details
                Table appointmentListTable = section.AddTable();
                appointmentListTable.TopPadding = 10;
                Column appointmentListColumn = appointmentListTable.AddColumn();
                appointmentListColumn.Format.Alignment = ParagraphAlignment.Left;

                //appointmentListColumn = appointmentListTable.AddColumn();
                //appointmentListColumn.Format.Alignment = ParagraphAlignment.Left;

                appointmentListTable.Format.Font.Size = 11;

                appointmentListTable.Columns[0].Width = 450;
                //appointmentListTable.Columns[1].Width = 225;


                MigraDoc.DocumentObjectModel.Tables.Row appointmentListRow = appointmentListTable.AddRow();
                appointmentListRow.Cells[0].AddParagraph("Appointment Details : ").Format.Font.Bold = true;
                appointmentListRow.Cells[0].Format.Font.Size = 12;
                Paragraph appointmentListPara = new Paragraph();
                appointmentListPara.AddLineBreak();
                //appointmentListPara = appointmentListRow.Cells[0].AddParagraph();
                appointmentListRow = appointmentListTable.AddRow();

                appointmentListPara = appointmentListRow.Cells[0].AddParagraph();

                appointmentListPara.AddFormattedText("Upcoming Appointments : ").Font.Bold = true;
                appointmentListRow.Cells[0].Format.Font.Size = 10;
                appointmentListPara.AddText(Convert.ToString(patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.StartDateTime.ToString("MM/dd/yy h:mm tt") + " - " + patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.EndDateTime.ToString("h:mm tt")));

                appointmentListPara = appointmentListRow.Cells[0].AddParagraph();

                //appointmentListPara.AddFormattedText("Notes to Member : ").Font.Bold = true;
                //appointmentListRow.Cells[0].Format.Font.Size = 10;
                //appointmentListPara.AddText( patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.NotesToMember == null ? " " : patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.NotesToMember);

                //appointmentListPara = appointmentListRow.Cells[0].AddParagraph();
                //appointmentListPara.AddFormattedText("Appointment Type : ").Font.Bold = true;
                //appointmentListRow.Cells[0].Format.Font.Size = 10;
                //appointmentListPara.AddText(patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.AppointmentType == null ? " " : patientEncounterSummaryPDFModel.PatientAppointmentDetailsModel.AppointmentType);
            }

            //PrintPatientCurrentMedication
            if (patientEncounterSummaryPDFModel.PrintPatientCurrentMedicationModel.Count > 0 && showMedications == true)
            {
                section.AddPageBreak();
                Paragraph PrintPatientCurrentMedicationParagraph = section.AddParagraph();

                Table PrintPatientCurrentMedicationTable = section.AddTable();
                Column PrintPatientCurrentMedicationTableColumn = PrintPatientCurrentMedicationTable.AddColumn();
                PrintPatientCurrentMedicationTableColumn.Format.Alignment = ParagraphAlignment.Left;

                PrintPatientCurrentMedicationTable.Columns[0].Width = 450;

                MigraDoc.DocumentObjectModel.Tables.Row PrintPatientCurrentMedicationTabletRow = PrintPatientCurrentMedicationTable.AddRow();

                PrintPatientCurrentMedicationTabletRow.Cells[0].AddParagraph("Medications").Format.Font.Bold = true;
                PrintPatientCurrentMedicationTabletRow.Cells[0].Format.Font.Size = 12;
                PrintPatientCurrentMedicationTabletRow = PrintPatientCurrentMedicationTable.AddRow();


                Table reportTable = section.AddTable();
                reportTable.Borders.Visible = true;
                reportTable.Borders.Color = new Color(211, 211, 211);
                reportTable.LeftPadding = 0;
                reportTable.RightPadding = 0;
                reportTable.Format.Font.Size = 8;
                reportTable.TopPadding = 10;
                // reportTable.BottomPadding = 10;


                Column reportColumn = reportTable.AddColumn();
                reportColumn.LeftPadding = 0;
                reportColumn.RightPadding = 0;
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;
                reportColumn = reportTable.AddColumn();
                reportColumn.Format.Alignment = ParagraphAlignment.Center;

                reportTable.Columns[0].Width = 70;
                reportTable.Columns[1].Width = 50;
                reportTable.Columns[2].Width = 25;
                reportTable.Columns[3].Width = 25;
                reportTable.Columns[4].Width = 40;
                reportTable.Columns[5].Width = 50;
                reportTable.Columns[6].Width = 60;
                reportTable.Columns[7].Width = 80;
                reportTable.Columns[8].Width = 50;
                reportTable.Columns[9].Width = 25;
                reportTable.Columns[10].Width = 55;

                MigraDoc.DocumentObjectModel.Tables.Row reportHeadingRow = reportTable.AddRow();
                reportHeadingRow.HeadingFormat = true;
                reportHeadingRow.Cells[0].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberMedication);
                reportHeadingRow.Cells[1].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberDosageForm);
                reportHeadingRow.Cells[2].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberDose);
                reportHeadingRow.Cells[3].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberQty);
                reportHeadingRow.Cells[4].AddParagraph("Days Supply");
                reportHeadingRow.Cells[5].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberFrequency);
                reportHeadingRow.Cells[6].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberCondition);
                reportHeadingRow.Cells[7].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMember_Name);
                reportHeadingRow.Cells[8].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberDate);
                reportHeadingRow.Cells[9].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberRefills);
                reportHeadingRow.Cells[10].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberSource);

                reportHeadingRow.Format.Font.Bold = true;

                if (patientEncounterSummaryPDFModel.PrintPatientCurrentMedicationModel.Count > 0)
                {
                    foreach (var item in patientEncounterSummaryPDFModel.PrintPatientCurrentMedicationModel)
                    {
                        MigraDoc.DocumentObjectModel.Tables.Row reportRow = reportTable.AddRow();
                        reportRow.Cells[0].AddParagraph(item.Medication != null ? item.Medication : "");
                        reportRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        reportRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
                        reportRow.Cells[1].AddParagraph(item.DosageForm != null ? item.DosageForm.ToString() : "");
                        reportRow.Cells[2].AddParagraph(item.Dose != null ? item.Dose.ToString() : "");
                        reportRow.Cells[3].AddParagraph(item.Quantity != null ? item.Quantity.ToString() : "");
                        reportRow.Cells[4].AddParagraph(item.DaySupply != null ? item.DaySupply.ToString() : "");
                        reportRow.Cells[5].AddParagraph(item.Frequency != null ? item.Frequency : "");
                        reportRow.Cells[6].AddParagraph(item.Condition != null ? item.Condition : "");
                        reportRow.Cells[7].AddParagraph(item.ProviderName != null ? item.ProviderName : "");
                        reportRow.Cells[8].AddParagraph(item.PrescribedDate != null ? item.PrescribedDate.Value.ToString("MM/dd/yyyy") : "");
                        reportRow.Cells[9].AddParagraph(item.Refills != null ? item.Refills.ToString() : "");
                        reportRow.Cells[10].AddParagraph(item.Source != null ? item.Source : "");

                        if (!string.IsNullOrEmpty(item.Notes))
                        {
                            MigraDoc.DocumentObjectModel.Tables.Row notesRow = reportTable.AddRow();
                            notesRow.TopPadding = 0;
                            notesRow.BottomPadding = 10;
                            notesRow.Cells[0].MergeRight = 9;
                            notesRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            notesRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
                            notesRow.Cells[0].AddParagraph("Notes : " + item.Notes);
                        }
                    }
                }
            }
            //else
            //{
            //    Paragraph PrintPatientCurrentMedicationParagraph = section.AddParagraph();

            //    Table PrintPatientCurrentMedicationTable = section.AddTable();
            //    Column PrintPatientCurrentMedicationTableColumn = PrintPatientCurrentMedicationTable.AddColumn();
            //    PrintPatientCurrentMedicationTableColumn.Format.Alignment = ParagraphAlignment.Left;

            //    PrintPatientCurrentMedicationTable.Columns[0].Width = 450;

            //    MigraDoc.DocumentObjectModel.Tables.Row PrintPatientCurrentMedicationTabletRow = PrintPatientCurrentMedicationTable.AddRow();

            //    PrintPatientCurrentMedicationTabletRow.Cells[0].AddParagraph("No medications prescribed").Format.Font.Bold = true;
            //    PrintPatientCurrentMedicationTabletRow.Cells[0].Format.Font.Size = 12;
            //    PrintPatientCurrentMedicationTabletRow = PrintPatientCurrentMedicationTable.AddRow();

            //}

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf
            if (isEncrypt)
                pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }

        public JsonModel DiscardEncounterChanges(int encounterId, TokenModel token)
        {
            PatientEncounter patientEncounter = _patientEncounterRepository.GetByID(encounterId);
            patientEncounter.IsActive = false;
            patientEncounter.UpdatedBy = token.UserID;
            patientEncounter.UpdatedDate = DateTime.UtcNow;
            _patientEncounterRepository.Update(patientEncounter);
            _patientEncounterRepository.SaveChanges();
            // _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Discard, patientEncounter.PatientID, token.UserID, "EncounterId/" + encounterId, token, null, null);
            return new JsonModel(null, StatusMessage.EncounterDiscard, (int)HttpStatusCodes.OK);
        }

        #region Bulk email and message Methods
        //public SignalRNotificationForBulkMessageModel SendBulkMessageForEncounters(EncounterFilterModel filterModel, TokenModel tokenModel)
        //{
        //    List<BulkEmailUserModel> bulkMessageMemberData = _patientEncounterRepository.GetAllPatientEncounterUsers<BulkEmailUserModel>(filterModel, tokenModel).ToList();
        //    List<Chat> chats = new List<Chat>();
        //    MemoryStream memoryStream = new MemoryStream();
        //    LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
        //    if (bulkMessageMemberData != null && bulkMessageMemberData.Count > 0)
        //    {
        //        using (var transaction = _patientEncounterRepository.StartTransaction())
        //        {
        //            try
        //            {
        //                BulkMessage bulkMessage = new BulkMessage();
        //                chats = bulkMessageMemberData.Select(patient => new Chat
        //                {
        //                    FromUserId = tokenModel.UserID,
        //                    ToUserId = patient.UserId,
        //                    Message = CommonMethods.Encrypt(filterModel.Message),
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = tokenModel.UserID,
        //                    OrganizationID = tokenModel.OrganizationID,
        //                    IsDeleted = false,
        //                    IsActive = true,
        //                    ChatDate = DateTime.UtcNow,
        //                }).ToList();

        //                _chatRepository.Create(chats.ToArray());
        //                _chatRepository.SaveChanges();

        //                SaveChatNotification(chats, (int)NotificationActionType.ChatMessage, tokenModel);

        //                if (bulkMessageMemberData != null && bulkMessageMemberData.Count() > 0 && chats != null && chats.Count() > 0)
        //                {
        //                    List<PrintPatientBulkMessageStatus> printPatientEmails = new List<PrintPatientBulkMessageStatus>();
        //                    bulkMessageMemberData.ForEach(x =>
        //                    {
        //                        PrintPatientBulkMessageStatus printPatientEmail = new PrintPatientBulkMessageStatus();
        //                        printPatientEmail.FirstName = x.FirstName;
        //                        printPatientEmail.LastName = x.LastName;
        //                        printPatientEmail.Email = x.Email;
        //                        printPatientEmail.Dob = DateTime.Parse(x.Dob).ToString("MM/dd/yyyy");
        //                        printPatientEmail.Date = CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("MM/dd/yyyy");
        //                        printPatientEmail.Status = x.IsActive == true ? "Active" : "In Active";
        //                        printPatientEmails.Add(printPatientEmail);
        //                    });

        //                    //SaveBulkEmailAndMessageReportLogs(filterModel, tokenModel, bulkMessageMemberData);

        //                    if (printPatientEmails != null && printPatientEmails.Count() > 0)
        //                    {
        //                        memoryStream = GenerateMessageExcelReport(filterModel, tokenModel, locationModel, printPatientEmails);
        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //    SignalRNotificationForBulkMessageModel signalRModel = new SignalRNotificationForBulkMessageModel();
        //    signalRModel.MemoryStream = memoryStream;
        //    signalRModel.Chat = chats;
        //    return signalRModel; // new JsonModel(messageCount, string.Format("Email log: No. of messages sent are {0}, and no. of messages not sent are {1}", messageCount.SendCount, messageCount.UnSendCount), (int)HttpStatusCodes.OK);
        //}
        //public SignalRNotificationForBulkEmailModel SendBulkEmailForEncounters(EncounterFilterModel filterModel, TokenModel tokenModel)
        //{
        //    List<BulkEmailUserModel> bulkMessagePatientModel = _patientEncounterRepository.GetAllPatientEncounterUsers<BulkEmailUserModel>(filterModel, tokenModel).ToList();
        //    BuklEmailSuccessMessageCountModel messageCount = new BuklEmailSuccessMessageCountModel();
        //    LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
        //    MemoryStream memoryStream = new MemoryStream();
        //    SignalRNotificationForBulkEmailModel signalRNotificationForBulkEmailModel = new SignalRNotificationForBulkEmailModel();
        //    if (bulkMessagePatientModel != null && bulkMessagePatientModel.Count > 0)
        //    {
        //        using (var transaction = _patientEncounterRepository.StartTransaction())
        //        {
        //            try
        //            {
        //                BulkMessage bulkMessage = new BulkMessage();
        //                bulkMessage.Message = CommonMethods.Encrypt(filterModel.Message);
        //                bulkMessage.Subject = CommonMethods.Encrypt(filterModel.Subject);
        //                bulkMessage.ModuleType = LogModuleType.MemberEncounters;
        //                bulkMessage.CreatedDate = DateTime.UtcNow;
        //                bulkMessage.CreatedBy = tokenModel.UserID;
        //                bulkMessage.OrganizationID = tokenModel.OrganizationID;
        //                bulkMessage.IsDeleted = false;
        //                bulkMessage.IsActive = true;
        //                _bulkMessageRepository.Create(bulkMessage);
        //                _bulkMessageRepository.SaveChanges();

        //                List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping = bulkMessagePatientModel.Select(patientid => new BulkMessageAndPatientMapping
        //                {
        //                    PatientId = Convert.ToInt32(patientid.PatientId),
        //                    MessageId = bulkMessage.MessageId,
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = tokenModel.UserID,
        //                    OrganizationID = tokenModel.OrganizationID,
        //                    IsDeleted = false,
        //                    IsActive = true,
        //                    UserId = patientid.UserId
        //                }).ToList();


        //                _bulkMessageAndPatientMappingRepository.Create(bulkMessageAndPatientMapping.ToArray());
        //                _bulkMessageAndPatientMappingRepository.SaveChanges();

        //                SendBulkEmailPatient(bulkMessagePatientModel, filterModel.Message, filterModel.Subject, bulkMessageAndPatientMapping, bulkMessage.MessageId, tokenModel);

        //                _bulkMessageAndPatientMappingRepository.Update(bulkMessageAndPatientMapping.ToArray());
        //                _bulkMessageAndPatientMappingRepository.SaveChanges();

        //                SaveEmailNotification(bulkMessageAndPatientMapping, (int)NotificationActionType.BulkEmail, tokenModel);

        //                //messageCount = bulkMessageAndPatientMapping.Select(x => new BuklEmailSuccessMessageCountModel
        //                //{
        //                //    SendCount = bulkMessageAndPatientMapping.Where(y => y.IsMessageSend == true && y.MessageId == bulkMessage.MessageId).Count(),
        //                //    UnSendCount = bulkMessageAndPatientMapping.Where(y => y.IsMessageSend == false && y.MessageId == bulkMessage.MessageId).Count(),
        //                //}).FirstOrDefault();

        //                // SaveBulkEmailAndMessageReportLogs(filterModel, tokenModel, bulkMessageMemberData);

        //                signalRNotificationForBulkEmailModel.MemoryStream = GenerateEmailExcelReport(filterModel, bulkMessagePatientModel, memoryStream, LogModuleType.MemberEncounters, bulkMessageAndPatientMapping, tokenModel, locationModel);
        //                signalRNotificationForBulkEmailModel.BulkMessageAndPatientMapping = bulkMessageAndPatientMapping;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //    return signalRNotificationForBulkEmailModel; // new JsonModel(messageCount, string.Format("Email log: No. of messages sent are {0}, and no. of messages not sent are {1}", messageCount.SendCount, messageCount.UnSendCount), (int)HttpStatusCodes.OK);
        //}
        //private void SaveChatNotification(List<Chat> chats, int actionId, TokenModel token)
        //{
        //    List<NotificationModel> notifications = chats.Select(chat => new NotificationModel
        //    {
        //        PatientId = _patientRepository.GetPatientByUserId(chat.ToUserId),
        //        StaffId = _tokenRepository.GetStaffByuserID(chat.FromUserId).Id,
        //        ActionTypeId = actionId,
        //        ChatId = chat.Id,
        //        Message = chat.Message,
        //    }).ToList();
        //    if (notifications.Count() > 0)
        //    {
        //        _notificationService.SaveNotification(notifications, token);
        //    }
        //}
        //private MemoryStream GenerateMessageExcelReport(EncounterFilterModel filterModel, TokenModel tokenModel, LocationModel locationModel, List<PrintPatientBulkMessageStatus> printPatientEmails)
        //{
        //    using (var excel = new ExcelPackage())
        //    {
        //        excel.Workbook.Worksheets.Add("Worksheet1");

        //        var headerRow = new List<string[]>() { new string[] { "First Name", "Last Name", "Email", "DOB", "Message", "Date", "Status" } };

        //        // Determine the header range (e.g. A1:D1)
        //        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        worksheet.DefaultRowHeight = 12;

        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.Font.Bold = true;

        //        worksheet.Cells[1, 1].Value = "Message";
        //        worksheet.Cells[1, 2].Value = filterModel.Message;
        //        worksheet.Cells[1, 2, 1, 6].Merge = true;

        //        worksheet.Row(2).Height = 20;
        //        worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(2).Style.Font.Bold = true;


        //        worksheet.Column(1).Width = 30;
        //        worksheet.Column(2).Width = 30;
        //        worksheet.Column(3).Width = 30;
        //        worksheet.Column(4).Width = 20;
        //        worksheet.Column(5).Width = 20;
        //        worksheet.Column(6).Width = 20;


        //        worksheet.Cells[2, 1].Value = "First Name"; //headerRow[0];
        //        worksheet.Cells[2, 2].Value = "Last Name"; //headerRow[2];
        //        worksheet.Cells[2, 3].Value = "Email"; //headerRow[1];
        //        worksheet.Cells[2, 4].Value = "DOB"; //headerRow[3];
        //        worksheet.Cells[2, 5].Value = "Date"; //headerRow[5];
        //        worksheet.Cells[2, 6].Value = "Status"; //headerRow[6];

        //        //worksheet.Cells["A1"].Value = "Report";
        //        //// Popular header row data
        //        //worksheet.Cells["A1"].LoadFromArrays(headerRow);

        //        worksheet.Cells[3, 1].LoadFromCollection(printPatientEmails);

        //        return ReadFileAndSaveLocally(excel, "MessageLog", LogModuleType.MemberEncounters, tokenModel, locationModel, (int)MemberEmailAndMessageReportType.Message);
        //    }
        //}
        //private MemoryStream ReadFileAndSaveLocally(ExcelPackage excel, string logType, string moduleType, TokenModel tokenModel, LocationModel locationModel, int reportType)
        //{

        //    string webRootPath = Directory.GetCurrentDirectory();

        //    //add your custom path
        //    webRootPath = webRootPath + ImagesPath.HRAEmailAndMessageReport;

        //    //check 
        //    if (!Directory.Exists(webRootPath))
        //    {
        //        Directory.CreateDirectory(webRootPath);
        //    }
        //    string fileName = logType + "_" + moduleType + "Report-" + CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
        //    //var excelPrint = excel;
        //    string filePath = string.Format("{0}/{1}", webRootPath, fileName);
        //    FileInfo fileInfo = new FileInfo(filePath);
        //    excel.SaveAs(fileInfo);
        //    MemoryStream memStream = new MemoryStream();
        //    using (FileStream fileStream = File.OpenRead(filePath))
        //    {

        //        memStream.SetLength(fileStream.Length);
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
        //    }

        //    SaveHRAEmailAndMessageReportLog(tokenModel, fileName, locationModel, reportType, moduleType);
        //    return memStream;
        //}
        //private void SaveHRAEmailAndMessageReportLog(TokenModel tokenModel, string fileName, LocationModel locationModal, int reportType, string moduleType)
        //{
        //    HRAEmailAndMessageReportLog hraEmailAndMessageReportLog = new HRAEmailAndMessageReportLog();
        //    hraEmailAndMessageReportLog.CreatedDate = DateTime.UtcNow;
        //    hraEmailAndMessageReportLog.IsActive = true;
        //    hraEmailAndMessageReportLog.IsDeleted = false;
        //    hraEmailAndMessageReportLog.FileName = fileName;
        //    hraEmailAndMessageReportLog.ModuleType = moduleType;
        //    hraEmailAndMessageReportLog.OrganizationId = tokenModel.OrganizationID;
        //    hraEmailAndMessageReportLog.ReportDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, locationModal.TimeZoneName);
        //    hraEmailAndMessageReportLog.ReportType = reportType;
        //    hraEmailAndMessageReportLog.UserID = tokenModel.UserID;
        //    _hraEmailAndMessageReportLogRepository.Create(hraEmailAndMessageReportLog);
        //    _hraEmailAndMessageReportLogRepository.SaveChanges();
        //}
        //public void SendBulkEmailPatient(List<BulkEmailUserModel> patientInfo, string message, string vEmailSubject, List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, int messageId, TokenModel tokenModel)
        //{
        //    OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
        //    OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
        //    AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
        //    organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);
        //    var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/bulk-email-template.html");

        //    foreach (var data in bulkMessageAndPatientMapping)
        //    {
        //        bool isSend = false;
        //        BulkEmailUserModel patient = patientInfo.Where(w => w.PatientId == data.PatientId).FirstOrDefault();

        //        if (!string.IsNullOrEmpty(patient.Email))
        //        {
        //            string htmlFile = string.Empty;
        //            htmlFile = emailHtml;
        //            //htmlFile = htmlFile.Replace("{{patientName}}", string.Concat(patient.FirstName.Substring(0, 3), "*** ", patient.LastName.Substring(0, 3), "***"));
        //            htmlFile = htmlFile.Replace("{{message}}", message);
        //            isSend = _emailService.SendEmail(patient.Email, vEmailSubject, htmlFile, organizationSMTPDetailModel).Result;
        //        }
        //        data.IsMessageSend = isSend ? true : false;
        //    }
        //}
        //private void SaveEmailNotification(List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, int actionId, TokenModel token)
        //{
        //    List<NotificationModel> notifications = bulkMessageAndPatientMapping.Where(x => x.IsMessageSend == true && x.IsActive == true && x.IsDeleted == false).Select(y => new NotificationModel
        //    {
        //        PatientId = y.PatientId,
        //        StaffId = _tokenRepository.GetStaffByuserID(token.UserID).Id,
        //        ActionTypeId = actionId,
        //    }).ToList();
        //    if (notifications.Count() > 0)
        //    {
        //        _notificationService.SaveNotification(notifications, token);
        //    }
        //}
        //private MemoryStream GenerateEmailExcelReport(EncounterFilterModel filterModel, List<BulkEmailUserModel> bulkMessagePatientModel, MemoryStream memoryStream, string moduleKey, List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, TokenModel tokenModel, LocationModel locationModel)
        //{
        //    string plainText = HtmlToPlainText(filterModel.Message);


        //    if (bulkMessagePatientModel != null && bulkMessagePatientModel.Count() > 0 && bulkMessageAndPatientMapping != null && bulkMessageAndPatientMapping.Count() > 0)
        //    {
        //        List<PrintPatientBulkEmailSentStatus> printPatientEmail = (from l1 in bulkMessagePatientModel
        //                                                                   join l2 in bulkMessageAndPatientMapping
        //                                                                   on l1.PatientId equals l2.PatientId
        //                                                                   select new PrintPatientBulkEmailSentStatus
        //                                                                   {
        //                                                                       FirstName = l1.FirstName,
        //                                                                       LastName = l1.LastName,
        //                                                                       Dob = DateTime.Parse(l1.Dob).ToString("MM/dd/yyyy"),
        //                                                                       Email = l1.Email,
        //                                                                       Date = CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("MM/dd/yyyy"),
        //                                                                       Status = l2.IsMessageSend == true ? "Sent" : "Failed",
        //                                                                   }).ToList();

        //        if (printPatientEmail != null && printPatientEmail.Count() > 0)
        //        {
        //            using (var excel = new ExcelPackage())
        //            {
        //                excel.Workbook.Worksheets.Add("Worksheet1");

        //                //var headerRow = new List<string[]>() { new string[] { "First Name", "Last Name", "Email", "DOB", "Template", "Subject", "Date", "Status" } };

        //                // Determine the header range (e.g. A1:D1)
        //                //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

        //                // Target a worksheet
        //                var worksheet = excel.Workbook.Worksheets["Worksheet1"];

        //                worksheet.DefaultRowHeight = 12;
        //                worksheet.Row(1).Height = 20;
        //                worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(1).Style.Font.Bold = true;

        //                worksheet.Cells[1, 1].Value = "Subject";
        //                worksheet.Cells[1, 2].RichText.Text += filterModel.Subject;
        //                worksheet.Cells[1, 2].Style.Font.Bold = false;
        //                worksheet.Cells[1, 2, 1, 7].Merge = true;

        //                worksheet.DefaultRowHeight = 12;
        //                worksheet.Row(2).Height = 20;
        //                worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(2).Style.Font.Bold = true;

        //                worksheet.Cells[2, 1].Value = "Message";
        //                worksheet.Cells[2, 2].RichText.Text += plainText;
        //                worksheet.Cells[2, 2].Style.Font.Bold = false;
        //                worksheet.Cells[2, 2, 2, 7].Merge = true;

        //                worksheet.Row(3).Height = 20;
        //                worksheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(3).Style.Font.Bold = true;
        //                worksheet.Cells[3, 1, 3, 6].Merge = true;

        //                worksheet.Row(4).Height = 20;
        //                worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(4).Style.Font.Bold = true;

        //                worksheet.Column(1).Width = 30;
        //                worksheet.Column(2).Width = 30;
        //                worksheet.Column(3).Width = 40;
        //                worksheet.Column(4).Width = 30;
        //                worksheet.Column(5).Width = 30;
        //                worksheet.Column(6).Width = 30;

        //                worksheet.Cells[4, 1].Value = "First Name"; //headerRow[0];
        //                worksheet.Cells[4, 2].Value = "Last Name"; //headerRow[1];
        //                worksheet.Cells[4, 3].Value = "Email"; //headerRow[2];
        //                worksheet.Cells[4, 4].Value = "DOB"; //headerRow[3];
        //                worksheet.Cells[4, 5].Value = "Date"; //headerRow[5];
        //                worksheet.Cells[4, 6].Value = "Status"; //headerRow[6];

        //                //worksheet.Cells["A1"].Value = "Hello World!";
        //                // Popular header row data
        //                //worksheet.Cells["headerRange"].LoadFromArrays(headerRow);
        //                worksheet.Row(4).Height = 20;
        //                worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(4).Style.Font.Bold = false;
        //                worksheet.Cells[5, 1].LoadFromCollection(printPatientEmail);

        //                return ReadFileAndSaveLocally(excel, "EmailLog", moduleKey, tokenModel, locationModel, (int)MemberEmailAndMessageReportType.Email);

        //            }
        //        }
        //    }

        //    return memoryStream;
        //}
        //private static string HtmlToPlainText(string html)
        //{
        //    if (string.IsNullOrWhiteSpace(html))
        //    {
        //        return null;
        //    }
        //    const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
        //    const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
        //    const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
        //    var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
        //    var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
        //    var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
        //    var text = html;
        //    //Decode html specific characters
        //    text = System.Net.WebUtility.HtmlDecode(text);
        //    //Remove tag whitespace/line breaks
        //    text = tagWhiteSpaceRegex.Replace(text, "><");
        //    //Replace <br /> with line breaks
        //    text = lineBreakRegex.Replace(text, Environment.NewLine);
        //    //Strip formatting
        //    text = stripFormattingRegex.Replace(text, string.Empty);

        //    RegexOptions options = RegexOptions.None;
        //    Regex regex = new Regex("[ ]{2,}", options);
        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        text = regex.Replace(text, " ");
        //    }
        //    return text;
        //}
        #endregion

        //public MemoryStream ExportEncounters(EncounterFilterModel filterModel, TokenModel tokenModel)
        //{
        //    DataTable dynamiceQueryDataModel = _patientEncounterRepository.ExportEncounters(filterModel, tokenModel);
        //    MemoryStream stream = null;
        //    if (dynamiceQueryDataModel != null)
        //    {
        //        stream = GenerateExcel(dynamiceQueryDataModel);
        //    }
        //    return stream;
        //}

        //private MemoryStream GenerateExcel(DataTable dynamiceQueryDataModel)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    using (var excel = new ExcelPackage())
        //    {
        //        //excel.Workbook.Worksheets.Add("Worksheet1");
        //        //var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        //worksheet.DefaultRowHeight = 12;

        //        excel.Workbook.Worksheets.Add("Worksheet1");

        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        worksheet.DefaultRowHeight = 12;
        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.WrapText = true;
        //        worksheet.Row(1).Style.Font.Bold = true;

        //        worksheet.DefaultRowHeight = 12;
        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.Font.Bold = true;

        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.Font.Bold = true;

        //        worksheet.Column(1).Width = 30;
        //        worksheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Column(2).Width = 30;
        //        worksheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Column(3).Width = 40;
        //        worksheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Column(3).Style.WrapText = true;

        //        worksheet.Column(4).Width = 30;
        //        worksheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Column(4).Style.WrapText = true;

        //        worksheet.Column(5).Width = 30;
        //        worksheet.Column(5).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Column(6).Width = 50;
        //        worksheet.Column(6).Style.WrapText = true;
        //        worksheet.Column(6).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Column(7).Width = 30;
        //        worksheet.Column(7).Style.WrapText = true;
        //        worksheet.Column(7).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


        //        worksheet.Column(8).Width = 30;
        //        worksheet.Column(8).Style.WrapText = true;
        //        worksheet.Column(8).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Column(9).Width = 50;
        //        worksheet.Column(9).Style.WrapText = true;
        //        worksheet.Column(9).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //        worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.Font.Bold = true;
        //        worksheet.Cells[1, 1].LoadFromDataTable(dynamiceQueryDataModel, true);
        //        byte[] data = excel.GetAsByteArray();

        //        memoryStream = new MemoryStream(data);

        //    }
        //    return memoryStream;
        //}
        //public MemoryStream PrintEncountersPDF(EncounterFilterModel filterModel, TokenModel token)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    List<PrintEncounterListModel> printEncounterListModel = _patientEncounterRepository.PrintEncountersPDF<PrintEncounterListModel>(filterModel, token).ToList();

        //    if (printEncounterListModel != null && printEncounterListModel.Count() > 0)
        //    {
        //        memoryStream = GenerateEncounterListPDF(printEncounterListModel, token);
        //    }
        //    return memoryStream;
        //}
        //public MemoryStream GenerateEncounterListPDF(List<PrintEncounterListModel> printEncounterListModel, TokenModel token)
        //{
        //    MemoryStream tempStream = null;
        //    Document document = new Document();
        //    document.DefaultPageSetup.Orientation = Orientation.Portrait;
        //    document.DefaultPageSetup.PageHeight = 792;
        //    document.DefaultPageSetup.PageWidth = 612;
        //    document.DefaultPageSetup.TopMargin = 30;
        //    document.DefaultPageSetup.BottomMargin = 80;
        //    document.DefaultPageSetup.LeftMargin = 35;
        //    document.DefaultPageSetup.RightMargin = 15;

        //    //Header Section

        //    Section section = document.AddSection();
        //    LocationModel locationModal = _locationService.GetLocationOffsets(token.LocationID, token);
        //    //Document Footer
        //    //section.PageSetup.DifferentFirstPageHeaderFooter = true;
        //    Table pageNumberFooterTable = section.Footers.Primary.AddTable();
        //    pageNumberFooterTable.BottomPadding = 0;
        //    Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    //pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    //pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

        //    pageNumberFooterTable.Columns[0].Width = 100;
        //    pageNumberFooterTable.Columns[1].Width = 100;
        //    pageNumberFooterTable.Columns[2].Width = 80;
        //    pageNumberFooterTable.Columns[3].Width = 100;
        //    pageNumberFooterTable.Columns[4].Width = 70;

        //    MigraDoc.DocumentObjectModel.Tables.Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
        //    pageNumberFooterRow.TopPadding = 5;
        //    pageNumberFooterRow.Cells[0].AddParagraph("");
        //    pageNumberFooterRow.Cells[1].AddParagraph("");
        //    pageNumberFooterRow.Cells[2].AddParagraph("");
        //    pageNumberFooterRow.Cells[3].AddParagraph("Page ").AddPageField();
        //    pageNumberFooterRow.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        //    string path = _env.WebRootPath + "\\PDFImages\\overture-updated-bottom-logo.png";
        //    pageNumberFooterRow.Cells[4].AddImage(path).Height = 35;

        //    pageNumberFooterRow.Format.Alignment = ParagraphAlignment.Right;
        //    pageNumberFooterRow.Format.Font.Size = 10;
        //    int index = 0;
        //    if (index == 0)
        //    {
        //        Paragraph topHeaderLogoParagraph = section.AddParagraph();
        //        string logoPath = _env.WebRootPath + "\\PDFImages\\overture-updated-top-logo.png";
        //        topHeaderLogoParagraph.AddImage(logoPath).Width = 200;
        //        topHeaderLogoParagraph.Format.Alignment = ParagraphAlignment.Left;
        //        index++;
        //    }

        //    Paragraph topHeaderLogoEmptyParagraph = section.AddParagraph();
        //    Paragraph topHeaderLogoEmpty1Paragraph = section.AddParagraph();

        //    Paragraph headerParagraph = section.AddParagraph();
        //    //headerParagraph.AddText("Overture Health Care");
        //    //headerParagraph.AddLineBreak();
        //    headerParagraph.AddText("Encounters");
        //    headerParagraph.Format.Font.Name = "Arial";
        //    headerParagraph.Format.Font.Size = 15;
        //    headerParagraph.Format.Alignment = ParagraphAlignment.Left;
        //    headerParagraph.Format.Font.Bold = true;

        //    Paragraph emptyParagraph = section.AddParagraph();
        //    // Patient details
        //    Table encounterListInfoTable = section.AddTable();
        //    encounterListInfoTable.Borders.Visible = true;
        //    Column encounterListInfoColumn = encounterListInfoTable.AddColumn();
        //    encounterListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
        //    for (int ind = 0; ind < 7; ind++)
        //    {
        //        encounterListInfoColumn = encounterListInfoTable.AddColumn();
        //        encounterListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
        //    }

        //    encounterListInfoTable.Format.Font.Size = 9;
        //    for (int y = 0; y < 8; y++)
        //    {
        //        encounterListInfoTable.Columns[y].Width = 73;
        //    }

        //    MigraDoc.DocumentObjectModel.Tables.Row encounterListInfoRow = encounterListInfoTable.AddRow();
        //    encounterListInfoRow.HeadingFormat = true;
        //    //encounterListInfoRow.Format.Font.Size = 10;
        //    encounterListInfoRow.BottomPadding = 5;
        //    encounterListInfoRow.TopPadding = 5;
        //    encounterListInfoRow.Format.Alignment = ParagraphAlignment.Left;
        //    encounterListInfoRow.Cells[0].AddParagraph("MEMBER NAME").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[1].AddParagraph("DATE OF SERVICE").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[2].AddParagraph("DURATION").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[3].AddParagraph("CARE MANAGER").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[4].AddParagraph("ENCOUNTER TYPE").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[5].AddParagraph("CHIEF COMPLAINT").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[6].AddParagraph("STATUS").Format.Font.Bold = true;
        //    encounterListInfoRow.Cells[7].AddParagraph("UPCOMING APPOINTMENT").Format.Font.Bold = true;

        //    foreach (var obj in printEncounterListModel)
        //    {
        //        encounterListInfoRow = encounterListInfoTable.AddRow();
        //        //encounterListInfoRow.Format.Font.Size = 10;
        //        encounterListInfoRow.BottomPadding = 5;
        //        encounterListInfoRow.TopPadding = 5;
        //        encounterListInfoRow.Format.Alignment = ParagraphAlignment.Left;
        //        encounterListInfoRow.Cells[0].AddParagraph(obj.PatientName != null ? obj.PatientName : "");
        //        encounterListInfoRow.Cells[1].AddParagraph(obj.DateOfService != null ? obj.DateOfService : "");
        //        encounterListInfoRow.Cells[2].AddParagraph(obj.Duration != null ? obj.Duration : "");
        //        encounterListInfoRow.Cells[3].AddParagraph(obj.CareManager != null ? obj.CareManager : "");
        //        encounterListInfoRow.Cells[4].AddParagraph(obj.EncounterType != null ? obj.EncounterType : "");
        //        encounterListInfoRow.Cells[5].AddParagraph(obj.ManualChiefComplaint != null ? obj.ManualChiefComplaint : "");
        //        encounterListInfoRow.Cells[6].AddParagraph(obj.Status != null ? obj.Status : "");
        //        encounterListInfoRow.Cells[7].AddParagraph(obj.NextAppointmentDate != null ? obj.NextAppointmentDate : "");
        //    }

        //    // Create a renderer for the MigraDoc document.
        //    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

        //    // Associate the MigraDoc document with a renderer
        //    pdfRenderer.Document = document;

        //    // Layout and render document to PDF
        //    pdfRenderer.RenderDocument();

        //    tempStream = new MemoryStream();
        //    // Save the document...
        //    //pdf
        //    pdfRenderer.PdfDocument.Save(tempStream, false);

        //    return tempStream;
        //}

        public JsonModel SavePatientEncounterNotes(PatientEncounterNotesModel requestObj, TokenModel token)
        {
            try
            {
                if (requestObj.PatientEncounterId > 0)
                {
                    PatientEncounterNotes patientEncounterNotes = new PatientEncounterNotes();

                    var result = _patientEncounterNotesRepository.Get(x => x.PatientEncounterId == requestObj.PatientEncounterId);
                    if (result == null)
                    {
                        patientEncounterNotes.Notes = requestObj.Notes;
                        patientEncounterNotes.PatientEncounterId = requestObj.PatientEncounterId;
                        patientEncounterNotes.CreatedBy = token.UserID;
                        patientEncounterNotes.CreatedDate = DateTime.UtcNow;
                        patientEncounterNotes.IsActive = true;
                        patientEncounterNotes.IsDeleted = false;
                        patientEncounterNotes.NotesAddedDate = DateTime.UtcNow;
                        patientEncounterNotes.UserId = token.UserID;

                        _patientEncounterNotesRepository.Create(patientEncounterNotes);
                        _patientEncounterNotesRepository.SaveChanges();
                    }
                    return new JsonModel()
                    {
                        data = patientEncounterNotes,
                        Message = Common.HC.Common.StatusMessage.PatientEncounterNotes,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = Common.HC.Common.StatusMessage.EncounterNotesError,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity
                    };
                }
            }
            catch (Exception ex)
            {

                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                };
            }
        }

        public JsonModel GetPatientDiagnosisCodes(int patientId, FilterModel filterModel, TokenModel tokenModel)
        {
            JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            List<PatientEncounterDiagnosisCodesModel> patientEncounterDiagnosisCodesModels = new List<PatientEncounterDiagnosisCodesModel>();
            if (patientId > 0)
            {
                patientEncounterDiagnosisCodesModels = _patientRepository.GetPatientDiagnosisCodes<PatientEncounterDiagnosisCodesModel>(patientId, filterModel).ToList();
            }
            else
                patientEncounterDiagnosisCodesModels = null;
            if (patientEncounterDiagnosisCodesModels != null && patientEncounterDiagnosisCodesModels.Count > 0)
            {
                response = new JsonModel(patientEncounterDiagnosisCodesModels, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new HC.Model.Meta(patientEncounterDiagnosisCodesModels, filterModel);
                return response;
            }

            else
                return response;

        }

        public JsonModel SavePatientEncounterSOAP(PatientEncounterModel requestObj, bool isAdmin, TokenModel token)
        {
            pId = requestObj.PatientID;
            PatientEncounter patientEncounter = null;
            SoapNotes soapNote = null;
            List<PatientEncounterTemplates> patientEncounterTemplates = null;
            List<PatientEncounterTemplates> patientEncounterTemplatesInsertList = null;
            int? appointmentTypeId = _appointmentRepository.GetByID(requestObj.PatientAppointmentId).AppointmentTypeID;
            PatientEncounterCodesMapping CodesMapping = null;
            List<PatientEncounterDiagnosisCodes> ICDCodesList = null;
            List<PatientEncounterServiceCodes> serviceCodesList = null;
            List<PatientEncounterCodesMapping> codesMappingList = null;
            List<PatientEncounterServiceCodes> serviceCodesInsertList = null;
            PatientEncounterServiceCodes serviceCodeObj = null;
            List<PatientEncounterDiagnosisCodes> ICDCodesInsertList = null;
            PatientEncounterDiagnosisCodes ICDCodeObj = null;
            PatientEncounterTemplates patientEncounterTemplateObj = null;
            PatientEncounterNotesModel patientEncounterNotesModel = null;

            //if (requestObj.PatientID != null && requestObj.PatientID > 0 && _patientRepository.CheckAuthorizationSetting())
            //{
            //    string serviceCodesString = string.Join(",", requestObj.PatientEncounterServiceCodes.Where(p => p.Id > 0)
            //                         .Select(p => p.ServiceCode.ToString()));
            //    List<AppointmentAuthModel> authDetails = _patientRepository.CheckServiceCodesAuthorizationForPatient<AppointmentAuthModel>(Convert.ToInt32(requestObj.PatientID), InsurancePlanType.Primary.ToString(), serviceCodesString, requestObj.StartDateTime).ToList();
            //    if (authDetails != null && authDetails.Count > 0 && authDetails.First().AuthorizationMessage.ToLower() != "valid")
            //        return new JsonModel()
            //        {
            //            data = new object(),
            //            Message = authDetails.First().AuthorizationMessage,
            //            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
            //        };
            //}
            LocationModel locationModal = _locationService.GetLocationOffsets(requestObj.ServiceLocationID, token);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (requestObj.Id == 0)
                    {
                        //  if (requestObj.ParentAppointmentId != null && requestObj.ParentAppointmentId != 0)
                        //    CreateAppointment(requestObj, token);

                        patientEncounter = new PatientEncounter(); soapNote = new SoapNotes(); patientEncounterTemplates = new List<PatientEncounterTemplates>();
                        CodesMapping = new PatientEncounterCodesMapping(); ICDCodesList = new List<PatientEncounterDiagnosisCodes>();
                        serviceCodesList = new List<PatientEncounterServiceCodes>(); codesMappingList = new List<PatientEncounterCodesMapping>();
                        AutoMapper.Mapper.Map(requestObj, patientEncounter);
                        patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        AutoMapper.Mapper.Map(requestObj.SOAPNotes, soapNote);
                        AutoMapper.Mapper.Map(requestObj.patientEncounterTemplate, patientEncounterTemplates);
                        AutoMapper.Mapper.Map(requestObj.PatientEncounterServiceCodes, serviceCodesList);
                        AutoMapper.Mapper.Map(requestObj.PatientEncounterDiagnosisCodes, ICDCodesList);
                        patientEncounter.CreatedBy = token.UserID;
                        patientEncounter.OrganizationID = token.OrganizationID;
                        patientEncounter.CreatedDate = DateTime.UtcNow;
                        patientEncounter.IsActive = true;
                        patientEncounter.IsDeleted = false;
                        //mark encounter rendered
                        patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "PENDING" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                        //
                        _patientEncounterRepository.Create(patientEncounter);
                        _patientEncounterRepository.SaveChanges();
                        if (patientEncounter.Id > 0)
                        {
                            //Save Soap Note
                            soapNote.PatientEncounterId = patientEncounter.Id;
                            soapNote.CreatedBy = token.UserID;
                            soapNote.CreatedDate = DateTime.UtcNow;
                            soapNote.IsActive = true; soapNote.IsDeleted = false;
                            _soapNoteRepository.Create(soapNote);
                            _soapNoteRepository.SaveChanges();

                            //Save encounter notes 
                            if (requestObj.Notes != null)
                            {
                                patientEncounterNotesModel = new PatientEncounterNotesModel();
                                patientEncounterNotesModel.PatientEncounterId = patientEncounter.Id;
                                patientEncounterNotesModel.Notes = requestObj.Notes;
                                SavePatientEncounterNotes(patientEncounterNotesModel, token);
                            }

                            //Save patientEncounterTemplates
                            if (patientEncounterTemplates != null && patientEncounterTemplates.Count > 0)
                            {
                                patientEncounterTemplates.ForEach(x =>
                                {
                                    x.OrganizationID = token.OrganizationID;
                                    x.PatientEncounterId = patientEncounter.Id;
                                    x.CreatedBy = token.UserID;
                                    x.IsActive = true;
                                    x.IsDeleted = false;
                                });
                                _patientEncounterTemplateRepository.Create(patientEncounterTemplates.ToArray());
                                _patientEncounterTemplateRepository.SaveChanges();
                            }
                            //Save Encounter Service Codes
                            if (serviceCodesList != null && serviceCodesList.Count > 0)
                            {
                                serviceCodesList.ForEach(x =>
                                {
                                    x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.PatientEncounterId = patientEncounter.Id; x.IsActive = true; x.IsDeleted = false;
                                    if (!_patientRepository.CheckAuthorizationSetting())
                                    { x.AuthProcedureCPTLinkId = null; x.AuthorizationNumber = null; }
                                });
                                _patientEncounterServiceCodesRepository.Create(serviceCodesList.ToArray());
                                _patientEncounterServiceCodesRepository.SaveChanges();
                                if (_patientRepository.CheckAuthorizationSetting())
                                    BlockServiceCodeUnits(token, serviceCodesList);
                            }
                            //Save Encounter ICD Codes
                            if (ICDCodesList != null && ICDCodesList.Count > 0)
                            {
                                ICDCodesList.ForEach(x =>
                                {
                                    x.Id = 0;
                                    //if (x.Id == 0)
                                    //{
                                    x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.PatientEncounterId = patientEncounter.Id; x.IsActive = true; x.IsDeleted = false;
                                    //}
                                    //else
                                    //{
                                    //    x.UpdatedBy = token.UserID;
                                    //    x.UpdatedDate = DateTime.UtcNow;
                                    //}
                                    // _patientEncounterICDCodesRepository.SaveChanges();
                                });
                                //if (ICDCodesList != null && ICDCodesList.Where(x => x.Id > 0).Count() > 0)
                                //{
                                //    _patientEncounterICDCodesRepository.Update(ICDCodesList.Where(x => x.Id > 0).ToArray());
                                //    _patientEncounterICDCodesRepository.SaveChanges();

                                //}
                                //if (ICDCodesList != null && ICDCodesList.Where(x => x.Id == 0).Count() > 0)
                                //{
                                //    _patientEncounterICDCodesRepository.Create(ICDCodesList.Where(x => x.Id == 0).ToArray());
                                //    _patientEncounterICDCodesRepository.SaveChanges();

                                //}
                                _patientEncounterICDCodesRepository.Create(ICDCodesList.ToArray());
                                _patientEncounterICDCodesRepository.SaveChanges();
                            }
                            //Save Codes Mappings
                            CreateEncounterCodesMappings(token, patientEncounter, ICDCodesList, serviceCodesList, codesMappingList);
                        }
                    }
                    else
                    {
                        //#region check config settings
                        //List<AppConfigurations> appCon = _context.AppConfigurations.Where(a => a.OrganizationID == token.OrganizationID).ToList();
                        //foreach (var item in appCon)
                        //{

                        //    if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.CLINICIAN_SIGN.ToString() && item.Value == "true" && (requestObj.ClinicianSign == null || requestObj.ClinicianSign.Length == 0 || requestObj.ClinicianSignDate == null))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Clinician sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };
                        //    }
                        //    else if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.PATIENT_SIGN.ToString() && item.Value == "true" && (requestObj.PatientSign == null || requestObj.PatientSign.Length == 0 || requestObj.PatientSignDate == null))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Patient sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };

                        //    }
                        //    else if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.GUARDIAN_SIGN.ToString() && item.Value == "true" && (requestObj.GuardianSign == null || requestObj.GuardianSign.Length == 0 || requestObj.GuardianSignDate == null || string.IsNullOrEmpty(requestObj.GuardianName)))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Guardian sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };
                        //    }
                        //}
                        //#endregion
                        patientEncounter = _patientEncounterRepository.Get(x => x.Id == requestObj.Id && x.IsActive == true && x.IsDeleted == false);
                        if (!ReferenceEquals(patientEncounter, null))
                        {
                            //Update patient encounter
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            //patientEncounter.ClinicianSign = requestObj.ClinicianSign;
                            //patientEncounter.ClinicianSignDate = requestObj.ClinicianSignDate;
                            //patientEncounter.PatientSign = requestObj.PatientSign;
                            //patientEncounter.PatientSignDate = requestObj.PatientSignDate;
                            //patientEncounter.GuardianSign = requestObj.GuardianSign;
                            //patientEncounter.GuardianSignDate = requestObj.GuardianSignDate;
                            //patientEncounter.GuardianName = requestObj.GuardianName;
                            patientEncounter.AppointmentTypeId = appointmentTypeId;
                            patientEncounter.CustomAddressID = requestObj.CustomAddressID;
                            patientEncounter.CustomAddress = requestObj.CustomAddress;
                            patientEncounter.PatientAddressID = requestObj.PatientAddressID;
                            patientEncounter.OfficeAddressID = requestObj.OfficeAddressID;
                            patientEncounter.ServiceLocationID = requestObj.ServiceLocationID;
                            patientEncounter.StaffID = requestObj.StaffID;

                            //TODO
                            //_imageService.SaveImages(requestObj.ClinicianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.ClinicianSign.ToString());
                            //_imageService.SaveImages(requestObj.GuardianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.GuardianSign.ToString());
                            //_imageService.SaveImages(requestObj.PatientSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.PatientSign.ToString());


                            //mark encounter rendered
                            patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                            //
                            patientEncounter.UpdatedBy = token.UserID;
                            patientEncounter.UpdatedDate = DateTime.UtcNow;
                            _patientRepository.SaveChanges();
                            //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                            if (requestObj.SOAPNotes != null)
                            {
                                //Update patient soap note
                                soapNote = _soapNoteRepository.Get(x => x.Id == requestObj.SOAPNotes.Id && x.IsDeleted == false && x.IsActive == true);
                                if (!ReferenceEquals(soapNote, null))
                                {
                                    soapNote.Subjective = string.IsNullOrEmpty(requestObj.SOAPNotes.Subjective) ? null : requestObj.SOAPNotes.Subjective;
                                    soapNote.Objective = string.IsNullOrEmpty(requestObj.SOAPNotes.Objective) ? null : requestObj.SOAPNotes.Objective;
                                    soapNote.Assessment = string.IsNullOrEmpty(requestObj.SOAPNotes.Assessment) ? null : requestObj.SOAPNotes.Assessment;
                                    soapNote.Plans = string.IsNullOrEmpty(requestObj.SOAPNotes.Plans) ? null : requestObj.SOAPNotes.Plans;
                                    soapNote.UpdatedBy = token.UserID;
                                    soapNote.UpdatedDate = DateTime.UtcNow;
                                    _soapNoteRepository.Update(soapNote);
                                    _soapNoteRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                            }
                            if (requestObj.PatientEncounterServiceCodes != null)
                            {
                                if (isAdmin)
                                {
                                    UpdatePreviousServiceCodesAndMappings(requestObj, token);
                                }
                                //List to insert new cptcodes
                                serviceCodesInsertList = new List<PatientEncounterServiceCodes>();
                                serviceCodesList = _patientEncounterServiceCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterServiceCodesModel serviceCodeModel in requestObj.PatientEncounterServiceCodes)
                                {
                                    if (serviceCodeModel.Id > 0)
                                    {
                                        //Update service codes if exist in Soap
                                        serviceCodeObj = serviceCodesList.Find(x => x.Id == serviceCodeModel.Id);
                                        if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == true)
                                        {
                                            serviceCodeObj.IsDeleted = serviceCodeModel.IsDeleted;
                                            serviceCodeObj.DeletedDate = DateTime.UtcNow;
                                            serviceCodeObj.DeletedBy = token.UserID;
                                        }
                                        else if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == false)
                                        {
                                            serviceCodeObj.Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1;
                                            serviceCodeObj.Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2;
                                            serviceCodeObj.Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3;
                                            serviceCodeObj.Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4;
                                            serviceCodeObj.UpdatedBy = token.UserID;
                                            serviceCodeObj.UpdatedDate = DateTime.UtcNow;
                                        }
                                    }
                                    else
                                    {
                                        //Insert new cptcodes to soap
                                        serviceCodeObj = new PatientEncounterServiceCodes()
                                        {
                                            ServiceCodeId = serviceCodeModel.ServiceCodeId,
                                            Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1,
                                            Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2,
                                            Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3,
                                            Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4,
                                            AuthorizationNumber = (_patientRepository.CheckAuthorizationSetting() == false || string.IsNullOrEmpty(serviceCodeModel.AuthorizationNumber)) ? null : serviceCodeModel.AuthorizationNumber,
                                            AuthProcedureCPTLinkId = (_patientRepository.CheckAuthorizationSetting() == false || serviceCodeModel.AuthProcedureCPTLinkId == null || serviceCodeModel.AuthProcedureCPTLinkId == 0) ? null : serviceCodeModel.AuthProcedureCPTLinkId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        serviceCodesInsertList.Add(serviceCodeObj);
                                    }
                                }
                                if (serviceCodesList != null && serviceCodesList.Count() > 0)
                                {
                                    _patientEncounterServiceCodesRepository.Update(serviceCodesList.ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (serviceCodesInsertList != null && serviceCodesInsertList.Count > 0)
                                {
                                    _patientEncounterServiceCodesRepository.Create(serviceCodesInsertList.ToArray());
                                    _patientEncounterServiceCodesRepository.SaveChanges();
                                    if (_patientRepository.CheckAuthorizationSetting())
                                        BlockServiceCodeUnits(token, serviceCodesInsertList);
                                }
                            }
                            if (requestObj.PatientEncounterDiagnosisCodes != null && requestObj.PatientEncounterDiagnosisCodes.Count > 0)
                            {
                                ICDCodesInsertList = new List<PatientEncounterDiagnosisCodes>();
                                ICDCodesList = _patientEncounterICDCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterDiagnosisCodesModel ICDCodeModel in requestObj.PatientEncounterDiagnosisCodes)
                                {
                                    if (ICDCodeModel.Id > 0 && ICDCodeModel.IsDeleted == true)
                                    {
                                        //Update service codes if exist in Soap
                                        ICDCodeObj = ICDCodesList.Find(x => x.Id == ICDCodeModel.Id);
                                        if (!ReferenceEquals(ICDCodeObj, null))
                                        {
                                            ICDCodeObj.IsDeleted = ICDCodeModel.IsDeleted;
                                            ICDCodeObj.DeletedDate = DateTime.UtcNow;
                                            ICDCodeObj.DeletedBy = token.UserID;
                                        }
                                    }
                                    else if (ICDCodeModel.Id == 0)
                                    {
                                        //Insert new cptcodes to soap
                                        ICDCodeObj = new PatientEncounterDiagnosisCodes()
                                        {
                                            ICDCodeId = ICDCodeModel.ICDCodeId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        ICDCodesInsertList.Add(ICDCodeObj);
                                    }
                                }
                                if (ICDCodesList != null && ICDCodesList.Where(x => x.IsDeleted == true).Count() > 0)
                                {
                                    _patientEncounterICDCodesRepository.Update(ICDCodesList.Where(x => x.IsDeleted == true).ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0)
                                {
                                    _patientEncounterICDCodesRepository.Create(ICDCodesInsertList.ToArray());
                                    _patientEncounterICDCodesRepository.SaveChanges();
                                }
                                // if ((ICDCodesList != null && ICDCodesList.Where(x => x.IsDeleted == true).Count() > 0) || (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0))
                            }

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (requestObj.patientEncounterTemplate != null && requestObj.patientEncounterTemplate.Count > 0)
                            {
                                patientEncounterTemplatesInsertList = new List<PatientEncounterTemplates>();
                                patientEncounterTemplates = _patientEncounterTemplateRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterTemplateModel patientEncounterTemplateModel in requestObj.patientEncounterTemplate)
                                {
                                    if (patientEncounterTemplateModel.Id > 0 && patientEncounterTemplateModel.IsDeleted == true)
                                    {
                                        //Update service codes if exist in Soap
                                        patientEncounterTemplateObj = patientEncounterTemplates.Find(x => x.Id == patientEncounterTemplateModel.Id);
                                        if (!ReferenceEquals(patientEncounterTemplateObj, null))
                                        {
                                            patientEncounterTemplateObj.IsDeleted = patientEncounterTemplateModel.IsDeleted;
                                            patientEncounterTemplateObj.DeletedDate = DateTime.UtcNow;
                                            patientEncounterTemplateObj.DeletedBy = token.UserID;
                                        }
                                    }
                                    else if (patientEncounterTemplateModel.Id == 0)
                                    {
                                        //Insert new cptcodes to soap
                                        patientEncounterTemplateObj = new PatientEncounterTemplates()
                                        {
                                            OrganizationID = token.OrganizationID,
                                            TemplateData = patientEncounterTemplateModel.TemplateData,
                                            MasterTemplateId = patientEncounterTemplateModel.MasterTemplateId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        patientEncounterTemplatesInsertList.Add(patientEncounterTemplateObj);
                                    }
                                }

                                if (patientEncounterTemplates != null && patientEncounterTemplates.Where(x => x.IsDeleted == true).Count() > 0)
                                {
                                    _patientEncounterTemplateRepository.Update(patientEncounterTemplates.Where(x => x.IsDeleted == true).ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (patientEncounterTemplatesInsertList != null && patientEncounterTemplatesInsertList.Count > 0)
                                {
                                    _patientEncounterTemplateRepository.Create(patientEncounterTemplatesInsertList.ToArray());
                                    _patientEncounterTemplateRepository.SaveChanges();
                                }
                            }
                            ///////////////////////////////////////////////////////////////////////////
                            if (serviceCodesInsertList != null && serviceCodesInsertList.Count > 0)
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesList, serviceCodesInsertList, codesMappingList);
                            if (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0)
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesInsertList, serviceCodesList, codesMappingList);
                            if ((serviceCodesInsertList != null && serviceCodesInsertList.Count > 0) && (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0))
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesInsertList, serviceCodesInsertList, codesMappingList);
                        }
                    }
                    SaveTimesheetData(token, patientEncounter);
                    SaveDriveTimeData(token, patientEncounter);
                    //transaction.Rollback();
                    transaction.Commit();
                    return new JsonModel()
                    {
                        data = patientEncounter,
                        Message = Common.HC.Common.StatusMessage.SoapSuccess,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }


        public JsonModel CheckPatientEligibility(PatientEligibilityRequestModel eligibilityRequestModel, TokenModel tokenModel)
        {
            bool isEligible = false;
            JsonModel response = new JsonModel(isEligible, StatusMessage.PatientNotEligible, (int)HttpStatusCode.NotFound);
            
            try
            {
                //Check if insurance details exists
                int insurancePlanTypeID = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.INSURANCEPLANTYPE, "primary", tokenModel, true);
                PatientInsuranceDetails patientInsurances = _context.PatientInsuranceDetails.Where(a => a.PatientID == eligibilityRequestModel.ClientId && a.InsurancePlanTypeID == insurancePlanTypeID  && a.IsDeleted == false && a.IsActive == true).FirstOrDefault();
                if (patientInsurances != null)
                {
                    switch (eligibilityRequestModel.ApiId)
                    {
                        case 1:
                            isEligible = CheckPatientEligibility(eligibilityRequestModel.ClientId, tokenModel);
                            break;
                        case 2:
                            isEligible = CheckPatientEligibilityAB2D(eligibilityRequestModel.ClientId, tokenModel);
                            break;
                        case 3:
                            isEligible = CheckPatientEligibilityDPC(eligibilityRequestModel.ClientId, eligibilityRequestModel.AuthToken, tokenModel);
                            break;
                        case 4:
                            isEligible = CheckPatientEligibilityBCDA(eligibilityRequestModel.ClientId, tokenModel);
                            break;
                        default:
                            break;
                    }

                    if (isEligible)
                        response = new JsonModel(isEligible, StatusMessage.PatientEligible, (int)HttpStatusCodes.OK);
                }
                else
                    response = new JsonModel(isEligible, StatusMessage.PatientInsuranceDataNotFound, (int)HttpStatusCodes.NotFound);
            }
            catch
            {
                return new JsonModel(false, StatusMessage.InternalServerError, (int)HttpStatusCodes.InternalServerError);
            }

            return response;
        }

        public JsonModel SendBBIntructionsMail(BlueButtonModel blueButtonModel, TokenModel token)
        {
            Patients patient = _context.Patients.Where(a => a.Id == blueButtonModel.ClientId && a.IsDeleted == false).FirstOrDefault();
            PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB != null ? patient.DOB : null, patient.Email, patient.SSN, null, null, null, null, null, null, null, null).FirstOrDefault();
            var error = SendBBInstructionEmail(pHIDecryptedModel, pHIDecryptedModel.EmailAddress,
                                  pHIDecryptedModel.FirstName + " " + pHIDecryptedModel.LastName,
                                  (int)EmailType.PatientBlueButton,
                                  (int)EmailSubType.PatientBlueButtonInstructions,
                                  blueButtonModel.ClientId,
                                  "/templates/client-BBInstructions.html",
                                  "Sync your data with Medicare",
                                  token,
                                  blueButtonModel.LoginUrl
                                  );
            //if (string.IsNullOrEmpty(error))
            //{
                //Enable BB sync button for patient to get access Medicare data
                patient.IsBBEnabled = true;
                _patientRepository.Update(patient);
                _patientRepository.SaveChanges();

                response = new JsonModel(true, StatusMessage.BBMailSuccess, (int)HttpStatusCodes.OK);
            //}
            //else
            //{
                //response = new JsonModel(false, StatusMessage.InternalServerError, (int)HttpStatusCodes.InternalServerError);
           // }

            return response;


        }

        private bool CheckPatientEligibility(int pId, TokenModel token)
        {
            bool isEligible = false;
            PatientInsuranceDetails patientInsuranceDetails = _patientInsuranceRepository.Get(x => x.PatientID == pId && x.IsActive == true && x.IsDeleted == false);
            if (patientInsuranceDetails != null)
            {
                Patients patient = _context.Patients.Where(a => a.Id == pId && a.IsDeleted == false && a.IsActive == true && a.OrganizationID == token.OrganizationID).FirstOrDefault();
                isEligible = patient.IsEligible;
            }
            return isEligible;
        }

        private bool CheckPatientEligibilityBCDA(int pId, TokenModel token)
        {
            bool isEligible = false;
            string uToken = string.Empty;
            string contentURL = string.Empty;
            BCDAModel bCDAModel = null;
            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                               EligibilityTokens.BCDAClientID + ":" + EligibilityTokens.BCDAClientSecret));
            uToken = GetAuthenticationTokenBCDAandAB2D(EligibiltyAPIs.AuthTokenBCDA, base64authorization);
            if (!string.IsNullOrEmpty(uToken))
            {
                //Task task = Task.Factory.StartNew(() =>
                //{
                    contentURL = ExportJobIDBCDA(uToken, EligibiltyAPIs.ExportJobBCDA).Result;
                //});

                //task.Wait();

                if (!string.IsNullOrEmpty(contentURL))
                {
                    //Task task2 = Task.Factory.StartNew(() =>
                    //{
                        bCDAModel = GetJobStatusBCDA(contentURL, uToken).Result;
                    //});

                    //task2.Wait();
                    
                    if (bCDAModel != null)
                    {
                        List<BCDAResponseModel> response = GetPatientDataBCDA(bCDAModel, uToken);

                        if (response != null && response.Count > 0)
                        {
                            BCDAResponseModel bCDAResponse = new BCDAResponseModel();
                            Patients patient = _context.Patients.Where(a => a.Id == pId && a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).FirstOrDefault();
                            PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB != null ? patient.DOB : null, patient.Email, patient.SSN, null, null, null, null, null, null, null, patient.Identifier).FirstOrDefault();
                            if (patient != null)
                            {
                                if (!string.IsNullOrEmpty(pHIDecryptedModel.HealthPlanBeneficiaryNumber))
                                {
                                    bCDAResponse = response.Where(x => x.id == pHIDecryptedModel.HealthPlanBeneficiaryNumber).FirstOrDefault();
                                    if (bCDAResponse != null)
                                    {
                                        isEligible = true;
                                    }
                                }
                                else
                                {
                                    bCDAResponse = response.Where(x => Convert.ToDateTime(x.birthDate) == Convert.ToDateTime(pHIDecryptedModel.DateOfBirth) && (x.name.Select(x => x.family)).ToString().ToLower() == pHIDecryptedModel.LastName.ToLower() && (x.name.Select(x => x.given[0])).ToString().ToLower() == pHIDecryptedModel.FirstName.ToLower()).FirstOrDefault();
                                    if (bCDAResponse != null)
                                    {
                                        isEligible = true;
                                    }
                                }

                                patient.IsEligible = isEligible;
                                _patientRepository.Update(patient);
                                _patientRepository.SaveChanges();
                            }


                        }
                    }
                }
            }
            return isEligible;
        }

        private bool CheckPatientEligibilityAB2D(int pId, TokenModel token)
        {
            bool isEligible = false;
            string uToken = string.Empty;
            string contentURL = string.Empty;
            BCDAModel bCDAModel = null;
            uToken = GetAuthenticationTokenBCDAandAB2D($"{EligibiltyAPIs.AuthTokenAB2D}grant_type=client_credentials&scope=clientCreds", EligibilityTokens.AB2DBase64EncodedCred);
            if (!string.IsNullOrEmpty(uToken))
            {
                //Task task = Task.Factory.StartNew(() =>
                //{
                    contentURL = ExportJobIDAB2D(uToken, EligibiltyAPIs.ExportJobIDAB2D);
                //});

                //task.Wait();
                
                if (!string.IsNullOrEmpty(contentURL))
                {
                    Task task2 = Task.Factory.StartNew(() =>
                    {
                        bCDAModel = GetJobStatusAB2D(contentURL, uToken);
                    });

                    task2.Wait();

                    if (bCDAModel != null)
                    {
                        List<AB2DEOBModel> response = GetPatientDataAB2D(bCDAModel, uToken);

                        if (response.Count > 0)
                        {
                            AB2DEOBModel bCDAResponse = new AB2DEOBModel();
                            Patients patient = _context.Patients.Where(a => a.Id == pId && a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).FirstOrDefault();
                            PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB != null ? patient.DOB : null, patient.Email, patient.SSN, null, null, null, null, null, null, null, patient.Identifier).FirstOrDefault();
                            if (patient != null)
                            {
                                if (!string.IsNullOrEmpty(pHIDecryptedModel.HealthPlanBeneficiaryNumber))
                                {
                                    bCDAResponse = response.Where(x => x.patient.reference.Replace("Patient/", string.Empty).ToString() == pHIDecryptedModel.HealthPlanBeneficiaryNumber).FirstOrDefault();
                                    if (bCDAResponse != null)
                                    {
                                        isEligible = true;
                                    }
                                }

                                patient.IsEligible = isEligible;
                                _patientRepository.Update(patient);
                                _patientRepository.SaveChanges();
                            }


                        }
                    }
                }
                }
            return isEligible;
        }

        private string GetAuthenticationTokenBCDAandAB2D(string requestURL, string base64authorization)
        {
            string uToken = string.Empty;
            var client = new RestClient(requestURL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Basic {base64authorization}");
            IRestResponse response = client.Execute(request);
            var userObj = response.Content == null ? null : JObject.Parse(response.Content);
            uToken = (userObj == null) ? string.Empty : Convert.ToString(userObj["access_token"]);
            return uToken;
        }

        private async Task<string> ExportJobIDBCDA(string token, string requestURL)
        {
            string contentURL = string.Empty;
            var client = new RestClient(requestURL);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/fhir+json");
            request.AddHeader("Prefer", "respond-async");
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Headers.Any(t => t.Name == "Content-Location"))
            {
                contentURL =
                response.Headers.FirstOrDefault(t => t.Name == "Content-Location").Value.ToString();
            }
            return contentURL;
        }

        private async Task<BCDAModel> GetJobStatusBCDA(string contentURL, string token)
        {
            BCDAModel bCDAModel = new BCDAModel();
            var client = new RestClient(contentURL);
            client.Timeout = -1;
            IRestResponse result = null;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("accept", "application/fhir+json");
            request.AddHeader("ContentType", "application/fhir+json");
            do
            {
                result = client.Execute(request);
            }
            while (result.StatusCode != HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.BadRequest);
            Console.WriteLine(result.Content);
            bCDAModel = JsonConvert.DeserializeObject<BCDAModel>(result.Content);

            //BCDAModel bCDAModel = new BCDAModel();
            //HttpWebResponse httpResponse = null;
            //var httpRequest = (HttpWebRequest)WebRequest.Create(contentURL);

            //httpRequest.Headers["Authorization"] = $"Bearer {token}";
            //httpRequest.Accept = "application/fhir+json";
            //httpRequest.ContentType = "application/fhir+json";

            //do
            //   httpResponse =  (HttpWebResponse)httpRequest.GetResponse();
            //while (httpResponse.StatusCode != HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.Unauthorized || httpResponse.StatusCode == HttpStatusCode.BadRequest);
            

            //if ((int)httpResponse.StatusCode == 200)
            //{
            //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //    {
            //        string result = streamReader.ReadToEnd();
            //        bCDAModel = JsonConvert.DeserializeObject<BCDAModel>(result);
            //    }
            //}

            return bCDAModel;
        }

        private List<BCDAResponseModel> GetPatientDataBCDA(BCDAModel bCDA, string token)
        {
            List<BCDAResponseModel> response = null;
            if (bCDA != null && bCDA.output != null)
            {
                var requestURL = bCDA.output[0].url;
                var httpRequest = (HttpWebRequest)WebRequest.Create(requestURL);
                httpRequest.Headers["Authorization"] = $"Bearer {token}";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if ((int)httpResponse.StatusCode == 200)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();

                        // Add commas between the list of objects...
                        var data = result.Split("}\n{").Join("},{");

                        // ...remove all carriage return line feeds...
                        var data1 = '[' + data.Split("\r\n").Join("|") + ']';

                        response = JsonConvert.DeserializeObject<List<BCDAResponseModel>>(data1);
                    }
                }
            }

            return response;
        }

        private List<AB2DEOBModel> GetPatientDataAB2D(BCDAModel bCDA, string token)
        {
            List<AB2DEOBModel> response = null;
            if (bCDA != null && bCDA.output != null)
            {
                var requestURL = bCDA.output[0].url;
                var httpRequest = (HttpWebRequest)WebRequest.Create(requestURL);
                httpRequest.Headers["Authorization"] = $"Bearer {token}";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if ((int)httpResponse.StatusCode == 200)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();

                        // Add commas between the list of objects...
                        var data = result.Split("}\n{").Join("},{");

                        // ...remove all carriage return line feeds...
                        var data1 = '[' + data.Split("\r\n").Join("|") + ']';

                        response = JsonConvert.DeserializeObject<List<AB2DEOBModel>>(data1);
                    }
                }
            }

            return response;
        }
        private string ExportJobIDAB2D(string token, string requestURL)
        {
            string contentURL = string.Empty;
            var client = new RestClient(requestURL);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "*/*");
            request.AddHeader("Prefer", "respond-async");
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Headers.Any(t => t.Name == "Content-Location"))
            {
                contentURL =
                response.Headers.FirstOrDefault(t => t.Name == "Content-Location").Value.ToString();
            }
            return contentURL;
        }

        private BCDAModel GetJobStatusAB2D(string contentURL, string token)
        {
            BCDAModel bCDAModel = new BCDAModel();
            var client = new RestClient(contentURL);
            client.Timeout = -1;
            IRestResponse result = null;
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");
            do
            {
                result = client.Execute(request);
            }
            while (result.StatusCode != HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.BadRequest);
            Console.WriteLine(result.Content);
            bCDAModel = JsonConvert.DeserializeObject<BCDAModel>(result.Content);

            return bCDAModel;
        }

        private string SendBBInstructionEmail(PHIDecryptedModel patientData, string toEmail, string username, int emailType, int emailSubType, int primaryId, string templatePath, string subject, TokenModel tokenModel, string loginUrl)
        {
            //Get Current Login User Organization
            tokenModel.Request.Request.Headers.TryGetValue("BusinessToken", out StringValues businessName);
            Entity.Organization organization = _tokenService.GetOrganizationByOrgId(tokenModel.OrganizationID, tokenModel);

            //Get Current User Smtp Details
            OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
            AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
            organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + templatePath);

            var hostingServer = _configuration.GetSection("HostingServer").Value;
            emailHtml = emailHtml.Replace("{{img_url}}", hostingServer + "img/cbimage.jpg");
            emailHtml = emailHtml.Replace("{{username}}", username);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            //emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            emailHtml = emailHtml.Replace("{{patientName}}", username);
            emailHtml = emailHtml.Replace("{{portalUrl}}", loginUrl);


            EmailModel emailModel = new EmailModel
            {
                EmailBody = CommonMethods.Encrypt(emailHtml),
                ToEmail = CommonMethods.Encrypt(toEmail),
                EmailSubject = CommonMethods.Encrypt(subject),
                EmailType = emailType,
                EmailSubType = emailSubType,
                PrimaryId = primaryId,
                CreatedBy = tokenModel.UserID
            };
            var error = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            if (!string.IsNullOrEmpty(error))
                emailModel.EmailStatus = false;
            else
                emailModel.EmailStatus = true;
            //Maintain Email log into Db
            var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
            return error;
        }

private bool CheckPatientEligibilityDPC(int pId, string jwt, TokenModel token)
        {
            string userToken = string.Empty;
            bool isEligible = false;

            Patients patients = _context.Patients.Where(a => a.Id == pId && a.IsActive == true && a.IsDeleted == false && a.OrganizationID == token.OrganizationID).FirstOrDefault();
            if (patients != null)
            {
                PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB != null ? patients.DOB : null, patients.Email, patients.SSN, null, null, null, null, null, null, null, patients.Identifier).FirstOrDefault();
                if (!string.IsNullOrEmpty(pHIDecryptedModel.HealthPlanBeneficiaryNumber))
                {
                    userToken = _organizationService.GetDPCAuthToken(jwt);

                    var url = EligibiltyAPIs.PatientDPC + pHIDecryptedModel.HealthPlanBeneficiaryNumber;

                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                    httpRequest.Headers["Authorization"] = $"Bearer {userToken}";
                    httpRequest.Accept = "application/fhir+json";
                    httpRequest.ContentType = "application/fhir+json";


                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    if ((int)httpResponse.StatusCode == 200)
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            string result = streamReader.ReadToEnd();
                            var finalResult = JsonConvert.DeserializeObject<PatientBBDataModel>(result);
                            if (finalResult.total > 0)
                            {
                                isEligible = true;
                            }
                            else
                            {
                                isEligible = false;
                            }
                            patients.IsEligible = isEligible;
                            _patientRepository.Update(patients);
                            _patientRepository.SaveChanges();
                        }
                    }
                }
            }

            return isEligible;
        }
    }
}