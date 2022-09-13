using HC.Model;
using HC.Patient.Entity;
using HC.Repositories;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HC.Patient.Repositories.IRepositories.Payroll
{
    public interface IStaffPayrollRateForActivityRepository : IRepositoryBase<StaffPayrollRateForActivity>
    {
        IQueryable<T> SaveUpdateStaffPayrollRateForActivity<T>(XElement headerElement) where T : class, new();
        IQueryable<T> GetStaffPayRateOfActivity<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
