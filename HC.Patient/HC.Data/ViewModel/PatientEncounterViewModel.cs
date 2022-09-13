using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.ViewModel
{
    public class PatientEncounterViewModel
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public int? AppointmentID { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime AppointmentStartDateTime { get; set; }
        public DateTime AppointmentEndDateTime { get; set; }
        public int? StaffID { get; set; }
        public int? LocationID { get; set; }
        public Byte[] PatientSign { get; set; }
        public DateTime? PatientSignDate { get; set; }
        public Byte[] ClinicianSign { get; set; }
        public DateTime? ClinicianSignDate { get; set; }
        public int? NotetypeId { get; set; }
        public SOAPNotesViewModel SOAPNotes { get; set; }
        public List<PatientEncounterServiceCodesViewModel> PatientEncounterServiceCodes { get; set; }
        public List<PatientEncounterICDCodesViewModel> PatientEncounterICDCodes { get; set; }
        public List<PatientEncounterCodesMappingViewModel> PatientEncounterCodesMapping { get; set; }
    }
    public class SOAPNotesViewModel
    {
        public int Id { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }
    }
    public class PatientEncounterServiceCodesViewModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }

    }
    public class PatientEncounterICDCodesViewModel
    {
        public int Id { get; set; }
        public string ICDCode { get; set; }
        public string Description { get; set; }
        public DateTime DiagnosisDate { get; set; }
    }
    public class PatientEncounterCodesMappingViewModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string ICDCode { get; set; }
        public bool IsMapped { get; set; }
        
    }
}
