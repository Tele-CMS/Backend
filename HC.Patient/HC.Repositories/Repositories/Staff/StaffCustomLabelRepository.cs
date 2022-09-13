using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class StaffCustomLabelRepository : RepositoryBase<StaffCustomLabels>, IStaffCustomLabelRepository
    {
        private HCOrganizationContext _context;
        public StaffCustomLabelRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public Dictionary<string, object> GetStaffCustomLabel(int staffId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffId",staffId),
                                          new SqlParameter("@OrganizationID",token.OrganizationID)
                };

            return _context.ExecStoredProcedureForStaffCustomLabels(SQLObjects.STF_GetStaffCustomLabels.ToString(), parameters.Length, parameters);
        }
    }
}
