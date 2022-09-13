using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Options;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Login;
using HC.Patient.Service.IServices.Password;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Service.Users.Interfaces;
using HC.Patient.Web.Filters;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [ActionFilter]
    public class PatientCommonController : BaseController
    {
        private readonly IPatientCommonService _patientCommonService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ITokenService _tokenService;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IUserService _userService;
        private readonly IEmailService _emailSender;
        private readonly IHostingEnvironment _env;
        private JsonModel response;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IPatientService _patientService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILoginService _loginService;
        private readonly IPatientRepository _patientRepository;

        #region Construtor of the class
        public PatientCommonController(IPatientCommonService patientCommonService, IOptions<JwtIssuerOptions> jwtOptions, ITokenService tokenService,
            IUserService userService, IEmailService emailSender, IHostingEnvironment env, IPatientAppointmentService patientAppointmentService, IRolePermissionService rolePermissionService, IPatientService patientService, IOrganizationSMTPRepository organizationSMTPRepository, IPasswordService passwordService, ILoginService loginService, IPatientRepository patientRepository)
        {
            try
            {
                _patientService = patientService;
                _patientAppointmentService = patientAppointmentService;
                _jwtOptions = jwtOptions.Value;
                ThrowIfInvalidOptions(_jwtOptions);

                _tokenService = tokenService;
                this._userService = userService;
                _rolePermissionService = rolePermissionService;
                _emailSender = emailSender;
                _env = env;
                _organizationSMTPRepository = organizationSMTPRepository;
                _passwordService = passwordService;
                _loginService = loginService;
                _serializerSettings = new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                this._patientCommonService = patientCommonService;
                _patientRepository = patientRepository;

            }
            catch
            {

            }
        }

        #endregion

        #region Class Methods
        /// <summary>
        /// this method is used for check patient exist or not
        /// </summary>
        /// <param name="patientInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PatientExist")]
        public JsonResult PatientExist([FromBody]Patients patientInfo)
        {
            Patients patient = _patientCommonService.PatientExist(patientInfo);
            if (patient != null)
            {
                Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                return Json(new
                {
                    data = patient,
                    Message = StatusMessage.PatientAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Not Found)
                });
            }
        }

        /// <summary>
        /// get user info by token (Refresh token)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByToken")]
        public JsonResult GetUserByToken()
        {
            StringValues authorizationToken;
            TokenModel tokenModel = new TokenModel();
            tokenModel.Request = HttpContext;
            var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            if (authToken != null)
            {
                var encryptData = CommonMethods.GetDataFromToken(authToken);
                if (encryptData != null && encryptData.Claims != null)
                {
                    if (encryptData.Claims.Count > 0)
                    {
                        //super admin
                        if (encryptData.Claims[1].Value == "0" && encryptData.Claims[3].Value == "0")
                        {
                            //check super admin user exits in database or not
                            //in this variable 'encryptData.Claims[2].Value' only username from token
                            var superAdminUser = _tokenService.GetSupadminUserByUserName(encryptData.Claims[2].Value);
                            //return response
                            var response = Json(new JsonModel
                            {
                                access_token = authToken,
                                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                                data = superAdminUser,
                            });
                            return response;
                        }
                        //admin & staff & client
                        else
                        {
                            int defaultLocation = 0;//default loation id initalize with 0         
                            int userID = 0; //for all (patient, staff, admin)     
                            int patientID = 0;
                            int staffID = 0;//default staff id initalize with 0
                            Patients client = null; //patient entity
                            Staffs staff = null; //for staff

                            //check user exits in database or not from user name which you get from token
                            var dbUser = _tokenService.GetUserByUserName(encryptData.Claims[2].Value, Convert.ToInt32(encryptData.Claims[3].Value));
                            if (dbUser != null)
                            {
                                //get role of user
                                string[] userRole = { dbUser.UserRoles.RoleName };

                                //get user id from token for all (patient, staff, admin)     
                                userID = Convert.ToInt32(encryptData.Claims[0].Value);

                                Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)

                                if (userRole[0] == OrganizationRoles.Patient.ToString())
                                {
                                    // get patient
                                    client = _tokenService.GetPaitentByUserID(dbUser.Id, tokenModel);

                                    //get patient id and his location
                                    if (client != null)
                                    {
                                        patientID = client.Id; defaultLocation = client.LocationID;

                                        #region check patient is Active
                                        if (!client.IsActive)
                                        {
                                            Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                                            return Json(new
                                            {
                                                data = new object(),
                                                Message = StatusMessage.ClientActiveStatus,
                                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                                            });
                                        }
                                        #endregion

                                        #region check patient portal is enabled
                                        if (!client.IsPortalActivate)
                                        {
                                            Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                                            return Json(new
                                            {
                                                data = new object(),
                                                Message = StatusMessage.ClientPortalDeactivedAtLogin,
                                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                                            });
                                        }
                                        #endregion

                                        
                                        PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(client.FirstName, client.MiddleName, client.LastName, client.DOB, client.Email, null, null, null, null, null, null, null, null, null).FirstOrDefault();
                                        PatientModel patientModel = new PatientModel
                                        {
                                            FirstName = pHIDecryptedModel.FirstName,
                                            LastName = pHIDecryptedModel.LastName,
                                            PatientId=client.Id,
                                            IsActive=client.IsActive,
                                            IsBlock=client.IsBlock,
                                            PhotoThumbnailPath=client.PhotoThumbnailPath,
                                            Email= pHIDecryptedModel.EmailAddress

                                        };
                                        //patientModel.DOB = pHIDecryptedModel.DateOfBirth;

                                        #region return response
                                        var response = Json(new JsonModel
                                        {
                                            access_token = authToken,
                                            expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                                            data = client,
                                            PatientData= patientModel,
                                            PasswordExpiryStatus = _loginService.CheckPasswordExpiredMessage(dbUser, tokenModel)
                                        });
                                        return response;
                                        #endregion
                                    }
                                    else
                                    {
                                        Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                                        return Json(new
                                        {
                                            data = new object(),
                                            Message = StatusMessage.InvalidToken,
                                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                                        });
                                    }
                                }
                                else /*if (dbUser != null)*/ //admin & staff
                                {
                                    // get default location
                                    defaultLocation = _tokenService.GetDefaultLocationOfStaff(dbUser.Id);

                                    // get staff
                                    staff = _tokenService.GetDoctorByUserID(dbUser.Id, tokenModel);

                                    //set staff id
                                    staffID = staff.Id;

                                    //add organizationID 
                                    TokenModel token = new TokenModel();

                                    //set default location id of login's user 
                                    token.LocationID = defaultLocation;

                                    // set organization id
                                    token.OrganizationID = dbUser.OrganizationID;

                                    //get role permission
                                    JsonModel UserPermission = _rolePermissionService.GetUserPermissions(token, dbUser.RoleID);

                                    //get app configuration for user
                                    List<AppConfigurationsModel> AppConfigurations = _tokenService.GetAppConfigurationByOrganizationByID(token);

                                    //get users all location
                                    List<UserLocationsModel> userLocation = _tokenService.GetUserLocations(dbUser.Id);

                                    #region return respose
                                    var response = Json(new JsonModel
                                    {
                                        access_token = authToken,
                                        expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                                        data = staff,
                                        UserPermission = UserPermission.data,
                                        AppConfigurations = AppConfigurations,
                                        UserLocations = userLocation,
                                        PasswordExpiryStatus = _loginService.CheckPasswordExpiredMessage(dbUser, tokenModel)
                                    });
                                    return response;
                                    #endregion
                                }
                                //else //unauthorized
                                //{
                                //    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                                //    return Json(new
                                //    {
                                //        data = new object(),
                                //        Message = StatusMessage.InvalidToken,
                                //        StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                                //    });
                                //}
                            }
                            else //unauthorized
                            {
                                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                                return Json(new
                                {
                                    data = new object(),
                                    Message = StatusMessage.InvalidToken,
                                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                                });
                            }
                        }
                    }
                    else //unauthorized
                    {
                        Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.InvalidToken,
                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                        });
                    }
                }
                else //unauthorized
                {
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidToken,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                    });
                }
            }
            else //unauthorized
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.TokenRequired,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                });
            }
        }

        /// <summary>
        /// forgot password 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public JsonResult ForgotPassword([FromBody]JObject userInfo)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            token.OrganizationID = OrgID;
            return Json(_passwordService.ForgotPassword(userInfo, token, Request));
        }


        /// <summary>
        /// forgot password 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("SetPassword")]
        public async Task<IActionResult> SetPassword([FromBody]JObject userInfo)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            if (userInfo != null)
            {
                UserPassword forgotPassword = userInfo.ToObject<UserPassword>();
                User userData = _userService.GetUserByUserName(forgotPassword.UserName);
                if (userData != null)
                {
                    Staffs staffData = _tokenService.GetDoctorByUserID(userData.Id, token);
                    OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.GetByID(userData.OrganizationID);
                    OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
                    AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
                    organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

                    AuthenticationToken authenticationToken = new AuthenticationToken();

                    authenticationToken.ResetPasswordToken = Guid.NewGuid().ToString();
                    authenticationToken.UserID = userData.Id;

                    var authenticationData = _userService.AuthenticationToken(authenticationToken);
                    if (authenticationData != null)
                    {
                        var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                        var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/reset-email.html");
                        emailHtml = emailHtml.Replace("{{action_url}}", forgotPassword.ResetPasswordURL + "/" + authenticationToken.ResetPasswordToken);
                        emailHtml = emailHtml.Replace("{{name}}", staffData.FirstName + " " + staffData.LastName);
                        emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
                        emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
                        await _emailSender.SendEmailAsync(staffData.Email, "Reset Password", emailHtml, organizationSMTPDetailModel, "Health care");

                        Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.ResetPassword,
                            StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                        });
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCodes.InternalServerError;//(Internal server error)
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.ServerError,
                            StatusCode = (int)HttpStatusCodes.InternalServerError//(Internal server error)
                        });
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidData,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                    });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Unauthorized)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidData,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Unauthorized)
                });
            }
        }


        /// <summary>
        /// reset user password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public JsonResult ResetPassword([FromBody]JObject userInfo)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            token.OrganizationID = OrgID;
            return Json(_passwordService.ResetPasswordForUser(userInfo, token));
        }


        //[HttpGet("GetStaffAvailability")]
        //public JsonResult GetStaffAvailability(string staffID, string fromDate, string toDate)
        //{
        //    try
        //    {
        //        TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
        //        DateTime FromDate = new DateTime();
        //        DateTime ToDate = new DateTime();
        //        DateTime.TryParse(fromDate, out FromDate);
        //        DateTime.TryParse(toDate, out ToDate);
        //        FromDate = FromDate != null ? CommonMethods.ConvertUtcTime(FromDate,token) : FromDate;
        //        ToDate = ToDate != null ? CommonMethods.ConvertUtcTime(ToDate, token) : ToDate;
        //        return Json(_patientAppointmentService.GetStaffAvailability(staffID, FromDate, ToDate,token));


        //    }
        //    catch (Exception ex)
        //    {
        //        response = new JsonModel()
        //        {
        //            data = new object(),
        //            Message = ex.Message,
        //            StatusCode = (int)HttpStatusCodes.BadRequest
        //        };
        //        return Json(response);
        //    }
        //}

        [HttpGet]
        [Route("GetPatientCCDA/{id}")]
        public IActionResult GetPatientCCDA(int id)
        {
            MemoryStream tempStream = null;
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext); //{ UserID = 1, OrganizationID = 1 }; //
            tempStream = _patientService.GetPatientCCDA(id, token);
            if (ReferenceEquals(tempStream, null))
                tempStream = new MemoryStream();

            //string xmlString = patientDocService.Download();

            //XmlSerializer xml = new XmlSerializer(this.GetType());
            //FileStream xmlStream = new FileStream(Directory.GetCurrentDirectory(), FileMode.Open);
            //var result = xml.Deserialize(xmlStream);

            XmlDocument xDocument = new XmlDocument();
            xDocument.Load(Directory.GetCurrentDirectory() + "\\wwwroot\\CDA\\CDA.xsl");
            MemoryStream xmlStream = new MemoryStream();
            xDocument.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            //string fileName = "CDA.zip";
            ZipFile zip = new ZipFile();
            zip.AddEntry("cda.xml", tempStream);
            zip.AddEntry("cda.xsl", xmlStream);
            //ExportAuditLog("Download Patient Document");

            //var Response = HttpContext.Response;

            //Response.ContentType = "application/zip";
            //Response.Headers.Add("Content-Disposition", "attachment;" + (string.IsNullOrEmpty(fileName) ? "" : "filename=" + fileName));
            MemoryStream xmlStream1 = new MemoryStream();

            zip.Save(xmlStream1);
            // return new ZipFileResult(zip, fileName);
            return File(tempStream, "text/plain", "cda.xml");

        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
        private int GetOrganizationIDByBusinessName()
        {
            int OrgID = 0;
            StringValues businessName;
            HttpContext.Request.Headers.TryGetValue("BusinessToken", out businessName); //get host name from header            
            OrgID = _tokenService.GetOrganizationIDByName(CommonMethods.Decrypt(businessName));
            return OrgID;
        }
        #endregion

    }
}