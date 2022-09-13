using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.Payer;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Payer
{
    public interface IPayerService : IBaseService
    {
        JsonModel GetPayerList(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SavePayerData(InsuranceCompanyModel insuranceCompanyModel, TokenModel tokenModel);
        JsonModel GetPayerDataById(int id, TokenModel tokenModel);
        JsonModel DeletePayerData(int id, TokenModel tokenModel);
        JsonModel SaveKeyword(KeywordModel keywordmodel, TokenModel tokenModel);
        JsonModel GetKeywordList(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel ProviderAvailableListForMobile(AppointmentSearchModelForMobile appointmentSearchModel, TokenModel tokenModel);
        JsonModel GetKeywordDataById(int id, TokenModel tokenModel);
        JsonModel DeleteKeywordData(int id, TokenModel tokenModel);
        JsonModel SaveCareCategory(ProviderCareCategoryModel carecategorymodel, TokenModel tokenModel);
        JsonModel GetCareCategoryById(int id, TokenModel tokenModel);
        JsonModel DeleteCareCategory(int id, TokenModel tokenModel);
        JsonModel SaveSymptomatePatientReport(SymptomatePatientReportData symptomatepatientreportdata, TokenModel tokenModel);
        JsonModel GetSymptomateReportById(int id, TokenModel tokenModel);
        JsonModel GetEncounterListingById(int id, TokenModel tokenModel);
        JsonModel AddSymptomateReport(SymptomatePatientReportData symptomatepatientreportdata, TokenModel tokenModel);
        JsonModel GetSymptomateReportListing(PatientSymptomateFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel SaveProviderQuestionnaireQuestions(ProviderQuestionnaireModel providerquestionnairemodel, TokenModel tokenModel);
        //JsonModel GetQuestionnaireQuestionsList(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetQuestionnaireQuestionsList(ProviderQuestionnaireFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetQuestionById(int id, TokenModel tokenModel);
        JsonModel DeleteQuestion(int id, TokenModel tokenModel);
        JsonModel SaveQuestionOptions(QuestionOptionsModel questionoptionsmodel, TokenModel tokenModel);
        JsonModel GetQuestionOptionDataById(int id, TokenModel tokenModel);
        JsonModel AddQuestionnaire(ManageQuestionnaireModel managequestionnairemodel, TokenModel tokenModel);
        JsonModel GetQuestionnaireList(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetQuestionnaireById(int id, TokenModel tokenModel);
        JsonModel DeleteQuestionnaire(int id, TokenModel tokenModel);
    }
}
