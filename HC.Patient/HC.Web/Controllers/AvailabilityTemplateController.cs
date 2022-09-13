//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Availability;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AvailabilityTemplateController : CustomJsonApiController<Entity.StaffAvailability, int>
    {

        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IStaffAvailabilityService _staffAvailabilityService;
               

        #region Construtor of the class
        public AvailabilityTemplateController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.StaffAvailability, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository, IStaffAvailabilityService staffAvailabilityService)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _staffAvailabilityService = staffAvailabilityService;
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                this._patientCommonService = patientCommonService;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Class Methods
        [HttpPost]
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]StaffAvailability availabilityTemplate)
        {
            if (ModelState.IsValid)
            {
                return await base.PostAsync(availabilityTemplate);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Not Found)
                });
            }

        }

        [HttpPost("postMultiple")]
        public JsonModel PostAsync([FromBody]AvailabilityModel entity)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return _staffAvailabilityService.SaveStaffAvailabilty(entity, token);
        }

        [HttpGet("GetAvailability/{staffID}/{isLeaveNeeded}")]
        public JsonResult GetAvailability(string staffID = "", bool isLeaveNeeded = false)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_staffAvailabilityService.GetStaffAvailabilty(staffID, token, isLeaveNeeded));
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]StaffAvailability availabilityTemplate)
        {
            try
            {
                return Json(new
                {
                    data = await base.PatchAsync(id, availabilityTemplate),
                    Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "User's Availability"),
                    StatusCode = Response.StatusCode//(Status unprocessed entity)
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    data = new object(),
                    Message = e.Message,
                    StatusCode = Response.StatusCode//(Status unprocessed entity)
                });
            }
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
        #endregion

        #region Helping Methods      
        #endregion
    }
}