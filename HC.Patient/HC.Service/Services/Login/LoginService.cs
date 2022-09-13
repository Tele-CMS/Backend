using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.Patient;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.Login;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Service.IServices.SecurityQuestion;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Service.Users.Interfaces;
using HC.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using static HC.Common.Enums.CommonEnum;


namespace HC.Patient.Service.Services.Login
{
    public class LoginService : BaseService, ILoginService
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IAuditLogService _auditLogService;
        private readonly IUserService _userService;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IStaffService _staffService;
        private readonly HC.Patient.Service.IServices.User.IUserService _usrService;
        private readonly double BlockedHour = 2;
        private readonly int lockNotificationDays = (int)AccountConfiguration.LockNotification;
        private string DomainName = HCOrganizationConnectionStringEnum.Host; //its Merging db
        private readonly IPatientRepository _patientRepository;
        public LoginService(ITokenService tokenService, IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IAuditLogService auditLogService, IUserService userService, ISecurityQuestionService securityQuestionService, IRolePermissionService rolePermissionService, HC.Patient.Service.IServices.User.IUserService usrService, IStaffService staffService, IPatientRepository patientRepository)
        {
            _tokenService = tokenService;
            _logger = loggerFactory.CreateLogger<LoginService>();
            _auditLogService = auditLogService;
            _userService = userService;

            _securityQuestionService = securityQuestionService;
            _usrService = usrService;
            _rolePermissionService = rolePermissionService;
            _staffService = staffService;
            _patientRepository = patientRepository;
        }
        #region mobile specific

        public JsonModel MobileLogin(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token)
        {

            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName(token);
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);

