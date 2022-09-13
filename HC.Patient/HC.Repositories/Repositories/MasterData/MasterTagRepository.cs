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
    public class MasterTagRepository:RepositoryBase<MasterTags>, IMasterTagRepository
    {
        private HCOrganizationContext _context;
        public MasterTagRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetMasterTagList<T>(SearchFilterModel searchFilterModel,TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
            new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
            new SqlParameter("@PageSize",searchFilterModel.pageSize),
            new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
            new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetMasterTags, parameters.Length, parameters).AsQueryable();

        }
    }
}
