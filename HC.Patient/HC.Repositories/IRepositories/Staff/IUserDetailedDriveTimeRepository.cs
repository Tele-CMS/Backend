using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IUserDetailedDriveTimeRepository :IRepositoryBase<UserDetailedDriveTime>
    {
        IQueryable<T> GetStaffTimesheetDataTabularView<T>(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token) where T : class, new();
        IQueryable<T> UpdateUserTimesheetStatus<T>(string timeSheetXML,TokenModel token) where T : class, new();
    }
}
