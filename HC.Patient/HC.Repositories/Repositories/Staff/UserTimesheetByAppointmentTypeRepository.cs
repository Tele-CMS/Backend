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
    public class UserTimesheetByAppointmentTypeRepository :RepositoryBase<UserTimesheetByAppointmentType>, IUserTimesheetByAppointmentTypeRepository
    {
        private HCOrganizationContext _context;
        public UserTimesheetByAppointmentTypeRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetStaffTimesheetDataSheetView<T>(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@WeekValue", weekValue),
                                          new SqlParameter("@StartDate", startDate),
                                          new SqlParameter("@EndDate", endDate)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffTimesheetDataSheetView, parameters.Length, parameters).AsQueryable();
        }

        public int GetTimesheetStatusId(string status, TokenModel token)
        {
            return _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "timesheetstatus" && a.OrganizationID == token.OrganizationID && a.GlobalCodeName.ToLower() == status.ToLower()).OrderBy(a => a.DisplayOrder).FirstOrDefault().Id;
        }
    }
}
