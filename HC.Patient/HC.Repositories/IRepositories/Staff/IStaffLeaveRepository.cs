using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IStaffLeaveRepository : IRepositoryBase<StaffLeave>
    {
        IQueryable<T> GetStaffLeaveList<T>(SearchFilterModel staffLeaveFilterModel, int staffId, TokenModel token) where T : class, new();
        StaffLeave GetAppliedLeaveByID(int StaffLeaveId, TokenModel token);
    }
}
