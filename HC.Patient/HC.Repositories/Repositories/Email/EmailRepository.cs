using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories
{
    public class EmailRepository : RepositoryBase<EmailLog>, IEmailRepository
    {
        private HCOrganizationContext _context;
        public EmailRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public EmailLog SaveEmailLog(EmailLog emailLog)
        {
            int result = 0;
            _context.EmailLogs.Update(emailLog);
            result = _context.SaveChanges();
            if (result > 0)
                return emailLog;
            else
                return null;
        }
    }
}
