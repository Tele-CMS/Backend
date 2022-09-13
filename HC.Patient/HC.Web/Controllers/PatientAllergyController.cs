//using Audit.WebApi;
using HC.Common.Filters;
using HC.Common.HC.Common;
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

    public class PatientAllergyController : CustomJsonApiController<Entity.PatientAllergies, int>
    {

        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;


        #region Construtor of the class
        public PatientAllergyController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientAllergies, int> resourceService,
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
            catch (Exception)
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
        public override async Task<IActionResult> PostAsync([FromBody]PatientAllergies patientInfo)
        {
            try
            {
                PatientAllergies alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientAllergies>().Where(l => l.AllergyTypeID == patientInfo.AllergyTypeID && l.PatientID == patientInfo.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if PatientAllergies already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClientAllergyAlreadyLink,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PostAsync(patientInfo);
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
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientAllergies patientAllergies)
        {
            try
            {
                PatientAllergies alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientAllergies>().Where(l => l.AllergyTypeID == patientAllergies.AllergyTypeID && l.Id != patientAllergies.Id && l.PatientID == patientAllergies.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClientAllergyAlreadyLink,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, patientAllergies);
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