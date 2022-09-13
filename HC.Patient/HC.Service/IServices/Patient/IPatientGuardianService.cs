using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientGuardianService : IBaseService
    {
        JsonModel GetPatientGuardian(PatientGuartdianFilterModel patientGuardianModel, TokenModel tokenModel);
        JsonModel CreateUpdatePatientGuardian(PatientGuardianModel patientGuardianModel, TokenModel tokenModel);
        JsonModel GetPatientGuardianById(int id, TokenModel tokenModel);
        JsonModel Delete(int id, TokenModel tokenModel);
    }
}
