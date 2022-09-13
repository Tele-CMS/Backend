using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("EDIGateways")]
    [ActionFilter]
    public class EDIGatewaysController : BaseController
    {
        private readonly IEdiGatewayService _ediGatewayService;
        public EDIGatewaysController(IEdiGatewayService ediGatewayService)
        {
            _ediGatewayService = ediGatewayService;
        }

        /// <summary>        
        /// Description  : get edi gateways
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetEDIGateways")]
        public JsonResult GetEDIGateways(SearchFilterModel searchFilterModel)
        {
            return Json(_ediGatewayService.ExecuteFunctions(() => _ediGatewayService.GetEDIGateways(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description  : save/update edi gateways
        /// </summary>
        /// <param name="eDIModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUpdate")]
        public JsonResult SaveUpdate([FromBody]EDIModel eDIModel)
        {
            return Json(_ediGatewayService.ExecuteFunctions(() => _ediGatewayService.SaveUpdate(eDIModel, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description  : get edi gateway by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetEDIGateWayById")]
        public JsonResult GetEDIGateWayById(int id)
        {
            return Json(_ediGatewayService.ExecuteFunctions(() => _ediGatewayService.GetEDIGateWayById(id, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description  : delete edi gateway by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteEDIGateway")]
        public JsonResult DeleteEDIGateway(int id)
        {
            return Json(_ediGatewayService.ExecuteFunctions(() => _ediGatewayService.DeleteEDIGateway(id, GetToken(HttpContext))));
        }
    }
}