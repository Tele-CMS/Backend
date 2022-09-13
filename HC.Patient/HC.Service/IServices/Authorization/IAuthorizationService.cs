using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Authorization
{
    public interface IAuthorizationService : IBaseService
    {
        JsonModel GetAllAuthorizationsForPatient(int patientId, int pageNumber, int pageSize, string authType, TokenModel token);
        JsonModel SaveAuthorization(AuthModel authModel, TokenModel tokenModel);
        JsonModel GetAuthorizationById(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel DeleteAutorization(int id, TokenModel token);
    }
}
