using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System.Linq;

namespace HC.Patient.Repositories.Repositories
{
    public class OpenTokSettingsRepository : RepositoryBase<OpenTokSettings>, IOpenTokSettingsRepository
    {
        private HCOrganizationContext _context;
        public OpenTokSettingsRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public OpenTokSettings GetOpenTokKeysByOrganizationId(TokenModel tokenModel)
        {
            return _context.OpenTokSettings.Where(o => o.OrganizationId == tokenModel.OrganizationID && o.IsActive == true && o.IsDeleted == false).FirstOrDefault();
        }
    }
}
