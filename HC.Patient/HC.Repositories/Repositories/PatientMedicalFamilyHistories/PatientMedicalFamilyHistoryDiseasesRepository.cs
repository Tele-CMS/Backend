using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Repositories;
using HC.Patient.Repositories.IRepositories.PatientMedicalFamilyHistories;

namespace HC.Patient.Repositories.Repositories.PatientMedicalFamilyHistories
{
    public class PatientMedicalFamilyHistoryDiseasesRepository : RepositoryBase<PatientMedicalFamilyHistoryDiseases>, IPatientMedicalFamilyHistoryDiseasesRepository
    {
        private HCOrganizationContext _context;
        public PatientMedicalFamilyHistoryDiseasesRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
