using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class StaffTimesheetModel
    {
        public int Id { get; set; }
        public int AppointmentTypeId { get; set; }
        public string AppointmentTypeName { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public decimal Sunday { get; set; }
        public decimal Monday { get; set; }
        public decimal Tuesday { get; set; }
        public decimal Wednesday { get; set; }
        public decimal Thursday { get; set; }
        public decimal Friday { get; set; }
        public decimal Saturday { get; set; }
        public DateTime SunDate { get; set; }
        public DateTime MonDate { get; set; }
        public DateTime TueDate { get; set; }
        public DateTime WedDate { get; set; }
        public DateTime ThuDate { get; set; }
        public DateTime FriDate { get; set; }
        public DateTime SatDate { get; set; }
    }
}
