using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterNotesRepository : RepositoryBase<PatientEncounterNotes>, IPatientEncounterNotesRepository
    {
        private HCOrganizationContext _context;
        public PatientEncounterNotesRepository(HCOrganizationContext organizationContext) : base(organizationContext)
        {
            _context = organizationContext;
        }
    }
}
