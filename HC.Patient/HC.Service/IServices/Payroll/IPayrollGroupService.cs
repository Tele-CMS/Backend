using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Payroll
{
    public interface IPayrollGroupService:IBaseService
    {
        JsonModel SavePayrollGroup(PayrollGroupModel payrollGroup,TokenModel token);
        JsonModel GetPayrollGroupById(int Id, TokenModel token);
        JsonModel DeletePayrollGroup(int Id, TokenModel token);
        JsonModel GetPayrollGroupList(int pageNumber,int pageSize, string sortColumn,string sortOrder,TokenModel token);
        JsonModel GetPayrollGroupListForDropdown(TokenModel token);
    }
}
