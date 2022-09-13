using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
   public class StaffLeaveModel
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int LeaveStatusId { get; set; }
        public string LeaveStatus { get; set; }
        public string OtherLeaveType { get; set; }
        public string OtherLeaveReason { get; set; }
        public int LeaveReasonId { get; set; }
        public string LeaveReason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        public decimal TotalRecords { get; set; }
    }
    public class LeaveStatusModel
    {
        public int staffLeaveId { get; set; }
        public int leaveStatusId { get; set; }
        public string declineReason { get; set; }
    }
}
