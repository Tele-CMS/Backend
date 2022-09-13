using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Audit.WebApi;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using static HC.Common.Enums.CommonEnum;
using JsonApiDotNetCore.Internal.Query;
using Microsoft.AspNetCore.Http.Internal;
using HC.Patient.Entity;
using HC.Common.HC.Common;
using HC.Model;
using HC.Common;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class MasterModulesController : CustomJsonApiController<MasterModules, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        CommonMethods commonMethods = null;
        Common.CommonMethods _commonMethods = new Common.CommonMethods();
        #region Construtor of the class
        public MasterModulesController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.MasterModules, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                commonMethods = new CommonMethods();
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
        /// this method is used for get request for service codes
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


        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]MasterModules masterModules)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            MasterModules alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterModules>().Where(l => l.ModuleName == masterModules.ModuleName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
            if (alreadyExist != null)// if MasterModules already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ModuleAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                
                masterModules.OrganizationID = token.OrganizationID;
                return await base.PostAsync(masterModules);
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]MasterModules masterModules)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                MasterModules alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterModules>().Where(l => l.ModuleName == masterModules.ModuleName && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ModuleAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, masterModules);
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