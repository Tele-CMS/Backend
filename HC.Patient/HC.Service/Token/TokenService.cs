using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Service.IServices.SecurityQuestion;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Token
{
    public class TokenService : BaseService, ITokenService
    {
        #region Global Variables

        private readonly ITokenRepository _tokenRepository;
        private HCOrganizationContext _organizationContext;

        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IAuditLogService _auditLogService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityQuestionsRepository _securityQuestionRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly double BlockedHour = 2;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILocationService _locationService;
        #endregion

        public TokenService(ITokenRepository tokenRepository, HCOrganizationContext organizationContext,
            IOptions<JwtIssuerOptions> jwtOptions, IRolePermissionService rolePermissionService,
            IUserCommonRepository userCommonRepository, ISecurityQuestionService securityQuestionService,
            IUserRepository userRepository, ISecurityQuestionsRepository securityQuestionRepository
            , IAuditLogService auditLogService, IOrganizationRepository organizationRepository, INotificationRepository notificationRepository, ILocationService locationService)
        {
            this._tokenRepository = tokenRepository;
            _organizationContext = organizationContext;
            _jwtOptions = jwtOptions.Value;
            _auditLogService = auditLogService;
            _rolePermissionService = rolePermissionService;
            _userCommonRepository = userCommonRepository;
            _securityQuestionService = securityQuestionService;
            _userRepository = userRepository;
            _securityQuestionRepository = securityQuestionRepository;
            _organizationRepository = organizationRepository;
            _notificationRepository = notificationRepository;
            _locationService = locationService;

        }
        #region mobile specific
        public bool SaveUpdateDeviceToken(User user)
        {
            bool IsSuccess = false;
            if (user != null)
            {
                _organizationContext.User.Update(user);
                _organizationContext.SaveChanges();
                IsSuccess = true;
            }
            else
            {
                IsSuccess = false;
            }
            return IsSuccess;
        }
        #endregion
        public Staffs GetDoctorByUserID(int UserID, TokenModel token)
        {
            return _tokenRepository.GetDoctorByUserID(UserID, token);
        }

        public User GetUserByUserName(string userName, int organizationID)
        {
            return _tokenRepository.GetUserByUserName(userName, organizationID);
        }
       
        public User GetUserById(TokenModel  tokenModel)
        {
            return _tokenRepository.GetUserById(tokenModel);
        }

        public SuperUser GetSupadminUserByUserName(string userName)
        {
            return _tokenRepository.GetSupadminUserByUserName(userName);
        }

        public int GetOrganizationIDByName(string businessName)
        {
            return _tokenRepository.GetOrganizationIDByName(businessName);
        }

        public int GetDefaultLocationOfStaff(int UserID)
        {
            return _tokenRepository.GetDefaultLocationOfStaff(UserID);
        }

        public List<UserLocationsModel> GetUserLocations(int UserID)
        {
            return _tokenRepository.GetUserLocations(UserID);
        }

        public Patients GetPaitentByUserID(int UserID, TokenModel tokenModel)
        {
            return _tokenRepository.GetPaitentByUserID(UserID, tokenModel);
        }
        public List<AppConfigurationsModel> GetAppConfigurationByOrganizationByID(TokenModel tokenModel)
        {
            try
            {
                List<AppConfigurationsModel> appConfigs = new List<AppConfigurationsModel>();
                List<AppConfigurations> dbAppConfigs = _organizationContext.AppConfigurations.Where(m => m.OrganizationID == tokenModel.OrganizationID).ToList();
                AutoMapper.Mapper.Map(dbAppConfigs, appConfigs);
                return appConfigs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DomainToken GetDomain(DomainToken domainToken)
        {
            return _tokenRepository.GetDomain(domainToken);
        }

        public JsonModel AuthenticateSuperUser(ApplicationUser applicationUser, TokenModel token)
        {
            JsonModel responseModel = new JsonModel
            {
                data = new object(),
                StatusCode = (int)HttpStatusCodes.Unauthorized,
                access_token = null
            };

            var UserInfo = _tokenRepository.GetSupadminUserByUserName(applicationUser.UserName);

            if (UserInfo != null)
            {
                var Authorized = CheckUserInfo(applicationUser, UserInfo.UserName, UserInfo.Password);

                if (Authorized == null)
                {
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, UserInfo.Id, token, LoginLogLoginAttempt.Failed);
                }
                else
                {
                    token.UserID = UserInfo.Id;
                    token.RoleID = 0;
                    token.UserName = UserInfo.UserName;
                    token.OrganizationID = 0;
                    token.StaffID = 0;
                    token.LocationID = 0;

                    //var claims = new[]
                    //{
                    //new Claim("UserID", UserInfo.Id.ToString()),
                    //new Claim("RoleID", 0.ToString()),                      // not required please don't chamge
                    //new Claim("UserName", UserInfo.UserName.ToString()),
                    //new Claim("OrganizationID", 0.ToString()),              // not required please don't chamge
                    //new Claim("StaffID", 0.ToString()),                     // not required please don't chamge
                    //new Claim("LocationID", 0.ToString()),                  // not required please don't chamge
                    //new Claim("DomainName",token.DomainName),                     // Domain name always add in token
                    //new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                    //new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    //new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    //Authorized.FindFirst("HealthCare")
                    //};

                    //var jwt = new JwtSecurityToken(
                    //    issuer: _jwtOptions.Issuer,
                    //    audience: _jwtOptions.Audience,
                    //    claims: claims,
                    //    notBefore: _jwtOptions.NotBefore,
                    //    expires: _jwtOptions.Expiration,
                    //    signingCredentials: _jwtOptions.SigningCredentials);

                    var encodedJwt = GenerateToken(Authorized, token); //new JwtSecurityTokenHandler().WriteToken(jwt);
                    UserInfo.Password = null;
                    responseModel.data = UserInfo;
                    responseModel.StatusCode = (int)HttpStatusCodes.OK;
                    responseModel.access_token = encodedJwt;
                    responseModel.expires_in = (int)_jwtOptions.ValidFor.TotalSeconds;
                    _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, UserInfo.Id, token, LoginLogLoginAttempt.Success);
                }

            }
            return responseModel;
        }

        //public JsonModel AuthenticateAgency(ApplicationUser applicationUser, TokenModel token)
        //{
        //    JsonModel jsonModel = new JsonModel()
        //    {
        //        data = new object(),
        //        Message = StatusMessage.InvalidUserOrPassword,
        //        StatusCode = (int)HttpStatusCodes.Unauthorized
        //    };
        //    var UserInfo = _tokenRepository.GetUserByUserName(applicationUser.UserName);
        //    if (UserInfo != null)
        //    {
        //        if (UserInfo.UserRoles.RoleName != OrganizationRoles.Client.ToString())
        //        {
        //            jsonModel = LoginWithSecurityQuestion(applicationUser, UserInfo, token);
        //        }
        //    }
        //    return jsonModel;
        //}

        //public JsonModel SaveUserScurityQuestion(SecurityQuestionListModel questionListModel, TokenModel token)
        //{
        //    JsonModel response = new JsonModel()
        //    {
        //        data = new object(),
        //        Message = StatusMessage.InvalidCredentials,
        //        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
        //    };            
        //    var dbUser = _tokenRepository.GetUserByUserName(questionListModel.UserName);
        //    if (dbUser != null)
        //    {
        //        //set organization id 
        //        token.OrganizationID = dbUser.OrganizationID;
        //        //set userID id 
        //        token.UserID = dbUser.Id;
        //        //application user get username & password from request
        //        ApplicationUser appUser = new ApplicationUser()
        //        {
        //            Password = questionListModel.Password,
        //            UserName = questionListModel.UserName,
        //            IpAddress = questionListModel.IpAddress,
        //            MacAddress = questionListModel.MacAddress
        //        };
        //        //check credetials are valid or not and set identity
        //        var identity = CheckUserInfo(appUser, dbUser.UserName, dbUser.Password);

        //        if (identity != null)
        //        {
        //            //save user's security question
        //            bool status = _securityQuestionService.SaveUserSecurityQuestion(questionListModel, token);
        //            if (status)
        //            {
        //                //get role of user
        //                string[] userRole = { dbUser.UserRoles.RoleName };

        //                //patient login
        //                if (userRole[0] == OrganizationRoles.Client.ToString())
        //                {
        //                    //patient will be reset in "LoginPatient" function
        //                    //return patient with success
        //                    Patients patient = _tokenRepository.GetPaitentByUserID(dbUser.Id);
        //                    return LoginPatient(appUser, dbUser, token, identity, patient);
        //                }
        //                else
        //                {
        //                    //reset the user
        //                    _tokenRepository.ResetUserAccess(token.UserID);
        //                    //return user with success
        //                    return LoginUser(appUser, dbUser, token, identity);
        //                }
        //            }
        //            else
        //            {
        //                response.Message = UserAccountNotification.LoginFailed;
        //            }
        //        }
        //        else
        //        {
        //            _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);
        //            //increase failed login count and block if user attempt 3 or more time with wrong credentials 
        //            response = UpdateAccessFailedCount(token);
        //        }

        //    }

        //    return response;
        //}

        //public JsonModel CheckQuestionAnswer(SecurityQuestionModel securityQuestion, TokenModel token)
        //{
        //    JsonModel response = new JsonModel()
        //    {
        //        data = new object(),
        //        Message = StatusMessage.InvalidCredentials,
        //        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
        //    };
        //    var dbUser = _tokenRepository.GetUserByUserName(securityQuestion.UserName);
        //    if (dbUser != null)
        //    {
        //        //set organization id 
        //        token.OrganizationID = dbUser.OrganizationID;

        //        //set userID id 
        //        token.UserID = dbUser.Id;

        //        //application user get username & password from request
        //        ApplicationUser appUser = new ApplicationUser()
        //        {
        //            Password = securityQuestion.Password,
        //            UserName = securityQuestion.UserName,
        //            IpAddress = securityQuestion.IpAddress,
        //            MacAddress = securityQuestion.MacAddress
        //        };
        //        //check credetials are valid or not and set identity
        //        var identity = CheckUserInfo(appUser, dbUser.UserName, dbUser.Password);
        //        if (identity != null)
        //        {
        //            bool status = _securityQuestionService.CheckUserQuestionAnswer(
        //                securityQuestion.QuestionID, token.UserID, token.OrganizationID, securityQuestion.Answer
        //                );
        //            if (status && dbUser.IsBlock == false)
        //            {
        //                //get role of user
        //                string[] userRole = { dbUser.UserRoles.RoleName };

        //                //patient login
        //                if (userRole[0] == OrganizationRoles.Client.ToString())
        //                {
        //                    //patient will be reset in "LoginPatient" function
        //                    //return patient with success
        //                    Patients patient = _tokenRepository.GetPaitentByUserID(dbUser.Id);
        //                    return LoginPatient(appUser, dbUser, token, identity, patient);
        //                }
        //                else
        //                {
        //                    //reset the user
        //                    _tokenRepository.ResetUserAccess(token.UserID);
        //                    //return user with success
        //                    return LoginUser(appUser, dbUser, token, identity);
        //                }
        //            }
        //            //if user blocked
        //            else if (dbUser.IsBlock)
        //            {
        //                //increase failed login count and block if user attempt 3 or more time with wrong credentials 
        //                response = UpdateAccessFailedCount(token);

        //            }
        //            //if answer is wrong
        //            else
        //            {
        //                //increase failed login count and block if user attempt 3 or more time with wrong credentials 
        //                response = UpdateAccessFailedCount(token);
        //                if (!dbUser.IsBlock)
        //                {
        //                    response.data = new object();
        //                    response.Message = SecurityQuestionNotification.IncorrectAnswer;
        //                    response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;
        //                }


        //            }
        //        }
        //        else
        //        {
        //            _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Failed);
        //            //increase failed login count and block if user attempt 3 or more time with wrong credentials 
        //            response = UpdateAccessFailedCount(token);
        //        }
        //    }
        //    return response;
        //}

        private JsonModel LoginWithSecurityQuestion(ApplicationUser applicationUser, User user, TokenModel token)
        {
            JsonModel responseModel = new JsonModel();

            string[] userRole = { user.UserRoles.RoleName };
            token.OrganizationID = user.OrganizationID;
            var identity = CheckUserInfo(applicationUser, user.UserName, user.Password);

            if (identity == null)
            {
                token.UserID = user.Id;
                _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, token.UserID, token, LoginLogLoginAttempt.Failed);

                responseModel = UpdateAccessFailedCount(token);
            }
            else
            {
                //direct login for admin
                if (userRole[0] == OrganizationRoles.Admin.ToString())
                {
                    //  return user with success
                    responseModel = LoginUser(applicationUser, user, token, identity);
                }
                else
                {
                    // is question exist in db for user
                    List<UserSecurityQuestionAnswer> userSecurityQuestionAnswer = _securityQuestionService.GetUserSecurityQuestionsById(user.Id, token.OrganizationID);
                    token.UserID = user.Id;
                    token.IPAddress = applicationUser.IpAddress;
                    token.MacAddress = applicationUser.MacAddress;
                    token.OrganizationID = user.OrganizationID;
                    applicationUser.IsValid = IsMachineLoggedIn(token);

                    Patients patient = _tokenRepository.GetPaitentByUserID(user.Id, token);

                    if (userRole[0] == OrganizationRoles.Patient.ToString() && patient != null && (patient.IsPortalActivate == false || patient.IsActive == false))
                    {
                        //check patient is Active
                        if (!patient.IsActive)
                        {
                            responseModel.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            responseModel.data = new object();
                            responseModel.Message = StatusMessage.ClientActiveStatus;
                        }
                        //check patient portal is enabled
                        else if (!patient.IsPortalActivate)
                        {
                            responseModel.StatusCode = (int)HttpStatusCodes.NotAcceptable;//(Invalid credentials)
                            responseModel.data = new object();
                            responseModel.Message = StatusMessage.ClientPortalDeactivedAtLogin;
                        }
                        else
                        {
                            responseModel.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                            responseModel.data = new object();
                            responseModel.Message = UserAccountNotification.LoginFailed;
                        }
                    }
                    //if user blocked 
                    else if (user.IsBlock)
                    {
                        //checking user block time
                        if (DateTime.UtcNow - user.BlockDateTime >= TimeSpan.FromHours(BlockedHour))//BlockedHour set 2 hour TO DO it should be global
                        {
                            //first login of user which didn't give answer of any question
                            if (userSecurityQuestionAnswer == null || userSecurityQuestionAnswer.Count == 0)
                            {
                                responseModel.data = _securityQuestionRepository.GetSecurityQuestionByOrganization(token.OrganizationID);
                                responseModel.StatusCode = (int)HttpStatusCodes.OK;
                                responseModel.Message = SecurityQuestionNotification.RequiredAnswers;
                                responseModel.firstTimeLogin = true;

                            }
                            // if user is using another machine
                            else if (applicationUser.IsValid == false)
                            {

                                responseModel.data = _securityQuestionRepository.GetSecurityQuestionByOrganization(token.OrganizationID);
                                responseModel.StatusCode = (int)HttpStatusCodes.OK;
                                responseModel.Message = SecurityQuestionNotification.AtleastOneAnswer;
                                responseModel.firstTimeLogin = false;
                            }
                            //if user is vaild and answer are given
                            else
                            {
                                //patient login
                                if (userRole[0] == OrganizationRoles.Patient.ToString())
                                {
                                    //patient will be reset in "LoginPatient" function
                                    //return patient with success
                                    return LoginPatient(applicationUser, user, token, identity, patient);
                                }
                                else
                                {
                                    //reset the user
                                    _tokenRepository.ResetUserAccess(token.UserID);
                                    //return user with success
                                    return LoginUser(applicationUser, user, token, identity);
                                }
                            }
                        }
                        else
                        {
                            //increase failed login count and block if user attempt 3 or more time with wrong credentials 
                            responseModel = UpdateAccessFailedCount(token);
                        }
                    }
                    //if user not blocked
                    else if (!user.IsBlock)
                    {
                        //first login of user which didn't give answer of any question
                        if (userSecurityQuestionAnswer == null || userSecurityQuestionAnswer.Count == 0)
                        {
                            responseModel.StatusCode = (int)HttpStatusCodes.OK;//(OK)
                            responseModel.firstTimeLogin = true;
                            responseModel.data = _securityQuestionRepository.GetSecurityQuestionByOrganization(token.OrganizationID); //get all security question for user
                            responseModel.Message = SecurityQuestionNotification.RequiredAnswers; //"Please give the answers of the questions"
                        }
                        // if user is using another machine
                        else if (applicationUser.IsValid == false)
                        {

                            responseModel.StatusCode = (int)HttpStatusCodes.OK;//(OK)
                            responseModel.firstTimeLogin = false;
                            responseModel.data = _securityQuestionRepository.GetSecurityQuestionByOrganization(token.OrganizationID); //get all security question for user
                            responseModel.Message = SecurityQuestionNotification.AtleastOneAnswer; //"Please give the answers of the questions"
                        }
                        //if user is vaild and answer are given
                        else
                        {
                            //patient login
                            if (userRole[0] == OrganizationRoles.Patient.ToString())
                            {
                                //patient will be reset in "LoginPatient" function
                                //return patient with success
                                return LoginPatient(applicationUser, user, token, identity, patient);
                            }
                            else
                            {
                                //reset the user
                                _tokenRepository.ResetUserAccess(token.UserID);
                                //return user with success
                                return LoginUser(applicationUser, user, token, identity);
                            }
                        }
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                        responseModel.data = new object();
                        responseModel.Message = UserAccountNotification.LoginFailed;
                    }

                }
            }

            return responseModel;
        }

        private ClaimsIdentity CheckUserInfo(ApplicationUser user, string UserName, string password)
        {
            if (user.UserName.ToUpper() == UserName.ToUpper() && user.Password == CommonMethods.Decrypt(password))
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
        }

        private JsonModel LoginUser(ApplicationUser applicationUser, User dbUser, TokenModel token, ClaimsIdentity identity)
        {
            JsonModel jsonModel = new JsonModel();

            var staff = _tokenRepository.GetStaffByuserID(dbUser.Id);
            token.LocationID = staff.StaffLocation.Where(x => x.IsDefault == true).Select(x => x.LocationID).FirstOrDefault();

            //get login user role name
            string[] userRole = { dbUser.UserRoles.RoleName };

            //save IP and MAC address
            #region save IP and MAC Address
            bool machineData = false;
            token.MacAddress = applicationUser.MacAddress;
            token.UserID = dbUser.Id;
            token.RoleID = dbUser.RoleID;
            token.UserName = dbUser.UserName;
            token.OrganizationID = dbUser.OrganizationID;
            token.StaffID = staff.Id;
            token.IPAddress = applicationUser.IpAddress;
            if (!IsMachineLoggedIn(token))
            {
                machineData = SaveMachineDataIPAndMac(token);
            }
            #endregion
            ////create claim for login user
            //var claims = new[]
            //{
            //        new Claim("UserID", dbUser.Id.ToString()),
            //        new Claim("RoleID", dbUser.RoleID.ToString()),
            //        new Claim("UserName", dbUser.UserName.ToString()),
            //        new Claim("OrganizationID", dbUser.OrganizationID.ToString()),
            //        new Claim("StaffID", staff.Id.ToString()),
            //        new Claim("LocationID", token.LocationID.ToString()),
            //        new Claim("DomainName",token.DomainName),                     // Domain name always add in token
            //        new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
            //        new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            //        identity.FindFirst("HealthCare")
            //};

            ////Not required to reset user its already done from where this method call

            //// Create the JWT security token and encode it.
            //var jwt = new JwtSecurityToken(
            //    issuer: _jwtOptions.Issuer,
            //    audience: _jwtOptions.Audience,
            //    claims: claims,
            //    notBefore: _jwtOptions.NotBefore,
            //    expires: _jwtOptions.Expiration,
            //    signingCredentials: _jwtOptions.SigningCredentials);

            ////add login user's role in token
            //jwt.Payload["roles"] = userRole;


            var encodedJwt = GenerateToken(identity, token, userRole); //new JwtSecurityTokenHandler().WriteToken(jwt);


            _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);


            // Serialize and return the response
            //get role permission
            JsonModel UserPermission = _rolePermissionService.GetUserPermissions(token, dbUser.RoleID);


            // return the staff or patient response
            if (userRole[0] == OrganizationRoles.Patient.ToString())
            {
                jsonModel.access_token = encodedJwt;
                jsonModel.expires_in = (int)_jwtOptions.ValidFor.TotalSeconds;
                jsonModel.data = _tokenRepository.GetPaitentByUserID(dbUser.Id, token);
            }
            else
            {
                jsonModel.access_token = encodedJwt;
                jsonModel.expires_in = (int)_jwtOptions.ValidFor.TotalSeconds;

                jsonModel.UserPermission = UserPermission.data;
                jsonModel.AppConfigurations = _tokenRepository.GetAppConfigurationByOrganization(token.OrganizationID);
                jsonModel.UserLocations = _tokenRepository.GetUserLocations(token.UserID); //_tokenRepository.GetUserLocationsByStaff(token.StaffID);                
                jsonModel.data = staff;
            }
            return jsonModel;
        }

        private bool SaveMachineDataIPAndMac(TokenModel token)
        {

            MachineLoginLog machineLoginLog = new MachineLoginLog()
            {
                IpAddress = token.IPAddress,
                MacAddress = token.MacAddress,
                OrganizationID = token.OrganizationID,
                UserId = token.UserID,
                LoginDate = DateTime.UtcNow,
            };
            return _userCommonRepository.SaveMachineLoginUser(machineLoginLog);
        }

        private bool IsMachineLoggedIn(TokenModel token)
        {
            MachineLoginLog machineLoginLog = new MachineLoginLog
            {
                IpAddress = token.IPAddress,
                MacAddress = token.MacAddress,
                OrganizationID = token.OrganizationID,
                UserId = token.UserID,
                LoginDate = DateTime.UtcNow,
            };

            return _userCommonRepository.IsUserMachineLogin(machineLoginLog);
        }

        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private JsonModel UpdateAccessFailedCount(TokenModel tokenModel)
        {

            try
            {
                string Message = string.Empty;
                var user = _userRepository.GetUserByID(tokenModel.UserID);
                if (user.UserRoles.RoleName == OrganizationRoles.Admin.ToString())
                {
                    Message = StatusMessage.InvalidUserOrPassword;//If Admin login with wrong credentials
                }
                else if (user.AccessFailedCount >= 2) // if user attemped 3 time with wrong credentials
                {
                    user.IsBlock = true;
                    user.BlockDateTime = DateTime.UtcNow;
                    user.AccessFailedCount = user.AccessFailedCount + 1;
                    Message = UserAccountNotification.AccountDeactiveOrExpirePass;//block
                }
                else // if wrong attemped increase the failed count
                {
                    if (user.BlockDateTime == null)
                    {
                        Message = UserAccountNotification.AccountDeactive;
                        user.AccessFailedCount = user.AccessFailedCount + 1;
                        Message = UserAccountNotification.InvalidPassword;//Invaild Password
                    }
                    else
                    {
                        user.AccessFailedCount = user.AccessFailedCount + 1;
                        Message = UserAccountNotification.InvalidPassword;//Invaild Password
                    }
                }
                //save
                _userRepository.UpdateUser(user);
                //return
                return new JsonModel
                {
                    Message = Message,
                    data = new object(),
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                };
            }
            catch (Exception ex)
            {
                return new JsonModel
                {
                    Message = ex.Message,
                    data = new object(),
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                };

            }
        }

        private string GenerateToken(ClaimsIdentity identity, TokenModel token, string[] Roles = null)
        {
            var claims = new[]
            {
             new Claim("UserID", token.UserID.ToString()),
             new Claim("RoleID", token.RoleID.ToString()),                      // not required please don't chamge
             new Claim("UserName", token.UserName.ToString()),
             new Claim("OrganizationID", token.OrganizationID.ToString()),              // not required please don't chamge
             new Claim("StaffID", token.StaffID.ToString()),                     // not required please don't chamge
             new Claim("LocationID", token.LocationID.ToString()),                  // not required please don't chamge
             new Claim("DomainName",token.DomainName),                     // Domain name always add in token
             new Claim("PatientID", token.PateintID.ToString()),
             new Claim(JwtRegisteredClaimNames.Sub, token.UserName),
             new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
             new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
             identity.FindFirst("HealthCare")
             };

            var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: _jwtOptions.NotBefore,
            expires: _jwtOptions.Expiration,
            signingCredentials: _jwtOptions.SigningCredentials);
            if (Roles != null)
            {
                jwt.Payload["roles"] = Roles;
            }
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private JsonModel LoginPatient(ApplicationUser applicationUser, User dbUser, TokenModel token, ClaimsIdentity identity, Patients patient)
        {
            JsonModel responseModel = new JsonModel();
            int PatientID = 0;

            //get patient id and his location
            PatientID = patient.Id;
            token.LocationID = patient.LocationID;


            //check patient is Active
            if (!patient.IsActive)
            {
                responseModel.StatusCode = (int)HttpStatusCodes.NotAcceptable;
                responseModel.Message = StatusMessage.ClientActiveStatus;
                responseModel.data = new object();
            }

            //check patient portal is enabled
            if (!patient.IsPortalActivate)
            {
                responseModel.StatusCode = (int)HttpStatusCodes.NotAcceptable;
                responseModel.Message = StatusMessage.ClientPortalDeactivedAtLogin;
                responseModel.data = new object();
            }


            //save IP and MAC address
            #region save IP and MAC Address
            bool machineData = false;
            token.MacAddress = applicationUser.MacAddress;
            token.UserID = dbUser.Id;
            token.RoleID = dbUser.RoleID;
            token.UserName = dbUser.UserName;
            token.OrganizationID = dbUser.OrganizationID;
            token.PateintID = patient.Id;

            token.IPAddress = applicationUser.IpAddress;
            if (!IsMachineLoggedIn(token))
            {
                machineData = SaveMachineDataIPAndMac(token);
            }
            #endregion
            //get login user role name
            string[] userRole = { dbUser.UserRoles.RoleName };
            //  //create claim for login user
            //      var claims = new[]
            //      {
            //  new Claim("UserID", dbUser.Id.ToString()),
            //  new Claim("RoleID", dbUser.RoleID.ToString()),
            //  new Claim("UserName", dbUser.UserName.ToString()),
            //  new Claim("OrganizationID", dbUser.OrganizationID.ToString()),
            //  new Claim("PatientID", PatientID.ToString()),
            //  new Claim("LocationID", token.LocationID.ToString()),
            //  new Claim("DomainName",token.DomainName),                     // Domain name always add in token
            //  new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
            //  new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
            //  new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            //  identity.FindFirst("HealthCare")
            //};

            //reset the user
            _tokenRepository.ResetUserAccess(dbUser.Id);

            //// Create the JWT security token and encode it.
            //var jwt = new JwtSecurityToken(
            //    issuer: _jwtOptions.Issuer,
            //    audience: _jwtOptions.Audience,
            //    claims: claims,
            //    notBefore: _jwtOptions.NotBefore,
            //    expires: _jwtOptions.Expiration,
            //    signingCredentials: _jwtOptions.SigningCredentials);

            ////add login user's role in token
            //jwt.Payload["roles"] = userRole;

            //token.LocationID = defaultLocation;
            var encodedJwt = GenerateToken(identity, token, userRole);//new JwtSecurityTokenHandler().WriteToken(jwt);
            if (dbUser.Id > 0)
            {//login
                _auditLogService.AccessLogs(AuditLogsScreen.Login, AuditLogAction.Login, null, dbUser.Id, token, LoginLogLoginAttempt.Success);
            }
            // Serialize and return the response
            responseModel.access_token = encodedJwt;
            responseModel.expires_in = (int)_jwtOptions.ValidFor.TotalSeconds;
            responseModel.data = patient;

            return responseModel;
        }
        public Organization GetOrganizationByOrgId(int id, TokenModel token)
        {
            return _organizationRepository.GetByID(id);
        }
        public OrganizationModel GetOrganizationById(int id, TokenModel token)
        {
            try
            {
                OrganizationModel organizationModel = new OrganizationModel();
                Organization org = _organizationRepository.GetByID(id);
                //AutoMapper.Mapper.Map(org, organizationModel);
                String Logo = string.Empty;
                String Favicon = string.Empty;
                if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + org.Logo))
                {
                    string base64string = Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + org.Logo;
                    Byte[] bytes = File.ReadAllBytes(base64string);
                    Logo = Convert.ToBase64String(bytes);
                }
                else if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + "telehealth-logo.png"))
                {
                    string base64string = Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + "telehealth-logo.png";
                    Byte[] bytes = File.ReadAllBytes(base64string);
                    Logo = Convert.ToBase64String(bytes);
                }
                if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + org.Favicon))
                {
                    string base64string = Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + org.Favicon;
                    Byte[] bytes = File.ReadAllBytes(base64string);
                    Favicon = Convert.ToBase64String(bytes);
                    organizationModel.Favicon = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OrganizationImages + "Favicon//", org.Favicon);
                }
                else if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + "favicon.ico"))
                {
                    string base64string = Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + "favicon.ico";
                    Byte[] bytes = File.ReadAllBytes(base64string);
                    Favicon = Convert.ToBase64String(bytes);
                    organizationModel.Favicon = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OrganizationImages + "Favicon//", "favicon.ico");
                }

                organizationModel.LogoBase64 = Logo;
                organizationModel.FaviconBase64 = Favicon;
                organizationModel.BusinessName = org.BusinessName;
                organizationModel.OrganizationName = org.OrganizationName;
                return organizationModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public OrganizationModel GetOrganizationDetailsByBusinessName(string businessName)
        {
            try
            {
                Organization org = _organizationRepository.Get(a => a.BusinessName == businessName && a.IsActive == true && a.IsDeleted == false);
                OrganizationModel organizationModel = new OrganizationModel();
                AutoMapper.Mapper.Map(org, organizationModel);
                organizationModel.BusinessName = org.BusinessName;
                organizationModel.OrganizationName = org.OrganizationName;
                return organizationModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public NotificationModel GetLoginNotification(TokenModel tokenModel)
        //{
        //    NotificationModel notificationModel = _tokenRepository.GetLoginNotification(tokenModel);

        //    if (notificationModel != null)
        //    {
        //        notificationModel.MessageNotification.ForEach(x =>
        //        {
        //            x.Subject = CommonMethods.Decrypt(x.Subject);
        //            x.Thumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, x.IsPatient ? ImagesPath.PatientThumbPhotos : ImagesPath.StaffThumbPhotos, x.Thumbnail);
        //        });
        //    }

        //    return notificationModel;
        //}
        public NotificationModel GetLoginNotification(TokenModel token)
        {
            NotificationModel notificationModel = _tokenRepository.GetLoginNotification(token);

            if (notificationModel.UserDocumentNotification != null && notificationModel.UserDocumentNotification.Count > 0)
            {
                notificationModel.UserDocumentNotification.ForEach((x =>
                {
                    if (x.Message != null)
                    {
                        x.Message = CommonMethods.Decrypt(x.Message);
                    }
                    if (x.NotificationTypeId != null)
                    {
                        x.Type = Enum.GetName(typeof(NotificationActionType), x.NotificationTypeId).ToString();
                    }
                    if (x.NotificationSubTypeId != null)
                    {
                        x.SubType = Enum.GetName(typeof(NotificationActionSubType), x.NotificationSubTypeId).ToString();
                    }
                   
                }));
            }

            return notificationModel;
        }

        public Patients GetLastPatientByOrganization(TokenModel tokenModel)
        {
            Patients patientData = _tokenRepository.GetLastPatientByOrganization(tokenModel);
            return patientData;
        }

        public bool GetDefaultClient(int UserID)
        {
            return _tokenRepository.GetDefaultClient(UserID);
        }
        public JsonModel GetNotificationDetailsById(int notificationId, TokenModel tokenModel)
        {
            return new JsonModel(_tokenRepository.GetNotificationDetailsById<UserNotificationModel>(notificationId, tokenModel).FirstOrDefault(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
        }

        public JsonModel GetNotificationList(CommonFilterModel commonFilterModel, TokenModel tokenModel)
        {
            List<NotificationGroupItem<UserNotificationModel>> notificationList = new List<NotificationGroupItem<UserNotificationModel>>();
            List<UserNotificationModel> notificationModel = new List<UserNotificationModel>();
            notificationModel = _tokenRepository.GetNotificationList<UserNotificationModel>(commonFilterModel, tokenModel).ToList();

            if (notificationModel != null && notificationModel.Count > 0)
            {
                notificationModel.ForEach((x =>
                {
                    if (x.LocationId != null)
                    {
                        LocationModel locationModal = _locationService.GetLocationOffsets(x.LocationId, tokenModel);
                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                        x.TimeStamp = CommonMethods.ConvertFromUtcTimeWithOffset(x.TimeStamp, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                    }
                    if (x.NotificationAction == "ChatMessage")
                    {
                        x.Message = CommonMethods.Decrypt(x.Message);

                    }
                }));

                notificationList = notificationModel
                                  .GroupBy(i => i.CreatedDate).Select(g => new NotificationGroupItem<UserNotificationModel>
                                  {
                                      SpecificDay = g.Key.ToString(),
                                      NotificationGroup = g.Select(p => p).OrderByDescending(x => x.CreatedDateWitOutFormat).ToList<UserNotificationModel>()
                                  })
                                  .OrderByDescending(x => x.SpecificDay).ToList();

                notificationList.ForEach(x =>
                {
                    if (x.SpecificDay.Equals(DateTime.Now.ToString("MM/dd/yyyy")))
                    {
                        x.SpecificDay = "Today";
                    }
                    else if (x.SpecificDay.Equals(DateTime.Now.Date.AddDays(-1).ToString("MM/dd/yyyy")))
                    {
                        x.SpecificDay = "Yesterday";
                    }
                    else
                    {
                        x.SpecificDay = DateTime.Parse(x.SpecificDay).ToString("dd MMM yyyy");
                    }

                });
            }

            return new JsonModel
            {
                data = notificationList,
                meta = new Meta(notificationModel, commonFilterModel),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel ReadNotification(TokenModel tokenModel)
        {
            List<NotificationTypeMapping> data = _notificationRepository.GetAllUnReadALerts<NotificationTypeMapping>(tokenModel).ToList();
            if (data != null && data.Count > 0)
            {
                data.ForEach(x =>
                {
                    x.IsReadNotification = true;
                });
                _notificationRepository.SaveNotificationTypeMapping(data);
                _notificationRepository.SaveChanges();

                return new JsonModel
                {
                    data = data,
                    Message = StatusMessage.UpdatedSuccessfully,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return null;
        }

        public JsonModel GetPatientNotificationList(int patientID, int pageNumber, int pageSize, TokenModel tokenModel)
        {
            JsonModel jsonModel;
            List<GetNotificationListModel> notificationList = new List<GetNotificationListModel>();

            notificationList = _notificationRepository.GetPatientNotificationList<GetNotificationListModel>(patientID, pageNumber, pageSize, tokenModel).ToList();

            if (notificationList != null && notificationList.Count() > 0)
            {
                notificationList.ForEach(x =>
                {
                    if (x.TimeZoneName != null)
                    {
                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, GetMin(x.DaylightSavingTime), GetMin(x.StandardTime), x.TimeZoneName, tokenModel);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, GetMin(x.DaylightSavingTime), GetMin(x.StandardTime), x.TimeZoneName, tokenModel);
                    }
                    if (x.NotificationAction == "ChatMessage" && !string.IsNullOrEmpty(x.Message))
                    {
                        x.Message = CommonMethods.Decrypt(x.Message);
                    }
                });

                jsonModel = new JsonModel
                {
                    data = notificationList,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = notificationList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(notificationList[0].TotalRecords / pageSize))
                    },

                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                jsonModel = new JsonModel
                {
                    data = notificationList,
                    meta = null,
                    Message = StatusMessage.NoContent,
                    StatusCode = (int)HttpStatusCodes.NoContent
                };
            }
            return jsonModel;
        }

        public JsonModel GetPatientPortalNotificationList(int patientID, int pageNumber, int pageSize, TokenModel tokenModel)
        {
            //JsonModel jsonModel;
            List<NotificationGroupItem<PatientNotificationListModel>> notificationList = new List<NotificationGroupItem<PatientNotificationListModel>>();
            List<PatientNotificationListModel> patientnotification = new List<PatientNotificationListModel>();
            patientnotification = _notificationRepository.GetPatientPortalNotificationList<PatientNotificationListModel>(patientID, pageNumber, pageSize, tokenModel).ToList();

            if (patientnotification != null && patientnotification.Count > 0)
            {
                patientnotification.ForEach((x =>
                {
                    if (x.TimeZoneName != null)
                    {
                        //LocationModel locationModal = _locationService.GetLocationOffsets(x.LocationId, tokenModel);
                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, GetMin(x.DaylightSavingTime), GetMin(x.StandardTime), x.TimeZoneName, tokenModel);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, GetMin(x.DaylightSavingTime), GetMin(x.StandardTime), x.TimeZoneName, tokenModel);
                        // x.TimeStamp = CommonMethods.ConvertFromUtcTimeWithOffset(x.TimeStamp, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                    }
                    if (x.NotificationAction == "ChatMessage")
                    {
                        x.Message = CommonMethods.Decrypt(x.Message);

                    }
                }));

                notificationList = patientnotification
                                  .GroupBy(i => i.CreatedDate.Date).Select(g => new NotificationGroupItem<PatientNotificationListModel>
                                  {
                                      SpecificDay = g.Key.ToString(),
                                      NotificationGroup = g.Select(p => p).OrderByDescending(x => x.CreatedDate.Date).ToList<PatientNotificationListModel>()
                                  })
                                  .OrderByDescending(x => DateTime.Parse(x.SpecificDay).ToString("MM/dd/yyyy")).ToList();

                notificationList.ForEach(x =>
                {
                    if (x.SpecificDay != null)
                    {
                        if (DateTime.Parse(x.SpecificDay).ToString("MM/dd/yyyy").Equals(DateTime.Now.ToString("MM/dd/yyyy")))
                        {
                            x.SpecificDayStr = "Today";
                        }
                        else
                        if (DateTime.Parse(x.SpecificDay).ToString("MM/dd/yyyy").Equals(DateTime.Now.Date.AddDays(-1).ToString("MM/dd/yyyy")))
                        {
                            x.SpecificDayStr = "Yesterday";
                        }
                        else
                            x.SpecificDayStr = DateTime.Parse(x.SpecificDay).ToString("MM/dd/yyyy");
                    }
                });
            }

            return new JsonModel
            {
                data = notificationList,
                meta = new Meta()
                {
                    TotalRecords = patientnotification[0].TotalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal(patientnotification[0].TotalRecords / pageSize))
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        private decimal GetMin(decimal number)
        {
            string s = number.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = s.Split('.');
            //
            decimal i1 = int.Parse(parts[0]) * 60;

            decimal i2 = int.Parse(parts[1]);

            return i1 + i2;
        }
        public JsonModel ReadChatAndAllNotification(int patientId, bool isChatNotification, TokenModel tokenModel)
        {
            List<NotificationTypeMapping> data = _notificationRepository.ReadChatAndAllNotification(patientId, isChatNotification, tokenModel);
            if (data != null && data.Count > 0)
            {
                data.ForEach(x =>
                {
                    x.IsReadNotification = true;
                });
                _notificationRepository.SaveNotificationTypeMapping(data);
                _notificationRepository.SaveChanges();
                return new JsonModel
                {
                    data = _tokenRepository.GetChatAndNotificationCount(tokenModel.UserID, tokenModel),
                    Message = StatusMessage.UpdatedSuccessfully,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return new JsonModel
            {
                data = _tokenRepository.GetChatAndNotificationCount(tokenModel.UserID, tokenModel),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }


        public JsonModel ReadChatAndAllNotificationForPatient(int patientId, TokenModel tokenModel)
        {
            List<NotificationTypeMapping> data = _notificationRepository.ReadChatAndAllNotificationForPateint<NotificationTypeMapping>(patientId, tokenModel).ToList();
            if (data != null && data.Count > 0)
            {
                data.ForEach(x =>
                {
                    x.IsReadNotification = true;
                });
                _notificationRepository.SaveNotificationTypeMapping(data);
                _notificationRepository.SaveChanges();

                return new JsonModel
                {
                    data = data,
                    Message = StatusMessage.UpdatedSuccessfully,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return null;
        }
        public JsonModel GetChatAndNotificationCount(int userId, TokenModel tokenModel)
        {
            ChatAndNotificaitonModel chatAndNotificationCountModel = _tokenRepository.GetChatAndNotificationCount(userId, tokenModel);
            if (chatAndNotificationCountModel != null)
            {
                return new JsonModel()
                {
                    data = chatAndNotificationCountModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK//Success
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

    }
}
