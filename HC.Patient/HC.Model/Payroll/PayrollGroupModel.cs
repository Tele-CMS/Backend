using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payroll
{
    public class PayrollGroupModel
    {
        public  int Id { get; set; }
        public string GroupName { get; set; }
        public int PayPeriodId { get; set; }
        public int WorkWeekId { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal WeeklyLimit { get; set; }
        public decimal OverTime { get; set; }
        public decimal DoubleOverTime { get; set; }
        public bool IsCaliforniaRule { get; set; }
        public string PayPeriodName { get; set; }
        public string WorkWeekName { get; set; }
        public decimal TotalRecords { get; set; }
        public int PayrollBreakTimeId { get; set; }
    }
}
