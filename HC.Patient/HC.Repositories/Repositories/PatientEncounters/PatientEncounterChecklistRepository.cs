using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterChecklistRepository: RepositoryBase<PatientEncounterChecklist>, IPatientEncounterChecklistRepository
    {
        private HCOrganizationContext _context;
        public PatientEncounterChecklistRepository(HCOrganizationContext organizationContext): base(organizationContext)
        {
            _context = organizationContext;
        }
    }
}

