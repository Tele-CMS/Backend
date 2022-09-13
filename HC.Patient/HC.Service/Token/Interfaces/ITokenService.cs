using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.Staff;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.Token.Interfaces
{
    public interface ITokenService : IBaseService
    {
        User GetUserByUserName(string userName, int organizationID);
        Staffs GetDoctorByUserID(int UserID, TokenModel token);
        int GetDefaultLocationOfStaff(int UserID);
        List<UserLocationsModel> GetUserLocations(int UserID);
        Patients GetPaitentByUserID(int UserID, TokenModel tokenModel);
        SuperUser GetSupadminUserByUserName(string userName);
        List<AppConfigurationsModel> GetAppConfigurationByOrganizationByID(TokenModel tokenModel);
        DomainToken GetDomain(DomainToken domainToken);
        JsonModel AuthenticateSuperUser(ApplicationUser applicationUser, TokenModel token);
        //JsonModel AuthenticateAgency(ApplicationUser applicationUser, TokenModel token);
        //JsonModel SaveUserScurityQuestion(SecurityQuestionListModel questionListModel, TokenModel token);
        //JsonModel CheckQuestionAnswer(SecurityQuestionModel securityQuestion, TokenModel token);
        int GetOrganizationIDByName(string businessName);
        OrganizationModel GetOrganizationById(int id, TokenModel token);
        OrganizationModel GetOrganizationDetailsByBusinessName(string businessName);
        NotificationModel GetLoginNotification(TokenModel tokenModel);
        Patients GetLastPatientByOrganization(TokenModel tokenModel);
        bool GetDefaultClient(int UserID);
        //notification
        JsonModel GetPatientNotificationList(int PatientID, int pageNumber, int pageSize, TokenModel tokenModel);
        JsonModel GetPatientPortalNotificationList(int PatientID, int pageNumber, int pageSize, TokenModel tokenModel);
        JsonModel GetChatAndNotificationCount(int userId, TokenModel tokenModel);
        JsonModel GetNotificationDetailsById(int notificationId, TokenModel tokenModel);
        JsonModel GetNotificationList(CommonFilterModel commonFilterModel, TokenModel tokenModel);
        JsonModel ReadNotification(TokenModel tokenModel);
        JsonModel ReadChatAndAllNotification(int patientId, bool isChatMessage, TokenModel tokenModel);
        JsonModel ReadChatAndAllNotificationForPatient(int patientId, TokenModel tokenModel);

        /// <summary>
        /// To get organization detail with respect to oranization id
        /// </summary>
        /// <param name="id">current selected organization id</param>
        /// <param name="token">TokenModel</param>
        /// <returns>Organization Entity</returns>
        Organization GetOrganizationByOrgId(int id, TokenModel token);
        /// <summary>
        /// To get user detail based on specific userid 
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        User GetUserById(TokenModel tokenModel);
        bool SaveUpdateDeviceToken(User user);
    }
}
