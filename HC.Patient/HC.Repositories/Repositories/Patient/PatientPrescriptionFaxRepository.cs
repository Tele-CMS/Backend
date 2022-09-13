using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientPrescriptionFaxRepository : RepositoryBase<Entity.PatientPrescriptionFaxLog>, IPatientPrescriptionFaxRepository
    {
        private HCOrganizationContext _context;
        public PatientPrescriptionFaxRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
    }
}
