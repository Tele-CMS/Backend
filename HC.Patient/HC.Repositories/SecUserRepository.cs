using HC.Patient.Repositories.Interfaces;
using HC.Repositories;
using HC.Patient.Entity;
using HC.Patient.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories
{
    public class SecUserRepository : RepositoryBase<SecUser>, ISecUserRepository
    {
        private readonly HCPatientContext _context;
        public SecUserRepository(HCPatientContext context) : base(context) {
            this._context = context;
        }
    }
}
