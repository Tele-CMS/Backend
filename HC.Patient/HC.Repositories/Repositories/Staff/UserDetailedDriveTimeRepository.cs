using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class UserDetailedDriveTimeRepository :RepositoryBase<UserDetailedDriveTime>, IUserDetailedDriveTimeRepository
    {
        private HCOrganizationContext _context;
        public UserDetailedDriveTimeRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetStaffTimesheetDataTabularView<T>(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@WeekValue", weekValue),
                                          new SqlParameter("@StartDate", startDate),
                                          new SqlParameter("@EndDate", endDate)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffTimesheetDataTabularView, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> UpdateUserTimesheetStatus<T>(string timeSheetXML, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@TimesheetXML", timeSheetXML),
                                          new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_UpdateTimesheetStatus, parameters.Length, parameters).AsQueryable();
        }
    }
}
