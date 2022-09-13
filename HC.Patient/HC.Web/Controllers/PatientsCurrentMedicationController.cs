using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsCurrentMedication")]
    [ActionFilter]
    //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class PatientsCurrentMedication : BaseController
    {
        private readonly IPatientCurrentMedicationService _patientCurrentMedicationService;
        public PatientsCurrentMedication(IPatientCurrentMedicationService patientCurrentMedicationService)
        {
            _patientCurrentMedicationService= patientCurrentMedicationService;
        }

        /// <summary>
        /// Description : get listing of medications
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentMedication")]
        //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetCurrentMedication(PatientFilterModel patientFilterModel,bool isShowAlert)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentMedication(patientFilterModel, isShowAlert, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : save or update patient allergy
        /// </summary>
        /// <param name="patientsMedicationModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCurrentMedication")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SaveCurrentMedication([FromBody]PatientsCurrentMedicationModel patientsMedicationModel)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.SaveCurrentMedication(patientsMedicationModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Get medication by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentMedicationById")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetCurrentMedicationById(int id)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentMedicationById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Delete medication by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LinkedEncounterId"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCurrentMedication")]
        [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult DeleteCurrentMedication(int id, int LinkedEncounterId)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.DeleteCurrentMedication(id, LinkedEncounterId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Print patient current medication
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("PrintPatientCurrentMedication")]
       // [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public IActionResult PrintPatientCurrentMedication(int patientId , PatientFilterModel patientFilterModel)
        {
            MemoryStream tempStream = null;
            tempStream = _patientCurrentMedicationService.PrintPatientCurrentMedication(patientId, GetToken(HttpContext), patientFilterModel);
            return File(tempStream != null ? tempStream : new MemoryStream(), "application/pdf", "Patient Medication List");
        }

        /// <summary>
        /// Description : Get Current And Claim Medication List
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentAndClaimMedicationList")]
        public JsonResult GetCurrentAndClaimMedicationList(PatientFilterModel patientFilterModel)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentAndClaimMedicationList(patientFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : get listing of medications
        /// </summary>
        /// <param name="medicationName"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentMedicationStength")]
        [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetCurrentMedicationStength(string medicationName)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentMedicationStength(medicationName, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description : get listing of medications
        /// </summary>
        /// <param name="medicationName"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentMedicationUnit")]
        [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetCurrentMedicationUnit(string medicationName)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentMedicationUnit(medicationName, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description : get listing of medications
        /// </summary>
        /// <param name="medicationName"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentMedicationForm")]
        [Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetCurrentMedicationForm(string medicationName)
        {
            return Json(_patientCurrentMedicationService.ExecuteFunctions(() => _patientCurrentMedicationService.GetCurrentMedicationForm(medicationName, GetToken(HttpContext))));
        }

    }
}
