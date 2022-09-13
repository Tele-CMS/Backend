//using Audit.WebApi;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    //[Authorize(Roles = "Doctor")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class MasterTagsController : CustomJsonApiController<Entity.MasterTags, int>
    {
        
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        

        #region Construtor of the class
        public MasterTagsController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.MasterTags, int> resourceService,
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
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

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
            #region organization filter
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;
            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            #endregion

            #region get key filter step 1
            List<FilterQuery> filterQueryforKey = new List<FilterQuery>();
            List<string> typeKey = new List<string>();
            filterQueryforKey = _jsonApiContext.QuerySet.Filters.Where(a => a.Key.ToUpper() == PatientSearch.TYPEKEY.ToString()).ToList();
            if (filterQueryforKey.Count() > 0)
            {
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.TYPEKEY.ToString()) { typeKey.Add((p.Value.ToString())); } });
                _jsonApiContext.QuerySet.Filters.RemoveAll(a => a.Key.ToLower() == PatientSearch.TYPEKEY.ToString().ToLower());                
            }
            #endregion

            #region async call
            var asyncMasterTags = await base.GetAsync();
            #endregion

            #region filter by key type step 2            
            if (filterQueryforKey.Count() > 0)
            {   
                List<MasterTags> masterTagsObj = new List<MasterTags>();                
                masterTagsObj = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterTags>().Where(l => typeKey.Contains(l.UserRoleType.TypeKey) && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == OrganizationID).Include("UserRoleType").ToList();
                ((ObjectResult)asyncMasterTags).Value = masterTagsObj;
            }

            #endregion
            
            #region filter by role type id
            List<FilterQuery> filterQuery = new List<FilterQuery>();
            List<int?> roleTypeIds = new List<int?>();
            filterQuery = _jsonApiContext.QuerySet.Filters.Where(a => a.Key.ToUpper() == PatientSearch.ROLETYPEID.ToString()).ToList();
            if (filterQuery.Count() > 1)
            {
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.ROLETYPEID.ToString()) { roleTypeIds.Add(Convert.ToInt32(p.Value)); } });
                List<MasterTags> masterTagsObj = new List<MasterTags>();
                _jsonApiContext.QuerySet.Filters.RemoveAll(a => a.Key == PatientSearch.ROLETYPEID.ToString());
                masterTagsObj = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterTags>().Where(l => roleTypeIds.Contains(l.RoleTypeID) && l.IsDeleted == false && l.IsActive == true && l.OrganizationID== OrganizationID).ToList();
                ((ObjectResult)asyncMasterTags).Value = masterTagsObj;                
            }
            #endregion

            return asyncMasterTags;
        }

        [HttpPost]        
        public override async Task<IActionResult> PostAsync([FromBody]MasterTags masterTags)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);           
            MasterTags alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterTags>().Where(l => l.Tag == masterTags.Tag && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
            if (alreadyExist != null)// if mastertag already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.TagAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                masterTags.OrganizationID = token.OrganizationID;
                return await base.PostAsync(masterTags);
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]MasterTags masterTags)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);                
                MasterTags alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterTags>().Where(l => l.Tag == masterTags.Tag && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.TagAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, masterTags);
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