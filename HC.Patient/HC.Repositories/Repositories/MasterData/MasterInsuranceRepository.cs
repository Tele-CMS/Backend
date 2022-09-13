using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterInsuranceRepository : RepositoryBase<InsuranceCompanies>, IMasterInsuranceRepository
    {
        private HCOrganizationContext _context;
        public MasterInsuranceRepository(HCOrganizationContext context):base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetMasterInsuranceList<T>(string name, int pageNumber, int pageSize, int organizationId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@InsuranceName",name),
            new SqlParameter("@PageNumber",pageNumber),
            new SqlParameter("@PageSize",pageSize),
            new SqlParameter("@OrganizationId",organizationId)};
            return _context.ExecStoredProcedureListWithOutput<T>("GetMasterInsuranceList", parameters.Length, parameters).AsQueryable();

        }
    }
}
