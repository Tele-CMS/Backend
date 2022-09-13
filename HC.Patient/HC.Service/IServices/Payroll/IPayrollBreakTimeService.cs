using HC.Model;
using HC.Patient.Model.PayrollBreaktime;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Payroll
{
    public interface IPayrollBreaktimeService: IBaseService     
    {
        JsonModel SavePayrollBreakTime(PayrollBreaktimeModel payrollBreaktimeDetailsModel, TokenModel token);
        JsonModel GetPayrollBreakTimeById(int Id, TokenModel token);
        JsonModel DeletePayrollBreakTime(int Id, TokenModel token);
        JsonModel GetPayrollBreakTimeList(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
    }
}
