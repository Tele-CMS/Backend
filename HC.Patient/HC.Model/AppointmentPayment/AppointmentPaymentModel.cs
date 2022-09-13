using System;
using System.Collections.Generic;
using System.Text;
namespace HC.Patient.Model
{
    public class AppointmentPaymentModel
    {
        public int PaymentId { get; set; } = 0;
        public int AppointmentId { get; set; }
        public string PaymentToken { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
    }
    public class AppointmentPaymentListingModel
    {
        public decimal BookingAmount { get; set; }
        public int PayId { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int PatientID { get; set; }
        public int StaffID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentToken { get; set; }
        public string PaymentMode { get; set; }
        public int TotalRecords { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PaymentDate { get; set; }
        public int ServiceLocationID { get; set; }
        public string  Status { get; set; }
        public string  AppointmentType { get; set; }
        public decimal TotalNetAmount { get; set; }
    }
    public class ClientPaymentModel
    {
       
        public decimal MonthlyAmount { get; set; }
        public decimal NetAmount { get; set; }

    }
    public class ClientAppointmentPastUpcomingModel
    {
        public int pastAppointmentId { get; set; }
        public DateTime PastAppointmentStartDate { get; set; }
        public DateTime PastAppointmentEndDate { get; set; }
        public int upcomingAppointmentId { get; set; }
        public DateTime UpcomingAppointmentStartDate { get; set; }
        public DateTime UpcomingAppointmentEndDate { get; set; }
        public string AppointedPastStaff { get; set; }
        public string AppointedUpcomingStaff { get; set; }




    }
    public class AppointmentRefundListingModel
    {
        public decimal BookingAmount { get; set; }
        public int RefundId { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int PatientID { get; set; }
        public int StaffID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RefundToken { get; set; }
        public string PaymentMode { get; set; }
        public int TotalRecords { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RefundDate { get; set; }
        public int ServiceLocationID { get; set; }
    }
}
