using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Repositories;
using System.Linq;

namespace HC.Patient.Repositories.Repositories.MasterData
{
    public class RoundingRuleDetailRepository : RepositoryBase<RoundingRuleDetails>, IRoundingRuleDetailRepository
    {
        private readonly HCOrganizationContext _context;
        public RoundingRuleDetailRepository(HCOrganizationContext context):base(context)
        {
            this._context = context;
        }

    }
}
