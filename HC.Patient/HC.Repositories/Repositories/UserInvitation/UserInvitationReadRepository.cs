using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HC.Patient.Repositories.Repositories
{
    public class UserInvitationReadRepository : RepositoryBase<Entity.UserInvitation>, IUserInvitationReadRepository
    {
        private HCOrganizationContext _context;
        public UserInvitationReadRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public UserInvitation GetUserInvitationByEmailAndOrganizationId(string email, int orgId)
        {
            return _context.UserInvitation
                .Where(u => u.Email.ToLower() == email.ToLower()
                && u.OrganizationId == orgId
                && u.IsDeleted == false)
                .FirstOrDefault();
        }

        public List<UserInvitationResponseModel> GetUserInvitationList(TokenModel tokenModel, LocationModel locationModel, UserInvitationFilterModel userInvitationFilterModel)
        {
            if (string.IsNullOrEmpty(userInvitationFilterModel.sortColumn))
                userInvitationFilterModel.sortColumn = "InvitationId";

            if (string.IsNullOrEmpty(userInvitationFilterModel.sortOrder) || userInvitationFilterModel.sortOrder == "asc")
                userInvitationFilterModel.sortOrder = "true";
            else
                userInvitationFilterModel.sortOrder = "false";

            var userInvitationResponseModels = (CommonMethods.OrderByField((from inv in _context.UserInvitation
                                                                            join org in _context.Organization on inv.OrganizationId equals org.Id
                                                                            join loc in _context.Location on inv.LocationId equals loc.Id
                                                                            where inv.OrganizationId == tokenModel.OrganizationID && inv.IsDeleted == false && inv.FirstName.ToLower().Contains(userInvitationFilterModel.SearchText.ToLower())
                                                                            orderby ("" + userInvitationFilterModel.sortColumn + " " + userInvitationFilterModel.sortOrder + "")
                                                                            select new UserInvitationResponseModel()
                                                                            {
                                                                                InvitationId = inv.Id,
                                                                                FirstName = inv.FirstName,
                                                                                MiddleName = inv.MiddleName,
                                                                                LastName = inv.LastName,
                                                                                FullName = string.IsNullOrEmpty(inv.MiddleName) ? (string.IsNullOrEmpty(inv.LastName) ? inv.FirstName : inv.FirstName + " " + inv.LastName) : inv.FirstName + " " + inv.MiddleName + " " + inv.LastName,
                                                                                Email = inv.Email,
                                                                                Phone = inv.Phone,
                                                                                Location = loc.LocationName,
                                                                                Organization = org.OrganizationName,
                                                                                OrganizationId = inv.OrganizationId,
                                                                                LocationId = inv.LocationId,
                                                                                CreatedDate = (DateTime)inv.CreatedDate,
                                                                                InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset,locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
                                                                                InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()
                                                                            }).Distinct(),
                                                      userInvitationFilterModel.sortColumn, Convert.ToBoolean(userInvitationFilterModel.sortOrder)));
            var result = userInvitationResponseModels.Skip((userInvitationFilterModel.pageNumber - 1) * userInvitationFilterModel.pageSize)
                                              .Take(userInvitationFilterModel.pageSize).ToList();
            result[0].TotalRecords = userInvitationResponseModels.ToList().Count();
            return result;
        }

        public UserInvitation GetUserInvitationByIdAndOrganizationId(int id, TokenModel tokenModel)
        {
            return _context.UserInvitation
                .Where(u => u.Id == id
                && u.OrganizationId == tokenModel.OrganizationID
                && u.IsDeleted == false)
                .FirstOrDefault();
        }
        public Entity.User CheckUserNameExistance(string username, TokenModel tokenModel)
        {
            return _context.User
                .Where(u => u.UserName.ToLower() == username.ToLower()
                && u.OrganizationID == tokenModel.OrganizationID)
                .FirstOrDefault();
        }
    }
}
