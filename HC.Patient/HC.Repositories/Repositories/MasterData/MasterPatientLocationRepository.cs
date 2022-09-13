using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterPatientLocationRepository : RepositoryBase<MasterPatientLocation>, IRepositories.MasterData.IMasterPatientLocationRepository
    {
        private HCOrganizationContext _context;
        public MasterPatientLocationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

    }
}
