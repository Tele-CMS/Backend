using HC.Patient.Service.IServices;
using HC.Service;
using System;
using AutoMapper;
using HC.Patient.Repositories.IRepositories;
using HC.Model;
using HC.Patient.Model;
using HC.Common.HC.Common;
using System.Net;
using HC.Patient.Entity;
using Microsoft.Extensions.Primitives;
using HC.Common;
using HC.Patient.Service.Token.Interfaces;
using HC.Common.Model.OrganizationSMTP;
using HC.Patient.Repositories.IRepositories.Organizations;
using Microsoft.AspNetCore.Hosting;
using HC.Common.Services;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Service.IServices.Telehealth;
using HC.Patient.Model.NotificationSetting;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.MasterData;
using System.Linq;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Repositories.IRepositories.Staff;
using Microsoft.Extensions.Configuration;
using HC.Patient.Service.IServices.User;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.Chats;

namespace HC.Patient.Service.Services
{
    public class GroupSessionInvitationService : BaseService, IGroupSessionInvitationService
    {
        private readonly IMapper _mapper;
        private readonly IGroupSessionInvitationRepository _groupSessionInvitationRepository;
        private JsonModel _response;
        private readonly ITokenService _tokenService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IHostingEnvironment _env;
        private readonly IEmailService _emailSender;
        private readonly IEmailWriteService _emailWriteService;
        private readonly ITelehealthService _telehealthService;
        private readonly INotificationService _notificationService;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly ILocationService _locationService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public GroupSessionInvitationService(IGroupSessionInvitationRepository groupSessionInvitationRepository,
            IMapper mapper,
            ITokenService tokenService,
            IOrganizationSMTPRepository organizationSMTPRepository,
            IHostingEnvironment env,
            IEmailService emailSender,
            IEmailWriteService emailWriteService,
            ITelehealthService telehealthService,
            INotificationService notificationService,
            IPatientAppointmentService patientAppointmentService,
            IAppointmentRepository appointmentRepository,
            IStaffRepository staffRepository,
            ILocationService locationService,
            IConfiguration configuration,
            IUserService userService,
            IChatService chatService
            )
        {
            _groupSessionInvitationRepository = groupSessionInvitationRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _env = env;
            _emailSender = emailSender;
            _emailWriteService = emailWriteService;
            _telehealthService = telehealthService;
            _notificationService = notificationService;
            _patientAppointmentService = patientAppointmentService;
            _appointmentRepository = appointmentRepository;
            _staffRepository = staffRepository;
            _locationService = locationService;
            _configuration = configuration;
            _userService = userService;
            _chatService = chatService;
        }
        public JsonModel SaveGroupSessionInvitationModel(GroupSessionInvitationModel groupSessionInvitationModel, TokenModel tokenModel)
        {
            var groupSession = _groupSessionInvitationRepository.GetGroupSessionByEmail(groupSessionInvitationModel, tokenModel);
            if (groupSession != null)
            {

                var token = _telehealthService.GetOTSession(CommonMethods.Encrypt(groupSession.InvitaionId.ToString()), tokenModel);
                if (token.StatusCode != (int)HttpStatusCode.OK)
                    return new JsonModel(null, StatusMessage.InvitationNotSent, (int)HttpStatusCode.InternalServerError);

                var emailResult = SendInvitationEmail(
                   groupSession,
                   groupSessionInvitationModel,
                   groupSessionInvitationModel.WebRootUrl,
                   groupSession.Email,
                   groupSession.Name,
                   (int)EmailType.GroupSessionInvitation,
                   (int)EmailSubType.NewGroupSessionInvitation,
                   groupSession.Id,
                   "/templates/group-session-invitation.html",
                   "New Group Session Invitation",
                   tokenModel
                   );
                return new JsonModel(null, string.Format(StatusMessage.UserInvitedAlready, groupSession.Name), (int)HttpStatusCode.Created);
            }

            UserInviteModel userInvitationModel = new UserInviteModel()
            {
                Email = groupSessionInvitationModel.Email,
                Name = groupSessionInvitationModel.Name
            };
            int userId = 0;
            if (groupSessionInvitationModel.StaffId == 0)
            {
                var userInvited = _userService.CeatedInvitedUser(userInvitationModel, tokenModel);
                if (userInvited == null)
                    return new JsonModel(null, StatusMessage.InvitationSent, (int)HttpStatusCode.OK);
                userId = userInvited.Id;
            }
            else
            {
                var staff = _staffRepository.GetByID(groupSessionInvitationModel.StaffId);
                if (staff != null)
                    userId = staff.UserID;
            }
            var groupSessionInvitation = _mapper.Map<GroupSessionInvitations>(groupSessionInvitationModel);
            if (groupSessionInvitation == null)
                return new JsonModel(null, StatusMessage.BadRequest, (int)HttpStatusCode.BadRequest);
            if (groupSessionInvitation != null)
            {
                groupSessionInvitation.UserID = userId;
                groupSessionInvitation.CreatedBy = tokenModel.UserID;
                groupSessionInvitation.CreatedDate = DateTime.UtcNow;
                groupSessionInvitation.OrganizationId = tokenModel.OrganizationID;
            }
            var result = _groupSessionInvitationRepository.SaveGroupSessionInvitation(groupSessionInvitation, tokenModel);
            if (result != null)
            {
                var token = _telehealthService.GetOTSession(CommonMethods.Encrypt(result.InvitaionId.ToString()), tokenModel);
                if (token.StatusCode != (int)HttpStatusCode.OK)
                    _response = new JsonModel(null, StatusMessage.InvitationNotSent, (int)HttpStatusCode.InternalServerError);

                var emailResult = SendInvitationEmail(
                    result,
                    groupSessionInvitationModel,
                    groupSessionInvitationModel.WebRootUrl,
                    result.Email,
                    result.Name,
                    (int)EmailType.GroupSessionInvitation,
                    (int)EmailSubType.NewGroupSessionInvitation,
                    result.Id,
                    "/templates/group-session-invitation.html",
                    "New Group Session Invitation",
                    tokenModel
                    );
                if (!string.IsNullOrEmpty(emailResult))
                    new JsonModel(null, StatusMessage.InviationAddedButEmailNotSent, (int)HttpStatusCode.Created);
                _response = new JsonModel(null, StatusMessage.InvitationSent, (int)HttpStatusCode.OK);

                //if staffId exists then same need to reflect in staff scheduler
                var appointmentCreated = AddNewAppointmentIntoStaffSchedule(groupSessionInvitationModel.StaffId, groupSessionInvitationModel.AppointmentId, tokenModel);
                if (appointmentCreated.StatusCode == (int)HttpStatusCode.OK)
                {
                    var newAppointmentId = (int)appointmentCreated.data;
                    result.InvitedAppointmentId = newAppointmentId;
                    var updateResult = _groupSessionInvitationRepository.UpdateGroupSessionInvitation(result, tokenModel);
                }
                //Add Invited user into chat room
                var addInvitedInChatRoom = _chatService.GetChatRoomId(groupSessionInvitationModel.AppointmentId, userId, tokenModel);
            }
            else
                _response = new JsonModel(null, StatusMessage.InvitationNotSent, (int)HttpStatusCode.InternalServerError);
            return _response;
        }
        private JsonModel AddNewAppointmentIntoStaffSchedule(int staffId, int appointmentId, TokenModel tokenModel)
        {
            if (staffId > 0)
            {
                var result = _patientAppointmentService.SaveAppointmentWhenStaffInvitedForGroupSession(staffId, appointmentId, tokenModel);
                PushNotificationModel saveNotificationModel = new PushNotificationModel()
                {
                    Message = NotificationMessage.GroupSessionInvitation,
                    NotificationTypeId = (int)NotificationType.TextNotification,
                    TypeId = NotificationActionType.GroupSession,
                    SubTypeId = NotificationActionSubType.GroupSessionInvitaion,
                    StaffId = staffId,
                    NotificationType = NotificationType.PushNotification
                };
                _notificationService.SaveNotification(saveNotificationModel, tokenModel);
                return result;
            }
            else
                return new JsonModel(null, StatusMessage.BadRequest, (int)HttpStatusCode.BadRequest);

        }
        #region Email
        private string SendInvitationEmail(GroupSessionInvitations groupSessionInvitations, GroupSessionInvitationModel groupSessionInvitationModel, string webRootUrl, string toEmail, string username, int emailType, int emailSubType, int primaryId, string templatePath, string subject, TokenModel tokenModel)
        {
            string appointmentDate = "";
            string startDateTime = "";
            string endDateTime = "";
            PatientAppointmentModel patientAppointmentModel = _appointmentRepository.GetAppointmentDetails<PatientAppointmentModel>(groupSessionInvitationModel.AppointmentId).FirstOrDefault();
            if (groupSessionInvitationModel.StaffId > 0)
            {

                var staffLocations = _staffRepository.GetAssignedLocationsByStaffId(groupSessionInvitationModel.StaffId, tokenModel);

                LocationModel loggedInlocation = _locationService.GetLocationOffsets(patientAppointmentModel.ServiceLocationID, tokenModel);
                patientAppointmentModel.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientAppointmentModel.StartDateTime, loggedInlocation.DaylightOffset, loggedInlocation.StandardOffset, loggedInlocation.TimeZoneName, tokenModel);
                patientAppointmentModel.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientAppointmentModel.EndDateTime, loggedInlocation.DaylightOffset, loggedInlocation.StandardOffset, loggedInlocation.TimeZoneName, tokenModel);

