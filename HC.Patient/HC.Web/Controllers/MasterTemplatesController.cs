using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterTemplates")]
    [ActionFilter]
    public class MasterTemplatesController : BaseController
    {
        private readonly IMasterTemplatesService _masterTemplatesService;
        public MasterTemplatesController(IMasterTemplatesService masterTemplatesService)
        {
            _masterTemplatesService = masterTemplatesService;
        }

        /// <summary>        
        ///  Description  :  this method is used to get all master templates
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetMasterTemplates")]
        public JsonResult GetMasterTemplates(SearchFilterModel searchFilterModel)
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.GetMasterTemplates(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>        
        ///  Description  :  this method is used to save master templates
        /// </summary>
        /// <param name="masterTemplateModel"></param>
        /// <returns></returns>
        [HttpPost("SaveMasterTemplate")]
        public JsonResult SaveMasterTemplate([FromBody]MasterTemplatesModel masterTemplateModel)
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.SaveMasterTemplate(masterTemplateModel, GetToken(HttpContext))));
        }
        /// <summary>        
        ///  Description  :  this method is used to get master template by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetMasterTemplateById")]
        public JsonResult GetMasterTemplateById(int id)
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.GetMasterTemplateById(id, GetToken(HttpContext))));
        }

        /// <summary>        
        ///  Description  :  this method is used to delete master template by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteMasterTemplate")]
        public JsonResult DeleteMasterTemplate(int id)
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.DeleteMasterTemplate(id, GetToken(HttpContext))));
        }
        /// <summary>        
        ///  Description  : gets all the master template category for drop down
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMasterTemplatesCategoryForDD")]
        public JsonResult GetMasterTemplatesCategoryForDD()
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.GetMasterTemplatesCategoryForDD(GetToken(HttpContext))));
        }
        /// <summary>        
        ///  Description  : gets all the master template sub category for drop down using masterTemplateCategoryId
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMasterTemplatesSubCategoryForDD")]
        public JsonResult GetMasterTemplatesSubCategoryForDD(int masterTemplateCategoryId)
        {
            return Json(_masterTemplatesService.ExecuteFunctions(() => _masterTemplatesService.GetMasterTemplatesSubCategoryForDD(masterTemplateCategoryId,GetToken(HttpContext))));
        }
    }
}
