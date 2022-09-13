using HC.Patient.Model.Questionnaire;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.DiseaseManagementProgram
{
    public class DiseaseManagementProgramListModel
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
    }
    public class AssignNewProgramModel
    {
        public int PatientId { get; set; }
        public List<ProgramModel> PatientDiseaseManagementPrograms { get; set; }
        public List<LogModel> Logs { get; set; }

    }
    public class ProgramModel
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public DateTime? DateOfEnrollment { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public DateTime? GraduationDate { get; set; }
        public int? StatusId { get; set; }
        public int? FrequencyId { get; set; }
        public int? CareManagerId { get; set; }
        public string OtherFrequencyDescription { get; set; }
    }
    public class DiseaseProgramsListWithEnrollModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsEnrolled { get; set; }
        public int TotalRecords { get; set; }
    }
    public class DiseaseConditionsListModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ProgramId { get; set; }
        public int TotalRecords { get; set; }
    }

    public class HRAProgramReportLogModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
        public string ProviderName { get; set; }
        public int TotalRecords { get; set; }

    }


}
