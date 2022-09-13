using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class PendingAppointmentViewModel
    {
        public int PatientAppointmentId { get; set; }
        public int PatientID { get; set; }
        public int StatusId { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentMode { get; set; }
        public bool IsTelehealthAppointment { get; set; }
        
        public string FullName { get; set; }
        public string StatusName { get; set; }
        public string XmlString { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? ServiceLocationID { get; set; }
        public decimal TotalRecords { get; set; }
        public List<PendingAppointmentStaffs> PendingAppointmentStaffs { get; set; }
        public decimal DaylightSavingTime { get; set; }
        public decimal StandardTime { get; set; }
        public string TimeZoneName { get; set; }
        public string CancelReason { get; set; }
        public string CancelType { get; set; }
        public string Notes { get; set; }
        public string InvitationAcceptRejectRemarks { get; set; }
        public int? ReviewRatingId { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public string Address { get; set; }

        public bool IsSymptomateReportExist { get; set; }
        public int ReportId { get; set; }
    }
    public class PendingAppointmentStaffs
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public bool IsDeleted { get; set; }
        public string ProviderImage { get; set; }
        public string ProviderImageThumbnail { get; set; }
        public string Address { get; set; }
    }
}
