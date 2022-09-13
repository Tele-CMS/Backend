using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class StaffTagRepository : RepositoryBase<StaffTags>, IStaffTagRepository
    {
        private HCOrganizationContext _context;
        public StaffTagRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
