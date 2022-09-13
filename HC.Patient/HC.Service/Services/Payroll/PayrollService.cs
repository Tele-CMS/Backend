using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.Payroll;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Payroll;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payroll
{
    public class PayrollService : BaseService, IPayrollService
    {
        private readonly IPayrollGroupRepository _payrollGroupRepository;
        private readonly IStaffRepository _staffRepository;
        public PayrollService(IPayrollGroupRepository payrollGroupRepository,IStaffRepository staffRepository)
        {
            _payrollGroupRepository = payrollGroupRepository;
            _staffRepository = staffRepository;
        }
        public JsonModel GetPayrollReport(string staffIds, int payrollGroupId, DateTime fromDate, DateTime toDate, TokenModel token)
        {
            return new JsonModel(_payrollGroupRepository.GetPayrollReportData<PayrollReportModel>(staffIds, payrollGroupId, fromDate, toDate, token).ToList(),StatusMessage.FetchMessage,(int)HttpStatusCodes.OK,"");
        }

        public JsonModel GetUsersByPayrollGroup(int payrollGroupId, TokenModel token)
        {
            List<MasterDropDown> staffList = _staffRepository.GetAll(x => x.PayrollGroupID == payrollGroupId && x.IsActive == true && x.IsDeleted ==false).Select(y => new MasterDropDown() {
                id = y.Id,
                value = y.FirstName + (y.MiddleName !=null? " "+y.MiddleName:"")+ " " +y.LastName
            }).ToList();
            return new JsonModel(staffList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
    }
}
