using HC.Repositories.Interfaces;
using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using System.Collections.Generic;
using HC.Model;

namespace HC.Patient.Repositories.IRepositories.Organizations
{
    public interface IMasterOrganizationRepository : IMasterRepositoryBase<MasterOrganization>
    {
        List<MasterOrganizationModel> GetMasterOrganizations(string businessName,string orgName, string country, string sortOrder, string sortColumn, int page, int pageSize);
        OrganizationDetailModel GetOrganizationDetailsById(TokenModel token); 
    }
}
