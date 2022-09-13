using HC.Patient.Entity;
using HC.Common;
using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Password;
using Newtonsoft.Json.Linq;
using System;
using HC.Common.Model.OrganizationSMTP;
using Microsoft.AspNetCore.Hosting;
using HC.Common.Services;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Service.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Data;
using System.Linq;
using HC.Patient.Model.Organizations;
using Microsoft.Extensions.Primitives;
using HC.Service;
using HC.Patient.Repositories.IRepositories.Patient;

namespace HC.Patient.Service.Services.Password
{
    public class PasswordService : BaseService, IPasswordService
    {
        private readonly IHostingEnvironment _env;
        private readonly IEmailService _emailSender;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly HCOrganizationContext _context;
        private readonly IPatientRepository _patientRepository;
        public PasswordService(IHostingEnvironment env, IEmailService emailSender, ITokenService tokenService, IUserService userService, IOrganizationSMTPRepository organizationSMTPRepository,IPatientRepository patientRepository, HCOrganizationContext context)
        {
            _env = env;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _userService = userService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _context = context;
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// This API is use for forgot password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="token"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public JsonModel ForgotPassword(JObject userInfo, TokenModel token, HttpRequest Request)
        {

            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Email = string.Empty;
            Patients patientData = null;
            if (userInfo != null)
            {
                UserPassword forgotPassword = userInfo.ToObject<UserPassword>();
                // get user info
                Entity.User userData = _tokenService.GetUserByUserName(forgotPassword.UserName, token.OrganizationID);
                if (userData != null)
                {
                    //get staff info from user
                    Staffs staffData = _tokenService.GetDoctorByUserID(userData.Id,token);
                    if (staffData != null)
                    {
                        FirstName = staffData.FirstName;
                        LastName = staffData.LastName;
                        Email = staffData.Email;
                    }
                    else //else staff null then check for patient
                    {
                        patientData = _tokenService.GetPaitentByUserID(userData.Id,token);
                        if (patientData != null)
                        {
                            PHIDecryptedModel pHIDecryptedModel =_patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patientData.FirstName, null, patientData.LastName, null, patientData.Email, null, null, null, null, null, null, null, null, null).FirstOrDefault();
                            FirstName = pHIDecryptedModel.FirstName;
                            LastName = pHIDecryptedModel.LastName;
                            Email = pHIDecryptedModel.EmailAddress;
                        }
                    }

                    //if any of them not null
                    if (staffData != null || patientData != null)
                    {
                        StringValues businessName;
                        token.Request.Request.Headers.TryGetValue("BusinessToken", out businessName); //get host name from header                                   
                        OrganizationModel orgData = _tokenService.GetOrganizationDetailsByBusinessName(CommonMethods.Decrypt(businessName));
                        
                        OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == userData.OrganizationID && a.IsDeleted == false && a.IsActive == true);
                        OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
                        AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
                        organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);
                        AuthenticationToken authenticationToken = new AuthenticationToken();

                        authenticationToken.ResetPasswordToken = Guid.NewGuid().ToString();
                        authenticationToken.UserID = userData.Id;
                        authenticationToken.OrganizationID = userData.OrganizationID;

                        var authenticationData = _userService.AuthenticationToken(authenticationToken);

                        if (authenticationData != null)
                        {
                            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/reset-email.html");
                            emailHtml = emailHtml.Replace("{{action_url}}", forgotPassword.ResetPasswordURL + "?id=" + authenticationToken.ResetPasswordToken);
                            emailHtml = emailHtml.Replace("{{name}}", FirstName + " " + LastName);
                            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
                            emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
                            emailHtml = emailHtml.Replace("{{organizationName}}", orgData.OrganizationName);
                                                        
                            _emailSender.SendEmailAsync(Email, "Reset Password", emailHtml, organizationSMTPDetailModel, orgData.OrganizationName);

                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ResetPassword,
                                StatusCode = (int)HttpStatusCodes.OK
                            };
                        }
                        else
                        {
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ServerError,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };
                        }
                    }
                    else
                    {
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.InvalidData,
                            StatusCode = (int)HttpStatusCodes.Unauthorized
                        };
                    }
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidData,
                        StatusCode = (int)HttpStatusCodes.Unauthorized
                    };
                }
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.InvalidData,
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                };
            }
        }

        /// <summary>
        /// <Description>this method is use for reset password for user</Description>
        /// </summary>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public JsonModel ResetPasswordForUser(JObject userInfo, TokenModel token)
        {
            try
            {
                UserPassword userPassword = userInfo.ToObject<UserPassword>();
                AuthenticationToken authenticationToken = _context.AuthenticationToken.Where(p => p.ResetPasswordToken == userPassword.Token && p.IsActive == true && p.IsDeleted == false).FirstOrDefault();

                if (authenticationToken != null)
                {
                    Entity.User user = _context.User.Where(p => p.Id == authenticationToken.UserID).FirstOrDefault();
                    user.Password = CommonMethods.Encrypt(userPassword.Password);
                    _context.User.Update(user);
                    _context.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = UserAccountNotification.YourPasswordChanged,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ResetPasswordLinkNotVaild,
                        StatusCode = (int)HttpStatusCodes.Unauthorized
                    };
                }
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }
    }
}
