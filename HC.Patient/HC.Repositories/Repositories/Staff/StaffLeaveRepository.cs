using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class StaffLeaveRepository : RepositoryBase<StaffLeave>, IStaffLeaveRepository
    {
        private HCOrganizationContext _context;
        public StaffLeaveRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetStaffLeaveList<T>(SearchFilterModel staffLeaveFilterModel, int staffId,TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@PageNumber",staffLeaveFilterModel.pageNumber),
                new SqlParameter("@PageSize", staffLeaveFilterModel.pageSize),
                new SqlParameter("@SortColumn", staffLeaveFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", staffLeaveFilterModel.sortOrder),
                new SqlParameter("@SearchText", staffLeaveFilterModel.SearchText),
                new SqlParameter("@StaffId", staffId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffLeaveList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public StaffLeave GetAppliedLeaveByID(int StaffLeaveId, TokenModel token)
        {
            StaffLeave staffLeave  = _context.StaffLeave
                    .Where(x => x.Id == StaffLeaveId && x.IsDeleted == false && x.IsActive == true)
                    .FirstOrDefault();
            return staffLeave;
        }

    }
}
