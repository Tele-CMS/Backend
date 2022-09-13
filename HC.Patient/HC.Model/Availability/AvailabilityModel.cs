using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Availability
{
    public class AvailabilityModel
    {
        public int StaffID { get; set; }
        public int? LocationId { get; set; }
        public List<DayModel> Days { get; set; }
        public List<AvailabilityStatusModel> Available { get; set; }
        public List<AvailabilityStatusModel> Unavailable { get; set; }
    }

    public class DayModel
    {
        public int Id { get; set; }        
        public int? DayId { get; set; }
        public string DayName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }                
        public int StaffAvailabilityTypeID { get; set; }
        public int StaffID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? LocationId { get; set; }
        public string isAvailable { get; set; } = "Yes";
    }
    public class AvailabilityStatusModel
    {
        public int Id { get; set; }        
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Date { get; set; }
        public int StaffAvailabilityTypeID { get; set; }
        public int StaffID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? LocationId { get; set; }
        public string isAvailable { get; set; } = "Yes";
    }    
}
