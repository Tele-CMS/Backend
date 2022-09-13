//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Staff;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Service.IServices.SecurityQuestion;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Service.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    [Produces("application/json")]
    [Route("authentication")]
    [ValidateModel]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ITokenService _tokenService;
        private readonly IAuditLogService _auditLogService;
        private readonly IUserService _userService;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly HC.Patient.Service.IServices.User.IUserService _usrService;
        private readonly double BlockedHour = 2;
        CommonMethods commonMethods = null;
        private string DomainName = HCOrganizationConnectionStringEnum.Host; //its Merging db
        public AuthenticationController(ITokenService tokenService, IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IAuditLogService auditLogService, IUserService userService, ISecurityQuestionService securityQuestionService, IRolePermissionService rolePermissionService, HC.Patient.Service.IServices.User.IUserService usrService)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _auditLogService = auditLogService;
            _userService = userService;
            _securityQuestionService = securityQuestionService;
            _rolePermissionService = rolePermissionService;
            commonMethods = new CommonMethods();
            _usrService = usrService;

            _logger = loggerFactory.CreateLogger<AuthenticationController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            _tokenService = tokenService;
        }

        [HttpPost("superAdminLogin")]
        public IActionResult SuperAdminLogin([FromBody]ApplicationUser applicationUser)
        {
            //check user exit in database or not
            var dbUser = _tokenService.GetSupadminUserByUserName(applicationUser.UserName);

            if (dbUser != null) //if user exist in database
            {
                // token model just IP
                TokenModel token = GetIPFromRequst();

                //Check Credentials
                var identity = GetSuperAdminClaimsIdentity(applicationUser, dbUser);

                //if credentials are wrong
                if (identity == null)
                {
                    _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);
                    //if (dbUser.AccessFailedCount >= 3)
                    //{
                    //    //increase failed login count
                    //    _userService.UpdateAccessFailedCount(dbUser.Id, token);
                    //}
                    //else
                    //{
                    //    //increase failed login count
                    //    _userService.UpdateAccessFailedCount(dbUser.Id, token);
                    //}
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidUserOrPassword,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    });
                }

                //string[] userRole = { dbUser.UserRoles.RoleName };

                StringValues Host = string.Empty; HttpContext.Request.Headers.TryGetValue("BusinessToken", out Host);
                if (!string.IsNullOrEmpty(Host))
                {
                    DomainName = commonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : applicationUser.BusinessToken);
                }

                var claims = new[]
                {
                    new Claim("UserID", dbUser.Id.ToString()),
                    new Claim("RoleID", 0.ToString()),                      // not required please don't chamge
                    new Claim("UserName", dbUser.UserName.ToString()),
                    new Claim("OrganizationID", 0.ToString()),              // not required please don't chamge
                    new Claim("StaffID", 0.ToString()),                     // not required please don't chamge
                    new Claim("LocationID", 0.ToString()),                  // not required please don't chamge
                    new Claim("DomainName",DomainName),                     // Domain name always add in token
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst("HealthCare")
            };

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    notBefore: _jwtOptions.NotBefore,
                    expires: _jwtOptions.Expiration,
                    signingCredentials: _jwtOptions.SigningCredentials);

                //add login user's role in token
                //jwt.Payload["roles"] = userRole;

                //token.LocationID = defaultLocation;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                if (dbUser.Id > 0)
                {//login
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);
                }
                // Serialize and return the response
                // retuen the staff or patient response
                // if (userRole[0] == OrganizationRoles.Client.ToString())
                //{
                // var response = Json(new JsonModel
                // {
                //  access_token = encodedJwt,
                //    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                //    data = _tokenService.GetPaitentByUserID(dbUser.Id),
                //});
                //return response;
                //}
                //else
                // {

                dbUser.Password = null;
                var response = Json(new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    data = dbUser,
                });
                return response;

                //}
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidUserOrPassword,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }

        }

        [HttpPost("patientLogin")]
        public IActionResult Login([FromBody]ApplicationUser applicationUser)
        {
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);
            if (dbUser != null)
            {
                if (dbUser.UserRoles.RoleName == OrganizationRoles.Client.ToString())
                {
                    return LoginWithSecurityQuestion(applicationUser);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidUserOrPassword,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidUserOrPassword,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }
        }

        private int GetOrganizationIDByBusinessName()
        {
            int OrgID = 0;
            StringValues businessName;
            HttpContext.Request.Headers.TryGetValue("BusinessToken", out businessName); //get host name from header            
            OrgID = _tokenService.GetOrganizationIDByName(commonMethods.Decrypt(businessName));
            return OrgID;
        }

        [HttpPost("login")]
        public IActionResult Get([FromBody]ApplicationUser applicationUser)
        {
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);
            if (dbUser != null)
            {
                if (dbUser.UserRoles.RoleName != OrganizationRoles.Client.ToString())
                {
                    return LoginWithSecurityQuestion(applicationUser);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidUserOrPassword,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidUserOrPassword,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }
        }

        /// <summary>
        /// this method is use to check the security question for patient and staff and pass the admin to direct 
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        private IActionResult LoginWithSecurityQuestion(ApplicationUser applicationUser)
        {
            //check user exits in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);

            if (dbUser != null) //if user exist in database
            {
                //get role of user
                string[] userRole = { dbUser.UserRoles.RoleName };

                // token model just IP
                TokenModel token = GetIPFromRequst();

                //set organization id from login's user
                token.OrganizationID = dbUser.OrganizationID;

                //check credetials are valid or not and set identity
                var identity = GetClaimsIdentity(applicationUser, dbUser);

                //check if user credetials are wrong 
                if (identity == null) // if identity is null (wrong credentials)
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");

                    //response status
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return Json(new
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode//(Invalid credentials)
                    });
                    //return BadRequest("Invalid credentials");
                }
                else if (identity != null) //if credentials are vaild
                {
                    //direct login for admin 
                    if (userRole[0] == OrganizationRoles.Admin.ToString())
                    {
                        //return user with success
                        return LoginUser(applicationUser, dbUser, token, identity);
                    }

                    // is question exist in db for user
                    List<UserSecurityQuestionAnswer> userSecurityQuestionAnswer = _securityQuestionService.GetUserSecurityQuestionsById(dbUser.Id, token);

                    #region Check user already login on same machine
                    CheckUserAlreadyloginFromSameMachine(applicationUser, dbUser);
                    #endregion

                    Patients patient = _tokenService.GetPaitentByUserID(dbUser.Id);
                    // if user is patient and not null and his portal or isactive = false
                    if (userRole[0] == OrganizationRoles.Client.ToString() && patient != null && (patient.IsPortalActivate == false || patient.IsActive == false))
                    {
                        //check patient is Active
                        if (!patient.IsActive)
                        {
                            Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            return Json(new
                            {
                                data = new object(),
                                Message = StatusMessage.ClientActiveStatus,
                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                            });
                        }
                        //check patient portal is enabled
                        else if (!patient.IsPortalActivate)
                        {
                            //_logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                            Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            return Json(new
                            {
                                data = new object(),
                                Message = StatusMessage.ClientPortalDeactivedAtLogin,
                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                            });
                        }
                        else
                        {
                            Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                            return Json(new
                            {
                                data = new object(),
                                Message = UserAccountNotification.LoginFailed,
                                StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                            });
                        }
                    }
                    //if user blocked 
                    else if (dbUser.IsBlock == true)
                    {
                        //checking user block time
                        if (DateTime.UtcNow - dbUser.BlockDateTime >= TimeSpan.FromHours(BlockedHour))//BlockedHour set 2 hour TO DO it should be global
                        {
                            //first login of user which didn't give answer of any question
                            if (userSecurityQuestionAnswer.Count == 0 || userSecurityQuestionAnswer == null)
                            {
                                //return
                                var response = Json(new
                                {

                                    data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                    StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                    Message = SecurityQuestionNotification.RequiredAnswers,//"Please give the answers of the questions"
                                    firstTimeLogin = true
                                });
                                return response;
                            }
                            // if user is using another machine
                            else if (applicationUser.IsValid == false)
                            {
                                var response = Json(new
                                {
                                    data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                    StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                    Message = SecurityQuestionNotification.AtleastOneAnswer, //"Please give the answer of this question"
                                    firstTimeLogin = false
                                });
                                return response;
                            }
                            //if user is vaild and answer are given
                            else
                            {
                                //patient login
                                if (userRole[0] == OrganizationRoles.Client.ToString())
                                {
                                    //patient will be reset in "LoginPatient" function
                                    //return patient with success
                                    return LoginPatient(applicationUser, dbUser, token, identity);
                                }
                                else
                                {
                                    //reset the user
                                    _userService.ResetUserAccess(dbUser.Id, token);
                                    //return user with success
                                    return LoginUser(applicationUser, dbUser, token, identity);
                                }
                            }
                        }
                        else
                        {
                            //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                            JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                            //return
                            Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                            return Json(new
                            {
                                data = new object(),
                                Message = jsonModel.Message,
                                StatusCode = jsonModel.StatusCode//(Invalid credentials)
                            });
                        }
                    }
                    //if user not blocked
                    else if (dbUser.IsBlock == false)
                    {
                        //first login of user which didn't give answer of any question
                        if (userSecurityQuestionAnswer.Count == 0 || userSecurityQuestionAnswer == null)
                        {
                            var response = Json(new
                            {
                                data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                Message = SecurityQuestionNotification.RequiredAnswers, //"Please give the answers of the questions"
                                firstTimeLogin = true
                            });
                            return response;
                        }
                        // if user is using another machine
                        else if (applicationUser.IsValid == false)
                        {
                            var response = Json(new
                            {
                                data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                Message = SecurityQuestionNotification.AtleastOneAnswer, //"Please give the answer of this question"
                                firstTimeLogin = false
                            });
                            return response;
                        }
                        //if user is vaild and answer are given
                        else
                        {
                            //patient login
                            if (userRole[0] == OrganizationRoles.Client.ToString())
                            {
                                //patient will be reset in "LoginPatient" function
                                //return patient with success
                                return LoginPatient(applicationUser, dbUser, token, identity);
                            }
                            else
                            {
                                //reset the user
                                _userService.ResetUserAccess(dbUser.Id, token);
                                //return user with success
                                return LoginUser(applicationUser, dbUser, token, identity);
                            }
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                        return Json(new
                        {
                            data = new object(),
                            Message = UserAccountNotification.LoginFailed,
                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                        });
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return Json(new
                    {
                        data = new object(),
                        Message = UserAccountNotification.LoginFailed,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return Json(new
                {
                    data = new object(),
                    Message = UserAccountNotification.UserNamePasswordNotVaild,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });

            }
        }

        private void CheckUserAlreadyloginFromSameMachine(ApplicationUser applicationUser, User dbUser)
        {
            MachineLoginLog machineLoginLog = new MachineLoginLog();
            machineLoginLog.UserId = dbUser.Id;
            machineLoginLog.IpAddress = applicationUser.IpAddress;
            machineLoginLog.MacAddress = applicationUser.MacAddress;
            machineLoginLog.OrganizationID = dbUser.OrganizationID;
            bool checkuser = _userService.UserAlreadyLoginFromSameMachine(machineLoginLog);
            applicationUser.IsValid = checkuser;
        }

        /// <summary>
        /// this method is use to login staff members
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        /// <param name="token"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private IActionResult LoginUser(ApplicationUser applicationUser, User dbUser, TokenModel token, ClaimsIdentity identity)
        {
            int defaultLocation = 0;//default loation id initalize with 0
            int StaffID = 0;//default staff id initalize with 0
            Staffs staff = null;
            //get default location 

            if (dbUser.Id > 0)
            {
                defaultLocation = _tokenService.GetDefaultLocationOfStaff(dbUser.Id);
                staff = _tokenService.GetDoctorByUserID(dbUser.Id);
            }

            //set default location id of login's user 
            token.LocationID = defaultLocation;

            //staff id
            if (staff != null)
            {
                StaffID = staff.Id;
            }

            //get login user role name
            string[] userRole = { dbUser.UserRoles.RoleName };

            //save IP and MAC address
            #region save IP and MAC Address
            bool machineData = false;
            machineData = SaveMachineDataIPAndMac(applicationUser, dbUser);
            #endregion
            #region get doman name            
            ////string[] userRole = { dbUser.UserRoles.RoleName };

            StringValues Host = string.Empty; HttpContext.Request.Headers.TryGetValue("BusinessToken", out Host);
            if (!string.IsNullOrEmpty(Host))
            {
                DomainName = commonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : applicationUser.BusinessToken);
            }
            #endregion
            //create claim for login user
            var claims = new[]
            {
                    new Claim("UserID", dbUser.Id.ToString()),
                    new Claim("RoleID", dbUser.RoleID.ToString()),
                    new Claim("UserName", dbUser.UserName.ToString()),
                    new Claim("OrganizationID", dbUser.OrganizationID.ToString()),
                    new Claim("StaffID", StaffID.ToString()),
                    new Claim("LocationID", defaultLocation.ToString()),
                    new Claim("DomainName",DomainName),                     // Domain name always add in token
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst("HealthCare")
            };

            //Not required to reset user its already done from where this method call

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            //add login user's role in token
            jwt.Payload["roles"] = userRole;


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //login logs
            if (dbUser.Id > 0)
            {//login
                _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);

                // set this user online
                _usrService.SetOnline(dbUser.Id);
            }

            // Serialize and return the response
            //get role permission
            JsonModel UserPermission = _rolePermissionService.GetUserPermissions(token, dbUser.RoleID);

            //get app configuration for user
            List<AppConfigurationsModel> AppConfigurations = _tokenService.GetAppConfigurationByOrganizationByID(token);

            //get users all location
            List<UserLocationsModel> userLocation = _tokenService.GetUserLocations(dbUser.Id);

            // return the staff or patient response
            if (userRole[0] == OrganizationRoles.Client.ToString())
            {
                var response = Json(new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    data = _tokenService.GetPaitentByUserID(dbUser.Id),

                });
                return response;
            }
            else
            {
                var response = Json(new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    data = _tokenService.GetDoctorByUserID(dbUser.Id),
                    UserPermission = UserPermission.data,
                    AppConfigurations = AppConfigurations,
                    UserLocations = userLocation
                });
                return response;
            }
        }

        /// <summary>
        /// this method is use for patient login
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        /// <param name="token"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private IActionResult LoginPatient(ApplicationUser applicationUser, User dbUser, TokenModel token, ClaimsIdentity identity)
        {
            //patient inital
            int defaultLocation = 0;//default loation id initalize with 0
            int PatientID = 0;
            Patients patient = null;

            //get patient
            if (dbUser.Id > 0)
            {
                patient = _tokenService.GetPaitentByUserID(dbUser.Id);
            }

            //get patient id and his location
            if (patient != null) { PatientID = patient.Id; defaultLocation = patient.LocationID; }

            //check patient is Active
            if (!patient.IsActive)
            {
                Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ClientActiveStatus,
                    StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                });
            }

            //check patient portal is enabled
            if (!patient.IsPortalActivate)
            {
                //_logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ClientPortalDeactivedAtLogin,
                    StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                });
            }


            //save IP and MAC address
            #region save IP and MAC Address
            bool machineData = false;
            machineData = SaveMachineDataIPAndMac(applicationUser, dbUser);
            #endregion
            //get login user role name
            string[] userRole = { dbUser.UserRoles.RoleName };
            //string[] userRole = { dbUser.UserRoles.RoleName };

            StringValues Host = string.Empty; HttpContext.Request.Headers.TryGetValue("BusinessToken", out Host);
            if (!string.IsNullOrEmpty(Host))
            {
                DomainName = commonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : applicationUser.BusinessToken);
            }

            //create claim for login user
            var claims = new[]
            {
        new Claim("UserID", dbUser.Id.ToString()),
        new Claim("RoleID", dbUser.RoleID.ToString()),
        new Claim("UserName", dbUser.UserName.ToString()),
        new Claim("OrganizationID", dbUser.OrganizationID.ToString()),
        new Claim("PatientID", PatientID.ToString()),
        new Claim("LocationID", defaultLocation.ToString()),
        new Claim("DomainName",DomainName),                     // Domain name always add in token
        new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
        identity.FindFirst("HealthCare")
      };

            //reset the user
            _userService.ResetUserAccess(dbUser.Id, token);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            //add login user's role in token
            jwt.Payload["roles"] = userRole;

            //token.LocationID = defaultLocation;
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            if (dbUser.Id > 0)
            {//login
                _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);
                
                // set this user online
                _usrService.SetOnline(dbUser.Id);

            }
            // Serialize and return the response
            var response = Json(new JsonModel
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                data = _tokenService.GetPaitentByUserID(dbUser.Id),
            });
            return response;
        }

        private bool SaveMachineDataIPAndMac(ApplicationUser applicationUser, User dbUser)
        {
            bool machineData;
            MachineLoginLog machineLoginLog = new MachineLoginLog();
            machineLoginLog.IpAddress = applicationUser.IpAddress;
            machineLoginLog.MacAddress = applicationUser.MacAddress;
            machineLoginLog.OrganizationID = dbUser.OrganizationID;
            machineLoginLog.UserId = dbUser.Id;
            machineLoginLog.LoginDate = DateTime.UtcNow;
            machineData = _userService.SaveMachineLoginUser(machineLoginLog);
            return machineData;
        }

        /// <summary>
        /// this method is use to get the ip from request
        /// </summary>
        /// <returns></returns>
        private TokenModel GetIPFromRequst()
        {
            StringValues ipAddress;
            TokenModel token = new TokenModel();
            HttpContext.Request.Headers.TryGetValue("IPAddress", out ipAddress);
            if (!string.IsNullOrEmpty(ipAddress)) { token.IPAddress = ipAddress; } else { token.IPAddress = "203.129.220.76"; }
            return token;
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

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        /// <summary>
        /// save question's answer for users
        /// </summary>
        /// <param name="securityQuestionListModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUserSecurityQuestion")]
        public IActionResult SaveUserSecurityQuestion([FromBody]SecurityQuestionListModel securityQuestionListModel)
        {
            TokenModel token = new TokenModel();
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            var dbUser = _tokenService.GetUserByUserName(securityQuestionListModel.UserName, OrgID);
            if (dbUser != null)
            {
                //set organization id 
                token.OrganizationID = dbUser.OrganizationID;

                //set userID id 
                token.UserID = dbUser.Id;

                //application user get username & password from request
                ApplicationUser appUser = new ApplicationUser();
                appUser.Password = securityQuestionListModel.Password;
                appUser.UserName = securityQuestionListModel.UserName;
                appUser.IpAddress = securityQuestionListModel.IpAddress;
                appUser.MacAddress = securityQuestionListModel.MacAddress;

                //check credetials are valid or not and set identity
                var identity = GetClaimsIdentity(appUser, dbUser);

                //if credentials are right
                if (identity != null)
                {
                    //save user's security question
                    bool status = _securityQuestionService.SaveUserSecurityQuestion(securityQuestionListModel, token);

                    //check status true or false (saved successfully saved or not)
                    if (status)
                    {
                        //get role of user
                        string[] userRole = { dbUser.UserRoles.RoleName };

                        //patient login
                        if (userRole[0] == OrganizationRoles.Client.ToString())
                        {
                            //patient will be reset in "LoginPatient" function
                            //return patient with success
                            return LoginPatient(appUser, dbUser, token, identity);
                        }
                        else
                        {
                            //reset the user
                            _userService.ResetUserAccess(dbUser.Id, token);
                            //return user with success
                            return LoginUser(appUser, dbUser, token, identity);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = UserAccountNotification.LoginFailed,
                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                        });
                    }
                }
                else // if credentials are wrong
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({appUser.UserName}) or password ({appUser.Password})");

                    //response status
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return Json(new
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode//(Invalid credentials)
                    });
                    //return BadRequest("Invalid credentials");
                }
            }
            // if user not exist
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidCredentials,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }

        }

        /// <summary>
        /// compare answer of the question if user try to login with new machine
        /// </summary>
        /// <param name="securityQuestionModel"></param>
        /// <returns></returns>
        [HttpPost("CheckQuestionAnswer")]
        public IActionResult CheckQuestionAnswer([FromBody]SecurityQuestionModel securityQuestionModel)
        {
            TokenModel token = new TokenModel();
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName();
            var dbUser = _tokenService.GetUserByUserName(securityQuestionModel.UserName, OrgID);
            if (dbUser != null)
            {
                //set organization id 
                token.OrganizationID = dbUser.OrganizationID;

                //set userID id 
                token.UserID = dbUser.Id;

                //application user get username & password from request
                ApplicationUser appUser = new ApplicationUser();
                appUser.Password = securityQuestionModel.Password;
                appUser.UserName = securityQuestionModel.UserName;
                appUser.IpAddress = securityQuestionModel.IpAddress;
                appUser.MacAddress = securityQuestionModel.MacAddress;

                //check credetials are valid or not and set identity
                var identity = GetClaimsIdentity(appUser, dbUser);

                if (identity != null)
                {
                    //check question answer is right or wrong
                    bool status = _securityQuestionService.CheckUserQuestionAnswer(securityQuestionModel, token);

                    if (status && dbUser.IsBlock == false)
                    {
                        //get role of user
                        string[] userRole = { dbUser.UserRoles.RoleName };

                        //patient login
                        if (userRole[0] == OrganizationRoles.Client.ToString())
                        {
                            //patient will be reset in "LoginPatient" function
                            //return patient with success
                            return LoginPatient(appUser, dbUser, token, identity);
                        }
                        else
                        {
                            //reset the user
                            _userService.ResetUserAccess(dbUser.Id, token);
                            //return user with success
                            return LoginUser(appUser, dbUser, token, identity);
                        }
                    }
                    //if user blocked
                    else if (dbUser.IsBlock)
                    {
                        //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                        JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);
                        Response.StatusCode = jsonModel.StatusCode;//(Invalid credentials)
                        return Json(new
                        {
                            data = new object(),
                            Message = jsonModel.Message,
                            StatusCode = jsonModel.StatusCode//(Invalid credentials)
                        });
                    }
                    //if answer is wrong
                    else
                    {
                        //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                        JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);
                        if (dbUser.IsBlock)
                        {
                            Response.StatusCode = jsonModel.StatusCode; //wrong answer
                            return Json(new
                            {
                                data = new object(),
                                Message = jsonModel.Message,
                                StatusCode = jsonModel.StatusCode //wrong answer
                            });
                        }
                        else
                        {
                            Response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity; //wrong answer
                            return Json(new
                            {
                                data = new object(),
                                Message = SecurityQuestionNotification.IncorrectAnswer,
                                StatusCode = (int)HttpStatusCodes.UnprocessedEntity //wrong answer
                            });
                        }


                    }
                }
                else // if credentials are wrong
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({appUser.UserName}) or password ({appUser.Password})");

                    //response status
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return Json(new
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode//(Invalid credentials)
                    });
                    //return BadRequest("Invalid credentials");
                }
            }
            // if user not exist
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidCredentials,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }
        }

        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static ClaimsIdentity GetClaimsIdentity(ApplicationUser user, User dbUser)
        {
            CommonMethods commonMethods = new CommonMethods();

            if (dbUser != null && (user.UserName.ToUpper() == dbUser.UserName.ToUpper() && user.Password == commonMethods.Decrypt(dbUser.Password)))
            {
                return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new[]
                  {
                   new Claim("HealthCare", "IAmAuthorized")
                  });
            }
            else
            {
                return null;
            }

            // Credentials are invalid, or account doesn't exist
            //return Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static ClaimsIdentity GetSuperAdminClaimsIdentity(ApplicationUser user, SuperUser dbUser)
        {
            CommonMethods commonMethods = new CommonMethods();

            if (dbUser != null && (user.UserName.ToUpper() == dbUser.UserName.ToUpper() && user.Password == commonMethods.Decrypt(dbUser.Password)))
            {
                return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new[]
                  {
                   new Claim("HealthCare", "IAmAuthorized")
                  });
            }
            else
            {
                return null;
            }

            // Credentials are invalid, or account doesn't exist
            //return Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// this will set user offline
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Logout")]
        public JsonModel Logout()
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return _usrService.Logout(token);
        }
    }





    //public class AuthenticationController : Controller
    //{
    //    ITokenService _tokenService;
    //    CommonMethods commonMethods;
    //    public AuthenticationController(ITokenService tokenService)
    //    {
    //        _tokenService = tokenService;
    //        commonMethods = new CommonMethods();
    //    }

    //    [HttpPost("superAdminLogin")]
    //    public IActionResult SuperAdminLogin([FromBody]ApplicationUser applicationUser)
    //    {
    //        TokenModel Token = GetIPFromRequst(applicationUser.BusinessToken);
    //        var Result = _tokenService.AuthenticateSuperUser(applicationUser, Token);

    //        if (Result.StatusCode == (int)HttpStatusCodes.Unauthorized)
    //        {
    //            return Json(new
    //            {
    //                data = new object(),
    //                Message = StatusMessage.InvalidUserOrPassword,
    //                StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
    //            });
    //        }
    //        else
    //        {
    //            return Json(new
    //            {
    //                Result.data,
    //                access_token = Result.access_token,
    //                expires_in = Result.expires_in
    //            });
    //        }

    //    }

    //    [HttpPost("login")]
    //    public IActionResult Get([FromBody]ApplicationUser applicationUser)
    //    {
    //        TokenModel Token = GetIPFromRequst(applicationUser.BusinessToken);
    //        //check user exit in database or not
    //        var Result = _tokenService.AuthenticateAgency(applicationUser, Token);

    //        if (string.IsNullOrWhiteSpace(Result.access_token) && Result.data == new object())
    //        {
    //            return Json(new
    //            {
    //                data = new object(),
    //                Message = Result.Message,
    //                StatusCode = Result.StatusCode//(Invalid credentials)
    //            });
    //        }
    //        else
    //        {
    //            Result.StatusCode = (int)HttpStatusCodes.OK;
    //            return Json(Result);
    //        }

    //    }

    //    [HttpPost("patientLogin")]
    //    public IActionResult Login([FromBody]ApplicationUser applicationUser)
    //    {
    //        TokenModel Token = GetIPFromRequst(applicationUser.BusinessToken);
    //        //check user exit in database or not
    //        var Result = _tokenService.AuthenticateAgency(applicationUser, Token);

    //        if (string.IsNullOrWhiteSpace(Result.access_token))
    //        {
    //            return Json(new
    //            {
    //                data = new object(),
    //                Message = Result.Message,
    //                StatusCode = Result.StatusCode//(Invalid credentials)
    //            });
    //        }
    //        else
    //        {
    //            Result.StatusCode = (int)HttpStatusCodes.OK;
    //            return Json(Result);
    //        }
    //    }

    //    [HttpPost("SaveUserSecurityQuestion")]
    //    public IActionResult SaveUserSecurityQuestion([FromBody]SecurityQuestionListModel securityQuestionListModel)
    //    {
    //        TokenModel token = GetIPFromRequst("");
    //        var Result = _tokenService.SaveUserScurityQuestion(securityQuestionListModel, token);
    //        return Json(Result);
    //    }

    //    /// <summary>
    //    /// compare answer of the question if user try to login with new machine
    //    /// </summary>
    //    /// <param name="securityQuestionModel"></param>
    //    /// <returns></returns>
    //    [HttpPost("CheckQuestionAnswer")]
    //    public IActionResult CheckQuestionAnswer([FromBody]SecurityQuestionModel securityQuestionModel)
    //    {
    //        TokenModel token = GetIPFromRequst("");
    //        var Result = _tokenService.CheckQuestionAnswer(securityQuestionModel, token);
    //        return Json(Result);
    //    }


    //    private TokenModel GetIPFromRequst(string BusinessToken)
    //    {
    //        StringValues ipAddress;
    //        StringValues Host = string.Empty;
    //        HttpContext.Request.Headers.TryGetValue("IPAddress", out ipAddress);
    //        HttpContext.Request.Headers.TryGetValue("BusinessToken", out Host);
    //        TokenModel token = new TokenModel();
    //        if (!string.IsNullOrEmpty(Host))
    //        {
    //            token.DomainName = commonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : BusinessToken);
    //        }
    //        else
    //        {
    //            token.DomainName = string.Empty;
    //        }
    //        if (!string.IsNullOrEmpty(ipAddress))
    //        {
    //            token.IPAddress = ipAddress;
    //        }
    //        else
    //        {
    //            token.IPAddress = "203.129.220.76";
    //        }
    //        return token;
    //    }
    //}
}