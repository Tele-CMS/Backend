using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Staff;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("StaffCustomLabel")]
    [ActionFilter]
    public class StaffCustomLabelController : BaseController
    {
        private readonly IStaffCustomLabelService _staffCustomLabelService;
        public StaffCustomLabelController(IStaffCustomLabelService staffCustomLabelService)
        {
            _staffCustomLabelService = staffCustomLabelService;

        }
        /// <summary>
        /// Get Custom Label for Staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStaffCustomLabels")]
        public JsonResult GetStaffCustomLabels(int staffId)
        {
            return Json(_staffCustomLabelService.ExecuteFunctions<JsonModel>(() => _staffCustomLabelService.GetStaffCustomLabels(staffId, GetToken(HttpContext))));
        }
        /// <summary>
        /// Save Staff Custom Labels
        /// </summary>
        /// <param name="staffCustomLabelModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCustomLabels")]
        public JsonResult SaveCustomLabels([FromBody]List<StaffCustomLabelModel> staffCustomLabelModel)
        {
            return Json(_staffCustomLabelService.ExecuteFunctions<JsonModel>(() => _staffCustomLabelService.SaveCustomLabels(staffCustomLabelModel, GetToken(HttpContext))));
        }
    }
}