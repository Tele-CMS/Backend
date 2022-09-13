using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Repositories;
using HC.Patient.Repositories.IRepositories.PatientMedicalFamilyHistories;

namespace HC.Patient.Repositories.Repositories.PatientMedicalFamilyHistories
{
    public class PatientMedicalFamilyHistoryRepository : RepositoryBase<PatientMedicalFamilyHistory>, IPatientMedicalFamilyHistoryRepository
    {
        private HCOrganizationContext _context;
        public PatientMedicalFamilyHistoryRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
