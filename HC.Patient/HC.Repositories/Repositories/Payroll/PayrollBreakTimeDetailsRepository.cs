using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Payroll
{
    public class PayrollBreakTimeDetailsRepository : RepositoryBase<PayrollBreakTimeDetails>, IPayrollBreakTimeDetailsRepository
    {
        private HCOrganizationContext _context;
        public PayrollBreakTimeDetailsRepository(HCOrganizationContext context) :base(context)
        {
            _context = context;
        }
    }
}
