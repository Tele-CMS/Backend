using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterTemplatesCategoryRepository : RepositoryBase<MasterTemplateCategory>, IMasterTemplatesCategoryRepository
    {
        private HCOrganizationContext _context;
        public MasterTemplatesCategoryRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        
    }
}
