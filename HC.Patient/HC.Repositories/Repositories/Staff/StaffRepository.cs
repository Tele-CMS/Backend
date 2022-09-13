using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Entity.Payments;
using HC.Patient.Model;
using HC.Patient.Model.Payment;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class StaffRepository : RepositoryBase<Staffs>, IStaffRepository
    {
        private HCOrganizationContext _context;
        public StaffRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<T> GetStaffByTags<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Tags", listingFiltterModel.Tags),
                                          new SqlParameter("@StartWith",listingFiltterModel.StartWith),
                                          new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@LocationIDs",listingFiltterModel.LocationIDs),
                                          new SqlParameter("@IsActive",listingFiltterModel.IsActive),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffByTags.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetStaff<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@LocationIds",listingFiltterModel.LocationIDs),
                                          new SqlParameter("@RoleIds",listingFiltterModel.RoleIds),
                                          new SqlParameter("@SearchKey",listingFiltterModel.SearchKey),
                                          new SqlParameter("@StartWith",listingFiltterModel.StartWith),
                                          new SqlParameter("@Tags",listingFiltterModel.Tags),
                                          new SqlParameter("@SortColumn",listingFiltterModel.sortColumn),
                                          new SqlParameter("@SortOrder",listingFiltterModel.sortOrder),
                                          new SqlParameter("@PageNumber",listingFiltterModel.pageNumber),
                                          new SqlParameter("@PageSize",listingFiltterModel.pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffUsers.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public StaffProfileModel GetStaffProfileData(int staffId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffId", staffId),
                                          new SqlParameter("@OrganizationId",token.OrganizationID)
            };
            return _context.ExecStoredProcedureForStaffProfileData(SQLObjects.STF_GetProfileData, parameters.Length, parameters);
        }
        public Staffs GetStaffProfileDataByEmailAndOrgId(string email, TokenModel token)
        {
            return _context.Staffs.Where(s => s.Email == email && s.OrganizationID == token.OrganizationID && s.IsDeleted == false).FirstOrDefault();
        }
        public IQueryable<T> GetAssignedLocationsById<T>(int staffId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@staffId",staffId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetAssignedLocationsById.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<AssignedLocationsModel> GetAssignedLocationsByStaffId(int staffId, TokenModel tokenModel)
        {
            var assignedLocations = (from loc in _context.Location
                                     join sLoc in _context.StaffLocation on loc.Id equals sLoc.LocationID
                                     where sLoc.OrganizationID == tokenModel.OrganizationID && sLoc.StaffId == staffId
                                     && loc.IsDeleted == false && loc.IsActive == true
                                     orderby sLoc.IsDefault descending
                                     select new AssignedLocationsModel()
                                     {
                                         LocationId = Common.CommonMethods.Encrypt(loc.Id.ToString()),
                                         Location = loc.LocationName,
                                         Address = loc.Address,
                                         IsDefault = sLoc.IsDefault,
                                         Latitude = loc.Latitude,
                                         Longitude = loc.Longitude
                                     }).AsQueryable();
            return assignedLocations;
        }

        public IQueryable<T> GetStaffHeaderData<T>(int staffId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@staffId",staffId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetHeaderData.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public T CheckStaffProfile<T>(int staffId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@staffId",staffId)
            };
            return _context.ExecStoredProcedureWithOutput<T>(SQLObjects.STF_CheckStaffProfile.ToString(), parameters.Length, parameters);
        }
        public Staffs GetStaffByUserId(int userId, TokenModel tokenModel)
        {
            return _context.Staffs.Where(x => x.UserID == userId && x.OrganizationID == tokenModel.OrganizationID && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
        }
        public IQueryable<T> GetStaffByName<T>(string name, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@name",name)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffUsersByName.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<Staffs> GetStaffByEmail(string email, TokenModel tokenModel)
        {
            return _context.Staffs.Where(s => s.Email == email && s.OrganizationID == tokenModel.OrganizationID && s.IsActive == true && s.IsDeleted == false).AsQueryable();
        }

        public IQueryable<ManageFeesRefundsModel> GetStaffsFeesAndRefundRules(List<int> staffIds)
        {

            return _context.Staffs.Where(x => staffIds.Contains(x.Id)).Include(x => x.ProviderCancellationRules)
                .Select(s => new ManageFeesRefundsModel()
                {
                    CancelationRules = s.ProviderCancellationRules != null ? s.ProviderCancellationRules.Select(cr => new CancelationRuleModel() { 
                            RefundPercentage = cr.RefundPercentage,
                            UptoHours = cr.UptoHour
                    }).ToList() : new List<CancelationRuleModel>(),
                    F2fFee = s.FTFpayRate,
                    FolowupDays = s.FollowUpDays,
                    FolowupFees = s.FollowUpPayRate,
                    NewOnlineFee = s.PayRate,
                    UrgentcareFee=s.UrgentCarePayRate,
                    Providers = staffIds

                });
        }


        public IQueryable<CancelationRuleModel> GetCancelationRules(List<int> staffIds)
        {

            var rules = _context.ProviderCancellationRules.Where(x => staffIds.Any(s => s == x.StaffId));

            return rules.Select(s => new CancelationRuleModel()
            {
                RefundPercentage = s.RefundPercentage,
                UptoHours = s.UptoHour,
                StaffId = s.StaffId
            });

        }



        public void RemoveStaffsCancellationRules(List<int> staffIds)
        {
            if (staffIds != null && staffIds.Count > 0)
            {
                var rules = _context.ProviderCancellationRules.Where(x => staffIds.Contains(x.StaffId)).ToList();
                if (rules.Count > 0)
                {
                    _context.ProviderCancellationRules.RemoveRange(rules);
                }
            }
        }

        public void AddStaffsCancellationRules(List<ProviderCancellationRules> rules)
        {
            if(rules != null && rules.Count>0)
            _context.ProviderCancellationRules.AddRange(rules);
        }
    }
}
