//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.GlobalCodes;
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
    [Route("api/GlobalCode")]
    [ActionFilter]
    public class GlobalCodeController : BaseController
    {
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IRoundingRuleService _roundingRuleService;
        private readonly IMasterDataService _masterDataService;
        private JsonModel response;

        #region Construtor of the class
        public GlobalCodeController(IMasterDataService masterDataService, IRoundingRuleService roundingRuleService, IGlobalCodeService globalCodeService)
        {
            this._masterDataService = masterDataService;
            _roundingRuleService = roundingRuleService;
            _globalCodeService = globalCodeService;
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
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
        public JsonResult SaveRoundingRule([FromBody] RoundingRuleModel roundingRules)
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

        #region Global Coode
        [HttpGet]
        [Route("GetGlobalCodes")]
        public JsonResult GetGlobalCodes([FromQuery] GlobalCodeFilterModel globalCodeFilterModel)
        {
            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.GetGlobalCodeByOrganizationId(GetToken(HttpContext), globalCodeFilterModel)));
        }

        [HttpGet]
        [Route("GetGlobalServiceIconName")]
        public JsonResult GetGlobalServiceIconName([FromBody] GlobalCodeFilterModel globalCodeFilterModel)
        {
            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.GetGlobalServiceIconName(GetToken(HttpContext), globalCodeFilterModel)));
        }

        [HttpGet]
        [Route("GetGlobalCodeById")]
        public JsonResult GetGlobalCodeById(string id)
        {
            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.GetGlobalCodeById(GetToken(HttpContext), id)));
        }

        [HttpGet]
        [Route("CheckGlobalCodeExistance")]
        public JsonResult CheckUsernameExistance(string name)
        {

            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.CheckGlobalCodeExistance(name, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("SaveGlobalCode")]
        public JsonResult SaveGlobalCode([FromBody] GlobalCodeModel globalCodeModel)
        {
            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.SaveUpdateGlobalCode(globalCodeModel, GetToken(HttpContext))));
        }

        [HttpPatch]
        [Route("DeleteGlobalCode")]
        public JsonResult DeleteGlobalCode(int id)
        {

            return Json(_globalCodeService.ExecuteFunctions(() => _globalCodeService.DeleteGlobalCode(GetToken(HttpContext), id)));
        }
        #endregion
    }
}