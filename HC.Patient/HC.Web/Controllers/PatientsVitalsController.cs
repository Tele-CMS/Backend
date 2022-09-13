using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsVitals")]
    [ActionFilter]
    public class PatientsVitalsController : BaseController
    {
        private readonly IPatientVitalService _patientVitalService;
        public PatientsVitalsController(IPatientVitalService patientVitalService)
        {
            _patientVitalService = patientVitalService;
        }

        /// <summary>
        /// Description : get listing of vitals
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetVitals")]
        public JsonResult GetVitals(PatientFilterModel patientFilterModel)
        {
            return Json(_patientVitalService.ExecuteFunctions(() => _patientVitalService.GetVitals(patientFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : save or update patient vitals
        /// </summary>
        /// <param name="patientVitalModel"></param>
        /// <returns></returns>
        [HttpPost("SaveVital")]
        public JsonResult SaveVital([FromBody]PatientVitalModel patientVitalModel)
        {
            return Json(_patientVitalService.ExecuteFunctions(() => _patientVitalService.SaveVital(patientVitalModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Get vital by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetVitalById")]
        public JsonResult GetVitalById(int id)
        {
            return Json(_patientVitalService.ExecuteFunctions(() => _patientVitalService.GetVitalById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Delete vital by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteVital")]
        public JsonResult DeleteVital(int id)
        {
            return Json(_patientVitalService.ExecuteFunctions(() => _patientVitalService.DeleteVital(id, GetToken(HttpContext))));
        }
    }
}