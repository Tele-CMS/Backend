using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsSocialHistory")]
    [ActionFilter]
    public class PatientsSocialHistoryController : BaseController
    {
        private readonly IPatientSocialHistoryService _patientSocialHistoryService;
        
        #region Construtor of the class
        public PatientsSocialHistoryController(IPatientSocialHistoryService patientSocialHistoryService)
        {
            _patientSocialHistoryService = patientSocialHistoryService;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Description  : this method is used to save/update patient social history
        /// </summary>
        /// <param name="patientSocialHistoryModel"></param>
        /// <returns></returns>
        [HttpPost("SavePatientSocialHistory")]
        public JsonResult SavePatientSocialHistory([FromBody]PatientSocialHistoryModel patientSocialHistoryModel)
        {
            return Json(_patientSocialHistoryService.ExecuteFunctions(() => _patientSocialHistoryService.SavePatientSocialHistory(patientSocialHistoryModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to get patient social history
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientSocialHistory")]
        public JsonResult GetPatientSocialHistory(int patientId)
        {
            return Json(_patientSocialHistoryService.ExecuteFunctions(() => _patientSocialHistoryService.GetPatientSocialHistory(patientId, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        #endregion
    }
}