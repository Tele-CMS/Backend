using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class InsuranceCompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InsuranceTypeId { get; set; }
        public string InsType { get; set; } //Insurance type
        public string Address { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public int? CountryID { get; set; }
        public string Fax { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        public string TPLCode { get; set; }
        public string InsOthers { get; set; }
        public bool IsEDIPayer { get; set; }
        public bool IsPractitionerIsRenderingProvider { get; set; }
        public int Form1500PrintFormat { get; set; }
        public string AdditionalClaimInfo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class KeywordModel
    {
        public int Id { get; set; }
        //public string KeywordName { get; set; }
        public List<HealthCareCategoryKeywordsModel> healthCareCategoryKeywords { get; set; }
        public int CareCategoryId { get; set; }
       
    }

    public class KeywordpaginationModel
    {
        public int Id { get; set; }
        public string KeywordName { get; set; }
        public string CareCategoryName { get; set; }
        public decimal TotalRecords { get; set; }

    }

    public class HealthcareKeywordsModel
    {
        
        public int Id { get; set; }
        public string KeywordName { get; set; }

        public int CareCategoryId { get; set; }

    }


    public class HealthCareCategoryKeywordsModel
    {

        public int Id { get; set; }
        public string KeywordName { get; set; }

        public int CareCategoryId { get; set; }

    }

    public class ProviderCareCategoryModel
    {
        public int Id { get; set; }
        public string CareCategoryName { get; set; }  

    }

    public class SymptomatePatientReportData
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string Sex { get; set; }

        public string Age { get; set; }

        public List<string> PatientReportedSymptoms { get; set; }
        public List<string> PatientPresentSymptoms { get; set; }
        public List<string> PatientAbsentSymptoms { get; set; }
        public List<string> PatientUnknownSymptoms { get; set; }
        public List<string> PatientFinalConditions { get; set; }

        public string ReportedSymptoms { get; set; }
        public string PresentSymptoms { get; set; }
        public string AbsentSymptoms { get; set; }
        public string UnknownSymptoms { get; set; }
        public string FinalConditions { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PrePatientID { get; set; }
    }

    public class PatientsSymptomaticReportModel
    {
        public int ReportId { get; set; }
        public int PatientId { get; set; }
        public DateTime CreatedDate { get; set; }      
        public decimal TotalRecords { get; set; }
        public string ReportName { get; set; }
        
    }

    public class PatientSymptomateFilterModel : FilterModel
    {
        public int PatientId { get; set; }
    }

    public class ProviderQuestionnaireModel
    {
        public int QuestionId { get; set; }
        public string QuestionNameName { get; set; }
        public int QuestionnaireId { get; set; }
    }

    public class QuestionpaginationModel
    {
        public int QuestionId { get; set; }
        public string QuestionNameName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalRecords { get; set; }

    }

    public class QuestionOptionsModel
    {
        public int Id { get; set; }
        //public string KeywordName { get; set; }
        public List<QuestionnaireQuestionOptionsModel> questionnaireoptions { get; set; }
        public int QuestionId { get; set; }

    }

    public class QuestionnaireQuestionOptionsModel
    {

        public int OptionId { get; set; }
        public string QuestionOptionName { get; set; }

        public int QuestionId { get; set; }

    }
    public class ManageQuestionnaireModel
    {
        public int QuestionnareId { get; set; }
        public string QuestionnnaireName { get; set; }

    }

    public class QuestionnairepaginationModel
    {
        public int QuestionnareId { get; set; }
        public string QuestionnnaireName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalRecords { get; set; }

    }

    public class ProviderQuestionnaireFilterModel
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public string sortOrder { get; set; } = string.Empty;
        public int QuestionnaireId { get; set; }
        public string SearchText { get; set; } = string.Empty;
    }
}
