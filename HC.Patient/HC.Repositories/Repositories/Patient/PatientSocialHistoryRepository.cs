using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientSocialHistoryRepository : RepositoryBase<PatientSocialHistory>, IPatientSocialHistoryRepository
    {
        private HCOrganizationContext _context;
        public PatientSocialHistoryRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
