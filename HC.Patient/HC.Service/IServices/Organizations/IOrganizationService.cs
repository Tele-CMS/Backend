using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.ContactUs;
using HC.Patient.Model.Organizations;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices.Organizations
{
    public interface IOrganizationService : IBaseService
    {
        JsonModel SaveOrganization(OrganizationModel organizationModel, TokenModel token, IHttpContextAccessor contextAccessor);
        JsonModel RegisterOrganization(OrganizationModel organizationModel, TokenModel token, IHttpContextAccessor contextAccessor);
        JsonModel GetOrganizationById(int Id, IHttpContextAccessor contextAccessor);
        JsonModel GetOrganizations(string businessName = "",string orgName = "", string country = "", string sortOrder = "", string sortColumn="", int page = 1, int pageSize = 10);
        JsonModel DeleteOrganization(int Id, TokenModel token, IHttpContextAccessor contextAccessor);
        JsonModel CheckOrganizationBusinessName(string BusinessName);
        JsonModel GetOrganizationEmailAddress();
        JsonModel GetOrganizationDatabaseDetails(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize);
        JsonModel SaveOrganizationDatabaseDetail(OrganizationDatabaseDetail organizationDatabaseDetail);
        JsonModel UpdateOrganizationDatabaseDetail(int id, OrganizationDatabaseDetail organizationDatabaseDetail);
        JsonModel DeleteOrganizationDatabaseDetail(int id, int userID);
        JsonModel GetOrganizationDetailsById(TokenModel token);
        JsonModel GetAllOrganizations();

        JsonModel SendContactUsData(ContactUsModel model, TokenModel tokenModel);
        JsonModel VerifyNPI(ApplicationUser applicationUser, TokenModel token, IHttpContextAccessor contextAccessor);
        string GetDPCAuthToken(string token);
        JsonModel GetOrganizationLogo(TokenModel token);
        JsonModel UpdateOrganizationLogo(OrganizationModel organizationModel, TokenModel token);
    }
}
