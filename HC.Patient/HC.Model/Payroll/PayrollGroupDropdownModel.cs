using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payroll
{
  public class PayrollGroupDropdownModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Workweek { get; set; }
        public string PayPeriodName { get; set; }
    }
}
