using HC.Model;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("SecurityQuestions")]
    [ActionFilter]
    public class SecurityQuestionsController : BaseController
    {

        private readonly IMasterSecurityQuestionService _masterSecurityQuestionService;
        public SecurityQuestionsController(IMasterSecurityQuestionService masterSecurityQuestionService)
        {
            _masterSecurityQuestionService = masterSecurityQuestionService;
        }

        /// <summary>
        /// Get Master Security Questions
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSecurityQuestions")]
        public JsonResult GetSecurityQuestions(SearchFilterModel searchFilterModel)
        {
            return Json(_masterSecurityQuestionService.ExecuteFunctions<JsonModel>(() => _masterSecurityQuestionService.GetSecurityQuestions(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save Master Security Questions
        /// </summary>
        /// <param name="masterSecurityQuestionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSecurityQuestions")]
        public JsonResult SaveSecurityQuestions([FromBody]MasterSecurityQuestionModel masterSecurityQuestionModel)
        {
            return Json(_masterSecurityQuestionService.ExecuteFunctions<JsonModel>(() => _masterSecurityQuestionService.SaveSecurityQuestions(masterSecurityQuestionModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Master Security Questions By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSecurityQuestionsById/{Id}")]
        public JsonResult GetSecurityQuestionsById(int Id)
        {
            return Json(_masterSecurityQuestionService.ExecuteFunctions<JsonModel>(() => _masterSecurityQuestionService.GetSecurityQuestionsById(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Delete Master Security Questions
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteSecurityQuestions/{Id}")]
        public JsonResult DeleteSecurityQuestions(int Id)
        {
            return Json(_masterSecurityQuestionService.ExecuteFunctions<JsonModel>(() => _masterSecurityQuestionService.DeleteSecurityQuestions(Id, GetToken(HttpContext))));
        }
    }
}