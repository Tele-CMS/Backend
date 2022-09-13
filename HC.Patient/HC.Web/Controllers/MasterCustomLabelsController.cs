using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterCustomLabels")]
    [ActionFilter]
    public class MasterCustomLabelsController : BaseController
    {
        private readonly IMasterCustomLabelService _masterCustomLabelService;
        public MasterCustomLabelsController(IMasterCustomLabelService masterCustomLabelService)
        {
            _masterCustomLabelService = masterCustomLabelService;
        }

        /// <summary>        
        ///  Description  :  this method is used to get all masters of custom labels
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetCustomLabel")]
        public JsonResult GetCustomLabel(SearchFilterModel searchFilterModel)
        {
            return Json(_masterCustomLabelService.ExecuteFunctions(() => _masterCustomLabelService.GetCustomLabel(searchFilterModel, GetToken(HttpContext))));
        }

        /// <summary>        
        ///  Description  : this method is used to save masters of custom labels
        /// </summary>
        /// <param name="masterCustomLabelModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCustomLabel")]
        public JsonResult SaveCustomLabel([FromBody]MasterCustomLabelModel masterCustomLabelModel)
        {
            return Json(_masterCustomLabelService.ExecuteFunctions(() => _masterCustomLabelService.SaveCustomLabel(masterCustomLabelModel, GetToken(HttpContext))));
        }

        /// <summary>        
        ///  Description  :  this method is used to get masters of custom labels by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCustomLabelById")]
        public JsonResult GetCustomLabelById(int id)
        {
            return Json(_masterCustomLabelService.ExecuteFunctions(() => _masterCustomLabelService.GetCustomLabelById(id, GetToken(HttpContext))));
        }

        /// <summary>        
        ///  Description  :  this method is used to delete masters of custom labels by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCustomLabel")]
        public JsonResult DeleteCustomLabel(int id)
        {
            return Json(_masterCustomLabelService.ExecuteFunctions(() => _masterCustomLabelService.DeleteCustomLabel(id, GetToken(HttpContext))));
        }
    }
}