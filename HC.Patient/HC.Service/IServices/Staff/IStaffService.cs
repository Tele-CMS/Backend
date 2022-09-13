using HC.Model;
using HC.Patient.Entity;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IStaffService : IBaseService
    {
        JsonModel GetStaffByTags(ListingFiltterModel listingFiltterModel, TokenModel tokenModel);
        JsonModel GetStaffs(ListingFiltterModel listingFiltterModel, TokenModel token);
        JsonModel CreateUpdateStaff(Staffs staffs, TokenModel token);
        JsonModel GetStaffById(int id, TokenModel token);
        JsonModel DeleteStaff(int id, TokenModel token);
        JsonModel UpdateStaffActiveStatus(int staffId, bool isActive, TokenModel token);
        JsonModel GetDoctorDetailsFromNPI(string npiNumber,string enumerationType);
        JsonModel GetStaffProfileData(int staffId, TokenModel token);
        JsonModel GetAssignedLocationsById(int id, TokenModel token);
        JsonModel GetStaffHeaderData(int staffId, TokenModel tokenModel);
        /// <summary>
        /// To get staff by user id
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Staffs GetStaffByUserId(int userid, TokenModel token);
        JsonModel CheckStaffProfile(int staffId, TokenModel tokenModel);
        /// <summary>
        /// To get staff basic detail with respect to staff name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetStaffByName(string name, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetStaffByEmail(string email, TokenModel tokenModel);
        JsonModel GetStaffFeeSettings(string id, TokenModel token);
        JsonModel UpdateProviderTimeInterval(string id, TokenModel token);
        JsonModel GetCancelationRules(List<int> staffIds);
        JsonModel UpdateProviderUrgentCareStatus(int staffId, bool isUrgentCare, TokenModel token);
    }
}
