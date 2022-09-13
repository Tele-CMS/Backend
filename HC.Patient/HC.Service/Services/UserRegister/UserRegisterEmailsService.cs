using HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Service.IServices;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services
{
    public class UserRegisterEmailsService : IUserRegisterEmailsService
    {
        private readonly IUserInvitationReadRepository _userInvitationReadRepository;
        private readonly ITokenService _tokenService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IHostingEnvironment _env;
        private readonly IEmailService _emailSender;
        private readonly IEmailWriteService _emailWriteService;
        public UserRegisterEmailsService(
            IUserInvitationReadRepository userInvitationReadRepository,
            ITokenService tokenService,
            IOrganizationSMTPRepository organizationSMTPRepository,
            IHostingEnvironment env,
            IEmailService emailSender,
            IEmailWriteService emailWriteService
            )
        {
            _userInvitationReadRepository = userInvitationReadRepository;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _env = env;
            _emailSender = emailSender;
            _emailWriteService = emailWriteService;
        }
        public string SendAcceptedEmailToRegisterUser(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request, bool isPatient)
        {
            var currentUserInvitation = _userInvitationReadRepository.GetUserInvitationByEmailAndOrganizationId(registerUser.Email, tokenModel.OrganizationID);
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
            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/to-register-provider.html");

            emailHtml = emailHtml.Replace("{{action_url}}", registerUser.WebUrl);
            emailHtml = emailHtml.Replace("{{username}}", registerUser.LastName + " " + registerUser.FirstName);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            string subject = string.Format("Welcome To ", organization.OrganizationName);
            string toEmail = registerUser.Email;
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
            ////await _emailSender.SendEmailAsync(userInvitationModel.Email, string.Format("Invitation From {0}", orgData.OrganizationName), emailHtml, organizationSMTPDetailModel, orgData.OrganizationName);
            //var isEmailSent = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            //emailModel.EmailStatus = isEmailSent;
            ////Maintain Email log into Db
            //var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
            //return isEmailSent;
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
