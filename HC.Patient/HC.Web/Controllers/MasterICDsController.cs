using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterICDs")]
    [ActionFilter]
    public class MasterICDsController : BaseController
    {
        private readonly IMasterICDService _masterICDService;
        public MasterICDsController(IMasterICDService masterICDService)
        {
            _masterICDService = masterICDService;
        }
        /// <summary>        
        /// Get Master ICD Codes
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterICDCodes")]
        public JsonResult GetMasterICDCodes(SearchFilterModel searchFilterModel)
        {
            return Json(_masterICDService.ExecuteFunctions<JsonModel>(() => _masterICDService.GetMasterICDCodes(searchFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Save Master ICD Codes
        /// </summary>
        /// <param name="masterICDModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveMasterICDCodes")]
        public JsonResult SaveMasterICDCodes([FromBody]MasterICDModel masterICDModel)
        {
            return Json(_masterICDService.ExecuteFunctions<JsonModel>(() => _masterICDService.SaveMasterICDCodes(masterICDModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Master ICD Codes By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterICDCodesById/{Id}")]
        public JsonResult GetMasterICDCodesById(int Id)
        {
            return Json(_masterICDService.ExecuteFunctions<JsonModel>(() => _masterICDService.GetMasterICDCodesById(Id, GetToken(HttpContext))));
        }
        /// <summary>
        /// Delete Master ICD Codes
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteMasterICDCodes/{Id}")]
        public JsonResult DeleteMasterICDCodes(int Id)
        {
            return Json(_masterICDService.ExecuteFunctions<JsonModel>(() => _masterICDService.DeleteMasterICDCodes(Id, GetToken(HttpContext))));
        }
    }
}