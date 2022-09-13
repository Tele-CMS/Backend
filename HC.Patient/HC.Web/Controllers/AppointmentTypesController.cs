using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AppointmentTypes")]
    [ActionFilter]
    public class AppointmentTypesController : BaseController
    {
        private readonly IAppointmentTypeService _appointmentTypeService;
        
        #region Construtor of the class
        public AppointmentTypesController(IAppointmentTypeService appointmentTypeService)
        {
            _appointmentTypeService = appointmentTypeService;            
        }
        #endregion

        #region Class Methods
        /// <summary>        
        ///  Description  : this method is used to get all appointments type
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetAppointmentType")]
        public JsonResult GetAppointmentType(SearchFilterModel searchFilterModel)
        {
            TokenModel tokenModel = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_appointmentTypeService.ExecuteFunctions(() => _appointmentTypeService.GetAppointmentType(searchFilterModel, tokenModel)));
        }

        /// <summary>        
        /// Description  : this method is used to save and update appointments type
        /// </summary>
        /// <param name="appointmentTypesModel"></param>
        /// <returns></returns>
        [HttpPost("SaveAppointmentType")]
        public JsonResult SaveAppointmentType([FromBody]AppointmentTypesModel appointmentTypesModel)
        {
            TokenModel tokenModel = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_appointmentTypeService.ExecuteFunctions(() => _appointmentTypeService.SaveAppointmentType(appointmentTypesModel, tokenModel)));
        }

        /// <summary>        
        /// Description  :this method is used to get appointments by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAppointmentTypeById")]
        public JsonResult GetAppointmentTypeById(int id)
        {
            TokenModel tokenModel = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_appointmentTypeService.ExecuteFunctions(() => _appointmentTypeService.GetAppointmentTypeById(id, tokenModel)));
        }

        /// <summary>        
        /// Description  : this method is used to delete appointments type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteAppointmentType")]
        public JsonResult DeleteAppointmentType(int id)
        {
            TokenModel tokenModel = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_appointmentTypeService.ExecuteFunctions(() => _appointmentTypeService.DeleteAppointmentType(id, tokenModel)));
        }
        #endregion

        #region Helping Methods
        #endregion
    }
}