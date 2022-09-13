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
    public class PatientCustomLabelsController : CustomJsonApiController<PatientCustomLabels, int>
    {
        private readonly IDbContextResolver _dbContextResolver;

        #region Construtor of the class
        public PatientCustomLabelsController(
       IJsonApiContext jsonApiContext,
       IResourceService<PatientCustomLabels, int> resourceService,
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
        /// this method is used for get request for patient
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
        /// this method is used for save PatientCustomLabels
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]PatientCustomLabels patientCustomLabels)
        {
            //return await base.PostAsync(patientCustomLabels);
            return Json(new
            {
                data = await base.PostAsync(patientCustomLabels),
                Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Client's custom fields"),
                StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
            });
        }
       

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientCustomLabels patientCustomLabels)
        {

            //return await base.PatchAsync(id, patientCustomLabels);
            return Json(new
            {
                data = await base.PatchAsync(id, patientCustomLabels),
                Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Client's custom fields"),
                StatusCode = (int)HttpStatusCodes.OK//(Invalid credentials)
            });
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
                PatientLabels patientLabels = entity.ToObject<PatientLabels>();
                int patientID = patientLabels.PatientID;
                var dbContext = _dbContextResolver.GetDbSet<PatientCustomLabels>();
                if (patientLabels.PatientCustomLabels != null && patientLabels.PatientCustomLabels.Count > 0)
                {

                    var patientLabel = dbContext.Where(m => m.PatientID == patientID).ToList();

                    dbContext.RemoveRange(patientLabel);
                    CommonMethods commonMethods = new CommonMethods();
                    foreach (PatientCustomLabels patientCustomLabel in patientLabels.PatientCustomLabels) { patientCustomLabel.CustomLabelDataType = commonMethods.ParseString(patientCustomLabel.CustomLabelValue).ToString(); await base.PostAsync(patientCustomLabel); }
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = patientLabels,
                        Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Client's custom fields"),
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
                else
                {
                    var patientLabel = dbContext.Where(m => m.PatientID == patientID).ToList();
                    dbContext.RemoveRange(patientLabel);
                    await _dbContextResolver.GetContext().SaveChangesAsync();
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Client's custom fields"),
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