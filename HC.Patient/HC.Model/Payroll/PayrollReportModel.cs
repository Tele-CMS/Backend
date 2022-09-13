using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payroll
{
    public class PayrollReportModel
    {
        public int StaffId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string StaffName { get; set; }
        public int AppointmentTypeId { get; set; }

        public string AppointmentType { get; set; }

        public decimal ActivityTime { get; set; }
        public decimal TotalHoursWorkedInDay { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal DayTimeWithoutOT { get; set; }
        public decimal Overtime { get; set; }
        public decimal DoubleOvertime { get; set; }
        public decimal TotalDistance { get; set; }
        public int NoOfBreaks { get; set; }
    }
}
