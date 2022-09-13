using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class MasterEncChecklistReviewItemsRepository: RepositoryBase<MasterEncChecklistReviewItems>,IMasterEncChecklistReviewItemsRepository
    {
        private readonly HCOrganizationContext _context;
        public MasterEncChecklistReviewItemsRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
