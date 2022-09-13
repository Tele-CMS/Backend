//using Audit.WebApi;
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
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PatientGuardianController : CustomJsonApiController<Entity.PatientGuardian, int>
    {
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;

        
        #region Construtor of the class
        public PatientGuardianController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientGuardian, int> resourceService,
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
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }
            catch(Exception)
            {
            }
        }
        #endregion

        #region Class Methods
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return await base.GetAsync();
        }

        [HttpPost]        
        public override async Task<IActionResult> PostAsync([FromBody]PatientGuardian patientInfo)
        {
            //return await base.PostAsync(patientInfo);
            return Json(new
            {
                data = await base.PostAsync(patientInfo),
                Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Guardian"),
                StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
            });
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientGuardian patientGuardian)
        {
            //return await base.PatchAsync(id, patientGuardian);
            return Json(new
            {
                data = await base.PatchAsync(id, patientGuardian),
                Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Guardian"),
                StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
            });
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