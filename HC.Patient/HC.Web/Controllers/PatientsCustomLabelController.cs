using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsCustomLabel")]
    [ActionFilter]
    public class PatientsCustomLabelController : BaseController
    {
        private readonly IPatientCustomLabelService _patientCustomLabelService;

        #region Construtor of the class
        public PatientsCustomLabelController(IPatientCustomLabelService patientCustomLabelService)
        {
            _patientCustomLabelService = patientCustomLabelService;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Description  : this method is used to save patient's custom labels
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientCustomLabels")]
        public JsonResult GetPatientCustomLabels(int patientId)
        {
            return Json(_patientCustomLabelService.ExecuteFunctions(() => _patientCustomLabelService.GetPatientCustomLabels(patientId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to save patient's custom labels 
        /// </summary>
        /// <param name="customLabelModels"></param>
        /// <returns></returns>
        [HttpPost("SavePatientCustomLabels")]
        public JsonResult SavePatientCustomLabels([FromBody]List<CustomLabelModel> customLabelModels)
        {
            return Json(_patientCustomLabelService.ExecuteFunctions(() => _patientCustomLabelService.SaveCustomLabels(customLabelModels, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        #endregion
    }
}