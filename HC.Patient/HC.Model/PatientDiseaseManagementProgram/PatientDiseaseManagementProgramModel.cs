using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramModel
    {
        public int PatientDiseaseManagementProgramId { get; set; }
        public DateTime? DateOfEnrollment { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? GraduationDate { get; set; }
        public int DiseaseManageProgramId { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public string Frequency { get; set; }
        public string CareManager { get; set; }
        public int PatientId { get; set; }
        public string DiseaseManageProgram { get; set; }
        public int PatientQuestionnaireId { get; set; }
        public int TotalRecords { get; set; }

        public string FrequencyDescription { get; set; }
    }

    public class AllPatientsDiseaseManagementProgramModel
    {
        public int PatientDiseaseManagementProgramId { get; set; }
        public DateTime? DateOfEnrollment { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? GraduationDate { get; set; }
        public int DiseaseManageProgramId { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public string Frequency { get; set; }
        public string CareManager { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string DiseaseManageProgram { get; set; }
        public int PatientQuestionnaireId { get; set; }
       // public int TotalRecords { get; set; }

        public string FrequencyDescription { get; set; }
    }
    public class DMPPatientModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string IsEligible { get; set; }
        public string DiseaseCondition { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
        public int TotalRecords { get; set; }
        public DateTime? NextAppointmentDate { get; set; }
    }
    public class DMPEnrolleesModel
    {
        public string PatientName { get; set; }
        public string IsEligible { get; set; }
        public string DiseaseCondition { get; set; }
        public string DiseaseManagePrograms { get; set; }
        public string EnrollmentDates { get; set; }
        public string GraduationDates { get; set; }
        public string TerminationDates { get; set; }
        public string CareManagers { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
        public string NextAppointmentDate { get; set; }
    }
}
