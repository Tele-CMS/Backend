using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterTemplatesRepository : RepositoryBase<MasterTemplates>, IMasterTemplatesRepository
    {
        private HCOrganizationContext _context;
        public MasterTemplatesRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetMasterTemplates<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize", searchFilterModel.pageSize),
                new SqlParameter("@SortColumn", searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", searchFilterModel.sortOrder ),
                new SqlParameter("@SearchText", searchFilterModel.SearchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetMasterTemplates.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
