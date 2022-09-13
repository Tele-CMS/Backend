//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.IServices.Authorization;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize("AuthorizedUser")]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class AuthorizationController : CustomJsonApiController<Authorization, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        
        public readonly IEntityRepository<Authorization> _entityRepository;
        public readonly IAuthorizationService _authorizationService;

        CommonMethods commonMethods = null;

        #region Construtor of the class
        public AuthorizationController(
       IJsonApiContext jsonApiContext,
       IResourceService<Authorization, int> resourceService,
       ILoggerFactory loggerFactory, IEntityRepository<Authorization> entityRepository, IUserCommonRepository userCommonRepository, IAuthorizationService authorizationService)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _entityRepository = entityRepository;
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                _authorizationService = authorizationService;
                commonMethods = new CommonMethods();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {
                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
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
        /// this method is used for get request for patient
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            return await base.GetAsync();

        }
        [HttpGet("{type}/{id}/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipAsync(id, relationshipName);
        }


        [HttpGet("{type}/{id}/relationships/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipsAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipsAsync(id, relationshipName);
        }

        /// <summary>
        /// this method is used for save authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]Authorization authorization)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);            
            authorization.OrganizationID = token.OrganizationID;
            return await base.PostAsync(authorization);
        }       

        /// <summary>
        /// for update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]Authorization authorization)
        {

            return await base.PatchAsync(id, authorization);
        }

        /// <summary>
        /// get included data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAndIncludeAsync/{id}")]
        public virtual async Task<List<Authorization>> GetAndIncludeAsync(int id)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return await _entityRepository.Get().Where(l => l.PatientID == id && l.IsDeleted == false && l.OrganizationID == token.OrganizationID)
                .Include(p => p.AuthorizationProcedures)
                .ThenInclude(pt => pt.AuthProcedureCPTLink)
                .ToListAsync();
        }
        
        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new JsonModel DeleteAsync(int id)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            return _authorizationService.DeleteAutorization(id, token);
        }
        #endregion


    }
}