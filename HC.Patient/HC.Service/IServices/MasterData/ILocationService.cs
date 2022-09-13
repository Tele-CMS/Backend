using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface ILocationService : IBaseService
    {
        JsonModel GetLocations(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveLocation(LocationModel locationModel, TokenModel tokenModel);
        JsonModel GetLocationById(int id, TokenModel tokenModel);
        JsonModel DeleteLocation(int id, TokenModel tokenModel);
        JsonModel GetMinMaxOfficeTime(string locationIds, TokenModel tokenModel);


        /// <summary>
        /// Get Location Offset for time zone
        /// </summary>
        /// <param name="locationId">location id to get location offset</param>
        /// <returns>Location model => LocationModel</returns>
        //LocationModel GetLocationOffsets(int? locationId);

        /// <summary>
        /// Get Location Offset for time zone
        /// </summary>
        /// <param name="locationId">location id to get location offset</param>
        /// <param name="token">Current Logged In token model</param>
        /// <returns>LocationModel</returns>
        LocationModel GetLocationOffsets(int? locationId, TokenModel token);
        JsonModel GetAllLocationsByOrganizationId(TokenModel tokenModel);
        JsonModel GetLocationByServiceLocationID(int id);
    }
}
