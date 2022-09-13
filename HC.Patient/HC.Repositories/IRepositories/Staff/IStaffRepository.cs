using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Entity.Payments;
using HC.Patient.Model;
using HC.Patient.Model.Payment;
using HC.Patient.Model.Staff;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IStaffRepository : IRepositoryBase<Staffs>
    {
        IQueryable<T> GetStaffByTags<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetStaff<T>(ListingFiltterModel listingFiltterModel, TokenModel token) where T : class, new();
        StaffProfileModel GetStaffProfileData(int staffId, TokenModel token);
        IQueryable<T> GetAssignedLocationsById<T>(int staffId, TokenModel token) where T : class, new();
        IQueryable<T> GetStaffHeaderData<T>(int staffId, TokenModel tokenModel) where T : class, new();
        Staffs GetStaffProfileDataByEmailAndOrgId(string email, TokenModel token);

        /// <summary>
        /// Get Assigned Locations By StaffId
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<AssignedLocationsModel> GetAssignedLocationsByStaffId(int staffId, TokenModel tokenModel);
        T CheckStaffProfile<T>(int staffId, TokenModel token) where T : class, new();
        Staffs GetStaffByUserId(int userId, TokenModel tokenModel);
        /// <summary>
        /// To get all staff user by name in organization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<T> GetStaffByName<T>(string name, TokenModel tokenModel) where T : class, new();

        IQueryable<Staffs> GetStaffByEmail(string email, TokenModel tokenModel);
        IQueryable<ManageFeesRefundsModel> GetStaffsFeesAndRefundRules(List<int> staffIds);
        void RemoveStaffsCancellationRules(List<int> staffIds);
        void AddStaffsCancellationRules(List<ProviderCancellationRules> rules);
        IQueryable<CancelationRuleModel> GetCancelationRules(List<int> staffIds);
    }
}
