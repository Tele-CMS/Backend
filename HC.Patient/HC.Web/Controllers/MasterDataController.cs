//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/MasterData")]
    [ActionFilter]
    public class MasterDataController : BaseController
    {
        private readonly IMasterDataService _masterDataService;
        private readonly IRoundingRuleService _roundingRuleService;
        private readonly IMasterServicesService _masterServicesService;
        private JsonModel response;

        #region Construtor of the class
        public MasterDataController(IMasterDataService masterDataService, IRoundingRuleService roundingRuleService, IMasterServicesService masterServicesService)
        {
            this._masterDataService = masterDataService;
            _roundingRuleService = roundingRuleService;
            _masterServicesService = masterServicesService;
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        }
        #endregion

        #region Class Methods        
        /// <summary>
        /// Get master Data by Name
        /// </summary>
        /// <param name="masterDataNames"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MasterDataByName")]
        public JsonResult MasterDataByName([FromBody]JObject masterDataNames)
        {
            List<string> masterDataNamesList = new List<string>(Convert.ToString(masterDataNames["masterdata"]).Split(','));
            return Json(_masterDataService.ExecuteFunctions(() => _masterDataService.GetMasterDataByName(masterDataNamesList, GetToken(HttpContext))));
        }

        /// <summary>
        /// to get State by country ID
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStateByCountryID")]
        public JsonResult GetStateByCountryID(int countryID)
        {
            var masterStates = _masterDataService.GetStateByCountryID(countryID);
            if (masterStates != null && masterStates.Count > 0)
            {
                Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                return Json(new JsonModel(masterStates, StatusMessage.Success, (int)HttpStatusCodes.OK));//(Status Ok)
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                return Json(response);//(Not Found)
            }
        }
        #endregion

        #region Helping Methods        
        #endregion

        #region Class Methods        
        /// <summary>
        /// Save Template and Rules in template
        /// </summary>
        /// <param name="roundingRules"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRoundingRule")]
        public JsonResult SaveRoundingRule([FromBody]RoundingRuleModel roundingRules)
        {
            return Json(_roundingRuleService.ExecuteFunctions(() => _roundingRuleService.SaveRoundingRules(roundingRules, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetRoundingRuleById")]
        public JsonResult GetRoundingRuleById(int id)
        {
            return Json(_roundingRuleService.ExecuteFunctions(() => _roundingRuleService.GetRoundingRuleById(id)));
        }

        [HttpGet]
        [Route("GetRoundingRules")]
        public JsonResult GetRoundingRules()
        {
            return Json(_roundingRuleService.ExecuteFunctions(() => _roundingRuleService.GetRoundingRules(GetToken(HttpContext))));
        }
        [HttpPatch]
        [Route("DeleteRoundingRule")]
        public JsonResult DeleteRoundingRule(int Id)
        {
            return Json(_roundingRuleService.ExecuteFunctions(() => _roundingRuleService.DeleteRoundingRule(Id, GetToken(HttpContext))));
        }


        [HttpGet]
        [Route("GetRoundingRulesList")]
        public JsonResult GetRoundingRulesList(string SearchText, int PageNumber, int PageSize, string SortColumn, string SortOrder)
        {
            return Json(_roundingRuleService.GetRoundingRules(SearchText, GetToken(HttpContext).OrganizationID, PageNumber, PageSize, SortColumn, SortOrder));
        }

        /// <summary>        
        /// <Description> this will get the autocomplete values dynamically</Description>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAutoComplateSearchingValues")]
        public JsonResult GetAutoComplateSearchingValues(string tableName, string columnName, string searchText)
        {
            return Json(_masterDataService.GetAutoComplateSearchingValues(tableName, columnName, searchText, GetToken(HttpContext)));
        }
        #endregion

        #region Master Services
        [HttpGet]
        [Route("GetMasterServices")]
        public JsonResult GetMasterServices([FromQuery]MasterServiceFilterModel masterServiceFilterModel)
        {
            return Json(_masterServicesService.ExecuteFunctions(() => _masterServicesService.getMasterServicesByOrganizationId(GetToken(HttpContext), masterServiceFilterModel)));
        }

        [HttpGet]
        [Route("GetMasterServicesById")]
        public JsonResult GetMasterServicesById(string id)
        {
            return Json(_masterServicesService.ExecuteFunctions(() => _masterServicesService.getMasterServicesById(GetToken(HttpContext), id)));
        }

        [HttpGet]
        [Route("CheckServiceNameExistance")]
        public JsonResult CheckUsernameExistance(string name)
        {

            return Json(_masterServicesService.ExecuteFunctions(() => _masterServicesService.ChecServiceNameExistance(GetToken(HttpContext), name)));
        }

        [HttpPost]
        [Route("SaveMasterService")]
        public JsonResult SaveMasterService([FromBody]MasterServicesModel masterServicesModel)
        {
            return Json(_masterServicesService.ExecuteFunctions(() => _masterServicesService.SaveUpdateMasterService(GetToken(HttpContext), masterServicesModel)));
        }

        [HttpPatch]
        [Route("DeleteMasterService")]
        public JsonResult DeleteMasterService(string id)
        {

            return Json(_masterServicesService.ExecuteFunctions(() => _masterServicesService.DeleteMasterService(GetToken(HttpContext), id)));
        }
        #endregion
    }
}