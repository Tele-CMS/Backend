using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.Availability;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HC.Patient.Service.Services
{
    public class StaffProfileService : BaseService, IStaffProfileService
    {
        private readonly IStaffService _staffService;
        private readonly IStaffProfileRepository _staffProfileRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IStaffAvailabilityService _staffAvailabilityService;
        private JsonModel _response;
        public StaffProfileService(IStaffProfileRepository staffProfileRepository, IStaffRepository staffRepository, IStaffService staffService, IStaffAvailabilityService staffAvailabilityService)
        {
            _staffProfileRepository = staffProfileRepository;
            _staffService = staffService;
            _staffRepository = staffRepository;
            _staffAvailabilityService = staffAvailabilityService;
        }
        public JsonModel GetStaffLocationAndAvailability(string staffId, TokenModel tokenModel)
        {
            _response = new JsonModel();
            int.TryParse(Common.CommonMethods.Decrypt(staffId.Replace(" ", "+")), out int id);
            List<StaffLocationAvailibilityModel> staffLocationAvailibilityModels = new List<StaffLocationAvailibilityModel>();
            List<AssignedLocationsModel> staffLocations = _staffRepository.GetAssignedLocationsByStaffId(id, tokenModel).ToList();
            if (staffLocations.Count > 0)
            {
                staffLocations.ForEach(loc =>
                {
                    int.TryParse(Common.CommonMethods.Decrypt(loc.LocationId), out int locId);
                    AvailabilityModel list = (AvailabilityModel)_staffAvailabilityService.GetStaffAvailabilityWithLocation(id.ToString(), locId, false, tokenModel).data;
                    StaffLocationAvailibilityModel staffLocationAvailibility = new StaffLocationAvailibilityModel()
                    {
                        AssignedLocationsModel = loc,
                        AvailabilityModel = list
                    };
                    staffLocationAvailibilityModels.Add(staffLocationAvailibility);
                });
            }
            if (staffLocationAvailibilityModels != null && staffLocationAvailibilityModels.Count > 0)
            {
                _response.data = staffLocationAvailibilityModels;
                _response.Message = StatusMessage.FetchMessage;
                _response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;
        }
    }
}
