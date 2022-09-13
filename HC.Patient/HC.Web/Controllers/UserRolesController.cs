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
using HC.Patient.Data;
using HC.Model;
using HC.Common;
using HC.Common.HC.Common;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class UserRolesController : CustomJsonApiController<Entity.UserRoles, int>
    {
        private readonly IDbContextResolver _dbContextResolver;

        #region Construtor of the class
        public UserRolesController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.UserRoles, int> resourceService,
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
        /// this method is used for get request for insurance companies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            return await base.GetAsync();

        }


        [HttpPost]        
        public override async Task<IActionResult> PostAsync([FromBody]Entity.UserRoles userRoles)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
          
            Entity.UserRoles alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Entity.UserRoles>().Where(l => l.RoleName == userRoles.RoleName && l.OrganizationID == token.OrganizationID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
            if (alreadyExist != null)// if InsuranceCompanies already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.UserRoleAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                userRoles.OrganizationID = token.OrganizationID;
                return await base.PostAsync(userRoles);
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]Entity.UserRoles userRoles)
        {

            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);              
                Entity.UserRoles alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Entity.UserRoles>().Where(l => l.RoleName == userRoles.RoleName && l.OrganizationID == token.OrganizationID && l.Id != id && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if InsuranceCompanies already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.UserRoleAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, userRoles);
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

            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                int OrganizationID = token.OrganizationID;

                Entity.User alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Entity.User>().Where(l => l.RoleID == id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if Role already assigned to user ..........Can't delete
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.UserRoleAlreadyAssignedToUser,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.DeleteAsync(id);
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
        
        #endregion
    }
}