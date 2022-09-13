using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsDiagnosis")]
    [ActionFilter]
    public class PatientsDiagnosisController : BaseController
    {   
        private readonly IPatientDiagnosisService _patientDiagnosisService;
        
        #region Construtor of the class
        public PatientsDiagnosisController(IPatientDiagnosisService patientDiagnosisService)
        {
            _patientDiagnosisService = patientDiagnosisService;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Description  : this method is used to save patient diagnosis
        /// </summary>
        /// <param name="patientDiagnosisModel"></param>
        /// <returns></returns>
        [HttpPost("SavePatientDiagnosis")]
        public JsonResult SavePatientDiagnosis([FromBody]PatientDiagnosisModel patientDiagnosisModel)
        {
            return Json(_patientDiagnosisService.ExecuteFunctions(() => _patientDiagnosisService.SavePatientDiagnosis(patientDiagnosisModel, GetToken(HttpContext))));
        }


        /// <summary>
        /// Description  : this method is used to get patient's diagnosis  
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetDiagnosis")]
        public JsonResult GetDiagnosis(PatientFilterModel filterModel)
        {
            return Json(_patientDiagnosisService.ExecuteFunctions(() => _patientDiagnosisService.GetDiagnosis(filterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to get patient's diagnosis
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDiagnosisById")]
        public JsonResult GetDiagnosisById(int id)
        {
            return Json(_patientDiagnosisService.ExecuteFunctions(() => _patientDiagnosisService.GetDiagnosisById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to delete patient's diagnosis 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteDiagnosis")]
        public JsonResult DeleteDiagnosis(int id)
        {
            return Json(_patientDiagnosisService.ExecuteFunctions(() => _patientDiagnosisService.DeleteDiagnosis(id, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        /////////////////////////
        //helping methods
        #endregion
    }
}