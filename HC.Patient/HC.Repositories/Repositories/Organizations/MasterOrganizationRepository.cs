using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Organizations
{
    public class MasterOrganizationRepository : MasterRepositoryBase<MasterOrganization>, IMasterOrganizationRepository
    {
        private HCMasterContext _masterContext;
        private HCOrganizationContext _context;
        public MasterOrganizationRepository(HCMasterContext masterContext, HCOrganizationContext context) : base(masterContext)
        {
            this._masterContext = masterContext;
            _context = context;
        }

        public List<MasterOrganizationModel> GetMasterOrganizations(string businessName, string orgName, string country, string sortOrder, string sortColumn, int page, int pageSize)
        {
            SqlParameter[] parameters = { new SqlParameter("@BusinessName", businessName),
                                          new SqlParameter("@organizationName",orgName),
                                          new SqlParameter("@Country",country),
                                          new SqlParameter("@PageNumber",page),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
            };
            return _masterContext.ExecStoredProcedureListWithOutput<MasterOrganizationModel>("Org_GetMasterOrganization", parameters.Length, parameters).ToList();
        }
        public OrganizationDetailModel GetOrganizationDetailsById(TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizatonID",token.OrganizationID ),                                          
            };
            return _context.ExecStoredProcedureListWithOutput<OrganizationDetailModel>(SQLObjects.ORG_GetOrganizationData, parameters.Length, parameters).FirstOrDefault();
        }
    }
}
