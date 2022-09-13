//using Audit.WebApi;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientVitalsController : CustomJsonApiController<Entity.PatientVitals, int>
    {

        private readonly IDbContextResolver _dbContextResolver;
        private readonly IPatientCommonService _patientCommonService;
        #region Construtor of the class
        public PatientVitalsController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PatientVitals, int> resourceService,
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
            catch
            {

            }
        }

        #endregion

        #region Class Methods
        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]Entity.PatientVitals entity)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    entity = calculateBmi(entity);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound//(Not Found)
                    });
                }
            }
            catch (Exception )
            {
            }
            return await base.PostAsync(entity);
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientVitals patientVital)
        {
            AttrAttribute BMI = new AttrAttribute(AttrToUpdate.BMI.ToString(), AttrToUpdate.BMI.ToString());
            _jsonApiContext.AttributesToUpdate.Remove(BMI);
            
            patientVital = calculateBmi(patientVital);
            _jsonApiContext.AttributesToUpdate.Add(BMI, patientVital.BMI);            
            return await base.PatchAsync(id, patientVital);
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
        #endregion

        #region Helping Methods
        /// <summary>
        /// Calculate BMI and update relevant fields
        /// </summary>
        /// <param name="patientVitals"></param>
        /// <returns></returns>
        private static Entity.PatientVitals calculateBmi(Entity.PatientVitals patientVitals)
        {
            double? weightKg = 0;
            double? heightCm = 0;            
            if (patientVitals.WeightLbs > 0)
            {
                //convert lbs into pound (.45 is 1kg value in pounds)
                weightKg = Math.Round((double)(patientVitals.WeightLbs * .45), 2);
            }

            if (patientVitals.HeightIn > 0)
            {
                ////convert height of feet and inches into cm
                //heightCm = Math.Round((double)((patientVitals.HeightFt * 12) + patientVitals.HeightIn) * 2.54, 2);
                //convert height of inches into cm
                heightCm = Math.Round((double)(patientVitals.HeightIn) * 2.54, 2);
            }

            //var height = patientVitals.Height_cm;
            //var weight = patientVitals.Weight_kg;

            if (heightCm > 0 && weightKg > 0)
            {
                //calculate BMI
                patientVitals.BMI = Math.Round((double)(weightKg / (heightCm / 100 * heightCm / 100)), 2);

                //if (patientVitals.BMI < 18.5)
                //{
                //    patientVitals.BMI_Status = "Below Normal";
                //}
                //if (patientVitals.BMI > 18.5 && patientVitals.BMI < 25)
                //{
                //    patientVitals.BMI_Status = "Normal";
                //}
                //if (patientVitals.BMI > 25)
                //{
                //    patientVitals.BMI_Status = "Overweight";
                //}
            }
            return patientVitals;
        }
        #endregion
    }
}