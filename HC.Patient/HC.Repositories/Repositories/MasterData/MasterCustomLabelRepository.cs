using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
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
    public class MasterCustomLabelRepository:RepositoryBase<MasterCustomLabels>, IMasterCustomLabelRepository
    {
        private HCOrganizationContext _context;
        public MasterCustomLabelRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetCustomLabel<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T:class,new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize", searchFilterModel.pageSize),
                new SqlParameter("@SortColumn", searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", searchFilterModel.sortOrder ),
                new SqlParameter("@SearchText", searchFilterModel.SearchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetCustomLabels, parameters.Length, parameters).AsQueryable();
        }
    }
}
