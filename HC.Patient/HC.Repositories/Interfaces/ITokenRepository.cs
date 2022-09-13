using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.Staff;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Interfaces
{
    public interface ITokenRepository//: IRepository<MasterDataModel>
    {
        User GetUserByUserName(string userName, int organizationID);
        Staffs GetDoctorByUserID(int UserID,TokenModel token);

        int GetDefaultLocationOfStaff(int UserID);
        List<UserLocationsModel> GetUserLocations(int UserID);
        List<UserLocationsModel> GetUserLocationsByStaff(int StaffID);
        Patients GetPaitentByUserID(int UserID, TokenModel token);
        SuperUser GetSupadminUserByUserName(string userName);
        DomainToken GetDomain(DomainToken domainToken);
        void ResetUserAccess(int userID);
        Staffs GetStaffByuserID(int UserID);
        List<AppConfigurationsModel> GetAppConfigurationByOrganization(int OrganizationID);
        int GetOrganizationIDByName(string businessName);
        NotificationModel GetLoginNotification(TokenModel token);
        Patients GetLastPatientByOrganization(TokenModel token);
        bool GetDefaultClient(int UserID);
        /// <summary>
        /// To get user detail and roles based on userid and organizationid
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        User GetUserById(TokenModel tokenModel);
        IQueryable<T> GetNotificationDetailsById<T>(int notificationId, TokenModel token) where T : class, new();
        ChatAndNotificaitonModel GetChatAndNotificationCount(int userId, TokenModel tokenModel);
        IQueryable<T> GetNotificationList<T>(CommonFilterModel commonFilterModel, TokenModel token) where T : class, new();

    }
}
