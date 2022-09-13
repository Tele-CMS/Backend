using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Reports
{
    public class AppointmentReportingResponseModel
    {
        public List<AppointmentReportingListModel> AppointmentReportingList { get; set; }
        public AppointmentReportingListCountModel AppointmentReportingListCount { get; set; }
    }

    public class AppointmentReportingListCountModel
    {
        public int TotalCount { get; set; }
        public int CancelledCount { get; set; }
        public int PendingCount { get; set; }
    }

    public class AppointmentReportingListModel
    {
        public int PatientAppointmentId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<AppointmentReportingCareManagerModel> CareManagers { get; set; }
        public string MemberName { get; set; }
        public int PatientID { get; set; }
        public int ServiceLocationID { get; set; }
        public string AppointmentType { get; set; }
        public List<AppointmentReportingProgramModel> Programs { get; set; }
        public string EncounterMethod { get; set; }
        public string EncounterType { get; set; }
        public string Status { get; set; }
        public string CancellationType { get; set; }
        public string CancellationNotes { get; set; }
        public string XmlProgramModel { get; set; }
        public string XmlCareMangerModel { get; set; }
        public decimal DaylightSavingTime { get; set; }
        public decimal StandardTime { get; set; }
        public string TimeZoneName { get; set; }
        public DateTime? NextAppointmentDate { get; set; }
        public int TotalRecords { get; set; }
    }
    public class AppointmentReportingProgramModel
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
    }
    public class AppointmentReportingCareManagerModel
    {
        public int Id { get; set; }
        public string CareManagerName { get; set; }
    }
    public class AppointmentReportDataForPDF
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string MemberName { get; set; }
        public string CareManagerName { get; set; }
        public string AppointmentType { get; set; }
        public string ProgramName { get; set; }
        public string EncounterType { get; set; }
        public string EncounterMethod { get; set; }
        public string Status { get; set; }
        public string CancellationType { get; set; }
        public string CancellationNotes { get; set; }
        public string NextAppointmentDate { get; set; }
    }
}