            if (dbUser != null)
            {
                var staff = _staffService.GetStaffByUserId(dbUser.Id, token);
                if (staff == null)
                    return new JsonModel(null, StatusMessage.StaffInfoNotFound, (int)HttpStatusCodes.NotFound);
                if (staff.IsActive == false)
                    return new JsonModel(null, StatusMessage.AccountDeactivated, (int)HttpStatusCodes.Unauthorized);

                if (dbUser.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                {
                    dbUser.DeviceToken = applicationUser.DeviceToken;
                    bool IsSuccess = _tokenService.SaveUpdateDeviceToken(dbUser);
                    if (IsSuccess == true)
                    {
                        return LoginWithSecurityQuestion(applicationUser, token, dbUser, _jwtOptions, OrgID);
                    }
                    return Result;

                }
                else
                {
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)  
                    Result.data = new object();
                    Result.Message = StatusMessage.InvalidUserOrPassword;
                    Result.StatusCode = (int)HttpStatusCodes.Unauthorized;
                    return Result;
                }
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                Result.data = new object();
                Result.Message = StatusMessage.InvalidUserOrPassword;
                Result.StatusCode = (int)HttpStatusCodes.Unauthorized;
                return Result;
            }
        }
        public JsonModel MobileLogout(TokenModel token)
        {
            JsonModel response = new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            bool IsSuccess = false;
            if (token.UserID != 0)
            {
                var dbUser = _tokenService.GetUserById(token);
                dbUser.DeviceToken = string.Empty;
                IsSuccess = _tokenService.SaveUpdateDeviceToken(dbUser);
                if (IsSuccess)
                {
                    response = new JsonModel
                    {
                        data = new object(),
                        Message = UserAccountNotification.LoggedOut,
                        StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
                    };
                }
                else
                {
                    response = new JsonModel
                    {
                        data = new object(),
                        Message = StatusMessage.UserNotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound//(Invalid credentials)
                    };
                }

            }
            else
            {
                response = new JsonModel
                {
                    data = new object(),
                    Message = StatusMessage.BadRequest,
                    StatusCode = (int)HttpStatusCodes.BadRequest,
                };
            }
            return response;
        }

        #endregion
        public JsonModel Login(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token)
        {

            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName(token);
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);

            if (dbUser != null)
            {
                var staff = _staffService.GetStaffByUserId(dbUser.Id, token);
                if (staff == null)
                    return new JsonModel(null, StatusMessage.StaffInfoNotFound, (int)HttpStatusCodes.NotFound);
                if (staff.IsActive == false)
                    return new JsonModel(null, StatusMessage.AccountDeactivated, (int)HttpStatusCodes.Unauthorized);

                if (dbUser.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                {
                    return LoginWithSecurityQuestion(applicationUser, token, dbUser, _jwtOptions, OrgID);
                }
                else
                {
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)  
                    Result.data = new object();
                    Result.Message = StatusMessage.InvalidUserOrPassword;
                    Result.StatusCode = (int)HttpStatusCodes.Unauthorized;
                    return Result;
                }
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                Result.data = new object();
                Result.Message = StatusMessage.InvalidUserOrPassword;
                Result.StatusCode = (int)HttpStatusCodes.Unauthorized;
                return Result;
            }
        }

        public JsonModel PatientLogin(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token)
        {
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName(token);
            var dbUser = _tokenService.GetUserByUserName(applicationUser.UserName, OrgID);
            if (dbUser != null)
            {
                if (dbUser.UserRoles.RoleName == OrganizationRoles.Patient.ToString())
                {
                    return LoginWithSecurityQuestion(applicationUser, token, dbUser, _jwtOptions, OrgID);
                }
                else
                {
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                    return new JsonModel
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidUserOrPassword,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    };
                }
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)                
                return new JsonModel
                {
                    data = new object(),
                    Message = StatusMessage.InvalidUserOrPassword,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };
            }
        }

        public JsonModel SaveUserSecurityQuestion(SecurityQuestionListModel securityQuestionListModel, JwtIssuerOptions _jwtOptions, TokenModel token)
        {
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName(token);
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
                        if (userRole[0] == OrganizationRoles.Patient.ToString())
                        {
                            //patient will be reset in "LoginPatient" function

                            // Patients patient = _tokenService.GetPaitentByUserID(dbUser.Id, token);
                            Patients patient = _patientRepository.Get(a => a.UserID == dbUser.Id && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false);

                            //patient.PhotoPath= patient.PhotoPath.Split('')
                            //patient.PhotoThumbnailPath=


                            //return patient with success
                            return LoginPatient(appUser, dbUser, token, identity, patient, _jwtOptions);
                        }
                        else
                        {
                            //reset the user
                            _userService.ResetUserAccess(dbUser.Id, token);
                            //return user with success
                            return LoginUser(appUser, dbUser, token, identity, _jwtOptions);
                        }
                    }
                    else
                    {
                        return new JsonModel
                        {
                            data = new object(),
                            Message = UserAccountNotification.LoginFailed,
                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                        };
                    }
                }
                else // if credentials are wrong
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({appUser.UserName}) or password ({appUser.Password})");

                    //response status
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return new JsonModel
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode//(Invalid credentials)
                    };
                    //return BadRequest("Invalid credentials");
                }
            }
            // if user not exist
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return new JsonModel
                {
                    data = new object(),
                    Message = StatusMessage.InvalidCredentials,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };
            }
        }

        public JsonModel CheckQuestionAnswer(SecurityQuestionModel securityQuestionModel, JwtIssuerOptions _jwtOptions, TokenModel token)
        {
            //check user exit in database or not
            int OrgID = GetOrganizationIDByBusinessName(token);
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
                        if (userRole[0] == OrganizationRoles.Patient.ToString())
                        {
                            //patient will be reset in "LoginPatient" function
                            Patients patient = _tokenService.GetPaitentByUserID(dbUser.Id, token);
                            //return patient with success
                            return LoginPatient(appUser, dbUser, token, identity, patient, _jwtOptions);
                        }
                        else
                        {
                            //reset the user
                            _userService.ResetUserAccess(dbUser.Id, token);
                            //return user with success
                            return LoginUser(appUser, dbUser, token, identity, _jwtOptions);
                        }
                    }
                    //if user blocked
                    else if (dbUser.IsBlock)
                    {
                        //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                        JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);
                        //Response.StatusCode = jsonModel.StatusCode;//(Invalid credentials)
                        return new JsonModel
                        {
                            data = new object(),
                            Message = jsonModel.Message,
                            StatusCode = jsonModel.StatusCode//(Invalid credentials)
                        };
                    }
                    //if answer is wrong
                    else
                    {
                        //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                        JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);
                        if (dbUser.IsBlock)
                        {
                            //Response.StatusCode = jsonModel.StatusCode; //wrong answer
                            return new JsonModel
                            {
                                data = new object(),
                                Message = jsonModel.Message,
                                StatusCode = jsonModel.StatusCode //wrong answer
                            };
                        }
                        else
                        {
                            //Response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity; //wrong answer
                            return new JsonModel
                            {
                                data = new object(),
                                Message = SecurityQuestionNotification.IncorrectAnswer,
                                StatusCode = (int)HttpStatusCodes.UnprocessedEntity //wrong answer
                            };
                        }


                    }
                }
                else // if credentials are wrong
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({appUser.UserName}) or password ({appUser.Password})");

                    //response status
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return new JsonModel
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode//(Invalid credentials)
                    };
                    //return BadRequest("Invalid credentials");
                }
            }
            // if user not exist
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return new JsonModel
                {
                    data = new object(),
                    Message = StatusMessage.InvalidCredentials,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };
            }
        }

        private JsonModel LoginWithSecurityQuestion(ApplicationUser applicationUser, TokenModel token, Entity.User dbUser, JwtIssuerOptions _jwtOptions, int OrgID)
        {
            if (!string.IsNullOrEmpty(applicationUser.UniqueDeviceId))
            {
                dbUser.IsSecurityQuestionMandatory = false;
            }
            if (dbUser != null) //if user exist in database
            {
                //get role of user
                string[] userRole = { dbUser.UserRoles.RoleName };

                // token model just IP
                token = GetIPFromRequst(token);

                //set organization id from login's user
                token.OrganizationID = dbUser.OrganizationID;

                //set user id from login's user
                token.UserID = dbUser.Id;

                //check credetials are valid or not and set identity
                var identity = GetClaimsIdentity(applicationUser, dbUser);

                //check if user credetials are wrong 
                if (identity == null) // if identity is null (wrong credentials)
                {
                    //logging
                    _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");

                    //response status
                    ////--------//Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)

                    //login logs
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);

                    //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                    JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                    //return
                    ////--------//Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = jsonModel.Message,
                        StatusCode = jsonModel.StatusCode,//(Invalid credentials)
                    };
                    //return BadRequest("Invalid credentials");
                }
                else /*if (identity != null)*/ //if credentials are vaild
                {
                    //******START******Code to Check Password Expiry**********************//
                    //TimeSpan days = dbUser.PasswordResetDate.Value.Date.AddDays(lockNotificationDays) - DateTime.UtcNow.Date;
                    //if (days.Days <= 0) //if password expired
                    //{
                    //    return new JsonModel(dbUser.Id, UserAccountNotification.PasswordExpired, (int)HttpStatusCode.ResetContent);
                    //}
                    //  //direct login for admin 
                    //else 
                    //******END******Code to Check Password Expiry**********************//
                    if (userRole[0] == OrganizationRoles.Admin.ToString())
                    {
                        //return user with success
                        return LoginUser(applicationUser, dbUser, token, identity, _jwtOptions);
                    }

                    // is question exist in db for uesr
                    List<UserSecurityQuestionAnswer> userSecurityQuestionAnswer = _securityQuestionService.GetUserSecurityQuestionsById(dbUser.Id, token);
                    if (dbUser.IsSecurityQuestionMandatory == false ? false : true)
                    {
                        #region Check user already login on same machine
                        if (string.IsNullOrEmpty(applicationUser.IpAddress) && string.IsNullOrEmpty(applicationUser.MacAddress))
                        {
                            CheckUserAlreadyloginFromSameMobile(applicationUser, dbUser);
                        }
                        else
                        {
                            CheckUserAlreadyloginFromSameMachine(applicationUser, dbUser);
                        }
                        #endregion
                    }

                    //#region Check user already login on same machine
                    //CheckUserAlreadyloginFromSameMachine(applicationUser, dbUser);
                    //#endregion

                    Patients patient = _tokenService.GetPaitentByUserID(dbUser.Id, token);

                    // if user is patient and not null and his portal or isactive = false
                    if (userRole[0] == OrganizationRoles.Patient.ToString() && patient != null && (patient.IsPortalActivate == false || patient.IsActive == false))
                    {
                        //check patient is Active
                        if (!patient.IsActive)
                        {
                            //Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ClientActiveStatus,
                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                            };
                        }
                        //check patient portal is enabled
                        else if (!patient.IsPortalActivate)
                        {
                            //_logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                            //Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ClientPortalDeactivedAtLogin,
                                StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                            };
                        }
                        else
                        {
                            //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = UserAccountNotification.LoginFailed,
                                StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                            };
                        }
                    }
                    //if user blocked 
                    else if (dbUser.IsBlock == true)
                    {
                        //checking user block time
                        if (DateTime.UtcNow - dbUser.BlockDateTime >= TimeSpan.FromHours(BlockedHour))//BlockedHour set 2 hour TO DO it should be global
                        {
                            //first login of user which didn't give answer of any question
                            if ((userSecurityQuestionAnswer == null && string.IsNullOrEmpty(applicationUser.UniqueDeviceId)) || userSecurityQuestionAnswer.Count == 0 && (dbUser.IsSecurityQuestionMandatory == false ? false : true))
                            {
                                //return
                                var response = new JsonModel()
                                {

                                    data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                    StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                    Message = SecurityQuestionNotification.RequiredAnswers,//"Please give the answers of the questions"
                                    firstTimeLogin = true
                                };
                                return response;
                            }
                            // if user is using another machine
                            else if (applicationUser.IsValid == false && (dbUser.IsSecurityQuestionMandatory == false ? false : true))
                            {
                                var response = new JsonModel()
                                {
                                    data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                    StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                    Message = SecurityQuestionNotification.AtleastOneAnswer, //"Please give the answer of this question"
                                    firstTimeLogin = false
                                };
                                return response;
                            }
                            //if user is vaild and answer are given
                            else
                            {
                                //patient login
                                if (userRole[0] == OrganizationRoles.Patient.ToString())
                                {
                                    //patient will be reset in "LoginPatient" function
                                    //return patient with success
                                    return LoginPatient(applicationUser, dbUser, token, identity, patient, _jwtOptions);
                                }
                                else
                                {
                                    //reset the user
                                    _userService.ResetUserAccess(dbUser.Id, token);
                                    //return user with success
                                    return LoginUser(applicationUser, dbUser, token, identity, _jwtOptions);
                                }
                            }
                        }
                        else
                        {
                            //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                            JsonModel jsonModel = _userService.UpdateAccessFailedCount(dbUser.Id, token);

                            ////return
                            //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = jsonModel.Message,
                                StatusCode = jsonModel.StatusCode//(Invalid credentials)
                            };
                        }
                    }
                    //if user not blocked
                    else if (dbUser.IsBlock == false)
                    {
                        //first login of user which didn't give answer of any question
                        if (userSecurityQuestionAnswer == null || userSecurityQuestionAnswer.Count == 0 && (dbUser.IsSecurityQuestionMandatory == false ? false : true))
                        {
                            var response = new JsonModel
                            {
                                data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                Message = SecurityQuestionNotification.RequiredAnswers, //"Please give the answers of the questions"
                                firstTimeLogin = true
                            };
                            return response;
                        }
                        // if user is using another machine
                        else if (applicationUser.IsValid == false && (dbUser.IsSecurityQuestionMandatory == false ? false : true))
                        {
                            var response = new JsonModel
                            {
                                data = _securityQuestionService.GetSecurityQuestion(token), //get all security question for user
                                StatusCode = (int)HttpStatusCodes.OK,//(OK)
                                Message = SecurityQuestionNotification.AtleastOneAnswer, //"Please give the answer of this question"
                                firstTimeLogin = false
                            };
                            return response;
                        }
                        //if user is vaild and answer are given
                        else
                        {
                            //patient login
                            if (userRole[0] == OrganizationRoles.Patient.ToString())
                            {
                                //patient will be reset in "LoginPatient" function
                                //return patient with success
                                return LoginPatient(applicationUser, dbUser, token, identity, patient, _jwtOptions);
                            }
                            else
                            {
                                //reset the user
                                _userService.ResetUserAccess(dbUser.Id, token);
                                //return user with success
                                return LoginUser(applicationUser, dbUser, token, identity, _jwtOptions);
                            }
                        }
                    }
                    else
                    {
                        //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                        return new JsonModel
                        {
                            data = new object(),
                            Message = UserAccountNotification.LoginFailed,
                            StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                        };
                    }
                }
                //else
                //{
                //    //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                //    return new JsonModel
                //    {
                //        data = new object(),
                //        Message = UserAccountNotification.LoginFailed,
                //        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                //    };
                //}
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                return new JsonModel
                {
                    data = new object(),
                    Message = UserAccountNotification.UserNamePasswordNotVaild,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };

            }
        }

        /// <summary>
        /// for staff
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        /// <param name="token"></param>
        /// <param name="identity"></param>
        /// <param name="_jwtOptions"></param>
        /// <returns></returns>
        private JsonModel LoginUser(ApplicationUser applicationUser, Entity.User dbUser, TokenModel token, ClaimsIdentity identity, JwtIssuerOptions _jwtOptions)
        {

            int StaffID = 0;//default staff id initalize with 0
            Staffs staff = null;

            //get default location 
            if (dbUser.Id > 0)
            {
                //set default location id of login's user 
                token.LocationID = _tokenService.GetDefaultLocationOfStaff(dbUser.Id);
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

            StringValues Host = string.Empty; token.Request.Request.Headers.TryGetValue("BusinessToken", out Host);
            if (!string.IsNullOrEmpty(Host))
            {
                DomainName = CommonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : applicationUser.BusinessToken);
            }
            #endregion



            //login logs
            if (dbUser.Id > 0)
            {//login
                _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);

                // set this user online
                _usrService.SetOnline(dbUser.Id);

                staff = _tokenService.GetDoctorByUserID(dbUser.Id, token);
            }

            //staff id
            if (staff != null)
            {
                StaffID = staff.Id;
            }
            //create claim for login user            
            var claims = new[]
            {
                    new System.Security.Claims.Claim("UserID", dbUser.Id.ToString()),
                    new System.Security.Claims.Claim("RoleID", dbUser.RoleID.ToString()),
                    new System.Security.Claims.Claim("UserName", dbUser.UserName.ToString()),
                    new System.Security.Claims.Claim("OrganizationID", dbUser.OrganizationID.ToString()),
                    new System.Security.Claims.Claim("StaffID", StaffID.ToString()),
                    new System.Security.Claims.Claim("LocationID", token.LocationID.ToString()),
                    new System.Security.Claims.Claim("DomainName",DomainName),                     // Domain name always add in token
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
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
            jwt.Payload["roles"] = userRole;

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //get role permission
            JsonModel UserPermission = _rolePermissionService.GetUserPermissions(token, dbUser.RoleID);

            //get app configuration for user
            List<AppConfigurationsModel> AppConfigurations = _tokenService.GetAppConfigurationByOrganizationByID(token);

            //get users all location
            List<UserLocationsModel> userLocation = _tokenService.GetUserLocations(dbUser.Id);

            // return the staff or patient response
            if (userRole[0] == OrganizationRoles.Patient.ToString())
            {
                var response = new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    data = _tokenService.GetPaitentByUserID(dbUser.Id, token),


                };
                return response;
            }
            else
            {
                var response = new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    //data = _tokenService.GetDoctorByUserID(dbUser.Id, token),
                    data = staff,
                    UserPermission = UserPermission.data,
                    AppConfigurations = AppConfigurations,
                    UserLocations = userLocation,
                    PasswordExpiryStatus = CheckPasswordExpiredMessage(dbUser, token),
                    Notifications = _tokenService.GetLoginNotification(token),
                    PatientData = _tokenService.GetLastPatientByOrganization(token),
                    OpenDefaultClient = _tokenService.GetDefaultClient(dbUser.Id),
                    StatusCode = (int)HttpStatusCodes.OK,
                    IsPasscodeEnable = Convert.ToBoolean(dbUser.IsPasscodeEnable)
                };
                return response;
            }
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

        /// <summary>
        /// this will save machine data IP Address and MAC address in database
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        private bool SaveMachineDataIPAndMac(ApplicationUser applicationUser, Entity.User dbUser)
        {
            bool machineData;
            MachineLoginLog machineLoginLog = new MachineLoginLog();
            machineLoginLog.IpAddress = applicationUser.IpAddress;
            machineLoginLog.MacAddress = applicationUser.MacAddress;
            machineLoginLog.OrganizationID = dbUser.OrganizationID;
            machineLoginLog.UniqueDeviceId = applicationUser.UniqueDeviceId;
            machineLoginLog.UserId = dbUser.Id;
            machineLoginLog.LoginDate = DateTime.UtcNow;
            machineData = _userService.SaveMachineLoginUser(machineLoginLog);
            return machineData;
        }

        /// <summary>
        /// this method is use for patient login
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        /// <param name="token"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private JsonModel LoginPatient(ApplicationUser applicationUser, Entity.User dbUser, TokenModel token, ClaimsIdentity identity, Patients patient, JwtIssuerOptions _jwtOptions)
        {
            //patient inital
            int defaultLocation = 0;//default loation id initalize with 0
            int PatientID = 0;

            //get patient id and his location
            PatientID = patient.Id; defaultLocation = patient.LocationID;

            //check patient is Active
            if (!patient.IsActive)
            {
                //Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ClientActiveStatus,
                    StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                };
            }

            //check patient portal is enabled
            if (!patient.IsPortalActivate)
            {
                //_logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                //Response.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ClientPortalDeactivedAtLogin,
                    StatusCode = (int)HttpStatusCodes.NotAcceptable//(Invalid credentials)
                };
            }


            //save IP and MAC address
            #region save IP and MAC Address
            bool machineData = false;
            machineData = SaveMachineDataIPAndMac(applicationUser, dbUser);
            #endregion
            //get login user role name
            string[] userRole = { dbUser.UserRoles.RoleName };
            //string[] userRole = { dbUser.UserRoles.RoleName };

            StringValues Host = string.Empty; token.Request.Request.Headers.TryGetValue("BusinessToken", out Host);
            if (!string.IsNullOrEmpty(Host))
            {
                DomainName = CommonMethods.Decrypt(Host.ToString());
                //DomainName = CommonMethods.Decrypt(!string.IsNullOrEmpty(Host) ? Host.ToString() : applicationUser.BusinessToken);
            }

            //create claim for login user
            var claims = new[]
            {
        new System.Security.Claims.Claim("UserID", dbUser.Id.ToString()),
        new System.Security.Claims.Claim("RoleID", dbUser.RoleID.ToString()),
        new System.Security.Claims.Claim("UserName", dbUser.UserName.ToString()),
        new System.Security.Claims.Claim("OrganizationID", dbUser.OrganizationID.ToString()),
        new System.Security.Claims.Claim("PatientID", PatientID.ToString()),
        new System.Security.Claims.Claim("LocationID", defaultLocation.ToString()),
        new System.Security.Claims.Claim("DomainName",DomainName),                     // Domain name always add in token
        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
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
            PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB, patient.Email, null, null, null, null, null, null, null, null, null).FirstOrDefault();
            PatientModel patientModel = new PatientModel
            {
                FirstName = pHIDecryptedModel.FirstName,
                LastName = pHIDecryptedModel.LastName,
                PatientId = patient.Id,
                IsActive = patient.IsActive,
                IsBlock = patient.IsBlock,
                PhotoThumbnailPath = patient.PhotoThumbnailPath

            };
            // Serialize and return the response
            var response = new JsonModel
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                data = patient,
                PatientData = patientModel,
                PasswordExpiryStatus = CheckPasswordExpiredMessage(dbUser, token),
                Notifications = _tokenService.GetLoginNotification(token),
                StatusCode = (int)HttpStatusCode.OK,
                IsPasscodeEnable = Convert.ToBoolean(dbUser.IsPasscodeEnable),
                PatientInformation = GetDetailedPatientInfo(patient.Id, token)
            };
            return response;
        }

        /// <summary>
        /// this will check user already login from same machine
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        private void CheckUserAlreadyloginFromSameMachine(ApplicationUser applicationUser, Entity.User dbUser)
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
        /// this will check user already login from same device.
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dbUser"></param>
        private void CheckUserAlreadyloginFromSameMobile(ApplicationUser applicationUser, Entity.User dbUser)
        {

            MachineLoginLog machineLoginLog = new MachineLoginLog();
            machineLoginLog.UserId = dbUser.Id;
            machineLoginLog.OrganizationID = dbUser.OrganizationID;
            machineLoginLog.UniqueDeviceId = applicationUser.UniqueDeviceId;
            bool checkuser = _userService.UserAlreadyLoginFromSameMachine(machineLoginLog);
            applicationUser.IsValid = checkuser;
        }
        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static ClaimsIdentity GetClaimsIdentity(ApplicationUser user, Entity.User dbUser)
        {
            if (dbUser != null && (user.UserName.ToUpper() == dbUser.UserName.ToUpper() && user.Password == CommonMethods.Decrypt(dbUser.Password)))
            {
                return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new[]
                  {
                   new System.Security.Claims.Claim("HealthCare", "IAmAuthorized")
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
        /// this will get the IP address from request
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private TokenModel GetIPFromRequst(TokenModel token)
        {
            StringValues ipAddress;
            token.Request.Request.Headers.TryGetValue("IPAddress", out ipAddress);
            if (!string.IsNullOrEmpty(ipAddress)) { token.IPAddress = ipAddress; } else { token.IPAddress = "203.129.220.76"; }
            return token;
        }

        /// <summary>
        /// this will get the organizationID from host name 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private int GetOrganizationIDByBusinessName(TokenModel token)
        {
            int OrgID = 0;
            StringValues businessName;
            token.Request.Request.Headers.TryGetValue("BusinessToken", out businessName); //get host name from header            
            OrgID = _tokenService.GetOrganizationIDByName(CommonMethods.Decrypt(businessName));
            return OrgID;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        /// <summary>
        /// checking password of the user and accordingly message in respond
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public PasswordCheckModel CheckPasswordExpiredMessage(Entity.User user, TokenModel token)
        {
            PasswordCheckModel passwordCheckModel = null;
            if (user != null && user.PasswordResetDate != null)
            {
                passwordCheckModel = new PasswordCheckModel();
                passwordCheckModel = new PasswordCheckModel(UserAccountNotification.Success, false, "", (int)HttpStatusCodes.OK, 60);
            }
            return passwordCheckModel;
            //******START******Code to Check Password Expiry**********************//
            //if (user != null && user.PasswordResetDate != null)
            //{
            //    passwordCheckModel = new PasswordCheckModel();
            //    TimeSpan days = user.PasswordResetDate.Value.Date.AddDays(lockNotificationDays) - DateTime.UtcNow.Date;

            //    if (days.Days <= lockNotificationDays && days.Days > lockNotificationDays - 2)
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Sucess, (int)HttpStatusCodes.OK, days.Days);
            //    }
            //    //if <= 15 or > 10 days left then just show warning to user
            //    else if (days.Days <= Math.Abs((lockNotificationDays / 4)) && days.Days > Math.Abs((lockNotificationDays / 6)))
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK, days.Days);
            //    }
            //    //if <= 10 or > 5 days left then just show warning to user
            //    else if (days.Days <= Math.Abs((lockNotificationDays / 6)) && days.Days > Math.Abs((lockNotificationDays / 12))) //warning
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK, days.Days);
            //    }
            //    else if (days.Days <= Math.Abs((lockNotificationDays / 12)) && days.Days > Math.Abs((1))) //warning
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger, (int)HttpStatusCodes.Unauthorized, days.Days);
            //    }
            //    //if <= 0 then block the user and show warning to user with red color
            //    else if (days.Days <= 0)//block 
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger, days.Days);
            //    }
            //    else
            //    {
            //        passwordCheckModel = new PasswordCheckModel(UserAccountNotification.Success, false, "", (int)HttpStatusCodes.OK, days.Days);
            //    }
            //}
            //return passwordCheckModel;
            //******END******Code to Check Password Expiry**********************//
        }

        public PatientInfo GetDetailedPatientInfo(int PatientID, TokenModel token)
        {
            PatientInfo patientinfo = null;
           
            patientinfo= _patientRepository.GetPatientsDetailedInfo(PatientID, token);
            return patientinfo;

        }
    }
}
