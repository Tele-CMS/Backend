using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.AuditLog
{
    public class AuditLogColumnRepository : RepositoryBase<AuditLogColumn>, IAuditLogColumnRepository
    {
        private HCOrganizationContext _context;
        public AuditLogColumnRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
