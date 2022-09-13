using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsAllergy")]
    [ActionFilter]
    public class PatientsAllergyController : BaseController
    {
        private readonly IPatientAllergyService _patientAllergyService;
        public PatientsAllergyController(IPatientAllergyService patientAllergyService)
        {
            _patientAllergyService = patientAllergyService;
        }

        /// <summary>
        /// Description : get listing of allergies
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetAllergies")]
        public JsonResult GetAllergies(PatientFilterModel patientFilterModel)
        {
            return Json(_patientAllergyService.ExecuteFunctions(()=>_patientAllergyService.GetAllergies(patientFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : save or update patient allergy
        /// </summary>
        /// <param name="patientAllergyModel"></param>
        /// <returns></returns>
        [HttpPost("SaveAllergy")]
        public JsonResult SaveAllergy([FromBody]PatientAllergyModel patientAllergyModel)
        {
            return Json(_patientAllergyService.ExecuteFunctions(() => _patientAllergyService.SaveAllergy(patientAllergyModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Get allergy by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAllergyById")]
        public JsonResult GetAllergyById(int id)
        {
            return Json(_patientAllergyService.ExecuteFunctions(() => _patientAllergyService.GetAllergyById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Delete allergy by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteAllergy")]
        public JsonResult DeleteAllergy(int id)
        {
            return Json(_patientAllergyService.ExecuteFunctions(() => _patientAllergyService.DeleteAllergy(id, GetToken(HttpContext))));
        }
    }
}