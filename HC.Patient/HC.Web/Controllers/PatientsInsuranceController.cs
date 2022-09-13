using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsInsurance")]
    [ActionFilter]
    public class PatientsInsuranceController : BaseController
    {
        private readonly IPatientsInsuranceService _patientsInsuranceService;
        public PatientsInsuranceController(IPatientsInsuranceService patientsInsuranceService)
        {
            _patientsInsuranceService = patientsInsuranceService;
        }

        /// <summary>
        /// Description  : this method is used to save/update patient insurance as well as insured person
        /// </summary>
        /// <param name="patientInsuranceListModel"></param>
        /// <returns></returns>
        [HttpPost("SavePatientInsurance")]
        public JsonResult SavePatientInsurance([FromBody]List<PatientInsuranceModel> patientInsuranceListModel)
        {
            return Json(_patientsInsuranceService.SavePatientInsurance(patientInsuranceListModel, GetToken(HttpContext)));
        }

        /// <summary>
        ///   Description  : this method is used to get patient insurance by patientId 
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientInsurances")]
        public JsonResult GetPatientInsurances(HC.Model.PatientFilterModel filterModel)
        {
            return Json(_patientsInsuranceService.GetPatientInsurances(filterModel, GetToken(HttpContext)));
        }
    }
}