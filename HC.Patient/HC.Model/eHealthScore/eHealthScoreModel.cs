using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.eHealthScore
{
   public class eHealthScoreModel
    {
        public PatientDetailsForeHealthScoreModel PatientDetailsForeHealthScoreModel { get; set; }
        public List<PatientConditionForeHealthScoreModel> PatientConditionForeHealthScoreModel { get; set; }
        public List<BiometricsAndHRAResultModel> BiometricsAndHRAResultModel { get; set; }
        public List<BiometricsBenchmarkModel> BiometricsBenchmarkModel { get; set; }
    }
    public class PatientDetailsForeHealthScoreModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string FirstDescriptionParagraph { get;set;}
        public string SecondDescriptionParagraph { get; set; }
        public string ThirdDescriptionParagraph { get; set;}
        public string FourthDescriptionParagraph { get; set; }
        public string FifthDescriptionParagraph { get; set; }
        public string NoteDescriptionParagraph { get; set; }
        public string NoneObserveConditions { get; set; }
        public string ObservedConditionText { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
    }
    public class PatientConditionForeHealthScoreModel
    {
        public int PatientDiseaseConditionId { get; set; }
        public string Condition { get; set; }
        public string DiseaseConditionNotes { get; set; }
        public string ObservedCondition { get; set; }
        public string SourceUsed { get; set; }
        public DateTime DateIdentified { get; set; }
        public bool IsConditionShowFlag { get; set; }
    }
    public class BiometricsAndHRAResultModel
    {
        public int Id { get; set; }
        public string LOINCCode { get; set; }
        public string BiometricName { get; set; }
        public string RecommendedGuidelines { get; set; }
        public decimal? BiometricValue { get; set; }
        public string BiometricValueString { get; set; }
        public DateTime? BiometricDate { get; set; }
        public string Risk { get; set; }
    }
    public class BiometricsBenchmarkModel
    {
       public int MasterHealtheScoreBiometricsId { get; set; }
        public string LOINCCode { get; set; }
        public string Risk { get; set; }
        public string FontColor { get; set; }
        public decimal LowValue { get; set; }
        public decimal HighValue { get; set; }
        public string DisplayRiskRanges { get; set; }
    }
}
