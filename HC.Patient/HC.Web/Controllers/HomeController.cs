using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Model.ContactUs;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.Telehealth;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Service.IServices.Telehealth;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Home")]
    public class HomeController : BaseController
    {
        private readonly IUserInvitationReadService _userInvitationReadService;
        private readonly IUserRegistrationWriteService _userRegistrationWriteService;
        private readonly ITokenService _tokenService;
        private readonly IMasterDataService _masterDataService;
        private readonly IProviderAppointmentService _providerAppointmentService;
        private readonly IStaffProfileService _staffProfileService;
        private readonly IStaffAvailabilityService _staffAvailabilityService;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IOrganizationService _organizationService;
        private readonly ILocationService _locationService;
        private JsonModel response;
        private readonly ITelehealthService _telehealthService;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IChatRoomUserService _chatRoomUserService;
        IHubContext<ChatHub> _chatHubContext;
        private readonly ITelehealthRepository _telehealthRepository;
        private readonly HCOrganizationContext _context;
        private readonly IPatientRepository _patientRepository;


        public HomeController(
            ITokenService tokenService,
            IUserInvitationReadService userInvitationReadService,
            IUserRegistrationWriteService userRegistrationWriteService,
            IMasterDataService masterDataService,
            IProviderAppointmentService providerAppointmentService,
            IStaffProfileService staffProfileService,
            IStaffAvailabilityService staffAvailabilityService,
            IPatientAppointmentService patientAppointmentService,
            IOrganizationService organizationService,
            ILocationService locationService,
            ITelehealthService telehealthService,
            IUserService userService,
            IChatService chatService,
            IChatRoomUserService chatRoomUserService,
            IHubContext<ChatHub> chatHubContext,
            ITelehealthRepository telehealthRepository,
            HCOrganizationContext context,
            IPatientRepository patientRepository
            )
        {
            _userInvitationReadService = userInvitationReadService;
            _userRegistrationWriteService = userRegistrationWriteService;
            _tokenService = tokenService;
            _masterDataService = masterDataService;
            _providerAppointmentService = providerAppointmentService;
            _staffProfileService = staffProfileService;
            _staffAvailabilityService = staffAvailabilityService;
            _patientAppointmentService = patientAppointmentService;
            _organizationService = organizationService;
            _locationService = locationService;
            _telehealthService = telehealthService;
            _userService = userService;
            _chatService = chatService;
            _chatRoomUserService = chatRoomUserService;
            _chatHubContext = chatHubContext;
            _telehealthRepository = telehealthRepository;
            _context = context;
            _patientRepository = patientRepository;
        }

        [HttpGet]
        [Route("CheckUsernameExistance")]
        public JsonResult CheckUsernameExistance(string username)
        {
            return Json(_userInvitationReadService.ExecuteFunctions(() => _userInvitationReadService.CheckUsernameExistance(GetBussinessToken(HttpContext, _tokenService), username)));
        }
        
        [HttpPost]
        [Route("MasterDataByName")]
        public JsonResult MasterDataByName([FromBody]JObject masterDataNames, string globalCodeId="")
            
        {
            List<string> masterDataNamesList = new List<string>(Convert.ToString(masterDataNames["masterdata"]).Split(','));
            return Json(_masterDataService.ExecuteFunctions(() => _masterDataService.GetMasterDataByName(masterDataNamesList, GetBussinessToken(HttpContext, _tokenService), globalCodeId)));
        }
        
        [HttpPost]
        [Route("ProviderAvailableList")]
        public JsonResult ProviderAvailableList([FromBody] AppointmentSearchModel appointmentSearchModel)
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.GetProviderListToMakeAppointment(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }
        [HttpGet]
        [Route("ProviderProfile")]
        public JsonResult ProviderProfile(string id)
        {
            //var bussinessName = CommonMethods.Decrypt(HttpContext.Request.Headers["businessToken"].ToString());
            //DomainToken domainToken = new DomainToken
            //{
            //    BusinessToken = bussinessName
            //};
            //DomainToken tokenData = _tokenService.GetDomain(domainToken);

            //TokenModel token = new TokenModel
            //{
            //    Request = HttpContext,
            //    OrganizationID = tokenData.OrganizationId
            //};
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.GetProvider(GetBussinessToken(HttpContext, _tokenService), id)));
        }

        [HttpPost]
        [Route("SortedProviderAvailableList")]
        public JsonResult SortedProviderAvailableList([FromBody] AppointmentSearchModel appointmentSearchModel) 
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.SortedProviderAvailableList(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }

        [HttpPost]
        [Route("SearchTextProviderAvailableList")]
        public JsonResult SearchTextProviderAvailableList([FromBody]  AppointmentSearchModel appointmentSearchModel)
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.SearchTextProviderAvailableList(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }


        #region Staff Location Availability
        [HttpGet("GetStaffLocationWithAvailability")]
        public JsonResult GetStaffLocationWithAvailability(string id)
        {
            return Json(_staffProfileService.ExecuteFunctions(() => _staffProfileService.GetStaffLocationAndAvailability(id, GetBussinessToken(HttpContext, _tokenService))));
        }
        #endregion Staff Location Availability

        [HttpGet("GetOrganizationDetail")]
        public JsonResult GetOrganizationDetail(string id)
        {
            return Json(_staffProfileService.ExecuteFunctions(() => _organizationService.GetOrganizationDetailsById(GetBussinessToken(HttpContext, _tokenService))));
        }





        #region Book Appointment
        //[HttpPost("CheckAvailability")]
        //public JsonResult CheckProviderAvailabilityForAppointment([FromBody] StaffAvailabilityModel staffAvailabilityModel)
        //{
        //    return Json(_staffProfileService.ExecuteFunctions(() => _staffProfileService.GetStaffLocationAndAvailability(id, GetBussinessToken(HttpContext, _tokenService))));
        //}
        [HttpPost]
        [Route("CheckIsValidAppointmentWithLocation")]
        public JsonResult CheckIsValidAppointmentWithLocation([FromBody]List<PatientAppointmentModel> patientAppointmentModel)
        {
            try
            {
                TokenModel token = GetToken(HttpContext);

                LocationModel locationModal = _locationService.GetLocationOffsets(patientAppointmentModel[0].LocationId, token);

                if (patientAppointmentModel != null && patientAppointmentModel.Count > 0)

                    response = new JsonModel()
                    {
                        data = (patientAppointmentModel != null && patientAppointmentModel.Count > 0) ?
                        _patientAppointmentService.CheckIsValidAppointmentWithLocation((patientAppointmentModel[0].AppointmentStaffs == null ? "" : String.Join(",", patientAppointmentModel[0].AppointmentStaffs.Select(x => x.StaffId).ToArray())),
                        CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName),
                        CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName),
                        patientAppointmentModel[0].StartDateTime,
                        patientAppointmentModel[0].PatientAppointmentId, patientAppointmentModel[0].PatientID, patientAppointmentModel[0].AppointmentTypeID,
                        CommonMethods.GetCurrentOffset(patientAppointmentModel[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName),
                        token) : null,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
            }
            catch
            {
                response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError);
            }
            return Json(response);
        }
        [HttpPost]
        [Route("SavePatientAppointment")]
        public JsonResult SavePatientAppointment([FromBody]List<Model.PatientAppointment.PatientAppointmentModel> patientApptList, bool IsFinish = false, bool isAdmin = false)
        {
            try
            {

                TokenModel token = GetToken(HttpContext);
                Model.MasterData.LocationModel locationModal = _locationService.GetLocationOffsets(patientApptList[0].ServiceLocationID, token);
                if (patientApptList != null && patientApptList.Count > 0)
                {
                    if (!IsFinish)//recurrence case to show all virtual aapointments on screen
                    {
                        DateTime currentDate = patientApptList[0].StartDateTime;
                        //patientApptList[0].StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        //patientApptList[0].EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        //patientApptList[0].AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointment(String.Join(",", patientApptList[0].AppointmentStaffs.Select(x => x.StaffId).ToArray()), patientApptList[0].StartDateTime, patientApptList[0].EndDateTime, currentDate, patientApptList[0].PatientAppointmentId, patientApptList[0].PatientID, patientApptList[0].AppointmentTypeID, token);
                        patientApptList[0].AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointmentWithLocation(String.Join(",", patientApptList[0].AppointmentStaffs.Select(x => x.StaffId).ToArray())
                                , CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , currentDate, patientApptList[0].PatientAppointmentId, patientApptList[0].PatientID, patientApptList[0].AppointmentTypeID
                                , CommonMethods.GetCurrentOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , token);
                        if (!string.IsNullOrEmpty(patientApptList[0].RecurrenceRule) && patientApptList[0].PatientAppointmentId == 0)
                        {
                            //GetOccurencesFromRRule(patientApptList[0], patientApptList, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                            //response = _patientAppointmentService.SaveAppointment(patientApptList[0], patientApptList.Skip(1).ToList(), token);

                            patientApptList[0].StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientApptList[0].EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            //patientApptList[0].StartDateTime=CommonMethods.ConvertFromUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            //patientApptList[0].EndDateTime=CommonMethods.ConvertFromUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientApptList.ForEach(x =>
                            {
                                x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                                x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                            });

                            response = new JsonModel(patientApptList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                        }
                        else response = _patientAppointmentService.SaveAppointment(patientApptList[0], null, isAdmin, token);
                    }
                    else
                    {
                        patientApptList.ForEach(x =>
                        {
                            DateTime dtStartDatetime = x.StartDateTime;
                            x.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            x.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            x.OffSet = (int)CommonMethods.GetCurrentOffset(dtStartDatetime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        });
                        response = _patientAppointmentService.SaveAppointment(patientApptList[0], patientApptList.Skip(1).ToList(), isAdmin, token);
                    }
                }
                return Json(response);
            }
            catch
            {
                return Json(new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError));
            }
        }
        #endregion Book Appointment

        #region Open Tok Group Session
        [HttpGet("GetOTSession")]
        public JsonResult GetTelehealthSession(string invitationId)
        {
            return Json(_staffProfileService.ExecuteFunctions(() => _telehealthService.GetOTSession(invitationId, GetBussinessToken(HttpContext, _tokenService))));
        }
        #endregion Open Tok Group Session

        [HttpPatch("UpdatePassword")]
        public JsonResult UpdateUserPasswordWithToken([FromBody] UpdatePasswordModel updatePassword)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.UpdateUserPasswordWithToken(updatePassword, GetBussinessToken(HttpContext, _tokenService))));
        }
        #region Chat Room
        [HttpGet]
        [Route("GetChatRoomId")]
        public JsonResult GetChatRoomId(int userId, int appointmentId)
        {
            return Json(_chatService.ExecuteFunctions(() => _chatService.GetChatRoomId(appointmentId, userId, GetBussinessToken(HttpContext, _tokenService))));
        }

        [HttpGet]
        [Route("GetAppointmentDetail")]
        public JsonResult GetAppointmentDetail(int appointmentId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetAppointmentDetails(appointmentId, GetBussinessToken(HttpContext, _tokenService))));
        }
        [HttpGet]
        [Route("GetChatHistory")]
        public JsonResult GetChatHistory(ChatParmModel chatParmModel)
        {
            return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetChatHistory(chatParmModel, GetBussinessToken(HttpContext, _tokenService))));
        }
        [HttpGet]
        [Route("GetUserInChatRoom")]
        public JsonResult GetUserInChatRoom(int roomId)
        {
            return Json(_chatRoomUserService.ExecuteFunctions<JsonModel>(() => _chatRoomUserService.GetUserInChatRoom(roomId, GetBussinessToken(HttpContext, _tokenService))));
        }
        //[HttpPost]
        //[Route("SendFileInChat")]
        //public async Task<JsonResult> SendFileInChat([FromForm(Name = "uploadedFile")] IFormFile file, int roomId, int userId)
        //{
        //    return Json(await _chatService.ExecuteFunctions<Task<JsonModel>>(() =>  _chatService.SendFileInChat(file, roomId, userId)));
        //}
        //[HttpGet]
        //[Route("StartVideoRecording")]
        //public async Task<JsonModel> StartVideoRecording(string sessionId)
        //{
        //    return await _telehealthService.StartVideoRecording(sessionId, GetBussinessToken(HttpContext, _tokenService));
        //    //return Json(await _telehealthService.ExecuteFunctions<Task<JsonModel>>(() => _telehealthService.StartVideoRecording(sessionId, GetBussinessToken(HttpContext, _tokenService))));
        //}
        //[HttpGet]
        //[Route("StopVideoRecording")]
        //public JsonResult StopVideoRecording(string archiveId)
        //{
        //    return Json(_telehealthService.ExecuteFunctions<JsonModel>(() => _telehealthService.StopVideoRecording(archiveId, GetBussinessToken(HttpContext, _tokenService))));
        //}
        [HttpGet]
        [Route("GetVideoRecording")]
        public async Task<JsonResult> GetVideoRecording(string archiveId)
        {
            return Json(await _telehealthService.ExecuteFunctions<Task<JsonModel>>(() => _telehealthService.GetVideoRecordingAsync(archiveId, GetBussinessToken(HttpContext, _tokenService))));
        }
        #endregion

        #region Call Initiate
        [HttpGet]
        [Route("CallInitiate")]
        public JsonResult CallInitiate(int appointmentId,int userId)
        {
            TokenModel tokenModel = GetBussinessToken(HttpContext, _tokenService);
            string connectionId = _chatService.GetConnectionId(userId);

            if (connectionId != null)
            {

                var otTokens = _telehealthRepository.GetOTTokenByAppointmentId(appointmentId, userId, tokenModel);
                if (otTokens != null & otTokens.ToList().Count > 0)
                {
                    otTokens.ToList().ForEach(async x =>
                    {
                        if (x.CreatedBy != userId)
                        {
                            var connId = _chatService.GetConnectionId((int)x.CreatedBy);
                           
                            if (connId != null)
                            {

                                await _chatHubContext.Clients.Client(connId).SendAsync("CallInitiated", appointmentId, userId, x.CreatedBy);

                            }
                        }
                    });

                }
                return new JsonResult(new JsonModel(null, StatusMessage.CallInitiated, (int)HttpStatusCodes.OK));
            }
            else
            {
                return new JsonResult(new JsonModel(null, StatusMessage.CallNotInitiated, (int)HttpStatusCodes.InternalServerError));
            }
        }
        #endregion


        [HttpGet]
        [Route("SendContactUsEmail")]
        [AllowAnonymous]
        public JsonResult SendContactUsEmail(string text)
        {
            response = _organizationService.GetOrganizationEmailAddress();
            return Json(response);
        }

        [HttpPost]
        [Route("BookNewAppointmentFromPaymentPage")]
        public JsonResult BookNewAppointmentFromPaymentPage([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            //TokenModel token = GetToken(HttpContext);
            //patientAppointmentModel.PatientID = token.StaffID;
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.BookNewAppointmentFromPaymentPage(patientAppointmentModel, GetBussinessToken(HttpContext, _tokenService))));
        }

        [HttpPatch]
        [Route("CheckAppointmentTimeExpiry")]
        public JsonResult CheckAppointmentTimeExpiry(int appointmentId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.CheckAppointmentTimeExpiry(appointmentId, GetBussinessToken(HttpContext, _tokenService))));
        }

        [HttpPost]
        [Route("UrgentCareProviderAvailableList")]
        public JsonResult UrgentCareProviderAvailableList([FromBody] AppointmentSearchModel appointmentSearchModel)
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.GetUrgentCareProviderListToMakeAppointment(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }

        [HttpPost]
        [Route("SearchTextUrgentCareProviderAvailableList")]
        public JsonResult SearchTextUrgentCareProviderAvailableList([FromBody] AppointmentSearchModel appointmentSearchModel)
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.SearchTextUrgentCareProviderAvailableList(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }

        [HttpPost]
        [Route("SortedUrgentCareProviderAvailableList")]
        public JsonResult SortedUrgentCareProviderAvailableList([FromBody] AppointmentSearchModel appointmentSearchModel)
        {
            return Json(_masterDataService.ExecuteFunctions(() => _providerAppointmentService.SortedUrgentCareProviderAvailableList(GetBussinessToken(HttpContext, _tokenService), appointmentSearchModel)));
        }

       
        [HttpGet]
        [Route("UrgentCareProviderActionInitiate")]
        public JsonResult UrgentCareProviderActionInitiate(int appointmentId, int userId)
        {
            TokenModel tokenModel = GetBussinessToken(HttpContext, _tokenService);
            string connectionId = _chatService.GetConnectionId(userId);

            if (connectionId != null)
            {
                int staffuserid = _patientAppointmentService.GetStaffUserId(appointmentId);
                var otTokens = _telehealthRepository.GetOTTokenByAppointmentId(appointmentId, userId, tokenModel);
                if (otTokens != null & otTokens.ToList().Count > 0)
                {
                    otTokens.ToList().ForEach(async x =>
                    {

                        //var connId = _chatService.GetConnectionId((int)x.CreatedBy);
                        var connId = _chatService.GetConnectionId(staffuserid);
                        if (connId != null)
                            {

                            //await _chatHubContext.Clients.Group()
                                await _chatHubContext.Clients.Client(connId).SendAsync("CallProviderUrgentCare", appointmentId, userId, x.CreatedBy);

                            }
                        
                    });

                }
                return new JsonResult(new JsonModel(null, "Calling Provider", (int)HttpStatusCodes.OK));
            }
            else
            {
                return new JsonResult(new JsonModel(null, StatusMessage.CallNotInitiated, (int)HttpStatusCodes.InternalServerError));
            }
        }


        [HttpGet]
        [Route("PatientInformProviderAvailability")]
        public JsonResult PatientInformProviderAvailability(int appointmentId, int userId)
        {
            TokenModel tokenModel = GetBussinessToken(HttpContext, _tokenService);
            string connectionId = _chatService.GetConnectionId(userId);

            if (connectionId != null)
            {

                var otTokens = _telehealthRepository.GetOTTokenByAppointmentId(appointmentId, userId, tokenModel);
                if (otTokens != null & otTokens.ToList().Count > 0)
                {
                    otTokens.ToList().ForEach(async x =>
                    {
                        if (x.CreatedBy != userId)
                        {
                            var connId = _chatService.GetConnectionId((int)x.CreatedBy);

                            if (connId != null)
                            {

                                await _chatHubContext.Clients.Client(connId).SendAsync("CallPatientUrgentCare", appointmentId, userId, x.CreatedBy);

                            }
                        }
                    });

                }
                return new JsonResult(new JsonModel(null, "Calling Patient", (int)HttpStatusCodes.OK));
            }
            else
            {
                return new JsonResult(new JsonModel(null, StatusMessage.CallNotInitiated, (int)HttpStatusCodes.InternalServerError));
            }
        }

        [HttpGet]
        [Route("CallEnd")]
        public JsonResult CallEnd(int appointmentId, int userId)
        {
            TokenModel tokenModel = GetBussinessToken(HttpContext, _tokenService);
            string connectionId = _chatService.GetConnectionId(userId);
            string UserType = "";



            if (connectionId != null)
            {



                var otTokens = _telehealthRepository.GetOTTokenByAppointmentId(appointmentId, userId, tokenModel);
                if (otTokens != null & otTokens.ToList().Count > 0)
                {
                    otTokens.ToList().ForEach(async x =>
                    {
                        if (x.CreatedBy != userId)
                        {



                            var connId = _chatService.GetConnectionId((int)x.CreatedBy);



                            if (connId != null)
                            {
                                var doctordetails = _context.Staffs.Where(x => x.UserID == userId).FirstOrDefault();
                                var patients = _context.Patients.Where(x => x.UserID == userId).FirstOrDefault();
                                string InitiatorName = string.Empty;




                                if (doctordetails != null)
                                {
                                    InitiatorName = doctordetails.FirstName + ' ' + doctordetails.LastName;
                                    UserType = "provider";
                                }
                                if (patients != null)
                                {
                                    PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB, patients.Email, patients.SSN, patients.MRN, null, null, null, null, null, null, null).FirstOrDefault();
                                    InitiatorName = pHIDecryptedModel.FirstName + ' ' + pHIDecryptedModel.LastName;
                                    UserType = "patient";
                                }
                                await _chatHubContext.Clients.Client(connId).SendAsync("CallEnd", appointmentId, userId, x.CreatedBy, InitiatorName, UserType);



                            }
                        }
                    });



                }
                return new JsonResult(new JsonModel(null, StatusMessage.CallInitiated, (int)HttpStatusCodes.OK));
            }
            else
            {
                return new JsonResult(new JsonModel(null, StatusMessage.CallNotInitiated, (int)HttpStatusCodes.InternalServerError));
            }
        }
    }
}