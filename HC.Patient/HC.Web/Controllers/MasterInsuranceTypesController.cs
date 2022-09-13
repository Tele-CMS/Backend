using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterInsuranceTypes")]
    [ActionFilter]
    public class MasterInsuranceTypesController : BaseController
    {
        private readonly IMasterInsuranceTypeService _masterInsuranceTypeService;
        public MasterInsuranceTypesController(IMasterInsuranceTypeService masterInsuranceTypeService)
        {
            _masterInsuranceTypeService = masterInsuranceTypeService;
        }
        /// <summary>
        ///  Description  : get all insurance types
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetInsuranceTypes")]
        public JsonResult GetInsuranceTypes(SearchFilterModel searchFilterModel)
        {
            return Json(_masterInsuranceTypeService.ExecuteFunctions(() => _masterInsuranceTypeService.GetInsuranceTypes(searchFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        ///  Description  : save/update insurance type
        /// </summary>
        /// <param name="masterInsuranceTypeModel"></param>
        /// <returns></returns>
        [HttpPost("SaveInsuranceType")]
        public JsonResult SaveInsuranceType([FromBody]MasterInsuranceTypeModel masterInsuranceTypeModel)
        {
            return Json(_masterInsuranceTypeService.ExecuteFunctions(() => _masterInsuranceTypeService.SaveInsuranceType(masterInsuranceTypeModel, GetToken(HttpContext))));
        }
        /// <summary>
        ///  Description  : get insurance type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetInsuranceTypeById")]
        public JsonResult GetInsuranceTypeById(int id)
        {
            return Json(_masterInsuranceTypeService.ExecuteFunctions(() => _masterInsuranceTypeService.GetInsuranceTypeById(id, GetToken(HttpContext))));
        }
        /// <summary>
        ///  Description  : delete insurance type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteInsuranceType")]
        public JsonResult DeleteInsuranceType(int id)
        {
            return Json(_masterInsuranceTypeService.ExecuteFunctions(() => _masterInsuranceTypeService.DeleteInsuranceType(id, GetToken(HttpContext))));
        }
    }
}