using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramActivityModel
    {
        public int? DiseaseManagementPlanPatientActivityId { get; set; }
        public int? PatientDiseaseManagementProgramId { get; set; }
        public int DiseaseManagementProgramActivityId { get; set; }
        public int DiseaseManageProgramId { get; set; }
        public string Descriptions { get; set; }
        public string ActivityType { get; set; }
        public int Frequency { get; set; }
        public int FrequencyValue { get; set; }
        public string FrequencyDescription { get; set; }
        public string GoalResultValue { get; set; }
        public int? ActivityUnitTypeId { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public int? Sign { get; set; }
        public bool AssignActivityToPatient { get; set; }
        public string SignValue { get; set; }

        public List<PatientDiseaseManagementProgramActivityNotificationsModel> PatientDiseaseManagementProgramActivityNotifications { get; set; }
    }
    public class PatientQuestionnaireAggregatedResponseModel
    {
        public string Questionnaire { get; set; }
        public string DiseaseManagmentProgram { get; set; }
        public decimal? QScore { get; set; }
        public string Risk { get; set; }
    }
    public class PatientDiseaseManagementProgramActivityNotificationsModel
    {
        public int DiseaseManagmentPlanPatientActivityNotificationId { get; set; }
        public int DiseaseManagmentPlanPatientActivityId { get; set; }
        public int DiseaseManagementProgramActivityId { get; set; }
        public string Message { get; set; }
        public int NotificationFrequency { get; set; }
        public int NotificationTypeId { get; set; }
        public int NotificationFrequencyValue { get; set; }
        public int? Sign { get; set; }
        public bool IsDeleted { get; set; }
    }
}
