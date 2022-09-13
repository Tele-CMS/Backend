using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payroll
{
    public interface IPayrollGroupRepository :IRepositoryBase<PayrollGroup>
    {
        IQueryable<T> GetPayrollGroupList<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new();
        IQueryable<T> GetPayrollReportData<T>(string staffIds,int payrollGroupId,DateTime fromDate,DateTime toDate,TokenModel token) where T : class, new();
        IQueryable<T> GetPayrollGroupListForDropdown<T>(TokenModel token) where T : class, new();
        
    }
}
