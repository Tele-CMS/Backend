//using Audit.WebApi;
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

    public class InsuredPersonController : CustomJsonApiController<Entity.InsuredPerson, int>
    {
        //private readonly IDbContextResolver _dbContextResolver;
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        
        #region Construtor of the class
        public InsuredPersonController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.InsuredPerson, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                //test commnet
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                //_dbContextResolver = jsonApiContext.GetDbContextResolver();
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
        public override async Task<IActionResult> PostAsync([FromBody]InsuredPerson insuredPerson)
        {
            var Token = HttpContext.Request.Headers.Skip(7).FirstOrDefault().Value; // 

            return await base.PostAsync(insuredPerson);
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]InsuredPerson insuredPerson)
        {

            return await base.PatchAsync(id, insuredPerson);
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