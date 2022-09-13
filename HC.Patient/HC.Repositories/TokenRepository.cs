using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly HCOrganizationContext _context;
        private readonly HCMasterContext _masterContext;
        private readonly IConfiguration _configuration;

        public TokenRepository(HCOrganizationContext context, HCMasterContext masterContext, IConfiguration configuration)
        {
            this._context = context;
            this._masterContext = masterContext;
            _configuration = configuration;
        }

        /// <summary>
        /// get user detail for their name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByUserName(string userName, int organizationID)
        {
            try
            {
                return _context.User.Where(m => m.UserName.ToUpper() == userName.ToUpper() && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == organizationID).Include(x => x.UserRoles).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User GetUserById(TokenModel tokenModel)
        {
            try
            {
                return _context.User.Where(m => m.Id == tokenModel.UserID && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).Include(x => x.UserRoles).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// get user detail for their name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public SuperUser GetSupadminUserByUserName(string userName)
        {
            try
            {
                return _masterContext.SuperUser.Where(m => m.UserName.ToUpper() == userName.ToUpper() && m.IsActive == true && m.IsDeleted == false && m.IsBlock == false).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// get doctor by userid
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public Staffs GetDoctorByUserID(int UserID ,TokenModel token)
        {
            try
            {
                var hostingServer = _configuration.GetSection("HostingServer").Value;
                Staffs staffs = _context.Staffs.Where(m => m.UserID == UserID && m.IsActive == true && m.IsBlock == false && m.IsDeleted == false).FirstOrDefault();
                if (staffs != null && !string.IsNullOrEmpty(staffs.PhotoPath) && !string.IsNullOrEmpty(staffs.PhotoThumbnailPath))
                {
                    //staffs.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, staffs.PhotoPath);
                    staffs.PhotoPath = hostingServer + "/Images/StaffPhotos/thumb/" + staffs.PhotoPath;
                    //staffs.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffs.PhotoThumbnailPath);
                    staffs.PhotoThumbnailPath = hostingServer + "/Images/StaffPhotos/thumb/" + staffs.PhotoThumbnailPath;
                }
                return staffs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetDefaultLocationOfStaff(int UserID)
        {
            try
            {

                //StaffLocation location = _context.User.
                //      Join(_context.Staffs, u => u.Id, uir => uir.UserID,
                //      (u, uir) => new { u, uir }).
                //      Join(_context.StaffLocation, r => r.uir.Id, ro => ro.StaffId, (r, ro) => new { r, ro })
                //      .Where(m => m.r.uir.UserID == UserID && m.ro.IsDefault == true)
                //      .Select(m => new StaffLocation
                //      {
                //          LocationID = m.ro.LocationID
                //      }).FirstOrDefault();
                //return location != null ? location.LocationID : 0;

                //get staff ID
                int staffID = _context.Staffs.Where(l => l.UserID == UserID).FirstOrDefault().Id;

                //get default location of staff
                StaffLocation staffLocation = _context.StaffLocation.Where(a => a.StaffId == staffID && a.IsDefault == true && a.Location.IsDeleted == false && a.Location.IsActive == true).FirstOrDefault();

                if (staffLocation != null)
                {
                    return staffLocation.LocationID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserLocationsModel> GetUserLocations(int UserID)
        {
            try
            {
                List<UserLocationsModel> userLocations = new List<UserLocationsModel>();
                //get staff ID
                int staffID = _context.Staffs.Where(l => l.UserID == UserID && l.IsActive == true && l.IsDeleted == false).FirstOrDefault().Id;

                //get default location of staff
                List<Location> userLocation = _context.StaffLocation.Where(a => a.StaffId == staffID && a.Location.IsDeleted==false && a.Location.IsActive==true).Select(a => a.Location).ToList();

                foreach (var item in userLocation)
                {
                    UserLocationsModel loc = new UserLocationsModel();
                    loc.Id = item.Id;
                    loc.LocationName = item.LocationName;
                    loc.OrganizationId = item.OrganizationID;
                    userLocations.Add(loc);
                }
                return userLocations;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;
            }
        }

        public List<UserLocationsModel> GetUserLocationsByStaff(int StaffID)
        {
            try
            {

                return _context.StaffLocation
                      .Where(x => x.StaffId == StaffID)
                      .Join(_context.Location,
                      SL => SL.LocationID,
                      L => L.Id,
                      (SL, L) => new UserLocationsModel
                      {
                          Id = SL.LocationID,
                          LocationName = L.LocationName,
                          OrganizationId = L.OrganizationID
                      }).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;
            }
        }

        public Patients GetPaitentByUserID(int UserID,TokenModel token)
        {
            try
            {

                Patients patients = _context.Patients.Where(m => m.UserID == UserID && m.IsActive == true && m.IsBlock == false && m.IsDeleted == false).FirstOrDefault();
                if (patients != null && !string.IsNullOrEmpty(patients.PhotoPath) && !string.IsNullOrEmpty(patients.PhotoThumbnailPath))
                {
                    patients.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patients.PhotoPath);
                    patients.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patients.PhotoThumbnailPath);
                }
                return patients;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DomainToken GetDomain(DomainToken domainToken)
        {
            try
            {
                DomainToken dbDomainData = new DomainToken();                
                MasterOrganization masterOrg = _masterContext.MasterOrganization.Where(a => a.BusinessName.ToLower() == domainToken.BusinessToken.ToLower() && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

                if (masterOrg != null)
                {
                    OrganizationDatabaseDetail orgDatabaseDetails = _masterContext.OrganizationDatabaseDetail.Where(a => a.Id == masterOrg.DatabaseDetailId).FirstOrDefault();
                    dbDomainData.BusinessToken = CommonMethods.Encrypt(masterOrg.BusinessName);
                    dbDomainData.OrganizationId = masterOrg.Id;
                    //dbDomainData.ServerName = orgDatabaseDetails.ServerName;
                    //dbDomainData.DatabaseName = orgDatabaseDetails.DatabaseName;
                    //dbDomainData.UserName = orgDatabaseDetails.UserName;
                    //dbDomainData.Password = orgDatabaseDetails.Password;
                    return dbDomainData;
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


        /// <summary>
        /// To reset user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenModel"></param>
        public void ResetUserAccess(int userID)
        {
            try
            {
                var user = _context.User.Where(p => p.Id == userID && p.IsActive == true && p.IsBlock == false && p.IsDeleted == false).FirstOrDefault();
                user.AccessFailedCount = 0;
                user.BlockDateTime = null;
                user.IsBlock = false;
                _context.User.Update(user);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Staffs GetStaffByuserID(int UserID)
        {
            try
            {
                return _context.Staffs
                    .Include("StaffLocation")
                    .Where(m => m.UserID == UserID && m.IsActive == true && m.IsBlock == false && m.IsDeleted == false)
                    .FirstOrDefault();
            }
            catch (Exception)
            {

                return null;
            }
        }


        public List<AppConfigurationsModel> GetAppConfigurationByOrganization(int OrganizationID)
        {
            return _context.AppConfigurations
                .Where(m => m.OrganizationID == OrganizationID)
                .Select(x => new AppConfigurationsModel
                {
                    ConfigType = x.ConfigType,
                    Id = x.Id,
                    Key = x.Key,
                    Label = x.Label,
                    Value = x.Value
                }).ToList();
        }

        public int GetOrganizationIDByName(string businessName)
        {
            return _masterContext.MasterOrganization.Where(a => a.BusinessName == businessName && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id).FirstOrDefault();
        }

        public NotificationModel GetLoginNotification(TokenModel token)
        {
            SqlParameter[] parameters = {new SqlParameter("@UserID", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutputForNotificationInfo(SQLObjects.ADM_GetNotificationDashboard.ToString(), parameters.Length, parameters);
        }

        public Patients GetLastPatientByOrganization(TokenModel token)
        {
            try
            {
                Patients patients = _context.Patients.Where(m => m.LocationID == token.LocationID && m.OrganizationID == token.OrganizationID && m.IsActive == true && m.IsDeleted == false).LastOrDefault();
                if (patients != null && !string.IsNullOrEmpty(patients.PhotoPath) && !string.IsNullOrEmpty(patients.PhotoThumbnailPath))
                {
                    patients.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientPhotos, patients.PhotoPath);
                    patients.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patients.PhotoThumbnailPath);
                }
                return patients;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public bool GetDefaultClient(int UserID)
        {
            try
            {
                bool OpenDefaultClient = _context.User.Where(U => U.Id == UserID).FirstOrDefault().OpenDefaultClient == true ? true : false;
                return OpenDefaultClient;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public IQueryable<T> GetNotificationList<T>(CommonFilterModel commonFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@UserID", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                            new SqlParameter("@PageNumber", commonFilterModel.pageNumber),
                                            new SqlParameter("@PageSize", commonFilterModel.pageSize),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetNotificationList, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetNotificationDetailsById<T>(int notificationId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@NotificationId", notificationId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_NotificationDetailsById.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public NotificationModel GetLoginNotification(int userID, TokenModel token)
        {
            SqlParameter[] parameters = {new SqlParameter("@UserID", userID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutputForNotificationInfo(SQLObjects.ADM_GetNotification.ToString(), parameters.Length, parameters);
        }

        public ChatAndNotificaitonModel GetChatAndNotificationCount(int userId, TokenModel tokenModel)
        {
            SqlParameter[] parameters = {new SqlParameter("@UserId", userId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutputForChatAndNotification(SQLObjects.PAT_TotalNotificationAndChatNotificationCount.ToString(), parameters.Length, parameters);
        }

    }
}
