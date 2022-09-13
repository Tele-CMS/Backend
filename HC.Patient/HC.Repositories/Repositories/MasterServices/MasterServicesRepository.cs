using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories
{
    public class MasterServicesRepository : RepositoryBase<MasterServices>, IMasterServicesRepository
    {
        private HCOrganizationContext _context;
        public MasterServicesRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<MasterServices> getMasterServicesByOrganizationId(TokenModel tokenModel, MasterServiceFilterModel masterServiceFilterModel)
        {
            if (string.IsNullOrEmpty(masterServiceFilterModel.sortColumn))
                masterServiceFilterModel.sortColumn = "ServiceName";

            if (string.IsNullOrEmpty(masterServiceFilterModel.sortOrder) || masterServiceFilterModel.sortOrder == "asc")
                masterServiceFilterModel.sortOrder = "true";
            else
                masterServiceFilterModel.sortOrder = "false";

            return (CommonMethods.OrderByField((_context.MasterServices
                .Where(s => s.OrganizationId == tokenModel.OrganizationID && s.IsDeleted == false))
                .Distinct(), masterServiceFilterModel.sortColumn, Convert.ToBoolean(masterServiceFilterModel.sortOrder)));
        }

        public MasterServices getMasterServicesById(TokenModel tokenModel, int id)
        {
            return _context.MasterServices
                .Where(s => s.Id == id && s.OrganizationId == tokenModel.OrganizationID && s.IsDeleted == false).FirstOrDefault();
        }

        public MasterServices CheckServiceNameExistance(string name, TokenModel tokenModel)
        {
            return _context.MasterServices
                .Where(s => s.ServiceName.ToLower() == name.ToLower()
                && s.OrganizationId == tokenModel.OrganizationID)
                .FirstOrDefault();
        }

        public MasterServices saveUpdateMasterServices(MasterServices masterServices, TokenModel tokenModel)
        {
            if (masterServices.Id == 0)
                _context.MasterServices.Add(masterServices);
            else
                _context.MasterServices.Update(masterServices);

            if (_context.SaveChanges() > 0)
                return masterServices;
            else
                return null;


        }
    }
}
