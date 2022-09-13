//using Audit.WebApi;
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
    public class EncounterICDController : CustomJsonApiController<Entity.PatientEncounterDiagnosisCodes, int>
    {        
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;

        
        #region Construtor of the class
        public EncounterICDController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientEncounterDiagnosisCodes, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();                
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                //_dbContextResolver = jsonApiContext.GetDbContextResolver();
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
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return await base.GetAsync();
        }

        [HttpPost("postMultiple")]
        public async Task<IActionResult> PostAsync([FromBody]JObject entity)
        {
            try
            {
                EncounterICDModel encountericd = entity.ToObject<EncounterICDModel>();
                int encounterID = encountericd.EncounterID;
                var dbContext = _dbContextResolver.GetDbSet<PatientEncounterServiceCodes>();
                if (encountericd.PatientEncounterICD != null && encountericd.PatientEncounterICD.Count > 0)
                {
                    var encounterICD = dbContext.Where(m => m.PatientEncounterId == encounterID).ToList();
                    if (encounterICD != null && encounterICD.Count > 0)//Patch case
                    {       
                        _dbContextResolver.GetDbSet<PatientEncounterServiceCodes>().RemoveRange(encounterICD);
                        await _dbContextResolver.GetContext().SaveChangesAsync();

                        foreach (var item in encountericd.PatientEncounterICD)
                        {
                            item.PatientEncounterId = encounterID;
                            await base.PostAsync(item);
                        }
                    }
                    else // new case
                    {
                        foreach (var item in encountericd.PatientEncounterICD)
                        {
                            item.PatientEncounterId = encounterID;
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
                    Response.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;//(Status Unprocessed Entity)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidData,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Status Unprocessed Entity)
                });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientEncounterDiagnosisCodes encounterICDLink)
        {

            return await base.PatchAsync(id, encounterICDLink);
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