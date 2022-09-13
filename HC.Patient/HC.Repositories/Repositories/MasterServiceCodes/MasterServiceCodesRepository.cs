using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Repositories.IRepositories.MasterServiceCodes;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterServiceCodes
{
    public class MasterServiceCodesRepository : RepositoryBase<Entity.MasterServiceCode>, IMasterServiceCodesRepository
    {
        private HCOrganizationContext _context;
        public MasterServiceCodesRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public MasterServiceCode AddMasterServiceCode(MasterServiceCode masterServiceCode)
        {
            try
            {
                _context.MasterServiceCode.Add(masterServiceCode);
                _context.SaveChanges();

            }
            catch (Exception )
            {
                throw;
            }
            return masterServiceCode;
        }

        public MasterServiceCode GetServiceCodeByID(int ServiceCodeID, TokenModel token)
        {
            MasterServiceCode masterServiceCode = _context.MasterServiceCode
                    .Where(x => x.Id == ServiceCodeID && x.IsDeleted == false && x.OrganizationID == token.OrganizationID)
                    .Include(x => x.MasterServiceCodeModifiers)
                    //.Where(f => f.MasterServiceCodeModifiers.Any(z => z.IsActive == true && z.IsDeleted == false))
                    .FirstOrDefault();

            return masterServiceCode;
        }

        public MasterServiceCode UpdateServiceCode(MasterServiceCode serviceCode, DateTime CurrentDate)
        {
            try
            {
                if (serviceCode.MasterServiceCodeModifiers.Any(x => x.DeletedDate == CurrentDate || x.UpdatedDate == CurrentDate))
                {
                    foreach (var item in serviceCode.MasterServiceCodeModifiers.Where(x => x.UpdatedDate == CurrentDate || x.DeletedDate == CurrentDate))
                    {
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }

                if (serviceCode.MasterServiceCodeModifiers.Any(x => x.CreatedDate == CurrentDate))
                {
                    _context.MasterServiceCodeModifiers.AddRange(serviceCode.MasterServiceCodeModifiers.Where(x => x.CreatedDate == CurrentDate));
                }
                _context.Entry(serviceCode).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception )
            {

            }
            return serviceCode;
        }

        public IQueryable<T> GetMasterServiceCodes<T>(string searchText, TokenModel token, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", token.OrganizationID),
                new SqlParameter("@PageNumber",pageNumber),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortOrder ", sortOrder ),
                new SqlParameter("@SearchText", searchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetMasterServiceCodes.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
