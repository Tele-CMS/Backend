//using Audit.WebApi;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
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
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]

    public class StaffTagsController : CustomJsonApiController<StaffTags, int>
    {   
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        #region Construtor of the class
        public StaffTagsController(
       IJsonApiContext jsonApiContext,
       IResourceService<StaffTags, int> resourceService,
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
            catch(Exception )
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
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]StaffTags staffTags)
        {   
            var userFav = _jsonApiContext.GetDbContextResolver().GetDbSet<StaffTags>().Where(l => l.TagID == staffTags.TagID && l.StaffID == staffTags.StaffID).FirstOrDefault();
            if(userFav != null)
            {   
                userFav.IsActive = staffTags.IsActive;
                return await base.PatchAsync(userFav.Id, userFav);
            }
            return await base.PostAsync(staffTags);
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]StaffTags staffTags)
        {           
            return await base.PatchAsync(id, staffTags);
        }

        [HttpPost("postMultiple")]
        public async Task<IActionResult> PostAsync([FromBody]JObject entity)
        {
            try
            {
                StaffTagModel staffTagModel = entity.ToObject<StaffTagModel>();
                int StaffID = staffTagModel.StaffID;
                var dbContext = _dbContextResolver.GetDbSet<StaffTags>();
                if (staffTagModel.StaffTags != null && staffTagModel.StaffTags.Count > 0)
                {
                    var staffTags = dbContext.Where(m => m.StaffID == StaffID).ToList();
                    if(staffTags != null && staffTags.Count>0)//Patch case
                    {
                        _dbContextResolver.GetDbSet<StaffTags>().RemoveRange(staffTags);
                        await _dbContextResolver.GetContext().SaveChangesAsync();

                        foreach (var item in staffTagModel.StaffTags)
                        {
                            item.StaffID = StaffID;
                            await base.PostAsync(item);
                        }
                    }
                    else // new case
                    {
                        foreach (var item in staffTagModel.StaffTags)
                        {
                            item.StaffID = StaffID;
                            await base.PostAsync(item);
                        }
                    }
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.Success,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;//(Unprocessed Entity)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidData,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
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

        #region Helping Methods
        #endregion


    }
}