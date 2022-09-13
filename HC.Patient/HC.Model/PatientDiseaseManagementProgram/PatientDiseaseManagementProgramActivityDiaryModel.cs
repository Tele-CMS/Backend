using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramActivityDiaryModel
    {
        public int Id { get; set; }
        public int DiseaseManagementPlanPatientActivityId { get; set; }
        public string Value { get; set; }
        public DateTime LoggedDate { get; set; }
        public int TotalRecords { get; set; }
    }
}
