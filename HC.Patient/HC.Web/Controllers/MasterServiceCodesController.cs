using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Service.IServices.MasterServiceCodes;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/MasterServiceCodes")]
    [ActionFilter]
    public class MasterServiceCodesController : BaseController
    {
        private readonly IMasterServiceCodesService _masterServiceCodesService;
        public MasterServiceCodesController(IMasterServiceCodesService masterServiceCodesService)
        {
            _masterServiceCodesService = masterServiceCodesService;            
        }

        /// <summary>
        /// Description  : save and update master service code
        /// </summary>
        /// <param name="masterServiceCodesModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveMasterServiceCode")]
        public JsonResult SaveMasterServiceCode([FromBody]MasterServiceCodesModel masterServiceCodesModel)
        {   
            return Json(_masterServiceCodesService.ExecuteFunctions(()=> _masterServiceCodesService.AddUpdateMasterServiceCode(masterServiceCodesModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : get list of master service codes 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetMasterServiceCodes")]
        public JsonResult GetMasterServiceCodes(string searchText = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_masterServiceCodesService.ExecuteFunctions(()=> _masterServiceCodesService.GetMasterServiceCodes(searchText, GetToken(HttpContext), pageNumber, pageSize, sortColumn, sortOrder)));
        }

        /// <summary>
        ///Description  : delete service code with check its dependency
        /// </summary>
        /// <param name="serviceCodeId"></param>
        /// <returns></returns>
        [HttpPatch("DeleteMasterServiceCodes")]
        public JsonResult DeleteMasterServiceCodes(int serviceCodeId)
        {
            return Json(_masterServiceCodesService.ExecuteFunctions(()=> _masterServiceCodesService.DeleteServiceCode(serviceCodeId, GetToken(HttpContext))));
        }

        /// <summary>
        ///Description  : get master service code by id
        /// </summary>
        /// <param name="serviceCodeId"></param>
        /// <returns></returns>
        [HttpGet("GetMasterServiceCodeById")]
        public JsonResult GetMasterServiceCodeById(int serviceCodeId)
        {
            return Json(_masterServiceCodesService.ExecuteFunctions(()=> _masterServiceCodesService.GetMasterServiceCodeById(serviceCodeId, GetToken(HttpContext))));
        }
    }
}