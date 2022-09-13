//using Audit.WebApi;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientImmunizationController : CustomJsonApiController<Entity.PatientImmunization, int>
    {
        private readonly IDbContextResolver _dbContextResolver;

        
        private readonly IPatientCommonService _patientCommonService;
        
        #region Construtor of the class
        public PatientImmunizationController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientImmunization, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
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
            catch
            {

            }
        }
        #endregion

        #region Class Methods

        [HttpGet]
        /// <summary>        
        /// <remarks>Following are the relationship tables</remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// </summary>
        public override async Task<IActionResult> GetAsync()
        {
            // return result from base class
            return await base.GetAsync();
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientImmunization patientImmunization)
        {
            try
            {
                PatientImmunization alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientImmunization>().Where(l => l.Immunization == patientImmunization.Immunization && l.Id != patientImmunization.Id && l.PatientID == patientImmunization.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if PatientImmunization already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClientImmunizationAlreadyLink,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, patientImmunization);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode//(Error Code)
                });
            }            
        }


        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]PatientImmunization patientImmunization)
        {

            PatientImmunization alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientImmunization>().Where(l => l.Immunization == patientImmunization.Immunization && l.PatientID == patientImmunization.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
            if (alreadyExist != null)// if PatientImmunization already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ClientAllergyAlreadyLink,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                return await base.PostAsync(patientImmunization);
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