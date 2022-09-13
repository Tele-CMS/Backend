using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterServiceCodesRepository:RepositoryBase<PatientEncounterServiceCodes>, IPatientEncounterServiceCodesRepository
    {
        private HCOrganizationContext _context;

        public PatientEncounterServiceCodesRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
