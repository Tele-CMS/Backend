using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterTag")]
    [ActionFilter]
    public class MasterTagController : BaseController
    {
        private readonly IMasterTagService _masterTagService;
        
        public MasterTagController(IMasterTagService masterTagService)
        {
            _masterTagService = masterTagService;            
        }

        /// <summary>
        ///Description  : get list of mater tag 
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetMasterTag")]
        public JsonResult GetMasterTag(SearchFilterModel searchFilterModel)
        {   
            return Json(_masterTagService.ExecuteFunctions(() => _masterTagService.GetMasterTag(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        ///Description  : save and update master tag 
        /// </summary>
        /// <param name="masterTagModel"></param>
        /// <returns></returns>
        [HttpPost("SaveMasterTag")]
        public JsonResult SaveMasterTag([FromBody]MasterTagModel masterTagModel)
        {   
            return Json(_masterTagService.ExecuteFunctions(() => _masterTagService.SaveMasterTag(masterTagModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : get master tag by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetMasterTagById")]
        public JsonResult GetMasterTagById(int id)
        {
            return Json(_masterTagService.ExecuteFunctions(() => _masterTagService.GetMasterTagById(id, GetToken(HttpContext))));
        }

        /// <summary>
        ///Description  : delete master tag (soft delete)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteMasterTag")]
        public JsonResult DeleteMasterTag(int id)
        {
            return Json(_masterTagService.ExecuteFunctions(() => _masterTagService.DeleteMasterTag(id, GetToken(HttpContext))));
        }
    }
}