using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsGuardian")]
    [ActionFilter]
    public class PatientsGuardianController : BaseController
    {
        private readonly IPatientGuardianService _patientGuardianService;

        public PatientsGuardianController(IPatientGuardianService patientGuardianService)
        {
            _patientGuardianService = patientGuardianService;
        }

        /// <summary>
        /// Description  : this method is used to get patient guardians 
        /// </summary>
        /// <param name="patientGuartdianFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPatientGuardian")]
        public JsonResult GetPatientGuardian(PatientGuartdianFilterModel patientGuartdianFilterModel)
        {
            return Json(_patientGuardianService.ExecuteFunctions(() => _patientGuardianService.GetPatientGuardian(patientGuartdianFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to get patient guardians by id  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPatientGuardianById")]
        public JsonResult GetPatientGuardianById(int id)
        {
            return Json(_patientGuardianService.ExecuteFunctions(() => _patientGuardianService.GetPatientGuardianById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to save/update patient guardian  
        /// </summary>
        /// <param name="patientGuardianModel"></param>
        /// <returns></returns>
        [HttpPost("CreateUpdatePatientGuardian")]
        public JsonResult CreateUpdatePatientGuardian([FromBody]PatientGuardianModel patientGuardianModel)
        {
            return Json(_patientGuardianService.ExecuteFunctions(() => _patientGuardianService.CreateUpdatePatientGuardian(patientGuardianModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to delete patient guardian  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeletePatientGuardian")]
        public JsonResult DeletePatientGuardian(int id)
        {
            return Json(_patientGuardianService.ExecuteFunctions(() => _patientGuardianService.Delete(id, GetToken(HttpContext))));
        }

    }
}