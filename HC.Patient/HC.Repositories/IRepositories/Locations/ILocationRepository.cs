using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Locations
{
    public interface ILocationRepository : IRepositoryBase<Location>
    {
        IQueryable<T> GetLocations<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        /// <summary>
        /// To Get Location Detail For Time Zone
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="LocationId">Courrent Selected Location Id</param>
        /// <param name="OrganizationId">Courrent Selected Organization Id</param>
        /// <returns></returns>
        IQueryable<T> GetLocationDetail<T>(int? LocationId, int OrganizationId) where T : class, new();

        /// <summary>
        /// Get Master State Details For DayLight Saving Time and Standard Time
        /// </summary>
        /// <param name="stateId">State ID</param>
        /// <param name="tokenModel">Current Logged in token</param>
        /// <returns></returns>
        MasterState GetMasterStateDetails(int? stateId, TokenModel tokenModel);
        IQueryable<T> GetAllLocationsByOrganizationId<T>(TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetLocationByServiceLocationID<T>(int id) where T : class, new();
    }
}
