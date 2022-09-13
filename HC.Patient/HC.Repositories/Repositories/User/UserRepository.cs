using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.User;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.User
{
    public class UserRepository : RepositoryBase<Entity.User>, IUserRepository
    {
        private HCOrganizationContext _context;
        public UserRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetFilteredStaff<T>(string @LocationIds, string RoleIds, string SearchKey, string StartWith, string Tags, string sortColumn, string sortOrder, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@LocationIds",@LocationIds),
                                          new SqlParameter("@RoleIds",RoleIds),
                                          new SqlParameter("@SearchKey",SearchKey),
                                          new SqlParameter("@StartWith",StartWith),
                                          new SqlParameter("@Tags",Tags),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffUsers.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public Entity.User GetUserByID(int UserID)
        {
            try
            {
                return _context.User.Where(x => x.Id == UserID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUser(Entity.User user)
        {
            try
            {
                _context.User.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ChangePasscodeSettings(int userId, bool isPasscodeEnable)
        {
            try
            {
                var user = _context.User.Where(z => z.Id == userId).FirstOrDefault();
                user.IsPasscodeEnable = isPasscodeEnable;
                _context.User.Update(user);
                _context.SaveChanges();
                return isPasscodeEnable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T GetApnToken<T>(int AppointmentId, int RoleId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@appointmentId",AppointmentId),
                                          new SqlParameter("@RoleId",RoleId)
            };
            return _context.ExecStoredProcedureWithOutput<T>(SQLObjects.USR_GetApnToken.ToString(), parameters.Length, parameters);
        }
    }
}
