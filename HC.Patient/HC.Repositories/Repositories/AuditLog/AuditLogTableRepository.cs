using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.AuditLog
{
    public class AuditLogTableRepository : RepositoryBase<AuditLogTable>, IAuditLogTableRepository
    {
        private HCOrganizationContext _context;
        public AuditLogTableRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
