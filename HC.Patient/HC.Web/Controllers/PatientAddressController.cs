//using Audit.WebApi;
using HC.Common.Filters;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]

    public class PatientAddressController : CustomJsonApiController<Entity.PatientAddress, int>
    {

        private readonly IDbContextResolver _dbContextResolver;
        #region Construtor of the class
        public PatientAddressController(
        IJsonApiContext jsonApiContext,
        IResourceService<Entity.PatientAddress, int> resourceService,
        ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
        : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
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

        /// <summary>
        /// this method is use to get the list of patient Address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            // custom code
            if (RequestIsValid() == false)
                return BadRequest();

            // return result from base class
            return await base.GetAsync();
        }
        /// <summary>
        /// this method is used to update the Patient Address
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patientAddress"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientAddress patientAddress)
        {   
            return await base.PatchAsync(id, patientAddress);
        }


        /// <summary>
        /// this method is used for get data on basis of type and relationship
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Id"></param>
        /// <param name="relationshipName"></param>
        /// <returns></returns>
        [HttpGet("{type}/{id}/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipAsync(string type, int Id, string relationshipName)
        {
            return await base.GetRelationshipAsync(Id, relationshipName);
        }

        [HttpGet("{type}/{id}/relationships/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipsAsync(string type, int Id, string relationshipName)
        {
            return await base.GetRelationshipsAsync(Id, relationshipName);
        }

        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]PatientAddress patientAddress)
        {
            return await base.PostAsync(patientAddress);
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

        #endregion

        #region Helping Methods
        // some custom validation logic
        private bool RequestIsValid() => true;
        #endregion


    }
}