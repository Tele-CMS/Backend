using HC.Common.HC.Common;
using HC.Model;
using HC.Service;
using System.Collections.Generic;
using System.Net;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Model.Chat;
using AutoMapper;
using HC.Patient.Entity;
using HC.Patient.Service.IServices;
using System.Linq;
using HC.Patient.Service.IServices.User;
using HC.Patient.Repositories.IRepositories.User;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.Users;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Hosting;
using HC.Common.Services;
using HC.Patient.Repositories.IRepositories.Organizations;
using Microsoft.Extensions.Configuration;
using AutoMapper.Configuration;
using System;
using HC.Common;
using HC.Patient.Model;
using HC.Common.Model.OrganizationSMTP;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.Services
{
    public class ChatRoomUserService : BaseService, IChatRoomUserService
    {
        private JsonModel response;

        private readonly IChatRoomUserRepository _chatRoomUserRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserRepository _userRepository;
        private readonly IPatientService _patientService;
        private readonly IStaffService _staffService;
        private readonly IGroupSessionInvitationRepository _groupSessionInvitationRepository;
        private readonly ITokenService _tokenService;
        private readonly IHostingEnvironment _env;
        private readonly IEmailService _emailSender;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public ChatRoomUserService(IChatRoomUserRepository chatRoomUserRepository,
            IMapper mapper,
            IUserService userService,
            IUserRoleService userRoleService,
            IUserRepository userRepository,
            IPatientService patientService,
            IStaffService staffService,
            IGroupSessionInvitationRepository groupSessionInvitationRepository
            ,ITokenService tokenService,
            IOrganizationSMTPRepository organizationSMTPRepository,
            IHostingEnvironment env,
            IEmailService emailSender,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IEmailWriteService emailWriteService)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _chatRoomUserRepository = chatRoomUserRepository;
            _mapper = mapper;
            _userService = userService;
            _userRoleService = userRoleService;
            _userRepository = userRepository;
            _patientService = patientService;
            _staffService = staffService;
            _groupSessionInvitationRepository = groupSessionInvitationRepository;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _env = env;
            _emailWriteService = emailWriteService;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public JsonModel SaveChatRoomUser(ChatRoomUserModel chatRoomUserModel, TokenModel tokenModel)
        {
            var chatRoomUser = _chatRoomUserRepository.GetRoomInfoByRoomIdAndUserId(chatRoomUserModel, tokenModel);
            if (chatRoomUser != null)
                return new JsonModel(chatRoomUser, StatusMessage.ChatRoomUserExisted, (int)HttpStatusCode.Created);

            var roomUser = _mapper.Map<ChatRoomUser>(chatRoomUserModel);
            roomUser.CreatedBy = chatRoomUserModel.UserId;

            var savedUserInRoom =  _chatRoomUserRepository.SaveNewChatRoomUser(roomUser, tokenModel);

            if (savedUserInRoom != null)
                response = new JsonModel(savedUserInRoom, StatusMessage.ChatRoomUserSaved, (int)HttpStatusCode.OK);
            else
                response = new JsonModel(null, StatusMessage.ChatRoomUserNotSaved, (int)HttpStatusCode.InternalServerError);
            return response;
        }

        public JsonModel GetUserInChatRoom(int roomId, TokenModel tokenModel)
        {
            var chatRoomUser = _chatRoomUserRepository.GetRoomInfoByRoomId(roomId, tokenModel);
            var staffName = "";
            if (chatRoomUser == null || chatRoomUser.ToList().Count == 0)
                return new JsonModel(chatRoomUser, StatusMessage.NoUserExistInRoom, (int)HttpStatusCode.NotFound);
            List<ChatRoomUserDetailModel> chatRoomUserDetailList = new List<ChatRoomUserDetailModel>();
            chatRoomUser.ToList().ForEach(u =>
            {
                Entity.User user = _userRepository.Get(a => a.Id == u.UserId);

                if (user != null)
                {
                    string role = string.Empty;
                    JsonModel userRole = _userRoleService.GetRoleById(user.RoleID, tokenModel);
                    if (userRole.StatusCode == (int)HttpStatusCodes.OK)
                    {
                        UserRoleModel userRoleModel = (UserRoleModel)userRole.data;
                        role = userRoleModel.UserType;
                    }

                    //var userRole=role                    
                    if (role.ToUpper() == UserTypeEnum.CLIENT.ToString().ToUpper())
                    {
                        var response = _patientService.GetPatientIdByUserId(u.UserId, tokenModel);
                        var usr = _userRepository.GetUserByID(user.Id);
                      
                        var patientId = 0;
                        if (response.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            patientId = (int)response.data;
                            var patientResponse = _patientService.GetPatientById(patientId, tokenModel);
                            PatientDemographicsModel patient;
                            if (patientResponse.StatusCode == (int)HttpStatusCodes.OK)
                                patient = (PatientDemographicsModel)patientResponse.data;
                            else
                                patient = null;
                            if (patient != null)
                            {
                                ChatRoomUserDetailModel chatRoomUserDetailModel = new ChatRoomUserDetailModel()
                                {
                                    Image = patient.PhotoThumbnailPath,
                                    Name = patient.FirstName + " " + patient.LastName,
                                    Role = role.ToUpper(),
                                    RoomId = user.RoleID,
                                    UserId = u.UserId
                                };
                                chatRoomUserDetailList.Add(chatRoomUserDetailModel);

                                if(usr!=null && !usr.IsOnline)
                                {
                                    
                                  var callingTime=  CommonMethods.ConvertUtcTime(DateTime.Now, tokenModel);
                                    if(chatRoomUserDetailList!=null)
                                    {
                                        staffName = chatRoomUserDetailList[0].Name;
                                    }
                                    string error = SendAppointmentEmail(
                                        u.UserId,
                                        staffName,Convert.ToString(callingTime),
                                        patient.Email,
                                         patient.FirstName + " " + patient.LastName,
                                         (int)EmailType.BookAppointment,
                                         (int)EmailSubType.BookAppointmentToClient,
                                         1,
                                         "/templates/chat-video.html",
                                         "Provider trying to contact you",
                                         tokenModel,
                                         tokenModel.Request.Request
                                         );
                                }




                            }
                        }

                    }
                    else if (role.ToUpper() == UserTypeEnum.PROVIDER.ToString().ToUpper() || role.ToUpper() == UserTypeEnum.STAFF.ToString().ToUpper() || role.ToUpper() == UserTypeEnum.ADMIN.ToString().ToUpper())
                    {
                        var staff = _staffService.GetStaffByUserId(user.Id, tokenModel);
                        
                        if (staff != null)
                        {
                            ChatRoomUserDetailModel chatRoomUserDetailModel = new ChatRoomUserDetailModel()
                            {
                                Image = Common.CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, staff.PhotoThumbnailPath),
                                Name = staff.FirstName + " " + staff.LastName,
                                Role = role.ToUpper(),
                                RoomId = user.RoleID,
                                UserId = u.UserId
                            };
                            chatRoomUserDetailList.Add(chatRoomUserDetailModel);
                        }
                    }
                    else
                    {
                        var invitaion = _groupSessionInvitationRepository.GetGroupSessionByUserId(u.UserId, tokenModel);
                        if(invitaion!=null)
                        {
                            ChatRoomUserDetailModel chatRoomUserDetailModel = new ChatRoomUserDetailModel()
                            {
                                Image = null,
                                Name = invitaion.Name,
                                Role = role.ToUpper(),
                                RoomId = user.RoleID,
                                UserId = u.UserId
                            };
                            chatRoomUserDetailList.Add(chatRoomUserDetailModel);
                        }
                    }

                }
            });
            if (chatRoomUserDetailList.Count>0)
                response = new JsonModel(chatRoomUserDetailList, StatusMessage.UserExistInRoom, (int)HttpStatusCode.OK);
            else
                response = new JsonModel(null, StatusMessage.NoUserExistInRoom, (int)HttpStatusCode.NotFound);
            return response;

        }
        private string SendAppointmentEmail(int userid,string staffName,string callTime, string toEmail, string username, int emailType, int emailSubType, int primaryId, string templatePath, string subject, TokenModel tokenModel, HttpRequest Request)
        {
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
            var hostingServer = _configuration.GetSection("HostingServer").Value;
            emailHtml = emailHtml.Replace("{{img_url}}", hostingServer + "img/cbimage.jpg");
            emailHtml = emailHtml.Replace("{{username}}", username);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            emailHtml = emailHtml.Replace("{{AppStaff}}",staffName);
            emailHtml = emailHtml.Replace("{{AppTime}}", callTime);
            EmailModel emailModel = new EmailModel
            {
                EmailBody = CommonMethods.Encrypt(emailHtml),
                ToEmail = CommonMethods.Encrypt(toEmail),
                EmailSubject = CommonMethods.Encrypt(subject),
                EmailType = emailType,
                EmailSubType = emailSubType,
                PrimaryId = primaryId,
                CreatedBy =userid
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
    }
}
