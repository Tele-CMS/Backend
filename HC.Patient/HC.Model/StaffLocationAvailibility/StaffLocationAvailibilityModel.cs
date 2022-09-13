using HC.Patient.Model.Availability;
using HC.Patient.Model.Staff;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class StaffLocationAvailibilityModel
    {
        public AssignedLocationsModel AssignedLocationsModel;
        public AvailabilityModel AvailabilityModel;
    }
    public class AssignedLocationsModel
    {
        public string LocationId;
        public string Location;
        public string Address;
        public bool IsDefault;
        public double? Latitude;
        public double? Longitude;
    }

}
