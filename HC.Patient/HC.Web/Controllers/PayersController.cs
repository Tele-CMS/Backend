using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Payers")]
    [ActionFilter]
    public class PayersController : BaseController
    {
        private readonly IPayerService _payerService;
        public PayersController(IPayerService payerService)
        {
            _payerService = payerService;
        }

        #region Payer information : Listing/Save/Update/Delete
        /// <summary>
        /// Get Payer List
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerList")]
        public JsonResult GetPayerList(SearchFilterModel searchFilterModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetPayerList(searchFilterModel, GetToken(HttpContext))));
        }
      
        /// <summary>
        /// Save Payers
        /// </summary>
        /// <param name="insuranceCompanyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePayerData")]
        public JsonResult SavePayerData([FromBody]InsuranceCompanyModel insuranceCompanyModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SavePayerData(insuranceCompanyModel, GetToken(HttpContext))));
        }
       
        /// <summary>
        /// Get Payer By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerDataById/{Id}")]
        public JsonResult GetPayerDataById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetPayerDataById(Id, GetToken(HttpContext))));
        }
      
        /// <summary>
        /// Delete Payer Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeletePayerData/{Id}")]
        public JsonResult DeletePayerData(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeletePayerData(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save keyword
        /// </summary>
        /// <param name="keywordmodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveKeyword")]
        public JsonResult SaveKeyword([FromBody] KeywordModel keywordmodel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SaveKeyword(keywordmodel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get keyword List
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetKeywordList")]
        public JsonResult GetKeywordList(SearchFilterModel searchFilterModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetKeywordList(searchFilterModel, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("ProviderAvailableListForMobile")]
        public JsonResult ProviderAvailableListForMobile([FromBody] AppointmentSearchModelForMobile appointmentSearchModel)
        {
            return Json(_payerService.ExecuteFunctions(() => _payerService.ProviderAvailableListForMobile(appointmentSearchModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get keyword By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetKeywordDataById/{Id}")]
        public JsonResult GetKeywordDataById(int Id) 
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetKeywordDataById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Keyword Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteKeywordData/{Id}")]
        public JsonResult DeleteKeywordData(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeleteKeywordData(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save CareCategory
        /// </summary>
        /// <param name="carecategorymodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCareCategory")]
        public JsonResult SaveCareCategory([FromBody] ProviderCareCategoryModel carecategorymodel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SaveCareCategory(carecategorymodel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get CareCategory By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCareCategoryById/{Id}")]
        public JsonResult GetCareCategoryById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetCareCategoryById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Keyword Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteCareCategory/{Id}")]
        public JsonResult DeleteCareCategory(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeleteCareCategory(Id, GetToken(HttpContext))));
        }


        /// <summary>
        /// Save CareCategory
        /// </summary>
        /// <param name="symptomatepatientreportdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSymptomatePatientReport")]
        public JsonResult SaveSymptomatePatientReport([FromBody] SymptomatePatientReportData symptomatepatientreportdata)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SaveSymptomatePatientReport(symptomatepatientreportdata, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get SymptomateReport By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSymptomateReportById/{Id}")]
        public JsonResult GetSymptomateReportById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetSymptomateReportById(Id, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get EncounterListing By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEncounterListingById/{Id}")]
        public JsonResult GetEncounterListingById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(()=> _payerService.GetEncounterListingById(Id, GetToken(HttpContext))));
        }


        /// <summary>
        /// Add Symptomate Report
        /// </summary>
        /// <param name="symptomatepatientreportdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSymptomateReport")]
        public JsonResult AddSymptomateReport([FromBody] SymptomatePatientReportData symptomatepatientreportdata)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.AddSymptomateReport(symptomatepatientreportdata, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : get listing of patient symptomate reports
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSymptomateReportListing")]
        public JsonResult GetSymptomateReportListing(PatientSymptomateFilterModel patientFilterModel)
        {
            return Json(_payerService.ExecuteFunctions(() => _payerService.GetSymptomateReportListing(patientFilterModel, GetToken(HttpContext))));
        }



        /// <summary>
        /// Save ProviderQuestionnaire Questions
        /// </summary>
        /// <param name="providerquestionnairemodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProviderQuestionnaireQuestions")]
        public JsonResult SaveProviderQuestionnaireQuestions([FromBody] ProviderQuestionnaireModel providerquestionnairemodel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SaveProviderQuestionnaireQuestions(providerquestionnairemodel, GetToken(HttpContext))));
        }

        ///// <summary>
        ///// Get QuestionnaireQuestions List
        ///// </summary>
        ///// <param name="searchFilterModel"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetQuestionnaireQuestionsList")]
        //public JsonResult GetQuestionnaireQuestionsList(SearchFilterModel searchFilterModel)
        //{
        //    return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionnaireQuestionsList(searchFilterModel, GetToken(HttpContext))));
        //}

        /// <summary>
        /// Get QuestionnaireQuestions List
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQuestionnaireQuestionsList")]
        public JsonResult GetQuestionnaireQuestionsList(ProviderQuestionnaireFilterModel searchFilterModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionnaireQuestionsList(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Question By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQuestionById/{Id}")]
        public JsonResult GetQuestionById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteQuestion/{Id}")]
        public JsonResult DeleteQuestion(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeleteQuestion(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save QuestionOptions
        /// </summary>
        /// <param name="questionoptionsmodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveQuestionOptions")]
        public JsonResult SaveQuestionOptions([FromBody] QuestionOptionsModel questionoptionsmodel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SaveQuestionOptions(questionoptionsmodel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get QuestionOptionDataById By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQuestionOptionDataById/{Id}")]
        public JsonResult GetQuestionOptionDataById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionOptionDataById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Add Questionnaire
        /// </summary>
        /// <param name="managequestionnairemodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddQuestionnaire")]
        public JsonResult AddQuestionnaire([FromBody] ManageQuestionnaireModel managequestionnairemodel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.AddQuestionnaire(managequestionnairemodel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Questionnaire List
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQuestionnaireList")]
        public JsonResult GetQuestionnaireList(SearchFilterModel searchFilterModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionnaireList(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Questionnaire By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQuestionnaireById/{Id}")]
        public JsonResult GetQuestionnaireById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetQuestionnaireById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteQuestionnaire/{Id}")]
        public JsonResult DeleteQuestionnaire(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeleteQuestionnaire(Id, GetToken(HttpContext))));
        }




        #endregion
    }
}