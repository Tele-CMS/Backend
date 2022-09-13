using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterLocations")]
    [ActionFilter]
    public class MasterLocationsController : BaseController
    {
        private readonly ILocationService _locationService;
        public MasterLocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Description  : get all locations
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetLocations")]
        public JsonResult GetLocations(SearchFilterModel searchFilterModel)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.GetLocations(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : save/update locations
        /// </summary>
        /// <param name="locationModel"></param>
        /// <returns></returns>
        [HttpPost("SaveLocation")]
        public JsonResult SaveLocation([FromBody]LocationModel locationModel)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.SaveLocation(locationModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetLocationById")]
        public JsonResult GetLocationById(int id)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.GetLocationById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : delete locations by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteLocation")]
        public JsonResult DeleteLocation(int id)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.DeleteLocation(id, GetToken(HttpContext))));
        }

        [HttpGet("GetMinMaxOfficeTime")]
        public JsonResult GetMinMaxOfficeTime(string locationIds)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.GetMinMaxOfficeTime(locationIds, GetToken(HttpContext))));
        }

        /// <summary>
        /// Return all locations by Organization Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLocationsByOrganizationId")]
        public JsonResult GetAllLocationsByOrganizationId()
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.GetAllLocationsByOrganizationId(GetToken(HttpContext))));
        }

        [HttpGet("GetLocationByServiceLocationID")]
        public JsonResult GetLocationByServiceLocationID(int id)
        {
            return Json(_locationService.ExecuteFunctions(() => _locationService.GetLocationByServiceLocationID(id)));
        }
    }
}