
using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private JsonModel response;
        private readonly IUserSecurityQuestionAnswerRepository _userSecurityQuestionAnswerRepository;
        private readonly HCOrganizationContext _context;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUserPasswordHistoryService _userPasswordHistoryService;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly ILocationService _locationService;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailSender;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly IStaffService _staffService;
        private readonly IPatientService _patientService;
        private readonly IUserRoleService _userRoleService;
        public UserService(IUserRepository userRepository,
            IUserSecurityQuestionAnswerRepository userSecurityQuestionAnswerRepository,
            IAuditLogRepository auditLogRepository,
            IUserPasswordHistoryService userPasswordHistoryService,
            IUserPasswordHistoryRepository userPasswordHistoryRepository,
            HCOrganizationContext context,
            ILocationService locationService,
            IUserCommonRepository userCommonRepository,
            IOrganizationSMTPRepository organizationSMTPRepository,
            ITokenService tokenService,
            IEmailService emailSender,
            IEmailWriteService emailWriteService,
            IConfiguration configuration,
            IHostingEnvironment env,
            IStaffService staffService,
            IPatientService patientService,
            IUserRoleService userRoleService
            )
        {
            _userRepository = userRepository;
            _userSecurityQuestionAnswerRepository = userSecurityQuestionAnswerRepository;
            _auditLogRepository = auditLogRepository;
            _context = context;
            _userPasswordHistoryService = userPasswordHistoryService;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _locationService = locationService;
            _userCommonRepository = userCommonRepository;
            _organizationSMTPRepository = organizationSMTPRepository;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _emailWriteService = emailWriteService;
            _configuration = configuration;
            _env = env;
            _staffService = staffService;
            _patientService = patientService;
            _userRoleService = userRoleService;
        }
        public List<StaffModels> GetFilteredStaff(string LocationIds, string RoleIds, string SearchKey, string StartWith, string Tags, string sortColumn, string sortOrder, int pageNumber, int pageSize, TokenModel tokenModel)
        {
            return _userRepository.GetFilteredStaff<StaffModels>(LocationIds, RoleIds, SearchKey, StartWith, Tags, sortColumn, sortOrder, pageNumber, pageSize, tokenModel).ToList();
        }

        

        public JsonModel UpdateUserPassword(UpdatePasswordModel updatePassword, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == updatePassword.UserId && a.Password == CommonMethods.Encrypt(updatePassword.CurrentPassword));
                if (user != null)
                {
                    if (updatePassword.NewPassword == updatePassword.ConfirmNewPassword)
                    {
                        List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(updatePassword.UserId);
                        if (passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == updatePassword.NewPassword).Count() > 0)
                        {
                            return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                        }
                        using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                user.Password = CommonMethods.Encrypt(updatePassword.NewPassword);
                                user.PasswordResetDate = DateTime.UtcNow;
                                _userRepository.Update(user);
                                _userPasswordHistoryService.SaveUserPasswordHistory(updatePassword.UserId, DateTime.UtcNow, user.Password);
                                _userRepository.SaveChanges();
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                            }
                        }

                        return response = new JsonModel()  //Sucess//
                        {
                            data = user,
                            Message = UserAccountNotification.YourPasswordChanged,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else // if new current password and confirm password doesn't match
                    {
                        return response = new JsonModel()
                        {
                            data = new object(),
                            Message = UserAccountNotification.YourPasswordNotMatchWithConfirmPassword,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                        };
                    }

                }
                else // if your current password doesn't match
                {
                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = UserAccountNotification.YourCurrentPassword,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel UpdateUserStatus(int userId, bool status, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.GetByID(userId);
                //
                if (status)//block
                {
                    user.IsBlock = status;
                    user.BlockDateTime = DateTime.UtcNow;
                    user.AccessFailedCount = 3;
                    //save
                    _userRepository.UpdateUser(user);
                    _userRepository.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.UserBlocked.Replace("[RoleName]", user.UserName),
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                else
                {//unblock
                    user.IsBlock = status;
                    user.BlockDateTime = null;
                    user.AccessFailedCount = 0;
                    //save
                    _userRepository.UpdateUser(user);
                    _userRepository.SaveChanges();

                    //on unblock time 
                    List<UserSecurityQuestionAnswer> userQuestionAnswer = _userSecurityQuestionAnswerRepository.GetAll(a => a.IsActive == true && a.IsDeleted == false && a.UserID == userId).ToList();
                    userQuestionAnswer.ForEach(a => { a.IsDeleted = true; a.DeletedDate = DateTime.UtcNow; a.DeletedBy = token.UserID; });
                    _userSecurityQuestionAnswerRepository.Update(userQuestionAnswer.ToArray());
                    _userSecurityQuestionAnswerRepository.SaveChanges();

                    return new JsonModel
                    {
                        Message = StatusMessage.UserUnblocked.Replace("[RoleName]", user.UserName),
                        data = new object(),
                        StatusCode = (int)HttpStatusCodes.OK
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


        public JsonModel UploadUserDocuments(UserDocumentsModel userDocuments, TokenModel token)
        {
            try
            {
                List<UserDocuments> userDocList = new List<UserDocuments>();
                string organizationName = _context.Organization.Where(a => a.Id == token.OrganizationID).FirstOrDefault().OrganizationName;

                #region saveDoc
                foreach (var item in userDocuments.Base64)
                {
                    UserDocuments userDoc = new UserDocuments();

                    item.Value.Replace("\"", "");
                    string[] extensionArr = { "jpg", "jpeg", "png", "txt", "docx", "doc", "xlsx", "pdf", "pptx", "mp4","mp3" };
                    //getting data from base64 url                    
                    string base64Data = item.Value.Replace("\"", "").Split(':')[0].ToString().Trim();
                    //getting extension of the image
                    string extension = item.Value.Replace("\"", "").Split(':')[1].ToString().Trim();

                    //out from the loop if document extenstion not exist in list of extensionArr
                    if (!extensionArr.Contains(extension)) { goto Finish; }

                    //create directory
                    //string webRootPath = Directory.GetCurrentDirectory()+ "\\PatientDocuments";
                    string webRootPath = Directory.GetCurrentDirectory();

                    //save folder
                    string DirectoryUrl = userDocuments.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper() ? ImagesPath.UploadClientDocuments : ImagesPath.UploadStaffDocuments;

                    if (!Directory.Exists(webRootPath + DirectoryUrl))
                    {
                        Directory.CreateDirectory(webRootPath + DirectoryUrl);
                    }

                    string fileName = organizationName + "_" + DateTime.UtcNow.TimeOfDay.ToString();

                    //update file name remove unsupported attr.
                    fileName = fileName.Replace(" ", "_").Replace(":", "_");

                    //create path for save location
                    string path = webRootPath + DirectoryUrl + fileName + "." + extension;

                    //convert files into base
                    Byte[] bytes = Convert.FromBase64String(base64Data);
                    //save int the directory
                    File.WriteAllBytes(path, bytes);



                    //create db path
                    //string uploadPath = @"/Documents/ClientDocuments/" + fileName + "." + extension;

                    userDoc.CreatedBy = token.UserID;
                    userDoc.CreatedDate = DateTime.UtcNow;
                    userDoc.IsActive = true;
                    userDoc.IsDeleted = false;
                    userDoc.UserId = userDocuments.UserId;
                    //userDoc.UserId = token.UserID;
                    userDoc.DocumentName = userDocuments.DocumentTitle;
                    userDoc.Expiration = userDocuments.Expiration;
                    userDoc.OtherDocumentType = userDocuments.OtherDocumentType;
                    userDoc.PatientAppointmentId = userDocuments.PatientAppointmentId;
                    userDoc.IsProviderEducationalDocument = userDocuments.IsProviderEducationalDocument;
                    if (userDocuments.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                    {
                        userDoc.DocumentTypeId = userDocuments.DocumentTypeId;
                    }
                    else
                    {
                        userDoc.DocumentTypeIdStaff = userDocuments.DocumentTypeIdStaff;
                    }
                    userDoc.CreatedDate = DateTime.UtcNow;
                    userDoc.UploadPath = fileName + "." + extension;
                    userDoc.Key = userDocuments.Key;
                    userDocList.Add(userDoc);

                }
                //save into db
                _context.UserDocuments.AddRange(userDocList);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateDocumentDetails, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                _context.SaveChanges();

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.DocumentUploaded,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            #endregion

            //return with invaild format message
            Finish:;
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.InvaildFormat,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = e.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel GetUserDocuments(int userId, DateTime? from, DateTime? to, TokenModel token)
        {
            try
            {
                List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.UserId == userId && a.IsActive == true && a.IsDeleted == false && (a.CreatedDate.Value.Date >= from && a.CreatedDate.Value.Date <= to)).OrderByDescending(a => a.CreatedDate).ToList();
                //string siteURL = _context.AppConfigurations.Where(a => a.Key == "SITE_URL" && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Value;
                if (userDocs != null && userDocs.Count() > 0)
                {
                    List<UserDocumentsModel> userDocListModel = new List<UserDocumentsModel>();
                    if (token.LocationID == 0)
                    {
                        token.LocationID = 101;
                    }
                    var location = _locationService.GetLocationOffsets(token.LocationID, token);
                    foreach (var item in userDocs)
                    {
                        UserDocumentsModel userDocModel = new UserDocumentsModel();

                        userDocModel.Id = item.Id;
                        userDocModel.UserId = userId;
                        //userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadDocuments, item.UploadPath);
                        userDocModel.DocumentTitle = item.DocumentName;

                        userDocModel.Url = item.UploadPath;

                        if (item.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        {
                            userDocModel.DocumentTypeId = item.DocumentTypeId;
                            userDocModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == item.DocumentTypeId).FirstOrDefault().Type;
                            userDocModel.DocumentTypeName = userDocModel.DocumentTypeName.ToUpper() == "OTHER" ? userDocModel.DocumentTypeName + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeName;
                        }
                        else
                        {
                            userDocModel.DocumentTypeIdStaff = item.DocumentTypeIdStaff;
                            if (item.DocumentTypeIdStaff != null)
                            {
                                userDocModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == item.DocumentTypeIdStaff).FirstOrDefault().Type;
                                userDocModel.DocumentTypeNameStaff = userDocModel.DocumentTypeNameStaff.ToUpper() == "OTHER" ? userDocModel.DocumentTypeNameStaff + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeNameStaff;
                            }
                            else
                                userDocModel.DocumentTypeNameStaff = "OTHER";

                        }

                        userDocModel.Extenstion = Path.GetExtension(item.UploadPath);
                        userDocModel.CreatedDate = ConvertFromUtcTimeNew((DateTime)item.CreatedDate, location, token);
                        userDocModel.Key = item.Key;
                        userDocModel.Expiration = item.Expiration;
                        userDocModel.OtherDocumentType = item.OtherDocumentType;
                        userDocListModel.Add(userDocModel);
                    }
                    return new JsonModel()
                    {
                        data = userDocListModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
        //, _locationService.GetLocationOffsets(token.LocationID, token)
        private DateTime ConvertFromUtcTimeNew(DateTime date, LocationModel locationModel, TokenModel token)
        {
            return CommonMethods.ConvertFromUtcTimeWithOffset(date, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, token);
        }
        public UserDocumentsResponseModel GetUserDocument(int id, TokenModel token)
        {
            try
            {
                UserDocuments userDoc = _context.UserDocuments.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

                //Save folder Directory
                string DirectoryUrl = userDoc.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper() ? ImagesPath.UploadClientDocuments : ImagesPath.UploadStaffDocuments;


                if (File.Exists(Directory.GetCurrentDirectory() + DirectoryUrl + userDoc.UploadPath))
                {
                    UserDocumentsResponseModel userDocumentModel = new UserDocumentsResponseModel();
                    string base64string = Directory.GetCurrentDirectory() + DirectoryUrl + userDoc.UploadPath;

                    Byte[] bytes = File.ReadAllBytes(base64string);
                    MemoryStream memoryFile = new MemoryStream(bytes);
                    String file = Convert.ToBase64String(bytes);
                    var location = _locationService.GetLocationOffsets(token.LocationID, token);
                    userDocumentModel.Id = userDoc.Id;
                    userDocumentModel.UserId = userDoc.UserId;
                    userDocumentModel.Base64 = file;
                    userDocumentModel.DocumentTitle = userDoc.DocumentName;
                    userDocumentModel.CreatedDate = ConvertFromUtcTimeNew((DateTime)userDoc.CreatedDate, location, token);

                    if (userDoc.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        userDocumentModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == userDoc.DocumentTypeId).FirstOrDefault().Type;
                    else
                        userDocumentModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == userDoc.DocumentTypeIdStaff).FirstOrDefault().Type;

                    //userDocumentModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == userDoc.DocumentTypeId).FirstOrDefault().Type;
                    userDocumentModel.Extenstion = Path.GetExtension(userDoc.UploadPath);
                    userDocumentModel.File = memoryFile;
                    userDocumentModel.FileName = userDoc.UploadPath;
                    userDocumentModel.Expiration = userDoc.Expiration;
                    userDocumentModel.OtherDocumentType = userDoc.OtherDocumentType;

                    return userDocumentModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonModel DeleteUserDocument(int id, TokenModel token)
        {
            try
            {
                UserDocuments userDoc = _context.UserDocuments.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (userDoc != null)
                {
                    userDoc.IsDeleted = true;
                    userDoc.DeletedDate = DateTime.UtcNow;
                    _context.Update(userDoc);
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteDocumentDetails, AuditLogAction.Delete, null, token.UserID, "" + null, token);
                    _context.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DocumentDelete,
                        StatusCode = (int)HttpStatusCodes.NoContent
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel Logout(TokenModel token)
        {
            Entity.User user = _userRepository.Get(a => a.Id == token.UserID && a.IsActive == true && a.IsDeleted == false);
            if (user != null)
            {
                user.IsOnline = false;
                _userRepository.Update(user);
                //audit logs
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.Login, AuditLogAction.Logout, null, token.UserID, "", token);

                return new JsonModel()
                {
                    data = new object(),
                    Message = UserAccountNotification.LoggedOut,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = UserAccountNotification.NoDataFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public bool SetOnline(int userId)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == userId && a.IsActive == true && a.IsDeleted == false);
                if (user != null)
                {
                    user.IsOnline = true;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PasswordCheckModel CheckUserPasswordStatus(TokenModel token)
        {
            PasswordCheckModel passwordCheckModel = null;
            Entity.User user = _userRepository.Get(x => x.Id == token.UserID && x.IsActive);
            if (user != null && user.PasswordResetDate != null)
            {
                int lockNotificationDays = (int)AccountConfiguration.LockNotification;
                passwordCheckModel = new PasswordCheckModel();
                TimeSpan days = user.PasswordResetDate.Value.Date.AddDays(lockNotificationDays) - DateTime.UtcNow.Date;

                if (days.Days <= lockNotificationDays && days.Days > lockNotificationDays - 2)
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Sucess, (int)HttpStatusCodes.OK);
                }
                //if <= 15 or > 10 days left then just show warning to user
                else if (days.Days <= Math.Abs((lockNotificationDays / 4)) && days.Days > Math.Abs((lockNotificationDays / 6)))
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK);
                }
                //if <= 10 or > 5 days left then just show warning to user
                else if (days.Days <= Math.Abs((lockNotificationDays / 6)) && days.Days > Math.Abs((lockNotificationDays / 12))) //warning
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK);
                }
                else if (days.Days <= Math.Abs((lockNotificationDays / 12)) && days.Days > Math.Abs((1))) //warning
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger, (int)HttpStatusCodes.Unauthorized);
                }
                //if <= 0 then block the user and show warning to user with red color
                else if (days.Days <= 0)//block 
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger);
                }
                else
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.Success, false);
                }
            }
            return passwordCheckModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatePassword"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel UpdateExpiredPassword(UpdatePasswordModel updatePassword, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == updatePassword.UserId && a.Password == CommonMethods.Encrypt(updatePassword.CurrentPassword));
                if (user != null)
                {
                    if (updatePassword.NewPassword == updatePassword.ConfirmNewPassword)
                    {
                        List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(updatePassword.UserId);
                        if (passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == updatePassword.NewPassword).Count() > 0)
                        {
                            return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                        }
                        using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                user.Password = CommonMethods.Encrypt(updatePassword.NewPassword);
                                user.PasswordResetDate = DateTime.UtcNow;
                                user.UpdatedBy = updatePassword.UserId;//own self
                                user.UpdatedDate = DateTime.UtcNow;
                                _userRepository.Update(user);

                                _userPasswordHistoryService.SaveUserPasswordHistory(updatePassword.UserId, DateTime.UtcNow, user.Password);
                                _userRepository.SaveChanges();

                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                            }
                        }

                        return response = new JsonModel()  //Sucess//
                        {
                            data = user,
                            Message = UserAccountNotification.YourPasswordChanged,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else // if new current password and confirm password doesn't match
                    {
                        return response = new JsonModel(new object(), UserAccountNotification.YourPasswordNotMatchWithConfirmPassword, (int)HttpStatusCodes.UnprocessedEntity);
                    }

                }
                else // if your current password doesn't match
                {
                    return response = new JsonModel(new object(), UserAccountNotification.YourCurrentPassword, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCodes.InternalServerError, ex.Message);
            }
        }
        public JsonModel UpdateUserPasswordWithToken(UpdatePasswordModel updatePassword, TokenModel token)
        {
            try
            {
                var authToken = _userCommonRepository.GetAuthenticationToken(updatePassword.Token, token);
                if (authToken == null)
                    return new JsonModel(null, StatusMessage.PasswordTokenExpired, (int)HttpStatusCodes.NotFound);
                token.UserID = updatePassword.UserId = (int)authToken.UserID;
                var tokenDate = (DateTime)authToken.CreatedDate;
                var currentDate = DateTime.UtcNow;
                TimeSpan ts = currentDate - tokenDate;
                if (ts.TotalMinutes > (24 * 60))
                    return new JsonModel(null, StatusMessage.PasswordTokenExpired, (int)HttpStatusCodes.BadRequest);

                Entity.User user = _userRepository.Get(a => a.Id == authToken.UserID);

                if (user != null)
                {
                    string role = string.Empty;
                    JsonModel userRole = _userRoleService.GetRoleById(user.RoleID, token);
                    if (userRole.StatusCode == (int)HttpStatusCodes.OK)
                    {
                        UserRoleModel userRoleModel = (UserRoleModel)userRole.data;
                        role = userRoleModel.UserType;
                    }

                    //var userRole=role
                    string toEmail = "";
                    string name = "";
                    if (role.ToUpper() != UserTypeEnum.CLIENT.ToString().ToUpper())
                    {
                        var staff = _staffService.GetStaffByUserId(user.Id, token);
                        if (staff == null)
                            return new JsonModel(null, StatusMessage.StaffInfoNotFound, (int)HttpStatusCodes.NotFound);
                        if (staff.IsActive == false)
                            return new JsonModel(null, StatusMessage.AccountDeactivated, (int)HttpStatusCodes.Unauthorized);
                        name = CommonMethods.getFullName(staff.FirstName, staff.MiddleName, staff.LastName);
                        toEmail = staff.Email;

                    }
                    else
                    {
                        var response = _patientService.GetPatientIdByUserId((int)authToken.UserID, token);
                        var patientId = 0;
                        if (response.StatusCode == (int)HttpStatusCodes.OK)
                            patientId = (int)response.data;
                        else
                            return new JsonModel(null, StatusMessage.AccountNotFound, (int)HttpStatusCodes.NotFound);
                        var patientResponse = _patientService.GetPatientById(patientId, token);
                        PatientDemographicsModel patient;
                        if (patientResponse.StatusCode == (int)HttpStatusCodes.OK)
                            patient = (PatientDemographicsModel)patientResponse.data;
                        else
                            patient = null;
                        if (patient == null)
                            return new JsonModel(null, StatusMessage.AccountNotFound, (int)HttpStatusCodes.NotFound);
                        //if (patient.IsActive == false)
                        //return new JsonModel(null, StatusMessage.AccountDeactivated, (int)HttpStatusCodes.Unauthorized);
                        name = CommonMethods.getFullName(patient.FirstName, patient.MiddleName, patient.LastName);
                        toEmail = patient.Email;
                    }
                    if (updatePassword.NewPassword == updatePassword.ConfirmNewPassword)
                    {
                        List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(updatePassword.UserId);
                        if (passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == updatePassword.NewPassword).Count() > 0)
                        {
                            return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                        }
                        using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                user.Password = CommonMethods.Encrypt(updatePassword.NewPassword);
                                user.PasswordResetDate = DateTime.UtcNow;
                                _userRepository.Update(user);
                                _userPasswordHistoryService.SaveUserPasswordHistory(updatePassword.UserId, DateTime.UtcNow, user.Password);
                                _userRepository.SaveChanges();
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                            }
                        }

                        var emailError = SendPasswordResetEmail(
                            toEmail,
                            name,
                             (int)EmailType.ResetPassword,
                                  (int)EmailSubType.none,
                                  user.Id,
                                  "/templates/reset-password-successfully.html",
                                  "Password Reset Successfully",
                                  token,
                                  token.Request.Request
                            );
                        return response = new JsonModel()  //Sucess//
                        {
                            data = user,
                            Message = UserAccountNotification.YourPasswordChanged,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else // if new current password and confirm password doesn't match
                    {
                        return response = new JsonModel()
                        {
                            data = new object(),
                            Message = UserAccountNotification.YourPasswordNotMatchWithConfirmPassword,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                        };
                    }

                }
                else // if your current password doesn't match
                {
                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = UserAccountNotification.YourCurrentPassword,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }
        private string SendPasswordResetEmail(string toEmail, string username, int emailType, int emailSubType, int primaryId, string templatePath, string subject, TokenModel tokenModel, HttpRequest Request)
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
            ////await _emailSender.SendEmailAsync(userInvitationModel.Email, string.Format("Invitation From {0}", orgData.OrganizationName), emailHtml, organizationSMTPDetailModel, orgData.OrganizationName);
            //var isEmailSent = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            //emailModel.EmailStatus = isEmailSent;
            ////Maintain Email log into Db
            ////var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
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
        public Entity.User CeatedInvitedUser(UserInviteModel userInvitelModel, TokenModel token)
        {
            UserRoleModel userRoleModel = new UserRoleModel()
            {
                IsActive = false,
                RoleName = "Invited",
                UserType = "INVITED"

            };
            var roleResult = _userRoleService.SaveRole(userRoleModel, token, false);
            if (roleResult.StatusCode != (int)HttpStatusCodes.OK && roleResult.StatusCode != (int)HttpStatusCodes.UnprocessedEntity)
                return null;

            Entity.User requestUser = new Entity.User();
            Random rnd = new Random();
            requestUser.UserName = userInvitelModel.Name.Replace(" ", "") + rnd.Next(1111, 9999);
            requestUser.Password = CommonMethods.Encrypt(userInvitelModel.Name.Replace(" ", "").ToUpper() + "@" + rnd.Next(1111, 9999));
            requestUser.RoleID = ((UserRoles)roleResult.data).Id; // TO DO it will be dynamic
            requestUser.CreatedBy = token.UserID;
            requestUser.CreatedDate = DateTime.UtcNow;
            requestUser.IsActive = false;
            requestUser.IsDeleted = false;
            requestUser.OrganizationID = token.OrganizationID;
            requestUser.PasswordResetDate = DateTime.UtcNow;
            _userRepository.Create(requestUser);
            if (_userRepository.SaveChanges() > 0)
                return requestUser;
            else
                return null;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changePasscodeSettings"></param>
        /// <param name="isPasscodeEnable"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonModel ChangePasscodeSettings(int userId, bool isPasscodeEnable)
        {
            try
            {
                return response = new JsonModel()  //Sucess//
                {
                    data = _userRepository.ChangePasscodeSettings(userId, isPasscodeEnable),
                    Message = UserAccountNotification.PasscodeSettingsChanged,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCodes.InternalServerError, ex.Message);
            }
        }
        public JsonModel SaveUpdateMobileTokens(string ApnToken, string deviceToken, int userId)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == userId);
                if (user != null)
                {
                    user.ApnToken = ApnToken;
                    user.DeviceToken = deviceToken;

                    _userRepository.UpdateUser(user);


                    return response = new JsonModel()
                    {
                        Message = StatusMessage.UpdatedSuccessfully,
                        StatusCode = (int)HttpStatusCodes.InternalServerError
                    };
                }
                else
                {
                    return response = new JsonModel()
                    {
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }
        public UserTokenModel GetApnToken(int AppointmentId, int roleId)
        {
            return _userRepository.GetApnToken<UserTokenModel>(AppointmentId, roleId);
        }

        public JsonModel GetPateintAppointmenttDocuments(int apptId, TokenModel token)
        {
            try
            {
                //List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.PatientAppointmentId == apptId && a.IsActive == true && a.IsDeleted == false && (a.CreatedDate.Value.Date >= from && a.CreatedDate.Value.Date <= to)).OrderByDescending(a => a.CreatedDate).ToList();
                List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.PatientAppointmentId == apptId && a.IsActive == true && a.IsDeleted == false).OrderByDescending(a => a.CreatedDate).ToList();
                //string siteURL = _context.AppConfigurations.Where(a => a.Key == "SITE_URL" && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Value;
                if (userDocs != null && userDocs.Count() > 0)
                {
                    List<UserDocumentsModel> userDocListModel = new List<UserDocumentsModel>();
                    if (token.LocationID == 0)
                    {
                        token.LocationID = 101;
                    }
                    var location = _locationService.GetLocationOffsets(token.LocationID, token);
                    foreach (var item in userDocs)
                    {
                        UserDocumentsModel userDocModel = new UserDocumentsModel();

                        userDocModel.Id = item.Id;
                        userDocModel.UserId = item.UserId;
                        //userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadDocuments, item.UploadPath);
                        userDocModel.DocumentTitle = item.DocumentName;

                        userDocModel.Url = item.UploadPath;

                        if (item.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        {
                            userDocModel.DocumentTypeId = item.DocumentTypeId;
                            userDocModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == item.DocumentTypeId).FirstOrDefault().Type;
                            userDocModel.DocumentTypeName = userDocModel.DocumentTypeName.ToUpper() == "OTHER" ? userDocModel.DocumentTypeName + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeName;
                        }
                        else
                        {
                            userDocModel.DocumentTypeIdStaff = item.DocumentTypeIdStaff;
                            userDocModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == item.DocumentTypeIdStaff).FirstOrDefault().Type;
                            userDocModel.DocumentTypeNameStaff = userDocModel.DocumentTypeNameStaff.ToUpper() == "OTHER" ? userDocModel.DocumentTypeNameStaff + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeNameStaff;
                        }

                        userDocModel.Extenstion = Path.GetExtension(item.UploadPath);
                        userDocModel.CreatedDate = ConvertFromUtcTimeNew((DateTime)item.CreatedDate, location, token);
                        userDocModel.Key = item.Key;
                        userDocModel.Expiration = item.Expiration;
                        userDocModel.OtherDocumentType = item.OtherDocumentType;
                        userDocListModel.Add(userDocModel);
                    }
                    return new JsonModel()
                    {
                        data = userDocListModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public JsonModel UpdateProviderEducationalDocumentStatus(int id, bool documentstatus, TokenModel token)
        {
            try
            {
                UserDocuments userDoc = _context.UserDocuments.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (userDoc != null)
                {
                    userDoc.IsActive = documentstatus;
                    userDoc.UpdatedDate = DateTime.UtcNow;
                    userDoc.UpdatedBy = token.UserID;
                    _context.Update(userDoc);
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteDocumentDetails, AuditLogAction.Delete, null, token.UserID, "" + null, token);
                    _context.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DocumentStatusUpdate,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }
        public JsonModel GetProviderEducationalDocuments(int userId, DateTime? from, DateTime? to, TokenModel token)
        {
            try
            {
                List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.UserId == userId && a.IsProviderEducationalDocument == true && a.IsDeleted == false && (a.CreatedDate.Value.Date >= from && a.CreatedDate.Value.Date <= to)).OrderByDescending(a => a.CreatedDate).ToList();
                //string siteURL = _context.AppConfigurations.Where(a => a.Key == "SITE_URL" && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Value;
                if (userDocs != null && userDocs.Count() > 0)
                {
                    List<UserDocumentsModel> userDocListModel = new List<UserDocumentsModel>();
                    if (token.LocationID == 0)
                    {
                        token.LocationID = 101;
                    }
                    var location = _locationService.GetLocationOffsets(token.LocationID, token);
                    foreach (var item in userDocs)
                    {
                        UserDocumentsModel userDocModel = new UserDocumentsModel();

                        userDocModel.Id = item.Id;
                        userDocModel.UserId = userId;
                        userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadStaffDocuments, item.UploadPath);
                        userDocModel.DocumentTitle = item.DocumentName;
                        userDocModel.IsActive = item.IsActive;
                        //userDocModel.Url = item.UploadPath;

                        if (item.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        {
                            userDocModel.DocumentTypeId = item.DocumentTypeId;
                            userDocModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == item.DocumentTypeId).FirstOrDefault().Type;
                            userDocModel.DocumentTypeName = userDocModel.DocumentTypeName.ToUpper() == "OTHER" ? userDocModel.DocumentTypeName + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeName;
                        }
                        else
                        {
                            userDocModel.DocumentTypeIdStaff = item.DocumentTypeIdStaff;
                            userDocModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == item.DocumentTypeIdStaff).FirstOrDefault().Type;
                            userDocModel.DocumentTypeNameStaff = userDocModel.DocumentTypeNameStaff.ToUpper() == "OTHER" ? userDocModel.DocumentTypeNameStaff + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeNameStaff;
                        }

                        userDocModel.Extenstion = Path.GetExtension(item.UploadPath);
                        userDocModel.CreatedDate = ConvertFromUtcTimeNew((DateTime)item.CreatedDate, location, token);
                        userDocModel.Key = item.Key;
                        userDocModel.Expiration = item.Expiration;
                        userDocModel.OtherDocumentType = item.OtherDocumentType;
                        userDocListModel.Add(userDocModel);
                    }
                    return new JsonModel()
                    {
                        data = userDocListModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }


        public JsonModel GetprovidereductaionalDocumentsForPatientCheckin(int staffid, TokenModel token)
        {
            try
            {
                int userId;
                Staffs staffdetails = null;
                var hostingServer = _configuration.GetSection("HostingServer").Value;
                staffdetails = _context.Staffs.Where(a => a.Id == staffid).FirstOrDefault();
                userId = staffdetails.UserID;
                List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.UserId == userId && a.IsActive == true && a.IsProviderEducationalDocument == true && a.IsDeleted == false).ToList();
                //string siteURL = _context.AppConfigurations.Where(a => a.Key == "SITE_URL" && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Value;
                if (userDocs != null && userDocs.Count() > 0)
                {
                    List<UserDocumentsModel> userDocListModel = new List<UserDocumentsModel>();
                    if (token.LocationID == 0)
                    {
                        token.LocationID = 101;
                    }
                    var location = _locationService.GetLocationOffsets(token.LocationID, token);
                    foreach (var item in userDocs)
                    {
                        UserDocumentsModel userDocModel = new UserDocumentsModel();

                        userDocModel.Id = item.Id;
                        userDocModel.UserId = userId;
                        //userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadDocuments, item.UploadPath);
                        //userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadStaffDocuments, item.UploadPath);
                        userDocModel.Url = hostingServer + "/Documents/StaffDocuments/" + item.UploadPath;
                       
                        userDocModel.DocumentTitle = item.DocumentName;
                        userDocModel.IsActive = item.IsActive;
                        //userDocModel.Url = item.UploadPath;

                        if (item.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        {
                            userDocModel.DocumentTypeId = item.DocumentTypeId;
                            userDocModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == item.DocumentTypeId).FirstOrDefault().Type;
                            userDocModel.DocumentTypeName = userDocModel.DocumentTypeName.ToUpper() == "OTHER" ? userDocModel.DocumentTypeName + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeName;
                        }
                        else
                        {
                            userDocModel.DocumentTypeIdStaff = item.DocumentTypeIdStaff;
                            userDocModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == item.DocumentTypeIdStaff).FirstOrDefault().Type;
                            userDocModel.DocumentTypeNameStaff = userDocModel.DocumentTypeNameStaff.ToUpper() == "OTHER" ? userDocModel.DocumentTypeNameStaff + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeNameStaff;
                        }

                        userDocModel.Extenstion = Path.GetExtension(item.UploadPath);
                        userDocModel.CreatedDate = ConvertFromUtcTimeNew((DateTime)item.CreatedDate, location, token);
                        userDocModel.Key = item.Key;
                        userDocModel.Expiration = item.Expiration;
                        userDocModel.OtherDocumentType = item.OtherDocumentType;
                        userDocListModel.Add(userDocModel);
                    }
                    return new JsonModel()
                    {
                        data = userDocListModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
    }
}
