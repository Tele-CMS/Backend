using HC.Model;
using HC.Patient.Model.SpringBPatient;
using HC.Patient.Service.IServices.SpringBPatientsVitals;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("SpringBData")]
    [ActionFilter]
    //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
    public class SpringBPatientsVitalsController : BaseController
    {
        private readonly ISpringBPatientsVitalsService _springBPatientsVitalsService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="springBPatientsVitalsService"></param>
        public SpringBPatientsVitalsController(ISpringBPatientsVitalsService springBPatientsVitalsService)
        {
            _springBPatientsVitalsService = springBPatientsVitalsService;
        }
        /// <summary>
        /// Get springB patient vital
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSpringBVital")]
       // [Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetSpringBVital(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel)
        {

            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetSpringBPatientVitals(patientId, fromDate, toDate, filterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get SprinB Patient Diagnosis
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSpringBPatientDiagnosis")]
        //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetSpringBPatientDiagnosis(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, bool isShowAlert)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetSpringBPatientDiagnosis(patientId, fromDate, toDate, filterModel, isShowAlert, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get SpringB Patient Vitals MobileView
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        //[HttpGet("GetSpringBPatientVitalsMobileView")]
        ////[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        //public JsonResult GetSpringBPatientVitalsMobileView(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel)
        //{
        //    return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetSpringBPatientVitalsMobileView(patientId, fromDate, toDate, filterModel, GetToken(HttpContext))));

        //}
        /// <summary>
        /// Get springb patient medication
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <param name="isShowAlert"></param>
        /// <returns></returns>
        [HttpGet("GetSpringBPatientMedication")]
        //[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        public JsonResult GetSpringBPatientMedication(PatientFilterModel patientFilterModel, bool isShowAlert)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetMedication(patientFilterModel, isShowAlert, GetToken(HttpContext))));
        }
        /// <summary>
        /// Link SpringB Claims medication to Current Medication
        /// </summary>
        /// <param name="medicationDataFilterModel"></param>
        /// <returns></returns>
        [HttpPost("AddClaimsMedToCurrent")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult AddClaimsMedToCurrent([FromBody] MedicationDataFilterModel medicationDataFilterModel)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.AddClaimsMedToCurrent(medicationDataFilterModel, GetToken(HttpContext))));
        }
        //[HttpGet("GetSpringBPatientLabResults")]
        ////[Authorize(Roles = "ADMIN, STAFF, CLIENT")]
        //public JsonResult GetSpringBPatientLabResults(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel)
        //{
        //    return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetSpringBPatientLabResults(patientId, fromDate, toDate, filterModel, GetToken(HttpContext))));

        //}

        //[HttpGet("GetLoincCodeDetail")]
        ////[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult GetLoincCodeDetail(LabFilterModel labFilterModel)
        //{
        //    return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetLoincCodeDetail(labFilterModel, GetToken(HttpContext))));
        //}

        //[HttpPost("SaveLoincCodeDetails")]
        ////[Authorize(Roles = "ADMIN, STAFF")]
        //public JsonResult SaveLoincCodeDetails([FromBody] List<LoincCodeDetailModel> loincCodeDetailModels, int LinkedEncounterId)
        //{
        //    return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.SaveLoincCodeDetails(loincCodeDetailModels, LinkedEncounterId, GetToken(HttpContext))));
        //}

        [HttpGet("GetDistinctDateOfVitals")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetDistinctDateOfVitals(int PatientId)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetDistinctDateOfVitals(PatientId, GetToken(HttpContext))));
        }


        #region Patient Medication
        [HttpGet("GetMasterMedicationAutoComplete")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetMasterMedicationAutoComplete(string SearchText)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetMasterMedicationAutoComplete(SearchText, GetToken(HttpContext))));
        }

        [HttpGet("GetPatientMedicationDetail")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetPatientMedicationDetail(int PatientMedicationID)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetPatientMedicationDetail(PatientMedicationID, GetToken(HttpContext))));
        }

        [HttpPost("SavePatientMedication")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult SavePatientMedication([FromBody]PatMedicationModel patMedicationModel)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.SavePatientMedication(patMedicationModel, GetToken(HttpContext))));

        }
        [HttpPatch("DeletePatientMedication")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult DeletePatientMedication(int PatientMedicationID, int linkedEncounterId)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.DeletePatientMedication(PatientMedicationID, linkedEncounterId, GetToken(HttpContext))));
        }
        #endregion


        [HttpGet("GetLatestPatientVitalDetail")]
        //[Authorize(Roles = "ADMIN, STAFF")]
        public JsonResult GetLatestPatientVitalDetail(int patientId, DateTime filterDate)
        {
            return Json(_springBPatientsVitalsService.ExecuteFunctions(() => _springBPatientsVitalsService.GetLatestPatientVitalDetail(patientId, filterDate, GetToken(HttpContext))));
        }
    }
}