using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payroll
{
    public class StaffPayrollRateForActivityRepository : RepositoryBase<StaffPayrollRateForActivity>, IStaffPayrollRateForActivityRepository
    {
        private HCOrganizationContext _context;
        public StaffPayrollRateForActivityRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> SaveUpdateStaffPayrollRateForActivity<T>(XElement headerElement) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Data", headerElement.ToString()) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAR_SaveUpdateStaffPayrollRateForActivity.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetStaffPayRateOfActivity<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffId",searchFilterModel.StaffId),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAR_GetStaffPayRateOfActivity, parameters.Length, parameters).AsQueryable();
        }
    }
}
