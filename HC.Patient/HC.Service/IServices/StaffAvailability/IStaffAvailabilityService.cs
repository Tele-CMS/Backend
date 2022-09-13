using HC.Model;
using HC.Patient.Model.Availability;
using HC.Service.Interfaces;
using System;

namespace HC.Patient.Service.IServices.StaffAvailability
{
    public interface IStaffAvailabilityService : IBaseService
    {
        JsonModel SaveStaffAvailabilty(AvailabilityModel entity, TokenModel token);
        JsonModel GetStaffAvailabilty(string staffID, TokenModel token, bool isLeaveNeeded);
        JsonModel SaveStaffAvailabiltyWithLocation(AvailabilityModel entity, TokenModel token);
        JsonModel GetStaffAvailabilityWithLocation(string staffID, int locationId, bool isLeaveNeeded, TokenModel token);
        JsonModel GetStaffAvailabilityWithLocationMobile(string staffID, int locationId, bool isLeaveNeeded, TokenModel token, DateTime date);
    }

}
