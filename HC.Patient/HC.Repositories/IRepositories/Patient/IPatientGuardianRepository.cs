using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientGuardianRepository : IRepositoryBase<PatientGuardian>
    {
        IQueryable<T> GetPatientGuardian<T>(PatientGuartdianFilterModel patientGuartdianFilterModel, TokenModel tokenModel) where T : class, new();
        PatientGuardian GetPatientGuardianById(int id, TokenModel tokenModel);
    }
}
