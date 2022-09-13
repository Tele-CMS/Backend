using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsPrescription")]
    [ActionFilter]
    public class PatientsPrescriptionController : BaseController
    {
        private readonly IPatientPrescriptionService _patientprescription;
        public PatientsPrescriptionController(IPatientPrescriptionService patientprescription)
        {
            _patientprescription = patientprescription;
        }

        [HttpGet]
        [Route("GetprescriptionDrugList")]
        public JsonResult GetprescriptionDrugList(int id)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.GetprescriptionDrugList()));
        }

        /// <summary>
        /// Description : save or update patient prescription
        /// </summary>
        /// <param name="patientsPrescriptionModel"></param>
        /// <returns></returns>
        [HttpPost("SavePrescription")]
        public JsonResult SavePrescription([FromBody] PatientsPrescriptionModel patientsPrescriptionModel)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.SavePrescription(patientsPrescriptionModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : get listing of prescription
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPrescriptions")]
        public JsonResult GetPrescriptions(PatientFilterModel patientFilterModel)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.GetPrescription(patientFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Get prescription by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPrescriptionById")]
        public JsonResult GetPrescriptionById(int id)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.GetPrescriptionById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description : Delete prescription by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeletePrescription")]
        public JsonResult DeletePrescription(int id)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.DeletePrescription(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// To generate and download the prescription pdf
        /// </summary>
        /// <param name="patientsPrescriptionfaxModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPrescriptionPdf")]
        public IActionResult GetPrescriptionPdf(PatientFaxModel patientsPrescriptionfaxModel)
        {
            MemoryStream tempStream = null;
            tempStream = _patientprescription.GetPrescriptionPdf(patientsPrescriptionfaxModel, GetToken(HttpContext));
            return File((tempStream != null ? tempStream : new MemoryStream()), "application/pdf", Convert.ToString(patientsPrescriptionfaxModel.PrescriptionId) + "-" + "DrugPrescription");
        }

        /// <summary>
        /// Description : send fax
        /// </summary>
        /// <param name="patientsPrescriptionfaxModel"></param>
        /// <returns></returns>
        [HttpPost("SendFax")]
        public JsonResult SendFax([FromBody] PatientFaxModel patientsPrescriptionfaxModel)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.SendFax(patientsPrescriptionfaxModel, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Get Master Prescription Drugs
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterprescriptionDrugs")]
        public JsonResult GetMasterprescriptionDrugs(SearchFilterModel searchFilterModel)
        {
            return Json(_patientprescription.ExecuteFunctions<JsonModel>(() => _patientprescription.GetMasterprescriptionDrugs(searchFilterModel, GetToken(HttpContext))));
        }
        /// <summary>        
        /// Get Master Pharmacy
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterPharmacy")]
        public JsonResult GetMasterPharmacy(SearchFilterModel searchFilterModel)
        {
            return Json(_patientprescription.ExecuteFunctions<JsonModel>(() => _patientprescription.GetMasterPharmacy(searchFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description : get listing of prescription
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSentPrescriptions")]
        public JsonResult GetSentPrescriptions(PatientFilterModel patientFilterModel)
        {
            return Json(_patientprescription.ExecuteFunctions(() => _patientprescription.GetSentPrescriptions(patientFilterModel, GetToken(HttpContext))));
        }
    }
}