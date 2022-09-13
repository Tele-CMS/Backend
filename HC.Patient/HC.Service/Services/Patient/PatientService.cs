using CreateClinicalReport;
using CreateClinicalReport.Model;
using HC.Common;
using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AppointmentTypes;
using HC.Patient.Model.Common;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Eligibility;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Staff;
using HC.Patient.Model.SymptomChecker;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientService : BaseService, IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly HCOrganizationContext _context;
        private readonly IPatientAuthorizationRepository _patientAuthorizationRepository;
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IEmailService _emailSender;
        private readonly IHostingEnvironment _env;
        private readonly ITokenService _tokenService;
        private readonly IPatientDiagnosisService _patientDiagnosisService;
        private readonly IPatientTagRepository _patientTagRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IPatientInsuranceRepository _patientInsuranceRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IConfiguration _configuration;


        private JsonModel response = new JsonModel();

        public PatientService(IPatientRepository patientRepository, IPatientAuthorizationRepository patientAuthorizationRepository, 
            HCOrganizationContext context, IImageService imageService, IUserRepository userRepository, 
            IOrganizationSMTPRepository organizationSMTPRepository, IEmailService emailSender, IHostingEnvironment env, 
            ITokenService tokenService, IPatientDiagnosisService patientDiagnosisService, IPatientTagRepository patientTagRepository,
            IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository, IPatientInsuranceRepository patientInsuranceRepository, IGlobalCodeService globalCodeService
            , IConfiguration configuration)
        {
            this._patientRepository = patientRepository;
            _patientAuthorizationRepository = patientAuthorizationRepository;
            _context = context;
            _imageService = imageService;
            _userRepository = userRepository;
            _organizationSMTPRepository = organizationSMTPRepository;
            _emailSender = emailSender;
            _env = env;
            _tokenService = tokenService;
            _patientDiagnosisService = patientDiagnosisService;
            _patientTagRepository = patientTagRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _patientInsuranceRepository = patientInsuranceRepository;
            _globalCodeService = globalCodeService;
            _configuration = configuration; 
        }
        //Need to remove coz Conevrted following method
        public List<PatientModel> GetPatientsByTags(string tags, string startWith = "", int? locationID = null, bool? isActive = null)
        {
            return _patientRepository.GetPatientsByTags<PatientModel>(tags, startWith, locationID, isActive).ToList();
        }
        public JsonModel GetPatientByTags(ListingFiltterModel patientFiltterModel, TokenModel token)
        {
            try
            {
                response.data = new object();
                response.Message = StatusMessage.ErrorOccured;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                string tags = patientFiltterModel.Tags;
                string startWith = patientFiltterModel.StartWith;
                bool? isActive = !string.IsNullOrEmpty(patientFiltterModel.IsActive) ? (bool?)Convert.ToBoolean(patientFiltterModel.IsActive) : null;
                int? locationID = !string.IsNullOrEmpty(patientFiltterModel.LocationIDs) ? (int?)Convert.ToInt32(patientFiltterModel.LocationIDs) : null;

                List<PatientModel> patients = _patientRepository.GetPatientsByTags<PatientModel>(tags, startWith, locationID, isActive).ToList();
                if (patients != null && patients.Count > 0)
                {
                    patients.ForEach(a =>
                    {
                        if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                            a.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.PhotoThumbnailPath);
                        else
                            a.PhotoThumbnailPath = "";
                    });

                    response.data = patients;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.Created;
                    return response;
                }
                //not found
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                /////
            }
            catch (Exception e)
            {
                response.AppError = e.Message;
            }
            return response;
        }
        public JsonModel GetPatientGuarantor(int patientId, TokenModel token)
        {
            try
            {
                List<PatientGuarantorModel> listPatientGuarantor = _patientRepository.GetPatientGuarantor<PatientGuarantorModel>(patientId, token).ToList();
                if (listPatientGuarantor != null && listPatientGuarantor.Count > 0)
                {
                    response.data = listPatientGuarantor;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCodes.OK;
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCodes.NotFound;
                }
            }
            catch (Exception e)
            {
                response.data = new object();
                response.Message = StatusMessage.ServerError;
                response.StatusCode = (int)HttpStatusCodes.InternalServerError;
                response.AppError = e.Message;
            }
            return response;
        }

        //full detail with all foreign table data
        public JsonModel GetPatientsDetails(int PatientID, TokenModel token)
        {
            try
            {
                PatientInfoDetails patientDetails = _patientRepository.GetPatientsDetails(PatientID, token);
                if (patientDetails.PatientInfo != null && patientDetails.PatientInfo.Count > 0 && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoPath) && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoThumbnailPath))
                {
                    patientDetails.PatientInfo[0].PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoPath);
                    patientDetails.PatientInfo[0].PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoThumbnailPath);
                }
                //add staff image's url in model
                if (patientDetails.UpcomingAppointmentDetails != null && patientDetails.UpcomingAppointmentDetails.Count() > 0)
                {
                    patientDetails.UpcomingAppointmentDetails.ForEach(a =>
                    {
                        // a.UpcomingAppointment = CommonMethods.ConvertFromUtcTime(a.UpcomingAppointment, token);
                        if (!string.IsNullOrEmpty(a.StaffImageUrl))
                            a.StaffImageUrl = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.StaffImageUrl);
                        else a.StaffImageUrl = string.Empty;
                    });
                }
                if (patientDetails.LastAppointmentDetails != null && patientDetails.LastAppointmentDetails.Count() > 0)
                {
                    patientDetails.LastAppointmentDetails.ForEach(a =>
                    {
                        //a.LastAppointment = CommonMethods.ConvertFromUtcTime(a.LastAppointment, token);
                        if (!string.IsNullOrEmpty(a.StaffImageUrl))
                            a.StaffImageUrl = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.StaffImageUrl);
                        else a.StaffImageUrl = string.Empty;
                    });
                }
                return response = new JsonModel()
                {
                    data = patientDetails,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }

        }
        /// <summary>
        /// get patient details for mobile
        /// </summary>
        /// <param name="PatientID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel GetPatientsDetailsForMobile(int PatientID, TokenModel token)
        {
            try
            {
                PatientInfoDetails patientDetails = _patientRepository.GetPatientsDetails(PatientID, token);
                if (patientDetails.PatientInfo != null && patientDetails.PatientInfo.Count > 0 && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoPath) && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoThumbnailPath))
                {
                    patientDetails.PatientInfo[0].PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoPath);
                    patientDetails.PatientInfo[0].PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoThumbnailPath);
                }
                PatientInfo patientInfo = new PatientInfo();
                foreach (var i in patientDetails.PatientInfo)
                {
                    patientInfo = i;
                }
                //
                PatientAddress patientAddressInDb = _context.PatientAddress.Where(x => x.PatientID == PatientID && x.AddressTypeID == 20).FirstOrDefault();
                //
                PhoneNumbers phoneNumberInDb = _context.PhoneNumbers.Where(x => x.PhoneNumberTypeId == 4 && x.PatientID == PatientID).FirstOrDefault();
                //
                if (patientAddressInDb != null && phoneNumberInDb != null)
                {
                    PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(null, null, null, null, null, null, null, null, patientAddressInDb.Address1, null, null, null, phoneNumberInDb.PhoneNumber, null).FirstOrDefault();
                    patientInfo.Address = pHIDecryptedModel.Address1;
                    patientInfo.StateId = patientAddressInDb.StateID;
                    patientInfo.Phone = pHIDecryptedModel.Phonenumber;
                }
                else
                {
                    patientInfo.Address = "";
                    patientInfo.Phone = patientDetails.PatientInfo[0].PatientEmergencyContactPhone;
                }

                return response = new JsonModel()
                {
                    data = patientInfo,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };

            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }

        }

        /// <summary>
        /// get patient header info
        /// </summary>
        /// <param name="PatientID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel GetPatientHeaderInfo(int PatientID, TokenModel token)
        {
            try
            {
                PatientHeaderModel patientHeaderModel = _patientRepository.GetPatientHeaderInfo(PatientID, token);
                if (patientHeaderModel.PatientBasicHeaderInfo != null)
                {
                    patientHeaderModel.PatientBasicHeaderInfo.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientHeaderModel.PatientBasicHeaderInfo.PhotoPath);
                    patientHeaderModel.PatientBasicHeaderInfo.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientHeaderModel.PatientBasicHeaderInfo.PhotoThumbnailPath);
                }
                return response = new JsonModel()
                {
                    data = patientHeaderModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }

        }

        public JsonModel GetPatientById(int patientId, TokenModel token)
        {
            PatientDemographicsModel patientDemographics = new PatientDemographicsModel();
            Patients patients = _context.Patients
                                .Include(Z => Z.Users3)
                                .Where(a => a.Id == patientId && a.IsDeleted == false)
                                .FirstOrDefault();

            if (patients != null)
            {
                PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB, patients.Email, patients.SSN, patients.MRN, null, null, null, null, null, null, patients.Identifier).FirstOrDefault();
                MapPatientEntityToDemographics(patientDemographics, patients, pHIDecryptedModel, token);
                patientDemographics.PatientTags = GetPatientTags(patients.Id);
                patientDemographics.PatientDiagnosis = GetPatientDiagnosis(patients.Id);
                patientDemographics.UserID = (int)patients.UserID;
                if (patientDemographics != null && !string.IsNullOrEmpty(patientDemographics.PhotoPath) && !string.IsNullOrEmpty(patientDemographics.PhotoThumbnailPath))
                {
                    patientDemographics.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDemographics.PhotoPath);
                    patientDemographics.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patientDemographics.PhotoThumbnailPath);
                }

                response = new JsonModel(patientDemographics, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }

        public JsonModel UpdatePatientPortalVisibility(int patientID, int userID, bool isPortalActive, string url,TokenModel token)
        {
            try
            {
                Patients patient = _patientRepository.Get(a => a.Id == patientID && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false);
                if (patient != null)
                {
                    patient.IsPortalActivate = isPortalActive;
                    _patientRepository.Update(patient);
                    _patientRepository.SaveChanges();


                    //check is portal active or inactive
                    if (isPortalActive)
                    {
                        Entity.User user = _userRepository.Get(a => a.Id == userID && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false);
                        OrganizationModel orgData = _tokenService.GetOrganizationDetailsByBusinessName(token.DomainName);
                        PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, null, patient.LastName, null, patient.Email, null, null, null, null, null, null, null, null, null).FirstOrDefault();
                        string FirstName = pHIDecryptedModel.FirstName;
                        string LastName = pHIDecryptedModel.LastName;
                        string Email = pHIDecryptedModel.EmailAddress;
                        string Username = user.UserName;
                        string Password = CommonMethods.Decrypt(user.Password);
                        HttpContext request = token.Request;
                       // var hostUrl = request.Request.Host;
                        var hostingServer = _configuration.GetSection("FrontendServerUrl").Value;
                        var hostingServerUrl = _configuration.GetSection("FrontendServer").Value;

                        OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == token.OrganizationID && a.IsDeleted == false && a.IsActive == true);
                        OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
                        AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
                        organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

                        var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                        var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/client-portal-activation.html");
                        emailHtml = emailHtml.Replace("{{name}}", FirstName + " " + LastName);
                        emailHtml = emailHtml.Replace("{{username}}", Username);
                        emailHtml = emailHtml.Replace("{{password}}", Password);
                        emailHtml = emailHtml.Replace("{{portalUrl}}", url);
                        //if(token.DomainName != null)
                        //{
                        //    emailHtml = emailHtml.Replace("{{portalUrl}}", "https://" + token.DomainName + "." + hostingServer + "/web" + "/client-login");

                        //}
                        //else
                        //{
                        //    emailHtml = emailHtml.Replace("{{portalUrl}}", hostingServerUrl + "web" + "/client-login");
                        //    
                        //}

                       // emailHtml = emailHtml.Replace("{{portalUrl}}", "https://" + token.DomainName + "." + HCOrganizationConnectionStringEnum.DomainUrl + "/client-login");
                        emailHtml = emailHtml.Replace("{{organizationName}}", orgData.OrganizationName);

                        _emailSender.SendEmailAsync(Email, "Portal Activation", emailHtml, organizationSMTPDetailModel, orgData.OrganizationName);

                        return new JsonModel()
                        {
                            data = patient,
                            Message = StatusMessage.ClientPortalActivated,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else
                    {
                        return new JsonModel()
                        {
                            data = patient,
                            Message = StatusMessage.ClientPortalDeactivated,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.AppError = e.Message;
            }
            return response;
        }

        public JsonModel UpdatePatientActiveStatus(int patientID, bool isActive, TokenModel token)
        {
            try
            {
                Patients patient = _patientRepository.Get(a => a.Id == patientID && a.OrganizationID == token.OrganizationID && a.IsDeleted == false);
                if (patient != null)
                {
                    patient.IsActive = isActive;
                    _patientRepository.Update(patient);
                    _patientRepository.SaveChanges();

                    //Response
                    response.data = patient;
                    response.StatusCode = (int)HttpStatusCodes.OK;

                    //check is active or inactive
                    if (isActive)
                    {
                        response.Message = StatusMessage.ClientActivation;
                    }
                    else
                    {
                        response.Message = StatusMessage.ClientDeactivation;
                    }
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.AppError = e.Message;
            }
            return response;
        }

        public JsonModel GetActivitiesForPatientPayer(int patientId, DateTime startDate, DateTime endDate, TokenModel token)
        {
            try
            {
                return new JsonModel()
                {
                    data = _patientRepository.GetActivitiesForPatientPayer<AppointmentTypeModel>(patientId, InsurancePlanType.Primary.ToString(), startDate, endDate, null, token).ToList(),
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel GetPatientAuthorizationData(Nullable<int> appointmentId, int patientId, int appointmentTypeId, DateTime startDate, DateTime endDate, bool isAdmin, Nullable<int> patientInsuranceId, Nullable<int> authorizationId, TokenModel token)
        {
            try
            {
                List<AppointmentAuthModel> authList = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>(patientId, appointmentTypeId, startDate, endDate, InsurancePlanType.Primary.ToString(), appointmentId, isAdmin, patientInsuranceId, authorizationId).ToList();    //_patientRepository.GetPatientAuthorizationData(patientId, appointmentTypeId, startDate,InsurancePlanType.Primary.ToString());
                bool isAuthMandatory = _patientRepository.CheckAuthorizationSetting();///Please save this setting in database and remove select statement from here in future if any performance issue occur
                authList.Select(x => { x.IsAuthorizationMandatory = isAuthMandatory; return x; }).ToList();
                return new JsonModel()
                {
                    data = authList,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public AuthorizationValidityModel CheckAuthorizationForServiceCodes(List<string> serviceCodesList, int patientId, int appointentTypeId, DateTime startDate, string payerPreference)
        {
            return null;
        }

        public JsonModel GetPatientPayerServiceCodes(int patientId, string payerPreference, DateTime date, int payerId, int patientInsuranceId, TokenModel token)
        {
            try
            {
                List<ServiceCodeSearchModel> patientDetails = _patientRepository.GetPatientPayerServiceCodes<ServiceCodeSearchModel>(patientId, payerPreference, date, payerId, patientInsuranceId).ToList();
                bool isAuthMandatory = _patientRepository.CheckAuthorizationSetting(); ///Please save this setting in database and remove select statement from here in future if any performance issue occur
                patientDetails.Select(x => { x.IsAuthorizationMandatory = isAuthMandatory; return x; }).ToList();
                return response = new JsonModel()
                {
                    data = patientDetails,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public JsonModel GetPatientPayerServiceCodesAndModifiers(int patientId, string payerPreference, DateTime date, int payerId, int patientInsuranceId, TokenModel token)
        {
            try
            {
                Dictionary<string, object> patientDetails = _patientRepository.GetPatientPayerServiceCodesAndModifiers(patientId, payerPreference, date, payerId, patientInsuranceId);
                //bool isAuthMandatory = _patientRepository.CheckAuthorizationSetting(); ///Please save this setting in database and remove select statement from here in future if any performance issue occur
                //patientDetails.Select(x => { x.IsAuthorizationMandatory = isAuthMandatory; return x; }).ToList();
                return response = new JsonModel()
                {
                    data = patientDetails,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public JsonModel GetAllAuthorizationsForPatient(int patientId, int pageNumber, int pageSize, string authType, TokenModel token)
        {
            try
            {
                Dictionary<string, object> dict = _patientAuthorizationRepository.GetAllAuthorizationsForPatient(patientId, pageNumber, pageSize, authType);
                return new JsonModel()
                {
                    data = dict,
                    meta = new Meta()
                    {
                        TotalRecords = (List<AuthorizationModel>)dict["Authorization"] != null && ((List<AuthorizationModel>)dict["Authorization"]).Count > 0 ? ((List<AuthorizationModel>)dict["Authorization"]).First().TotalCount : 0
                        ,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(((List<AuthorizationModel>)dict["Authorization"] != null && ((List<AuthorizationModel>)dict["Authorization"]).Count > 0) ? ((List<AuthorizationModel>)dict["Authorization"]).First().TotalCount : 0) / pageSize)
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public MemoryStream GetPatientCCDA(int patientID, TokenModel token)
        {
            try
            {
                // var patientDetails =
                return _patientRepository.GetPatientCCDA(patientID, token);
                //return response = new JsonModel()
                //{
                //    data = patientDetails,
                //    Message = StatusMessage.FetchMessage,
                //    StatusCode = (int)HttpStatusCodes.OK
                //};
                //return pat
            }
            catch (Exception)
            {
                //return response = new JsonModel()
                //{
                //    data = new object(),
                //    Message = StatusMessage.ServerError,
                //    StatusCode = (int)HttpStatusCodes.InternalServerError
                //};
                return null;
            }

        }

        public JsonModel ImportPatientCCDA(JObject file, TokenModel token)
        {
            try
            {

                string base64File = Convert.ToString(file["file"]);

                if (base64File.Contains("data:text/xml"))
                {
                    base64File = base64File.Replace("data:text/xml;base64,", "");

                    string filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\CDA\\" + String.Format("{0}_{1:yyyyMMddhhmmss}", "cda", DateTime.Now) + ".xml";
                    File.WriteAllBytes(filePath, Convert.FromBase64String(base64File));

                    RecordParser recordParser = new RecordParser();
                    PatientClinicalInformation patientClinicalInformation = new PatientClinicalInformation();
                    patientClinicalInformation = recordParser.ParseCCDAFile(filePath, true);

                    Patients patient = null;
                    string userName = String.Format("{0}{1:yyyyMMddhhmmss}", "email", DateTime.Now) + "@mailinator.com";
                    patient = _patientRepository.CheckExistingPatient<Patients>(patientClinicalInformation.ptDemographicDetail.Email, patientClinicalInformation.ptDemographicDetail.MRN, userName, 0, token).FirstOrDefault();
                    if (patient != null)
                    {
                        response.data = new object();
                        response.Message = StatusMessage.PatientAlreadyExist;
                        response.StatusCode = 422;
                        return response;
                    }

                    //var result = _patientRepository.ImportPatientCCDA(base64File, token.OrganizationID, token.UserID);
                    var result = _patientRepository.ImportPatientCCDA(patientClinicalInformation, token.OrganizationID, token.UserID);

                    if (result > 1)
                    {
                        response.data = result;
                        response.Message = StatusMessage.CCDAImportedSuccessfully;
                        response.StatusCode = (int)HttpStatusCodes.OK;
                    }
                    else
                    {
                        response.data = new object();
                        response.Message = StatusMessage.CCDAError;
                        response.StatusCode = (int)HttpStatusCodes.InternalServerError;

                    }
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.InvalidFile;
                    response.StatusCode = (int)HttpStatusCodes.NotAcceptable;
                }
            }
            catch (Exception ex)
            {
                response.data = new object();
                response.Message = StatusMessage.ServerError;
                response.StatusCode = (int)HttpStatusCodes.InternalServerError;
            }
            return response;
        }

        public JsonModel CreateUpdatePatient(PatientDemographicsModel patientDemographics, TokenModel token)
        {
            Patients patient = null;
            patient = _patientRepository.CheckExistingPatient<Patients>(patientDemographics.Email, patientDemographics.MRN, patientDemographics.UserName, patientDemographics.Id, token).FirstOrDefault();
            if (patient != null)
            {
                //response
                response.data = new object();
                response.Message = StatusMessage.PatientAlreadyExist;
                response.StatusCode = 422;
                return response;
            }
            else
            {
                PHIEncryptedModel pHIEncryptedModel = _patientRepository.GetEncryptedPHIData<PHIEncryptedModel>(patientDemographics.FirstName, patientDemographics.MiddleName, patientDemographics.LastName, patientDemographics.DOB != null ? patientDemographics.DOB.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : null, patientDemographics.Email, patientDemographics.SSN, patientDemographics.MRN, null, null, null, null, null, null, patientDemographics.Identifier).FirstOrDefault();
                PHIDecryptedModel pHIDecryptedModel = null;

                if (patientDemographics.Id != 0)
                {
                    patient = _context.Patients.Where(a => a.Id == patientDemographics.Id && a.IsDeleted == false).Include(Z => Z.Users3).FirstOrDefault();
                    pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB != null ? patient.DOB : null, patient.Email, patient.SSN, null, null, null, null, null, null, null, patient.Identifier).FirstOrDefault();
                }

                //using (var transaction = _context.Database.BeginTransaction())
                //{
                    try
                    {
                        if (patientDemographics.Id == 0)
                        {
                            Random rnd = new Random();
                            string password = "";
                            if (string.IsNullOrEmpty(patientDemographics.NewPassword))
                                password = CommonMethods.Encrypt(patientDemographics.FirstName.ToUpper() + "@" + rnd.Next(1111, 9999));
                            else
                                password = patientDemographics.NewPassword;
                            patient = new Patients();
                            MapPatientDemographicsToEntity(patientDemographics, patient, pHIEncryptedModel, token, "add");
                            //save image
                            _imageService.ConvertBase64ToImage(patient);

                            //save user
                            Entity.User requestUser = SaveUser(patient, token, password);
                            //save patient
                            patient.UserID = requestUser.Id;
                            patient.IsDeleted = false;
                            patient.IsBBEnabled = false;
                            //patient.CreatedBy=
                            patient = _patientRepository.AddPatient(patient);

                            //response
                            patientDemographics.Id = patient.Id;
                            response.Message = StatusMessage.ClientCreated;
                            response.StatusCode = (int)HttpStatusCodes.OK;

                        }

                        else
                        {
                            List<ChangesLog> changesLogs = new List<ChangesLog>();
                            patient = _context.Patients.Where(a => a.Id == patientDemographics.Id && a.IsDeleted == false && a.IsActive == true).Include(z => z.PatientDiagnosis).Include(x => x.PatientTags).FirstOrDefault();
                            patientDemographics.IsPortalActivate = patient.IsPortalActivate;
                            MapPatientDemographicsToEntity(patientDemographics, patient, pHIEncryptedModel, token, "update");
                            patient = _imageService.ConvertBase64ToImage(patient);
                            //changesLogs = _patientRepository.GetChangesLogData(token, patientDemographics, pHIDecryptedModel);
                            _context.Patients.Update(patient);
                            changesLogs = _patientRepository.GetChangesLogData(token, patientDemographics, pHIDecryptedModel);
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateDemographicDetails, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                            _context.SaveChanges();
                            _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patient.Id, patientDemographics.LinkedEncounterId, token);
                            //response
                            response.Message = StatusMessage.ClientUpdated;
                            response.StatusCode = (int)HttpStatusCodes.OK;

                        }
                        patientDemographics.PatientDiagnosis = GetPatientDiagnosis(patient.Id);
                        patientDemographics.PatientTags = GetPatientTags(patient.Id);
                        response.data = patientDemographics;
                        //transaction.Commit();

                    }
                    catch (Exception e)
                    {
                        //transaction.Rollback();
                        response.data = new object();
                        response.Message = StatusMessage.ErrorOccured;
                        response.StatusCode = (int)HttpStatusCodes.InternalServerError;
                        response.AppError = e.Message;
                    }
                    return response;
                //}
            }
        }
        public JsonModel CreateUpdateClient(PatientDemographicsModel patientDemographics, TokenModel token)
        {
            Patients patient = null;
            string phoneNumber = string.IsNullOrEmpty(patientDemographics.Phone) ? null : patientDemographics.Phone;
            patient = _patientRepository.CheckExistingPatient<Patients>(patientDemographics.Email, patientDemographics.MRN, patientDemographics.UserName, patientDemographics.Id, token).FirstOrDefault();
            if (patient != null)
            {
                //response
                response.data = new object();
                response.Message = StatusMessage.PatientAlreadyExist;
                response.StatusCode = 422;
                return response;
            }
            PHIEncryptedModel pHIEncryptedModel = _patientRepository.GetEncryptedPHIData<PHIEncryptedModel>(patientDemographics.FirstName, patientDemographics.MiddleName, patientDemographics.LastName, patientDemographics.DOB != null ? patientDemographics.DOB.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : null, patientDemographics.Email, patientDemographics.SSN, patientDemographics.MRN, null, patientDemographics.Address, null, null, null, phoneNumber, patientDemographics.Identifier).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (patientDemographics.Id == 0)
                    {
                        Random rnd = new Random();
                        string password = "";
                        if (string.IsNullOrEmpty(patientDemographics.NewPassword))
                            password = CommonMethods.Encrypt(patientDemographics.FirstName.ToUpper() + "@" + rnd.Next(1111, 9999));
                        else
                            password = patientDemographics.NewPassword;
                        patient = new Patients();
                        MapPatientDemographicsToEntity(patientDemographics, patient, pHIEncryptedModel, token, "add");
                        //save image
                        _imageService.ConvertBase64ToImage(patient);
                        //save user
                        Entity.User requestUser = SaveUser(patient, token, password);
                        //save patient
                        patient.UserID = requestUser.Id;
                        patient.IsDeleted = false;
                        //patient.CreatedBy=
                        patient = _patientRepository.AddPatient(patient);

                        //response
                        patientDemographics.Id = patient.Id;
                        response.Message = StatusMessage.ClientCreated;
                        response.StatusCode = (int)HttpStatusCodes.OK;
                    }
                    else
                    {
                        patient = _context.Patients.Where(a => a.Id == patientDemographics.Id && a.IsDeleted == false && a.IsActive == true).Include(z => z.PatientDiagnosis).Include(x => x.PatientTags).FirstOrDefault();
                        patientDemographics.IsPortalActivate = patient.IsPortalActivate;
                        patientDemographics.IsPortalRequired = patient.IsPortalRequired;
                        patientDemographics.RenderingProviderID = patient.RenderingProviderID;
                        patientDemographics.SecondaryRaceID = patient.SecondaryRaceID;
                        patientDemographics.PrimaryProvider = patient.PrimaryProvider;
                        patientDemographics.LocationID = patient.LocationID;
                        MapPatientDemographicsToEntity(patientDemographics, patient, pHIEncryptedModel, token, "update");
                        patient = _imageService.ConvertBase64ToImage(patient);
                        _context.Patients.Update(patient);
                        _context.SaveChanges();
                        //response
                        response.Message = StatusMessage.ClientUpdated;
                        response.StatusCode = (int)HttpStatusCodes.OK;
                    }
                    if (!string.IsNullOrEmpty(patientDemographics.Phone))
                    {
                        int phoneType = _context.GlobalCode.Where(x => x.GlobalCodeCategoryID == 1 && x.GlobalCodeName.ToUpper() == ("Main").ToUpper()).Select(x => x.Id).FirstOrDefault();
                        int preferenceId = _context.GlobalCode.Where(x => x.GlobalCodeCategoryID == 19 && x.GlobalCodeName.ToUpper() == ("No Message").ToUpper()).Select(x => x.Id).FirstOrDefault();
                        //Save phone number
                        PhoneNumbers phoneNumberInDb = _context.PhoneNumbers.Where(x => x.PhoneNumberTypeId == phoneType && x.PatientID == patient.Id).FirstOrDefault();
                        if (phoneNumberInDb != null)
                        {
                            phoneNumberInDb.PhoneNumber = pHIEncryptedModel.Phonenumber;
                            _context.PhoneNumbers.Update(phoneNumberInDb);
                            _context.SaveChanges();
                        }
                        else
                        {
                            PhoneNumbers phoneNumberModel = new PhoneNumbers()
                            {
                                IsActive = true,
                                IsDeleted = false,
                                PatientID = patient.Id,
                                PhoneNumberTypeId = phoneType,
                                CreatedBy = patient.UserID,
                                CreatedDate = DateTime.UtcNow,
                                PreferenceID = preferenceId,
                                PhoneNumber = pHIEncryptedModel.Phonenumber,
                            };
                            _context.PhoneNumbers.Add(phoneNumberModel);
                            _context.SaveChanges();

                        }
                    }
                    patientDemographics.PatientDiagnosis = GetPatientDiagnosis(patient.Id);
                    patientDemographics.PatientTags = GetPatientTags(patient.Id);
                    response.data = patientDemographics;
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    response.data = new object();
                    response.Message = StatusMessage.ErrorOccured;
                    response.StatusCode = (int)HttpStatusCodes.InternalServerError;
                    response.AppError = e.Message;
                }
                return response;
            }
        }

        private List<PatientTagsModel> GetPatientTags(int patientId)
        {
            List<PatientTagsModel> patientTags = _context.PatientTags.Where(x => x.PatientID == patientId && x.IsActive == true && x.IsDeleted == false).Select(y => new PatientTagsModel()
            {
                PatientTagID = y.Id,
                TagID = y.TagID
            }).ToList();
            return patientTags;
        }

        private List<PatientDiagnosisModel> GetPatientDiagnosis(int patientId)
        {
            List<PatientDiagnosisModel> diagnosisList = _context.PatientDiagnosis.Where(x => x.PatientID == patientId && x.IsActive == true && x.IsDeleted == false).Select(y => new PatientDiagnosisModel()
            {
                Id = y.Id,
                PatientID = y.PatientID,
                ICDID = y.ICDID,
                IsPrimary = y.IsPrimary
            }).ToList();
            return diagnosisList;
        }

        private void MapPatientDemographicsToEntity(PatientDemographicsModel patientDemographics, Patients patients, PHIEncryptedModel pHIEncryptedModel, TokenModel token, string action)
        {
            patients.MRN = pHIEncryptedModel.MRN;
            patients.FirstName = pHIEncryptedModel.FirstName;
            patients.MiddleName = pHIEncryptedModel.MiddleName;
            patients.LastName = pHIEncryptedModel.LastName;
            patients.DOB = pHIEncryptedModel.DateOfBirth;
            patients.SSN = pHIEncryptedModel.SSN;
            patients.Email = pHIEncryptedModel.EmailAddress;
            patients.Gender = patientDemographics.Gender;
            patients.OptOut = patientDemographics.OptOut;
            patients.Race = patientDemographics.Race;
            patients.SecondaryRaceID = patientDemographics.SecondaryRaceID;
            patients.Ethnicity = patientDemographics.Ethnicity;
            patients.PrimaryProvider = patientDemographics.PrimaryProvider;
            patients.RenderingProviderID = patientDemographics.RenderingProviderID;
            patients.PhotoPath = !string.IsNullOrEmpty(patientDemographics.PhotoPath) && patientDemographics.PhotoPath.Contains('/') ? Path.GetFileName(patientDemographics.PhotoPath) : patientDemographics.PhotoPath;
            patients.PhotoThumbnailPath = !string.IsNullOrEmpty(patientDemographics.PhotoThumbnailPath) && patientDemographics.PhotoThumbnailPath.Contains('/') ? Path.GetFileName(patientDemographics.PhotoThumbnailPath) : patientDemographics.PhotoThumbnailPath;
            patients.LocationID = patientDemographics.LocationID;
            patients.IsPortalRequired = patientDemographics.IsPortalRequired;
            patients.IsPortalActivate = patientDemographics.IsPortalActivate;
            patients.UserName = patientDemographics.UserName;
            patients.EmergencyContactFirstName = patientDemographics.EmergencyContactFirstName;
            patients.EmergencyContactLastName = patientDemographics.EmergencyContactLastName;
            patients.EmergencyContactPhone = patientDemographics.EmergencyContactPhone;
            //patients.EmergencyContactPhone = patientDemographics.Phone;
            patients.EmergencyContactRelationship = patientDemographics.EmergencyContactRelationship;
            patients.PhotoBase64 = patientDemographics.PhotoBase64;
            patients.Identifier = pHIEncryptedModel.HealthPlanBeneficiaryNumber;

            if (patientDemographics.StateId != 0)
            {
                List<PatientAddress> patientAddresses = new List<PatientAddress>();
                PatientAddress patientAddress = null;

                patientAddress = new PatientAddress
                {
                    Address1 = pHIEncryptedModel.Address1,
                    StateID = patientDemographics.StateId,
                    IsPrimary = true,
                    AddressTypeID = 20,
                    PatientLocationID = 1,
                    CountryID = 1
                };
                patientAddresses.Add(patientAddress);

                if (patientAddresses.Count > 0)
                    patients.PatientAddress = patientAddresses;
            }
            if (action == "add")
            {
                patients.CreatedBy = token.UserID != 0 ? token.UserID : (int?)null;
                patients.CreatedDate = DateTime.UtcNow;
                patients.OrganizationID = token.OrganizationID;
                patients.LocationID = patients.LocationID != 0 ? patients.LocationID : token.LocationID;
                if (patientDemographics.PatientDiagnosis != null && patientDemographics.PatientDiagnosis.Count > 0)
                {
                    List<PatientDiagnosis> diagnosis = new List<PatientDiagnosis>();
                    PatientDiagnosis patientDiagnosis = null;
                    patientDemographics.PatientDiagnosis.ForEach(a =>
                    {
                        patientDiagnosis = new PatientDiagnosis
                        {
                            DiagnosisDate = DateTime.UtcNow,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = token.UserID,
                            IsPrimary = true,
                            IsActive = true,
                            ICDID = a.ICDID
                        };
                        diagnosis.Add(patientDiagnosis);
                    });
                    if (diagnosis.Count > 0)
                        patients.PatientDiagnosis = diagnosis;
                }
            }
            else if (action == "update")
            {
                patients.UpdatedBy = token.UserID;
                patients.UpdatedDate = DateTime.UtcNow;
                if (patients.PatientDiagnosis != null && patients.PatientDiagnosis.Count > 0)
                {
                    PatientDiagnosis patDiag = patients.PatientDiagnosis.Find(a => a.IsPrimary == true);
                    if (patDiag != null && patients.PatientDiagnosis.Exists(a => a.IsPrimary == true))
                    {
                        patients.PatientDiagnosis.FirstOrDefault(a => a.IsPrimary == true).ICDID = patientDemographics.PatientDiagnosis.FirstOrDefault().ICDID;
                        patients.PatientDiagnosis.FirstOrDefault(a => a.IsPrimary == true).UpdatedBy = token.UserID;
                        patients.PatientDiagnosis.FirstOrDefault(a => a.IsPrimary == true).UpdatedDate = DateTime.UtcNow;
                    }
                }
                //add new 
                if (patientDemographics.PatientTags != null && patientDemographics.PatientTags.Count > 0)
                {
                    PatientTags patientTag = null;
                    List<int> patientTagIds = patientDemographics.PatientTags.Where(X => X.PatientTagID != 0).Select(a => a.PatientTagID).ToList();
                    patients.PatientTags.Where(x => !patientTagIds.Contains(x.Id)).ToList().ForEach(x =>
                    {
                        x.DeletedBy = token.UserID;
                        x.DeletedDate = DateTime.UtcNow;
                        x.IsDeleted = true;
                    });
                    patientDemographics.PatientTags.Where(x => x.PatientTagID == 0).ToList().ForEach(a =>
                        {
                            if (a.PatientTagID == 0)
                            {
                                patientTag = new PatientTags()
                                {
                                    TagID = a.TagID,
                                    PatientID = patients.Id,
                                    CreatedBy = token.UserID,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true
                                };
                                patients.PatientTags.Add(patientTag);
                            }
                        });
                }
            }
            patients.Note = patientDemographics.Note;
        }

        private void MapPatientEntityToDemographics(PatientDemographicsModel patientDemographics, Patients patients, PHIDecryptedModel pHIDecryptedModel, TokenModel token)
        {
            patientDemographics.MRN = pHIDecryptedModel.MRN;
            patientDemographics.FirstName = pHIDecryptedModel.FirstName;
            patientDemographics.MiddleName = pHIDecryptedModel.MiddleName;
            patientDemographics.LastName = pHIDecryptedModel.LastName;
            patientDemographics.DOB = Convert.ToDateTime(pHIDecryptedModel.DateOfBirth);
            patientDemographics.SSN = pHIDecryptedModel.SSN;
            patientDemographics.Email = pHIDecryptedModel.EmailAddress;
            patientDemographics.Gender = patients.Gender;
            patientDemographics.OptOut = patients.OptOut != null ? (bool)patients.OptOut : false;
            patientDemographics.Race = patients.Race;
            patientDemographics.SecondaryRaceID = patients.SecondaryRaceID;
            patientDemographics.Ethnicity = patients.Ethnicity;
            patientDemographics.PrimaryProvider = patients.PrimaryProvider;
            patientDemographics.RenderingProviderID = patients.RenderingProviderID != null ? (int)patients.RenderingProviderID : 0;
            patientDemographics.PhotoPath = patients.PhotoPath;
            patientDemographics.PhotoThumbnailPath = patients.PhotoThumbnailPath;
            patientDemographics.LocationID = patients.LocationID;
            patientDemographics.IsPortalRequired = patients.IsPortalRequired;
            patientDemographics.UserName = patients.UserName;
            patientDemographics.Note = patients.Note;
            patientDemographics.UserName = patients.Users3.UserName;
            patientDemographics.EmergencyContactFirstName = patients.EmergencyContactFirstName;
            patientDemographics.EmergencyContactLastName = patients.EmergencyContactLastName;
            patientDemographics.EmergencyContactPhone = patients.EmergencyContactPhone;
            patientDemographics.EmergencyContactRelationship = patients.EmergencyContactRelationship;
            patientDemographics.PhotoBase64 = patients.PhotoBase64;
            patientDemographics.IsBBEnabled = patients.IsBBEnabled;
            patientDemographics.Identifier = pHIDecryptedModel.HealthPlanBeneficiaryNumber;
        }


        private Entity.User SaveUser(Patients entity, TokenModel token, string password)
        {
            Entity.User requestUser = new Entity.User();
            requestUser.UserName = entity.UserName;
            requestUser.Password = password; //change it to dynamic later on and send email to patient
                                             // patient Role id  should be organization base
            requestUser.RoleID = _context.UserRoles.Where(m => m.RoleName.ToLower() == OrganizationRoles.Patient.ToString().ToLower() && m.OrganizationID == token.OrganizationID && m.IsActive == true && m.IsDeleted == false).FirstOrDefault().Id; ;//4; 
                                                                                                                                                                                                                                                       //
            requestUser.CreatedBy = token.UserID != 0 ? token.UserID : (int?)null;
            requestUser.CreatedDate = DateTime.UtcNow;
            requestUser.IsActive = true;
            requestUser.IsDeleted = false;
            requestUser.PasswordResetDate = DateTime.UtcNow;
            requestUser.OrganizationID = entity.OrganizationID;
            _userRepository.Create(requestUser);
            _userRepository.SaveChanges();
            return requestUser;
        }

        public JsonModel GetPatients(ListingFiltterModel patientFiltterModel, TokenModel token)
        {
            try
            {
                //not error
                response.data = new object();
                response.Message = StatusMessage.ErrorOccured;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                /////

                //List<PatientModel> patientModels = _patientRepository.GetPatients<PatientModel>(patientFiltterModel, token).ToList();
                List<PatientModel> patientModels = _patientRepository.GetStaffPatients<PatientModel>(patientFiltterModel, token).ToList();
                if (patientModels != null && patientModels.Count > 0)
                {
                    patientModels.ForEach(a =>
                    {
                        if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                            a.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.PhotoThumbnailPath);
                        else
                            a.PhotoThumbnailPath = "";
                    });

                    response.data = patientModels;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCodes.Created;
                }
                else
                {
                    //not found
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    /////
                }
            }
            catch (Exception e)
            {
                response.AppError = e.Message;
            }
            return response;
        }

        public JsonModel GetAuthorizationsForPatientPayer(int patientId, int patientInsuranceId, DateTime startDate, TokenModel token)
        {
            return new JsonModel(_patientRepository.GetAuthorizationsForPatientPayer<MasterDropDown>(patientId, patientInsuranceId, startDate, token).ToList(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetPatientIdByUserId(int userId, TokenModel token)
        {

            var patientId = _patientRepository.GetPatientByUserId(userId);
            if (patientId > 0)
            {

                response = new JsonModel(patientId, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }


        public JsonModel GetPatientByIdForPushNotificatons(int? patientId, TokenModel token)
        {
            PatientDemographicsModel patientDemographics = new PatientDemographicsModel();
            Patients patients = _context.Patients
                                .Include(Z => Z.Users3)
                                .Where(a => a.Id == patientId && a.IsDeleted == false)
                                .FirstOrDefault();

            if (patients != null)
            {
                //PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB, patients.Email, patients.SSN, patients.MRN, null, null, null, null, null, null, null).FirstOrDefault();
                //PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB, null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
                PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, null, patients.LastName, null, patients.Email, null, null, null, null, null, null, null, null, null).FirstOrDefault();
                MapPatientEntityToDemographics(patientDemographics, patients, pHIDecryptedModel, token);
                patientDemographics.PatientTags = GetPatientTags(patients.Id);
                patientDemographics.PatientDiagnosis = GetPatientDiagnosis(patients.Id);
                patientDemographics.UserID = (int)patients.UserID;
                if (patientDemographics != null && !string.IsNullOrEmpty(patientDemographics.PhotoPath) && !string.IsNullOrEmpty(patientDemographics.PhotoThumbnailPath))
                {
                    patientDemographics.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDemographics.PhotoPath);
                    patientDemographics.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patientDemographics.PhotoThumbnailPath);
                }

                response = new JsonModel(patientDemographics, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }
        public JsonModel GetPatientSymptoms(string search, TokenModel token)
        {


            var client = new RestClient(Infermedica.ApiUrl);
            var newRequest = new RestRequest("v2/search?phrase=" + search + "&sex=male&max_results=5&type=symptom", Method.GET);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<List<symptom>>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetSymptomQuestions(List<evidence> symptom, int age, string gender, TokenModel token)
        {
            var client = new RestClient(Infermedica.QuestApiUrl);

            var newRequest = new RestRequest("", Method.POST);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            newRequest.AddJsonBody(new question
            {
                age = age,
                sex = gender,
                extras = new extra { disable_groups = true },
                evidence = symptom

            });
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<QuestionResponse>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetObservations(List<evidence> symptom, int age, string gender, TokenModel token)
        {
            var client = new RestClient(Infermedica.TriageApiUrl);
            var newRequest = new RestRequest("", Method.POST);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            newRequest.AddJsonBody(new question
            {
                age = age,
                sex = gender,
                extras = new extra { disable_groups = true },
                evidence = symptom

            });
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<result>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }

        public JsonModel GetSummary(string type, TokenModel token)
        {
            var url = "";
            switch (type)
            {
                case "risk":
                    url = "https://api.infermedica.com/v2/risk_factors";
                    break;
                case "condition":
                    url = "https://api.infermedica.com/v2/conditions";
                    break;
                default:
                    url = "https://api.infermedica.com/v2/symptoms";
                    break;


            }
            var client = new RestClient(url);
            var newRequest = new RestRequest("", Method.GET);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<List<summary>>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetCovidQuestions(CovidQuestion quest, TokenModel token)
        {

            var client = new RestClient(Infermedica.CovidQuestApiUrl);

            var newRequest = new RestRequest("", Method.POST);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            newRequest.AddJsonBody(quest);
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<QuestionResponse>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetCovidSummary(List<CovidEvidence> symptom, int age, string gender, TokenModel token)
        {
            var client = new RestClient(Infermedica.TriageCovidApiUrl);
            var newRequest = new RestRequest("", Method.POST);
            newRequest.AddHeader("App-Id", CommonEnum.SymptomConfig.symptom_SenderId);
            newRequest.AddHeader("App-Key", CommonEnum.SymptomConfig.Symptom_SenderKey);
            newRequest.AddHeader("Content-Type", "application/json");
            newRequest.AddJsonBody(new CovidQuestion
            {
                age = age,
                sex = gender,
                evidence = symptom

            });
            var response = client.Execute(newRequest);
            //return myResponse.ToString();
            var root = JsonConvert.DeserializeObject<result>(response.Content);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        public JsonModel GetPatientsDetailedInfo(int patientId, TokenModel token)
        {
            PatientInfo patientinfo = null;
            patientinfo = _patientRepository.GetPatientsDetailedInfo(patientId, token);

            if (patientinfo != null)
            {
                response = new JsonModel(patientinfo, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }
        public JsonModel GetPatientsDashboardDetails(int PatientID, TokenModel token)
        {
            try
            {
                PatientInfoDetails patientDetails = _patientRepository.GetPatientsDashboardDetails(PatientID, token);
                if (patientDetails.PatientInfo != null && patientDetails.PatientInfo.Count > 0 && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoPath) && !string.IsNullOrEmpty(patientDetails.PatientInfo[0].PhotoThumbnailPath))
                {
                    patientDetails.PatientInfo[0].PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoPath);
                    patientDetails.PatientInfo[0].PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patientDetails.PatientInfo[0].PhotoThumbnailPath);
                }
                //add staff image's url in model

                return response = new JsonModel()
                {
                    data = patientDetails,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }

        }

        public JsonModel GetRedAlertsInfo(AlertsRedIndicatorFilterModel filterModel, TokenModel tokenModel)
        {

            List<AlertRepsoneModel> data = _patientRepository.GetRedAlertsInfo<AlertRepsoneModel>(filterModel, tokenModel).ToList();
            if (data != null && data.Count > 0)
            {
                response = new JsonModel(data, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(data, filterModel);
            }
            else
            {
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };

            }
            return response;
        }

        public bool IsValidUserForDataAccess(TokenModel tokenModel, int patientId)
        {
            bool response = false;
            var query = _context.User
   .Join(_context.UserRoles,
      usr => usr.RoleID,
      role => role.Id,
      (usr, role) => new { user = usr, roles = role })
   .Where(x => x.user.Id == tokenModel.UserID).FirstOrDefault();    // 
            if (query.roles.UserType == UserTypeEnum.ADMIN.ToString() || query.roles.UserType == UserTypeEnum.STAFF.ToString())
            {
                response = true;
            }
            else if (query.roles.UserType == UserTypeEnum.CLIENT.ToString())
            {
                var pat = _context.Patients.Where(x => x.UserID == tokenModel.UserID && x.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();
                if (pat.Id != patientId)
                    response = false;
                else response = true;
            }
            return response;
        }

        public JsonModel GetPatientDetailsBB(PatientBBAuthorizationModel bBAuthorizationModel, TokenModel token)
        {
            PatientBBDataModel patientDetails = new PatientBBDataModel();
            bool isSuccess = false;
            response = new JsonModel(isSuccess, StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError);
            try
            {
                var url = EligibiltyAPIs.AuthAPIBB + "grant_type=authorization_code&client_secret=" + EligibilityTokens.BBClientSecret + "&client_id=" + EligibilityTokens.BBClientID + "&redirect_uri=" + bBAuthorizationModel.ReturnURL + "&code=" + bBAuthorizationModel.Code + "&State=" + bBAuthorizationModel.State;
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if ((int)httpResponse.StatusCode == 200)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        BlueButtonAuthorizationModel authorizationModel = JsonConvert.DeserializeObject<BlueButtonAuthorizationModel>(result);

                        if (authorizationModel != null)
                        {
                            var httpRequest2 = (HttpWebRequest)WebRequest.Create(EligibiltyAPIs.PatientAPIBB + authorizationModel.patient);
                            httpRequest2.Method = "GET";
                            httpRequest2.Headers["Authorization"] = "Bearer " + authorizationModel.access_token;
                            var httpResponse2 = (HttpWebResponse)httpRequest2.GetResponse();

                            if ((int)httpResponse2.StatusCode == 200)
                            {
                                using (var streamReader2 = new StreamReader(httpResponse2.GetResponseStream()))
                                {
                                    string result2 = streamReader2.ReadToEnd();
                                    patientDetails = JsonConvert.DeserializeObject<PatientBBDataModel>(result2);
                                    
                                    //var httpRequest3 = (HttpWebRequest)WebRequest.Create("https://sandbox.bluebutton.cms.gov/v1/fhir/ExplanationOfBenefit/?patient=" + authorizationModel.patient);
                                    if (patientDetails != null )
                                    {
                                        bool dataMapped = MapPatientDataWithMedicareData(patientDetails, bBAuthorizationModel.ClientId, token);

                                        if (dataMapped)
                                        {
                                            var httpRequest3 = (HttpWebRequest)WebRequest.Create(EligibiltyAPIs.PatientCoverageAPIBB + authorizationModel.patient);
                                            httpRequest3.Method = "GET";
                                            httpRequest3.Headers["Authorization"] = "Bearer " + authorizationModel.access_token;
                                            var httpResponse3 = (HttpWebResponse)httpRequest3.GetResponse();

                                            if ((int)httpResponse3.StatusCode == 200)
                                            {
                                                using (var streamReader3 = new StreamReader(httpResponse3.GetResponseStream()))
                                                {
                                                    string result3 = streamReader3.ReadToEnd();
                                                    PatientBBCoverageData coverageDetails = JsonConvert.DeserializeObject<PatientBBCoverageData>(result3);
                                                    if (coverageDetails != null)
                                                    {
                                                        isSuccess = CheckInsuranceStatus(coverageDetails, bBAuthorizationModel.ClientId, token);
                                                        if (!isSuccess)
                                                        {
                                                            isSuccess = true;
                                                        }
                                                    }
                                                }
                                            }

                                            //Disable BB sync button for patient to get access Medicare data
                                            Patients patient = _context.Patients.Where(a => a.Id == bBAuthorizationModel.ClientId && a.IsDeleted == false).FirstOrDefault();
                                            patient.IsBBEnabled = false;
                                            patient.IsEligible = isSuccess;
                                            _patientRepository.Update(patient);
                                            _patientRepository.SaveChanges();
                                        }
                                        response = new JsonModel(isSuccess, StatusMessage.PatientRecordSyncSuccess, (int)HttpStatusCode.OK);

                                    }
                                    else
                                    {
                                        response = new JsonModel(isSuccess, StatusMessage.NotFound, (int)HttpStatusCode.OK);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return response;
            }

            return response;
        }

        private bool CheckInsuranceStatus(PatientBBCoverageData patientBBCoverage, int clientId, TokenModel token)
        {
            bool isSuccess = false;

            //Patient Insurance check
            int insurancePlanTypeID = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.INSURANCEPLANTYPE, "primary", token, true);
            PatientInsuranceDetails patientInsurances = _context.PatientInsuranceDetails.Where(a => a.PatientID == clientId && a.InsurancePlanTypeID == insurancePlanTypeID && a.IsDeleted == false && a.IsActive == true).FirstOrDefault();
            PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(null, null, null, null, null, null, null, null, null, null, null, null, null, patientInsurances.InsuranceIDNumber).FirstOrDefault();

            if (patientBBCoverage.subscriberId == pHIDecryptedModel.HealthPlanBeneficiaryNumber.ToString())
            {
                if (patientBBCoverage.period.start != null)
                {
                    DateTime todayDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DateTime startTempDate = Convert.ToDateTime(patientBBCoverage.period.start);
                    DateTime startDate = new DateTime(startTempDate.Year, startTempDate.Month, startTempDate.Day);
                    DateTime endTempDate = Convert.ToDateTime(patientBBCoverage.period.end);
                    DateTime endDate = new DateTime(endTempDate.Year, endTempDate.Month, endTempDate.Day);

                    if (patientBBCoverage.period.end != null)
                    {

                        int result = DateTime.Compare(endDate, todayDateTime);

                        if (result > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        DateTime endSTempDate = startDate.AddYears(1);
                        DateTime endSDate = new DateTime(endTempDate.Year, endTempDate.Month, endTempDate.Day);
                        int result = DateTime.Compare(endSDate, todayDateTime);
                        if (result > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                        }
                    }

                }

            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        private bool MapPatientDataWithMedicareData(PatientBBDataModel patientBBData, int clientId, TokenModel token)
        {
            bool isSuccess = false;
            PatientModel patient = new PatientModel();
            try
            {
                if (patientBBData != null)
                {
                    patient.DOB = patientBBData.birthDate;
                    patient.GenderName = patientBBData.gender;
                    patient.Identifier = patientBBData.id;
                    patient.PatientId = clientId;
                    patientBBData.name.ForEach(x =>
                    {
                        patient.LastName = x.family;
                        patient.FirstName = x.given[0];
                        patient.MiddleName = x.given[1] != null ? x.given[1] : String.Empty;
                    });
                }


                PatientAddressesModel patientAddresses = new PatientAddressesModel();
                if (patientBBData.address != null)
                {
                    patientBBData.address.ForEach(x =>
                    {
                        patientAddresses.StateName = x.state;
                        patientAddresses.Zip = x.postalCode;
                        patientAddresses.City = x.city;
                        patientAddresses.Address1 = x.line != null ? (x.line[0] != null ? (x.line[0] + (x.line[1] != null ? x.line[1] + (x.line[2] != null ? x.line[2] : String.Empty) : String.Empty)) : String.Empty) : String.Empty;
                    });
                }

                PhoneNumberModel phoneNumber = new PhoneNumberModel();
                if (patientBBData.telecom != null)
                {
                    patientBBData.telecom.ForEach(x =>
                {
                    phoneNumber.PhoneNumber = x.value;
                    phoneNumber.PhoneNumberType = x.use;
                });
                }

                SQLResponseModel response = _patientRepository.UpdatePatientBBData<SQLResponseModel>(patient.DOB, patient.GenderName, patient.Identifier, clientId, patient.FirstName, patient.LastName, patient.MiddleName, patientAddresses.Address1, patientAddresses.City, patientAddresses.Zip, patientAddresses.StateAbbr, phoneNumber.PhoneNumber, phoneNumber.PhoneNumberType, token).FirstOrDefault();

                if ((int)response.StatusCode == 200)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                return isSuccess;
            }

            return isSuccess;
        }

        public JsonModel CheckBlueButtonStatus(int clientId, TokenModel token)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            Patients patients = _patientRepository.Get(x => x.Id == clientId && x.IsActive == true && x.IsDeleted == false); 
            if(patients != null) {
                response = new JsonModel(patients.IsBBEnabled, StatusMessage.Success, (int)HttpStatusCode.OK);
            }
            return response;
        }
    }


}
