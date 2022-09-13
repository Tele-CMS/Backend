using HC.Model;
using HC.Patient.Model.Staff;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IStaffLeaveService : IBaseService
    {
        JsonModel GetStaffLeaveList(SearchFilterModel staffLeaveFilterModel,int staffId, TokenModel token);
        JsonModel AddUpdateStaffAppliedLeave(StaffLeaveModel staffLeaveModel, TokenModel token);
        JsonModel GetAppliedStaffLeaveById(int staffLeaveId, TokenModel token);
        JsonModel DeleteAppliedLeave(int staffLeaveId, TokenModel token);
        JsonModel UpdateLeaveStatus(List<LeaveStatusModel> leaveStatusModel, TokenModel token);
    }
}
