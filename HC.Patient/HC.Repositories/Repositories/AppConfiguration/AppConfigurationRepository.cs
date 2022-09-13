using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.AppConfiguration;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.AppConfiguration
{
    public class AppConfigurationRepository:RepositoryBase<AppConfigurations>, IAppConfigurationRepository
    {
        private HCOrganizationContext _context;
        public AppConfigurationRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