                int.TryParse(Common.CommonMethods.Decrypt(staffLocations.Where(x => x.IsDefault == true).FirstOrDefault().LocationId), out int locationId);
                patientAppointmentModel.ServiceLocationID = locationId;
                LocationModel locationModal = _locationService.GetLocationOffsets(locationId, tokenModel);

                patientAppointmentModel.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                patientAppointmentModel.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);


            }
            appointmentDate = patientAppointmentModel.StartDateTime.ToString("dddd, dd MMMM yyyy");
            startDateTime = patientAppointmentModel.StartDateTime.ToString("hh:mm tt");
            endDateTime = patientAppointmentModel.EndDateTime.ToString("hh:mm tt");
            //Get Current Login User Organization
            tokenModel.Request.Request.Headers.TryGetValue("BusinessToken", out StringValues businessName);
            Organization organization = _tokenService.GetOrganizationByOrgId(tokenModel.OrganizationID, tokenModel);

            //Get Current User Smtp Details
            OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
            AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
            organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + templatePath);
            //#if DEBUG

            //#else
            //            emailHtml = emailHtml.Replace("{{action_url}}",tokenModel.DomainName+ "/register" + "?token=" + invitaionId);
            //#endif
            //var image_url = CommonMethods.CreateImageUrl(tokenModel.Request, "/img/", "cbimage.jpg");
            //var hostingServer = _configuration.GetSection("HostingServer").Value;
            //emailHtml = emailHtml.Replace("{{img_url}}", image_url);//hostingServer + "img/cbimage.jpg");
            var hostingServer = _configuration.GetSection("HostingServer").Value;
            emailHtml = emailHtml.Replace("{{img_url}}", hostingServer + "img/cbimage.jpg");
            emailHtml = emailHtml.Replace("{{username}}", username);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            emailHtml = emailHtml.Replace("{{browser_name}}", tokenModel.Request.Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            emailHtml = emailHtml.Replace("{{action_url}}", webRootUrl + "" + "?token=" + Common.CommonMethods.Encrypt(groupSessionInvitations.InvitaionId.ToString()));



            emailHtml = emailHtml.Replace("{{AppDate}}", appointmentDate);
            emailHtml = emailHtml.Replace("{{AppTime}}", startDateTime + " - " + endDateTime);
            //emailHtml = emailHtml.Replace("{{AppMode}}", patientAppointmentModel.Mode);
            //emailHtml = emailHtml.Replace("{{AppType}}", patientAppointmentModel.Type);
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
            ////Send Email
            var error = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            if (!string.IsNullOrEmpty(error))
                emailModel.EmailStatus = false;
            else
                emailModel.EmailStatus = true;
            //Maintain Email log into Db
            var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
            return error;
        }
        #endregion
    }
}
