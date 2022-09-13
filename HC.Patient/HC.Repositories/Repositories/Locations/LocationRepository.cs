using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Locations
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        private HCOrganizationContext _context;
        public LocationRepository(HCOrganizationContext context) :base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetLocations<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize", searchFilterModel.pageSize),
                new SqlParameter("@SortColumn", searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder ", searchFilterModel.sortOrder ),
                new SqlParameter("@SearchText", searchFilterModel.SearchText)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetLocations, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetLocationDetail<T>(int? LocationId, int OrganizationId) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@LocationId", LocationId  ?? SqlInt32.Null),
                new SqlParameter("@OrganizationId",OrganizationId),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.LOC_GetLocationDetail, parameters.Length, parameters).AsQueryable();
        }
        public MasterState GetMasterStateDetails(int? stateId, TokenModel tokenModel)
        {
            return _context.MasterState.Where(s => s.Id == stateId && s.IsActive == true && s.IsDeleted == false && s.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();
        }
        public IQueryable<T> GetAllLocationsByOrganizationId<T>(TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetAllLocationsByOrganizationId, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetLocationByServiceLocationID<T>(int id) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ServiceLocationID", id)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetLocationByServiceLocationID, parameters.Length, parameters).AsQueryable();
        }
    }
}
