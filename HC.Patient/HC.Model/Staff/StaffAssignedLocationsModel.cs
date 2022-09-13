using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class StaffAssignedLocationsModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int StaffLocationId { get; set; }
        public string LocationName { get; set; }
        public int StaffId { get; set; }                
        public bool IsDefault { get; set; }
    }
}
