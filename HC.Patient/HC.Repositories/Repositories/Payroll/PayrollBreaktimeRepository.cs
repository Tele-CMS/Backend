using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Payroll
{
    public class PayrollBreaktimeRepository : RepositoryBase<PayrollBreakTime>, IPayrollBreaktimeRepository
    {
        private HCOrganizationContext _context;
        public PayrollBreaktimeRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
