using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using System.Linq;
using System.Data.SqlClient;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class EdiGatewayRepository : RepositoryBase<EDIGateway>, IEdiGatewayRepository
    {
        private HCOrganizationContext _context;
        public EdiGatewayRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetEDIGateways<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize", searchFilterModel.pageSize),
                new SqlParameter("@SortColumn", searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", searchFilterModel.sortOrder ),
                new SqlParameter("@SearchText", searchFilterModel.SearchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetEDIGateways, parameters.Length, parameters).AsQueryable();
        }
    }
}
