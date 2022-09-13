using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Service.IServices.AppConfiguration;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AppConfigurations")]
    [ActionFilter]
    public class AppConfigurationsController : BaseController
    {
        private readonly IAppConfigurationService _appConfigurationService;
        public AppConfigurationsController(IAppConfigurationService appConfigurationService)
        {
            _appConfigurationService = appConfigurationService;
        }
        /// <summary>        
        /// Description  : get all app settings
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAppConfigurations")]
        public JsonResult GetAppConfigurations()
        {
            return Json(_appConfigurationService.ExecuteFunctions(() => _appConfigurationService.GetAppConfigurations(GetToken(HttpContext))));
        }


        /// <summary>        
        /// Description  : updating all settings
        /// </summary>
        /// <param name="appConfigurationsModels"></param>
        /// <returns></returns>
        [HttpPost("UpdateAppConfiguration")]
        public JsonResult GetAppConfigurations([FromBody]List<AppConfigurationsModel> appConfigurationsModels)
        {
            return Json(_appConfigurationService.ExecuteFunctions(() => _appConfigurationService.UpdateAppConfiguration(appConfigurationsModels, GetToken(HttpContext))));
        }

    }
}