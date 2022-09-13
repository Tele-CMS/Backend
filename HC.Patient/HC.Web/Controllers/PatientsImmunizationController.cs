using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsImmunization")]
    [ActionFilter]
    public class PatientsImmunizationController : BaseController
    {
        private readonly IPatientImmunizationService _patientImmunizationService;

        #region Construtor of the class
        public PatientsImmunizationController(IPatientImmunizationService patientImmunizationService)
        {
            _patientImmunizationService = patientImmunizationService;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Description  : this method is used to save patient immunization
        /// </summary>
        /// <param name="patientImmunizationModel"></param>
        /// <returns></returns>
        [HttpPost("SavePatientImmunization")]
        public JsonResult SavePatientImmunization([FromBody]PatientImmunizationModel patientImmunizationModel)
        {
            return Json(_patientImmunizationService.ExecuteFunctions(() => _patientImmunizationService.SavePatientImmunization(patientImmunizationModel,GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to get patient immunizations
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetImmunization")]
        public JsonResult GetImmunization(PatientFilterModel filterModel)
        {
            return Json(_patientImmunizationService.ExecuteFunctions(() => _patientImmunizationService.GetImmunization(filterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to get patient immunization by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetImmunizationById")]
        public JsonResult GetImmunizationById(int id)
        {
            return Json(_patientImmunizationService.ExecuteFunctions(() => _patientImmunizationService.GetImmunizationById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to delete patient immunization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteImmunization")]
        public JsonResult DeleteImmunization(int id)
        {
            return Json(_patientImmunizationService.ExecuteFunctions(() => _patientImmunizationService.DeleteImmunization(id, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        #endregion

    }
}