using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class StaffDetailedTimesheetModel
    {
        public int Id { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? AppointmentTypeId { get; set; }
        public string AppointmentTypeName { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public decimal Distance { get; set; }
        public decimal DriveTime { get; set; }
        public decimal ExpectedTimeDuration { get; set; }
        public decimal ActualTimeDuration { get; set; }
        public bool IsTravelTime { get; set; }
        public string Notes { get; set; }
    }
}
