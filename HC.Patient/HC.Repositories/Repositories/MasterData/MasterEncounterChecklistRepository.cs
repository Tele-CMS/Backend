using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterEncounterChecklistRepository: RepositoryBase<MasterEncounterChecklist>, IMasterEncounterChecklistRepository
    {
        private HCOrganizationContext _context;
        public MasterEncounterChecklistRepository(HCOrganizationContext organizationContext) : base(organizationContext)
        {
            _context = organizationContext;
        }
    }
}
