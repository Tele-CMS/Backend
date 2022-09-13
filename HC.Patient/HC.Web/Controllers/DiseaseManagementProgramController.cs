using HC.Model;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Service.IServices.DiseaseManagementProgram;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("DiseaseManagementProgram")]
    [ActionFilter]
  //  [Authorize(Roles = "ADMIN, STAFF")]
    public class DiseaseManagementProgramController : BaseController
    {
        private readonly IDiseaseManagementProgramService _diseaseManagementProgramService;
        private readonly IDiseaseManagementProgramActivityService _diseaseManagementProgramActivityService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diseaseManagementProgramService"></param>
        /// <param name="diseaseManagementProgramActivityService"></param>
        public DiseaseManagementProgramController(IDiseaseManagementProgramService diseaseManagementProgramService , IDiseaseManagementProgramActivityService diseaseManagementProgramActivityService)
        {
            _diseaseManagementProgramService = diseaseManagementProgramService;
            _diseaseManagementProgramActivityService = diseaseManagementProgramActivityService;
        }

        /// <summary>
        /// GetDiseaseManagementProgramList
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDiseaseManagementProgramList")]
        public JsonResult GetDiseaseManagementProgramList(FilterModel filterModel)
        {
            return Json(_diseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _diseaseManagementProgramService.GetDiseaseManagementProgramList(filterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// GetDiseaseManagementProgramActivityList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDiseaseManagementProgramActivityList")]
        public JsonResult GetDiseaseManagementProgramActivityList(int diseaseManagementProgramId, FilterModel filterModel)
        {
            return Json(_diseaseManagementProgramActivityService.ExecuteFunctions<JsonModel>(() => _diseaseManagementProgramActivityService.GetDiseaseManagementProgramActivityList(diseaseManagementProgramId,filterModel,GetToken(HttpContext))));

        }

        /// <summary>
        /// Get disease programs list with current enrollments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDiseaseProgramsListWithEnrollments")]
        public JsonResult GetDiseaseProgramsListWithEnrollments()
        {
            return Json(_diseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _diseaseManagementProgramService.GetDiseaseProgramsListWithEnrollments(GetToken(HttpContext))));
        }

        /// <summary>
        /// Get disease conditions list from program Ids
        /// </summary>
        /// <param name="ProgramIds"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDiseaseConditionsFromProgramIds")]
        public JsonResult GetDiseaseConditionsFromProgramIds(string ProgramIds)
        {
            return Json(_diseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _diseaseManagementProgramService.GetDiseaseConditionsFromProgramIds(ProgramIds, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Disease Management Program By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDiseaseManagementProgramListById")]
        public IActionResult GetDiseaseManagementProgramListById(int id)
        {
            return Json(_diseaseManagementProgramService.ExecuteFunctions(() => _diseaseManagementProgramService.GetDiseaseManagementProgramListById(id, GetToken(HttpContext))));
        }
        /// <summary>
        /// Save DiseaseManagement Program
        /// </summary>
        /// <param name="diseaseManagementProgramListModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveDiseaseManagementProgram")]
        public JsonResult SaveDiseaseManagementProgram([FromBody]DiseaseManagementProgramListModel diseaseManagementProgramListModel)
        {
            return Json(_diseaseManagementProgramService.ExecuteFunctions<JsonModel>(() => _diseaseManagementProgramService.SaveDiseaseManagementProgram(diseaseManagementProgramListModel, GetToken(HttpContext))));
        }
    }
}