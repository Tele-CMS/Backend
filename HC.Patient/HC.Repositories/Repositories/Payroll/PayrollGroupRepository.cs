using HC.Common;
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
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payroll
{
    public class PayrollGroupRepository:RepositoryBase<PayrollGroup>,IPayrollGroupRepository
    {
        private HCOrganizationContext _context;
        
        public PayrollGroupRepository(HCOrganizationContext context):base(context)
        {
            _context = context;            
        }

        public IQueryable<T> GetPayrollGroupList<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PRL_GetPayrollGroupList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetPayrollGroupListForDropdown<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@OrganizationId", token.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PRL_GetPayrollGroupDropdown.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPayrollReportData<T>(string staffIds, int payrollGroupId, DateTime fromDate, DateTime toDate, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@PayrollGroupId", payrollGroupId),
                                          new SqlParameter("@OrganizationID", token.OrganizationID),
                                          new SqlParameter("@LocationID", token.LocationID),
                                          new SqlParameter("@Offset",CommonMethods.GetTimezoneOffset(DateTime.Now,token)),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate)
            };
            return _context.ExecStoredProcedureListWithOutput<T>("RPT_GetPayroll", parameters.Length, parameters).AsQueryable();
        }
    }
}
