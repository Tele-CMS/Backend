using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class RoundingRuleRepository:RepositoryBase<MasterRoundingRules>, IRoundingRuleRepository
    {
        private HCOrganizationContext _context;
        public RoundingRuleRepository(HCOrganizationContext context):base(context)
        {
            this._context = context;
        }

        public List<RoundingRuleModelList> GetRoundingRules(string RuleName,int OrganizationID, int PageNumber, int PageSize, string SortColumn, string SortOrder)
        {
            return _context.ExecStoredProcedureListWithOutput<RoundingRuleModelList>("GetRoundingRules",
                     typeof(RoundingRuleModelList).GetProperties().Count(),
               new SqlParameter()
               {
                   ParameterName = "@RuleName",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = RuleName,
               },
               new SqlParameter()
               {
                   ParameterName = "@OrganizationID",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = OrganizationID,
               },
               new SqlParameter()
               {
                   ParameterName = "@PageNumber",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = PageNumber,
               },
               new SqlParameter()
               {
                   ParameterName = "@PageSize",
                   SqlDbType = SqlDbType.Int,
                   Direction = ParameterDirection.Input,
                   Value = PageSize,
               },
               new SqlParameter()
               {
                   ParameterName = "@SortColumn",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = SortColumn,
               },
               new SqlParameter()
               {
                   ParameterName = "@SortOrder",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = SortOrder,
               }).ToList();
        }
    }
}
