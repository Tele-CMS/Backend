using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterData
{
    public class LocationService : BaseService, ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public LocationService(ILocationRepository locationRepository, IUserCommonRepository userCommonRepository)
        {
            _locationRepository = locationRepository;
            _userCommonRepository = userCommonRepository;
        }
        public JsonModel GetLocations(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<LocationModel> masterlocationModels = _locationRepository.GetLocations<LocationModel>(searchFilterModel, tokenModel).ToList();
            if (masterlocationModels != null && masterlocationModels.Count > 0)
            {
                response = new JsonModel(masterlocationModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterlocationModels, searchFilterModel);
            }
            return response;
        }
        public JsonModel SaveLocation(LocationModel locationModel, TokenModel tokenModel)
        {
            // Daylight saving time/Standard time fetch from MasterState
            MasterState masterState = _locationRepository.GetMasterStateDetails(locationModel.StateID, tokenModel);
            if (masterState != null)
            {
                locationModel.DaylightSavingTime = masterState.DaylightSavingTime;
                locationModel.StandardTime = masterState.StandardTime;
            }
            else
            {
                locationModel.DaylightSavingTime = 0;
                locationModel.StandardTime = 0;
            }
            Location location = null;
            if (locationModel.Id == 0)
            {
                location = _locationRepository.Get(l => l.LocationName == locationModel.LocationName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (location != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.LocationAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //insert new
                {
                    location = new Location();
                    AutoMapper.Mapper.Map(locationModel, location);
                    location.OrganizationID = tokenModel.OrganizationID;
                    location.CreatedBy = tokenModel.UserID;
                    location.CreatedDate = DateTime.UtcNow;
                    location.IsDeleted = false;
                    location.IsActive = true;
                    _locationRepository.Create(location);
                    _locationRepository.SaveChanges();
                    response = new JsonModel(location, StatusMessage.LocationSaved, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                location = _locationRepository.Get(l => l.LocationName == locationModel.LocationName && l.Id != locationModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (location != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.LocationAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //update existing
                {
                    location = _locationRepository.Get(a => a.Id == locationModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (location != null)
                    {
                        AutoMapper.Mapper.Map(locationModel, location);
                        location.UpdatedBy = tokenModel.UserID;
                        location.UpdatedDate = DateTime.UtcNow;
                        _locationRepository.Update(location);
                        _locationRepository.SaveChanges();
                        response = new JsonModel(location, StatusMessage.LocationUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetLocationById(int id, TokenModel tokenModel)
        {
            Location location = _locationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (location != null)
            {
                LocationModel locationModel = new LocationModel();
                AutoMapper.Mapper.Map(location, locationModel);
                response = new JsonModel(locationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteLocation(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.Location, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                Location location = _locationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (location != null)
                {
                    location.IsDeleted = true;
                    location.DeletedBy = tokenModel.UserID;
                    location.DeletedDate = DateTime.UtcNow;
                    _locationRepository.Update(location);
                    _locationRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.LocationDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }
        public JsonModel GetMinMaxOfficeTime(string locationIds, TokenModel tokenModel)
        {
            OfficeTimeModel officeTimeModel = null;
            if (!string.IsNullOrEmpty(locationIds))
            {
                officeTimeModel = new OfficeTimeModel();
                List<int> locations = new List<int>();
                locations = locationIds.Split(',').Select(Int32.Parse).ToList();
                var query = _locationRepository.GetAll(a => locations.Contains(a.Id)).AsQueryable();
                officeTimeModel.StartTime = query.Select(a => a.OfficeStartHour).Min().Value.ToString("HH:mm:ss");
                officeTimeModel.EndTime = query.Select(a => a.OfficeEndHour).Max().Value.ToString("HH:mm:ss");
                response = new JsonModel(officeTimeModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        //public LocationModel GetLocationOffsets(int? locationId)
        //{
        //    //LocationModel locationModal = new LocationModel();
        //    //Location location = _locationRepository.GetByID(locationId);
        //    //if (location != null)
        //    //{
        //    //    locationModal.DaylightOffset = (((decimal)location.DaylightSavingTime) * 60);
        //    //    locationModal.StandardOffset = (((decimal)location.StandardTime) * 60);
        //    }

        //    return getlo;
        //}
        //public LocationModel GetLocationOffsets(int? locationId, TokenModel token)
        //{
        //    var locationModal = _locationRepository.GetLocationDetail<LocationModel>(locationId, token.OrganizationID).FirstOrDefault();
        //    if (locationModal != null)
        //    {
        //        locationModal.DaylightOffset = CommonMethods.GetMin((decimal)locationModal.DaylightSavingTime);
        //        locationModal.StandardOffset = CommonMethods.GetMin(locationModal.StandardOffset);
        //    }
        //    return locationModal;
        //}
        public LocationModel GetLocationOffsets(int? locationId, TokenModel token)
        {
            LocationModel locationModal = _locationRepository.GetLocationDetail<LocationModel>(locationId, token.OrganizationID).FirstOrDefault();
            if (locationModal != null)
            {
                locationModal.DaylightOffset = GetMin((decimal)locationModal.DaylightSavingTime);
                locationModal.StandardOffset = GetMin((decimal)locationModal.StandardOffset);
            }
            return locationModal;
        }

        public JsonModel GetAllLocationsByOrganizationId(TokenModel tokenModel)
        {
            List<LocationModel> masterLocationModels = _locationRepository.GetAllLocationsByOrganizationId<LocationModel>(tokenModel).ToList();
            if (masterLocationModels != null && masterLocationModels.Count > 0)
            {
                response = new JsonModel(masterLocationModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        private decimal GetMin(decimal number)
        {
            string s = number.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = s.Split('.');
            //
            decimal i1 = int.Parse(parts[0]) * 60;

            decimal i2 = int.Parse(parts[1]);

            return i1 + i2;
        }

        public JsonModel GetLocationByServiceLocationID(int id)
        {
            //List<LocationModel> masterLocationModels = _locationRepository.GetLocationByServiceLocationID<LocationModel>(id).ToList();
            LocationModel masterLocationModels = _locationRepository.GetLocationByServiceLocationID<LocationModel>(id).FirstOrDefault();
            if (masterLocationModels != null)
            {
                response = new JsonModel(masterLocationModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
    }
}
