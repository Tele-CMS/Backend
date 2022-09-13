using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Authorization;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Authorization
{
    public class AuthorizationRepository : RepositoryBase<Entity.Authorization>, IAuthorizationRepository
    {
        private HCOrganizationContext _context;
        public AuthorizationRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public GetAuthorizationByIdModel GetAutorizationById(SearchFilterModel searchFilterModel,TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@AuthorizationId",searchFilterModel.AuthorizationId)
                                         ,new SqlParameter("@OrganizationId",tokenModel.OrganizationID)
            };
            return _context.ExecStoredProcedureForGetAutorizationById(SQLObjects.MTR_GetAuthorizationById.ToString(), parameters.Length, parameters);
        }
    }
}
