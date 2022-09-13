using HC.Model;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.StaffAvailability;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.StaffAvailability
{
    public class StaffAvailabilityRepository : RepositoryBase<Entity.StaffAvailability>, IStaffAvailabilityRepository
    {
        private HCOrganizationContext _context;
        public StaffAvailabilityRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetAllDatesForLeaveDateRange<T>(string staffIds, DateTime? startDate, DateTime? endDate) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffIds", staffIds),
                                          new SqlParameter("@StartDate", startDate),
                                          new SqlParameter("@EndDate", endDate),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetAllDatesForLeaveDateRange, parameters.Length, parameters).AsQueryable();
        }
    }
}
