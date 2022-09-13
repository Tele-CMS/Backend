using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IUserTimesheetByAppointmentTypeRepository :IRepositoryBase<UserTimesheetByAppointmentType>
    {
        IQueryable<T> GetStaffTimesheetDataSheetView<T>(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token) where T : class,new();
        int GetTimesheetStatusId(string status, TokenModel token);
    }
}
