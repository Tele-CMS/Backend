using HC.Model;
using HC.Service.Interfaces;
using System;

namespace HC.Patient.Service.IServices.Payroll
{
    public interface IPayrollService:IBaseService
    {
        JsonModel GetPayrollReport(string staffIds, int payrollGroupId, DateTime fromDate, DateTime toDate, TokenModel token);
        JsonModel GetUsersByPayrollGroup(int payrollGroupId, TokenModel token);
    }
}
