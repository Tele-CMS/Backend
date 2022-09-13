using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Reports
{
    public class ManualMileageReport
    {
        public int PatientAppointmentID { get; set; }
        public DateTime? Date { get; set; }
        public string StaffName { get; set; }
        public string ClientName { get; set; }
        public string AppointmentTimeFrom { get; set; }
        public string AppointmentTimeTo { get; set; }
        public TimeSpan? DriveTime { get; set; }
        public decimal? Mileage { get; set; }
    }

    public class PatientAppointmentDetailsReport
    {
        public int PatientID { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientMiddleName { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastname { get; set; }
        public string StaffMiddleName { get; set; }
        public string Title { get; set; }
        public string Payer { get; set; }
        public string ActivityType { get; set; }
        public DateTime ScheduleDateFrom { get; set; }
        public DateTime ScheduleDateTo { get; set; }
        public int? DurantionSchedule { get; set; }
        public DateTime RenderDatedFrom { get; set; }
        public DateTime RenderDatedTo { get; set; }
        public int? DurantionRenderinMin { get; set; }
        public int? DurantionRenderinHours { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Service { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }
        public string IsBillable { get; set; }
        public string Office { get; set; }
        public string RenderingProviderFirstName { get; set; }
        public string RenderingProviderLastName { get; set; }
        public string RenderingProviderMiddleName { get; set; }
        public DateTime DateTimeRendered { get; set; }
        public string IsNonBillable { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ActivityID { get; set; }
        public int FacilityID { get; set; }
        public string RatePerUnit { get; set; }
        public string Units { get; set; }
        public string Notes { get; set; }
        public bool? SignatoryOnFile { get; set; }
        public bool? SignatoreOnFile { get; set; }
    }

    public class PatientEncounterReport
    {
        public int PatientEncounterID { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public int Units { get; set; }
        public string Location { get; set; }
        public string ProcedureCode { get; set; }
        public string SessionDescription { get; set; }
        public string PractitionerName { get; set; }
        public byte[] PractitionerSignature { get; set; }
        public string ResponsibleAdultName { get; set; }
        public byte[] ResponsibleAdultSignature { get; set; }
    }

}
