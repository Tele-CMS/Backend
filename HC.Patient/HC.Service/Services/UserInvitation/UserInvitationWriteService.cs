using AutoMapper;
using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.NotificationSetting;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services
{
    public class UserInvitationWriteService : BaseService, IUserInvitationWriteService
    {
        private readonly IUserInvitationWriteRepository _userInvitationWriteRepository;
        private readonly IUserInvitationReadRepository _userInvitationReadRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;
        private JsonModel _response;
        private readonly IEmailService _emailSender;
        private readonly IHostingEnvironment _env;
        private readonly ITokenService _tokenService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly ILocationService _locationService;
        private JsonModel response;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public UserInvitationWriteService(IUserInvitationWriteRepository userInvitationWriteRepository,
            IUserInvitationReadRepository userInvitationReadRepository,
            IMapper mapper,
            IStaffRepository staffRepository,
            IEmailService emailSender,
            IHostingEnvironment env,
            ITokenService tokenService,
            IOrganizationSMTPRepository organizationSMTPRepository,
            ILocationService locationService,
            IEmailWriteService emailWriteService,
            IConfiguration configuration, INotificationService notificationService)
        {
            _userInvitationWriteRepository = userInvitationWriteRepository;
            _userInvitationReadRepository = userInvitationReadRepository;
            _mapper = mapper;
            _staffRepository = staffRepository;
            _emailSender = emailSender;
            _env = env;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _locationService = locationService;
            _emailWriteService = emailWriteService;
            _configuration = configuration;
            _notificationService = notificationService;

        }
        public JsonModel SendUserInvitation(UserInvitationModel userInvitationModel, TokenModel tokenModel, HttpRequest Request)
        {
            try
            {
                int result = 0;
                UserInvitation invitation = new UserInvitation();
                if (userInvitationModel != null)
                {
                    userInvitationModel.OrganizationId = tokenModel.OrganizationID;
                    userInvitationModel.LocationId = tokenModel.LocationID;

                    //Check If User Invitation Existed or not
                    var userInvitation = _userInvitationReadRepository.GetUserInvitationByEmailAndOrganizationId(userInvitationModel.Email, userInvitationModel.OrganizationId);
                    ////check if email existed in staff or not
                    var staffs = _staffRepository.GetStaffProfileDataByEmailAndOrgId(userInvitationModel.Email, tokenModel);

                   if (userInvitation != null)
                      return new JsonModel(new object(), StatusMessage.InvitationExisted, (int)HttpStatusCode.BadRequest);

                   if (staffs != null)
                      return new JsonModel(new object(), StatusMessage.StaffAlreadyExist, (int)HttpStatusCode.BadRequest);

                    LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

                    invitation = _mapper.Map<UserInvitation>(userInvitationModel);
                    invitation.CreatedBy = tokenModel.UserID;
                    invitation.InvitationSendDate = CommonMethods.ConvertFromUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel);
                   result = _userInvitationWriteRepository.SaveUpdateUserInvitation(invitation);
                }
               
                if (result > 0)
                {
                   var error = SendUserInvitationEmail(userInvitationModel, tokenModel, Request);
                    if (string.IsNullOrEmpty(error))
                    {
                        _response = new JsonModel(new object(), StatusMessage.InvitationSent, (int)HttpStatusCode.OK);
                        PushNotificationModel saveNotificationModel = new PushNotificationModel()
                        {
                            Message = NotificationMessage.Message,
                            NotificationTypeId = (int)NotificationType.TextNotification,
                            TypeId = NotificationActionType.UserInvitation,
                            SubTypeId = NotificationActionSubType.SendInvitation,
                            StaffId = tokenModel.StaffID,
                            NotificationType= NotificationType.TextNotification,
                        };
                        _notificationService.SaveNotification(saveNotificationModel, tokenModel);
                    }
                    else
                        _response = new JsonModel(new object(), error, (int)HttpStatusCode.InternalServerError);
                }
                else
                    _response = new JsonModel(new object(), StatusMessage.InvitationNotSent, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
            return _response;
        }

        public JsonModel ReSendUserInvitation(UserInvitationModel userInvitationModel, TokenModel tokenModel, HttpRequest Request, int invitationId)
        {
            try
            {
                int result = 0;
                if (userInvitationModel == null && invitationId == 0)
                    return new JsonModel(new object(), StatusMessage.RequestNotCompleted, (int)HttpStatusCode.BadRequest);

                userInvitationModel.OrganizationId = tokenModel.OrganizationID;
                userInvitationModel.LocationId = tokenModel.LocationID;

                //Check If User Invitation Existed or not
                var userInvitation = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(invitationId, tokenModel);

                if (userInvitation == null)
                    return new JsonModel(new object(), StatusMessage.InvitationExisted, (int)HttpStatusCode.BadRequest);

                _mapper.Map(userInvitationModel, userInvitation);
                userInvitation.UpdatedBy = tokenModel.UserID;
                userInvitation.UpdatedDate = DateTime.UtcNow;
                userInvitation.InvitationStatus = (int)UserInvitationStatus.ReInvited;

                var error = SendUserInvitationEmail(userInvitationModel, tokenModel, Request);
                if (string.IsNullOrEmpty(error))
                    result = _userInvitationWriteRepository.SaveUpdateUserInvitation(userInvitation);

                if (result > 0)
                    _response = new JsonModel(new object(), StatusMessage.InvitationSent, (int)HttpStatusCode.OK);
                else
                    _response = new JsonModel(new object(), error, (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                _response = new JsonModel(new object(), e.Message, (int)HttpStatusCode.InternalServerError);
                throw e;
            }
            return _response;
        }
        private string SendUserInvitationEmail(UserInvitationModel userInvitationModel, TokenModel tokenModel, HttpRequest Request)
        {
            var currentUserInvitation = _userInvitationReadRepository.GetUserInvitationByEmailAndOrganizationId(userInvitationModel.Email, userInvitationModel.OrganizationId);
            //Get Current Login User Organization
            tokenModel.Request.Request.Headers.TryGetValue("BusinessToken", out StringValues businessName);
            Organization organization = _tokenService.GetOrganizationByOrgId(tokenModel.OrganizationID, tokenModel);

            //Get Current User Smtp Details
            OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
            AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
            organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            string invitaionId = CommonMethods.Encrypt(currentUserInvitation.Id.ToString());
            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/invitation.html");
            //#if DEBUG
            emailHtml = emailHtml.Replace("{{action_url}}", userInvitationModel.WebUrl + "/register" + "?token=" + invitaionId);
            emailHtml = emailHtml.Replace("{{reject_action_url}}", userInvitationModel.WebUrl + "/reject-invitation" + "?token=" + invitaionId);

            //#else
            //            emailHtml = emailHtml.Replace("{{action_url}}",tokenModel.DomainName+ "/register" + "?token=" + invitaionId);
            //#endif
            var hostingServer = _configuration.GetSection("HostingServer").Value;
            emailHtml = emailHtml.Replace("{{img_url}}", hostingServer + "img/cbimage.jpg");
            emailHtml = emailHtml.Replace("{{username}}", userInvitationModel.LastName + " " + userInvitationModel.FirstName);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            string subject = string.Format("Invitation From {0}", organization.OrganizationName);
            string toEmail = userInvitationModel.Email;
            EmailModel emailModel = new EmailModel
            {
                EmailBody = CommonMethods.Encrypt(emailHtml),
                ToEmail = CommonMethods.Encrypt(toEmail),
                EmailSubject = CommonMethods.Encrypt(subject),
                EmailType = (int)EmailType.InvitationEmail,
                EmailSubType = (int)EmailSubType.none,
                PrimaryId = currentUserInvitation.Id,
                CreatedBy = tokenModel.UserID
            };
            //Send Email
            //await _emailSender.SendEmailAsync(userInvitationModel.Email, string.Format("Invitation From {0}", orgData.OrganizationName), emailHtml, organizationSMTPDetailModel, orgData.OrganizationName);
            var error = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            if (!string.IsNullOrEmpty(error))
                emailModel.EmailStatus = false;
            else
                emailModel.EmailStatus = true;
            //Maintain Email log into Db
            var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
            return error;
        }
        public JsonModel DeleteUserInvitation(int invitationId, TokenModel tokenModel)
        {
            response = new JsonModel();
            int result = 0;
            var userInvitation = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(invitationId, tokenModel);
            if (userInvitation != null)
            {
                userInvitation.IsDeleted = true;
                userInvitation.DeletedBy = tokenModel.UserID;
                userInvitation.DeletedDate = DateTime.UtcNow;
                result = _userInvitationWriteRepository.DeleteUserInvitation(userInvitation);
                if (result > 0)
                    response = new JsonModel(new object(), StatusMessage.InvitationDeleted, (int)HttpStatusCodes.OK);
                else
                    response = new JsonModel(new object(), StatusMessage.InvitationNotDeleted, (int)HttpStatusCodes.InternalServerError);
            }
            else
                response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            return response;
        }
    }
}
