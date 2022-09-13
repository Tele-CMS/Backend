using HC.Model;
using HC.Patient.Model.PatientMedicalFamilyHistory;
using HC.Patient.Service.IServices.PatientMedicalFamilyHistories;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientMedicalFamilyHistory")]
    [ActionFilter]
    public class PatientMedicalFamilyHistoryController : BaseController
    {
        private readonly IPatientMedicalFamilyHistoryService _patientMedicalFamilyHistoryService;
        #region Construtor of the class
        public PatientMedicalFamilyHistoryController(IPatientMedicalFamilyHistoryService patientMedicalFamilyHistoryService)
        {
            _patientMedicalFamilyHistoryService = patientMedicalFamilyHistoryService;
        }
        #endregion

        #region Class Methods

        [HttpPost]
        [Route("SavePatientMedicalfamilyHistory")]
        public JsonResult SavePatientMedicalfamilyHistory([FromBody]PatientMedicalFamilyHistoryModel patientMedicalFamilyHistoryModel)
        {
            return Json(_patientMedicalFamilyHistoryService.ExecuteFunctions(() => _patientMedicalFamilyHistoryService.SavePatientMedicalFamilyHistory(patientMedicalFamilyHistoryModel, GetToken(HttpContext))));

        }

        [HttpGet]
        [Route("GetPatientMedicalFamilyHistoryById")]
        public JsonResult GetPatientMedicalFamilyHistoryById(int Id = 0, int patientID = 0)
        {
            return Json(_patientMedicalFamilyHistoryService.ExecuteFunctions(() => _patientMedicalFamilyHistoryService.GetPatientMedicalFamilyHistoryById(Id, patientID)));
        }

        [HttpGet]
        [Route("GetPatientMedicalFamilyHistory")]
        public JsonResult GetPatientMedicalFamilyHistory(string firstName = "", string lastName = "", string Disease = "", string sortColumn = "", int pageNumber = 0, int pageSize = 10)
        {
            return Json(_patientMedicalFamilyHistoryService.ExecuteFunctions(() => _patientMedicalFamilyHistoryService.GetPatientMedicalFamilyHistory(firstName, lastName, Disease, sortColumn, pageNumber, pageSize)));
        }

        [HttpPatch]
        [Route("DeletePatientMedicalFamilyHistory")]
        public JsonResult DeletePatientMedicalFamilyHistory(int Id)
        {
            return Json(_patientMedicalFamilyHistoryService.ExecuteFunctions(() => _patientMedicalFamilyHistoryService.DeletePatientMedicalFamilyHistory(Id, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        #endregion
    }
}