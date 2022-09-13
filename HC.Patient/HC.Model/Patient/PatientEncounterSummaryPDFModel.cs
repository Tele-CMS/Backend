using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientEncounterSummaryPDFModel
    {
        public PatientDetailsModel PatientDetailsModel { get; set; }
        public CareManagerDetailsModel CareManagerDetailsModel { get; set; }
        public List<TaskDetailsModel> TaskDetailsModel { get; set; }
        public PatientAppointmentDetailsModel PatientAppointmentDetailsModel { get; set; }
        public PatientEncounterForPDFModel PatientEncounterModel { get; set; }
        public List<EncounterProgramsForPDFModel> ProgramTypeIds { get; set; }
        public  SOAPNotesForPDFModel SOAPNotes { get; set; }
        public List<EncounterSignatureForPDFModel> EncounterSignature { get; set; }
        public List<PatientEncounterChecklistForPDFModel> PatientEncounterChecklistModel { get; set; }
        public List<EncounterChecklistReviewForPDFItems> EncounterChecklistReviewItems { get; set; }
        public List<EncounterChangeHistoryForPDF> EncounterChangeHistory { get; set; }
        public List<PrintPatientCurrentMedicationModelforPDF> PrintPatientCurrentMedicationModel { get; set; }

    }

    public class PatientDetailsModel
    {
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }

    }
    public class CareManagerDetailsModel
    {
        public string CareManagerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class TaskDetailsModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public string Priority { get; set; }
        public string DueDate { get; set; }
    }
    public class PatientAppointmentDetailsModel
    {
        public int PatientAppointmentId { get; set; }
        public string NotesToMember { get; set; }
        public string NotesFromMember { get; set; }
        public string AppointmentType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

    }
    public class PatientEncounterForPDFModel
    {
        public int? Id { get; set; }
        public int? PatientID { get; set; }
        public int? PatientAppointmentId { get; set; }
        public int? ParentAppointmentId { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime AppointmentStartDateTime { get; set; }
        public DateTime AppointmentEndDateTime { get; set; }
        public int? StaffID { get; set; }
        public int? PatientAddressID { get; set; }
        public int? OfficeAddressID { get; set; }
        public int? ServiceLocationID { get; set; }
        public int? CustomAddressID { get; set; }
        public string CustomAddress { get; set; }
        public int? NotetypeId { get; set; }
        public String StaffName { get; set; }
        public String PatientName { get; set; }
        public String LocationName { get; set; }
        public string Duration { get; set; }
        public string AppointmentType { get; set; }
        public string Notes { get; set; }
        public string MemberNotes { get; set; }
        public string Status { get; set; }
        public decimal TotalRecords { get; set; }

        public int UnitsBlocked { get; set; }
        public int UnitsConsumed { get; set; }
        public string ServiceFacility { get; set; }
        public bool IsDirectService { get; set; }
        public string NonBillableNotes { get; set; }
        public bool? IsBillableEncounter { get; set; }
        public bool isCompleted { get; set; }
        public bool? IsActive { get; set; }
        public string ManualChiefComplaint { get; set; }
        public string ReferringProviderName { get; set; }
        public bool IsImported { get; set; }
    }
    public class SOAPNotesForPDFModel
    {
        public int Id { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }
    }

    public class EncounterProgramsForPDFModel
    {
        public int ProgramTypeId { get; set; }
        public string ProgramName { get; set; }
    }
    public class PatientEncounterChecklistForPDFModel
    {
        public int Id { get; set; }
        public int PatientEncounterId { get; set; }
        public string Notes { get; set; }
        public int MasterEncounterChecklistId { get; set; }
        public string Name { get; set; }
        public string NavigationLink { get; set; }
        public bool IsAdministrativeType { get; set; }
    }

    public class EncounterChecklistReviewForPDFItems
    {
        public int Id { get; set; }
        public int MasterEncounterChecklistId { get; set; }
        public string ItemName { get; set; }
    }
    public class EncounterSignatureForPDFModel
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? StaffId { get; set; }
        public int PatientEncounterId { get; set; }
        public Byte[] PatientSign { get; set; }
        public DateTime? PatientSignDate { get; set; }
        public Byte[] ClinicianSign { get; set; }
        public DateTime? ClinicianSignDate { get; set; }
        public Byte[] GuardianSign { get; set; }
        public DateTime? GuardianSignDate { get; set; }
        public string GuardianName { get; set; }
    }
    public class EncounterChangeHistoryForPDF
    {
        public int MasterEncounterChecklistId { get; set; }
        public string EntityName { get; set; }
        public string Changes { get; set; }
    }
      public class PrintEncounterListModel
    {
        public string PatientName { get; set; }
        public string DateOfService { get; set; }
        public string Duration { get; set; }
        public string CareManager { get; set; }
        public string EncounterType { get; set; }
        public string ManualChiefComplaint { get; set; }
        public string ReferringProviderName { get; set; }
        public string Status { get; set; }
        public string NextAppointmentDate { get; set; }
    }

    public class PrintPatientCurrentMedicationModelforPDF
    {
        public int Id { get; set; }
        public int? DaySupply { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public int? Quantity { get; set; }
        public int? Refills { get; set; }
        public string Condition { get; set; }
        public string Medication { get; set; }
        public string ProviderName { get; set; }
        public string Source { get; set; }
        public string Notes { get; set; }
        public string DosageForm { get; set; }
    }
}
