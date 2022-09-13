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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]    
    public class MasterICDController : CustomJsonApiController<Entity.MasterICD, int>
    {
        
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        CommonMethods commonMethods = null;

        #region Construtor of the class
        public MasterICDController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.MasterICD, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                commonMethods = new CommonMethods();

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
        /// <summary>
        /// this method is used for get request for diagnosis codes
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
        public override async Task<IActionResult> PostAsync([FromBody]MasterICD masterICD)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            MasterICD alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterICD>().Where(l => l.Code == masterICD.Code && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
            if (alreadyExist != null)// if mastertag already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ICDAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
               
                masterICD.OrganizationID = token.OrganizationID;
                return await base.PostAsync(masterICD);
            }
            
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]MasterICD masterICD)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                MasterICD alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterICD>().Where(l => l.Code == masterICD.Code && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ICDAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, masterICD);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode,//(Error Code)
                    AppError=ex.Message
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