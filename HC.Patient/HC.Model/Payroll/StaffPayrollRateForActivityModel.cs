using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payroll
{
    public class StaffPayrollRateForActivityModel
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int AppointmentTypeId { get; set; }
        public decimal PayRate { get; set; }
        public string AppointmentName { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
