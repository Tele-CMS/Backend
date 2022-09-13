//using Audit.WebApi;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
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

    public class PatientDiagnosisController : CustomJsonApiController<Entity.PatientDiagnosis, int>
    {
        private readonly IDbContextResolver _dbContextResolver;        
        #region Construtor of the class
        public PatientDiagnosisController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientDiagnosis, int> resourceService,
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
        

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientDiagnosis patientDiagnosis)
        {
            try
            {
                PatientDiagnosis alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientDiagnosis>().Where(l => l.ICDID == patientDiagnosis.ICDID && l.Id != patientDiagnosis.Id && l.PatientID == patientDiagnosis.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClientICDAlreadyLink,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, patientDiagnosis);
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

        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]PatientDiagnosis patientDiagnosis)
        {
            PatientDiagnosis alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<PatientDiagnosis>().Where(l => l.ICDID == patientDiagnosis.ICDID && l.PatientID == patientDiagnosis.PatientID && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();            
            if (alreadyExist != null)// if mastertag already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.ClientICDAlreadyLink,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                if (patientDiagnosis.DiagnosisDate == DateTime.MinValue) //when DiagnosisDate date now pass from front end
                {
                    patientDiagnosis.DiagnosisDate = DateTime.UtcNow;
                }
                return await base.PostAsync(patientDiagnosis);
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