using HC.Patient.Model.Patient;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class PatientAppointmentModel
    {
        public int PatientAppointmentId { get; set; }
        public DateTime StartDateTime { get; set;}
        public DateTime EndDateTime { get; set; }
        public string AppointmentTypeName { get; set; }
        public int AppointmentTypeID { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }

        public decimal DefaultDuration { get; set; }
        public bool IsBillable { get; set; }
        public bool? IsNotify { get; set; }
        public string PatientName { get; set; }
        public int? PatientID { get; set; }
        public Nullable<int> PatientEncounterId { get; set; }
        public Nullable<int> ClaimId { get; set; }
        public bool CanEdit { get; set; }
        public string Notes { get; set; }
        public int? PatientAddressID { get; set; }
        public int? OfficeAddressID { get; set; }
        public int? ServiceLocationID { get; set; }
        public string RecurrenceRule { get; set; }
        public Nullable<int> ParentAppointmentId { get; set; }
        public List<AppointmentStaffs> AppointmentStaffs { get; set; }

        public bool IsClientRequired { get; set; }

        public bool IsRecurrence { get; set; }
        //public List<Occurrencess> Occurences { get; set; }

        //public string RecurrencePattern { get; set; }

        public string XmlString { get; set; }

        public int OffSet { get; set; }

        public bool AllowMultipleStaff { get; set; }

        public int? CancelTypeId { get; set; }

        public string CancelTypeName { get; set; }

        public bool IsExcludedFromMileage { get; set; }

        public bool IsDirectService { get; set; }

        public decimal? Mileage { get; set; }
        
        public TimeSpan? DriveTime { get; set; }

        public string PatientPhotoThumbnailPath { get; set; }
        
        public string Timezone { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string Key { get; set; }
        public string Address { get; set; }
        public string CancelReason { get; set; }
        public List<AvailabilityMessageModel> AvailabilityMessages { get; set; }
        public string CustomAddress { get; set; }
        public int? CustomAddressID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public bool IsTelehealthAppointment { get; set; }
        public string ServiceFacility { get; set; }
        public int? LocationId { get; set; }

        public Nullable<int> PatientInsuranceId { get; set; }
        public Nullable<int> AuthorizationId { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public decimal PayRate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentToken { get; set; }
        public string PaymentMode { get; set; }
        public int TotalRecords { get; set; }
        public int? InvitationAppointentId { get; set; }
        public string XmlInvitedString { get; set; }
        public List<InvitedStaffs> InvitedStaffs { get; set; }
        public int? ReviewRatingId { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
        public bool IsSymptomateReportExist { get; set; }
        public int ReportId { get; set; }

        public bool IsSymptomaticProcess { get; set; }
        public bool? IsUrgentCareAppointment { get; set; }
        public PatientInfoDetails PatientInfoDetails { get; set; }

    }
    public class InvitedStaffs
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
    public class AppointmentStaffs
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string Photo { get; set; }
        public string PhotoThumbnail { get; set; }
    }

    public class Occurrencess
    {
        public Period Occurrence { get; set; }
        public int? AppointmentID { get; set; }
        public int? ParentAppointmentID { get; set; }
        public int? PatientEncounterID { get; set; }
    }
    public class AvailabilityMessageModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Valid { get; set; }
    }
    public class CancelAppointmentModel
    {
        public int[] Ids { get; set; }
        public string CancelReason { get; set; }
        public int CancelTypeId { get; set; }
    }

    public class AppointmentStatusModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
       
    }
}
