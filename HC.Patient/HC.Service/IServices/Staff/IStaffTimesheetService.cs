using HC.Model;
using HC.Patient.Model.Staff;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Staff
{
    public interface IStaffTimesheetService :IBaseService
    {
        JsonModel GetStaffTimesheetDataSheetView(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token);
        JsonModel GetStaffTimesheetDataTabularView(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token);
        JsonModel UpdateStaffTimesheet(List<StaffTimesheetModel> timesheetModel, TokenModel token);
        JsonModel SaveUserTimesheetDetails(StaffDetailedTimesheetModel staffDetailedTimesheet, TokenModel token);
        JsonModel DeleteUserTimesheetDetails(int Id, TokenModel token);
        JsonModel GetUserTimesheetDetails(int Id, TokenModel token);
        JsonModel UpdateUserTimesheetStatus(List<StaffDetailedTimesheetModel> staffDetailedTimesheet, TokenModel token);
        JsonModel SubmitUserTimesheet(string Ids, TokenModel token);
    }
}
