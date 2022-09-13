using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsMedication")]
    [ActionFilter]
    public class PatientsMedicationController : BaseController
    {
        private readonly IPatientMedicationService _patientMedicationService;
        public PatientsMedicationController(IPatientMedicationService patientMedicationService)
        {
            _patientMedicationService= patientMedicationService;
        }

        /// <summary>
        /// Description : get listing of medications
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetMedication")]
        public JsonResult GetMedication(PatientFilterModel patientFilterModel)
        {
            return Json(_patientMedicationService.ExecuteFunctions(() => _patientMedicationService.GetMedication(patientFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : save or update patient allergy
        /// </summary>
        /// <param name="patientsMedicationModel"></param>
        /// <returns></returns>
        [HttpPost("SaveMedication")]
        public JsonResult SaveMedication([FromBody]PatientsMedicationModel patientsMedicationModel)
        {
            return Json(_patientMedicationService.ExecuteFunctions(() => _patientMedicationService.SaveMedication(patientsMedicationModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Get medication by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetMedicationById")]
        public JsonResult GetMedicationById(int id)
        {
            return Json(_patientMedicationService.ExecuteFunctions(() => _patientMedicationService.GetMedicationById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Delete medication by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteMedication")]
        public JsonResult DeleteMedication(int id)
        {
            return Json(_patientMedicationService.ExecuteFunctions(() => _patientMedicationService.DeleteMedication(id, GetToken(HttpContext))));
        }
    }
}