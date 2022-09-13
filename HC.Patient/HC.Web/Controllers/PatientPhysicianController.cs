using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientPhysician")]
    [ActionFilter]
    // [Authorize(Roles = "ADMIN, STAFF")]
    public class PatientPhysicianController : BaseController
    {
        private readonly IPatientPhysicianService _patientPhysicianService;
        public PatientPhysicianController(IPatientPhysicianService patientPhysicianService)
        {
            _patientPhysicianService = patientPhysicianService;
        }

        #region Patient Physician: Listing/Save/Update/Delete
        /// <summary>
        /// <Description> this method is used to get patient physician by patient id </Description>        
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPatientPhysicianById")]
        public JsonResult GetPatientPhysicianById(int patientId, FilterModel filterModel)
        {
            return Json(_patientPhysicianService.ExecuteFunctions<JsonModel>(() => _patientPhysicianService.GetPatientPhysicianById(patientId, filterModel, GetToken(HttpContext))));
        }

        ///// <summary>
        ///// Save Patient Physician
        ///// </summary>
        ///// <param name="patientPhysicianModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("SavePatientPhysician")]
        //public JsonResult SavePatientPhysician([FromBody] PatientPhysicianModel patientPhysicianModel)
        //{
        //    return Json(_patientPhysicianService.ExecuteFunctions<JsonModel>(() => _patientPhysicianService.SavePatientPhysician(patientPhysicianModel, GetToken(HttpContext))));
        //}

        ///// <summary>
        ///// Get Patient Physician By Id
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetPatientPhysicianDataById")]
        //public JsonResult GetPatientPhysicianDataById(int Id)
        //{
        //    return Json(_patientPhysicianService.ExecuteFunctions<JsonModel>(() => _patientPhysicianService.GetPatientPhysicianDataById(Id, GetToken(HttpContext))));
        //}

        ///// <summary>
        ///// Delete Patient Physician Data
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <param name="linkedEncounterId"></param>
        ///// <returns></returns>
        //[HttpPatch]
        //[Route("DeletePatientPhysician")]
        //public JsonResult DeletePatientPhysician(int Id, int linkedEncounterId)
        //{
        //    return Json(_patientPhysicianService.ExecuteFunctions<JsonModel>(() => _patientPhysicianService.DeletePatientPhysician(Id, linkedEncounterId, GetToken(HttpContext))));
        //}

        #endregion
    }
}