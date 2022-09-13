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
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PatientPhoneNumberController : CustomJsonApiController<PhoneNumbers, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        #region Construtor of the class
        public PatientPhoneNumberController(
        IJsonApiContext jsonApiContext,
        IResourceService<PhoneNumbers, int> resourceService,
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
        /// this method is used to save phone numcber
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("postMultiple")]
        public async Task<IActionResult> PostAsync([FromBody]JObject entity)
        {
            try
            {
                PatientNumbers patientNumbers = entity.ToObject<PatientNumbers>();
                int patientID = patientNumbers.PatientID;
                var dbContext = _dbContextResolver.GetDbSet<PhoneNumbers>();
                if (patientNumbers.PhoneNumbers !=null && patientNumbers.PhoneNumbers.Count > 0)
                {
                    var phoneNumber = dbContext.Where(m => m.PatientID == patientID).ToList();

                    dbContext.RemoveRange(phoneNumber);
                    foreach (PhoneNumbers phone in patientNumbers.PhoneNumbers) { await base.PostAsync(phone); }
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
                    var phoneNumber = dbContext.Where(m => m.PatientID == patientID).ToList();
                    dbContext.RemoveRange(phoneNumber);
                    await _dbContextResolver.GetContext().SaveChangesAsync();
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.Success,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PhoneNumbers patientPhoneNumbers)
        {
            var attrToUpdate = _jsonApiContext.AttributesToUpdate;
            var patientPhoneNumbersOld = _dbContextResolver.GetDbSet<PhoneNumbers>().Where(m => m.Id == id).FirstOrDefault();

            CommonMethods commonMethods = new CommonMethods();         
            return await base.PatchAsync(id, patientPhoneNumbers);
        }

        [HttpPost]
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]PhoneNumbers phoneNumbers)
        {
            return await base.PostAsync(phoneNumbers);
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