//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[Authorize("AuthorizedUser")]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class StaffCustomLabelsController : CustomJsonApiController<StaffCustomLabels, int>
    {
        private readonly IDbContextResolver _dbContextResolver;

        #region Construtor of the class
        public StaffCustomLabelsController(
       IJsonApiContext jsonApiContext,
       IResourceService<StaffCustomLabels, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
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
        /// this method is used for get request for Staff
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
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
        /// this method is used for save StaffCustomLabels
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]StaffCustomLabels staffCustomLabels)
        {
            StaffCustomLabels alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<StaffCustomLabels>().Where(l => l.StaffID == staffCustomLabels.StaffID && l.CustomLabelValue == staffCustomLabels.CustomLabelValue && l.CustomLabelID == staffCustomLabels.CustomLabelID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
            if (alreadyExist != null)// if mastertag already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.StaffCustomValue,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                //return await base.PostAsync(staffCustomLabels);
                return Json(new
                {
                    data = await base.PostAsync(staffCustomLabels),
                    Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "User's custom fields"),
                    StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
                });
            }
        }
       

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]StaffCustomLabels staffCustomLabels)
        {

            try
            {
                StaffCustomLabels alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<StaffCustomLabels>().Where(l => l.StaffID == staffCustomLabels.StaffID && l.CustomLabelValue == staffCustomLabels.CustomLabelValue && l.CustomLabelID == staffCustomLabels.CustomLabelID && l.Id != id && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if StaffCustomLabels already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.StaffCustomValue,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    //return await base.PatchAsync(id, staffCustomLabels);
                    return Json(new
                    {
                        data = await base.PatchAsync(id, staffCustomLabels),
                        Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "User's custom fields"),
                        StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
                    });
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



        /// <summary>
        /// this method is used to save phone numcber
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("postMultiple")]
        public async Task<IActionResult> PostAsync([FromBody]JObject entity)
        {
            try
            {
                StaffLabels staffLabels = entity.ToObject<StaffLabels>();
                int staffID = staffLabels.StaffID;
                var dbContext = _dbContextResolver.GetDbSet<StaffCustomLabels>();
                if (staffLabels.StaffCustomLabels != null && staffLabels.StaffCustomLabels.Count > 0)
                {

                    var staffLabel = dbContext.Where(m => m.StaffID == staffID).ToList();

                    dbContext.RemoveRange(staffLabel);                    
                    foreach (StaffCustomLabels staffCustomLabel in staffLabels.StaffCustomLabels) { staffCustomLabel.CustomLabelDataType = CommonMethods.ParseString(staffCustomLabel.CustomLabelValue).ToString(); await base.PostAsync(staffCustomLabel); }
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = staffLabels,
                        Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "User's custom fields"),
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
                else
                {
                    var staffLabel = dbContext.Where(m => m.StaffID == staffID).ToList();
                    dbContext.RemoveRange(staffLabel);
                    await _dbContextResolver.GetContext().SaveChangesAsync();
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "User's custom fields"),
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
        #endregion


    }
}