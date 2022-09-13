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
    public class UserRoleRepository : RepositoryBase<UserRoles>, IUserRoleRepository
    {
        private HCOrganizationContext _context;
        public UserRoleRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetRoles<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize", searchFilterModel.pageSize),
                new SqlParameter("@SortColumn", searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", searchFilterModel.sortOrder ),
                new SqlParameter("@SearchText", searchFilterModel.SearchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetRoles, parameters.Length, parameters).AsQueryable();
        }
    }
}
