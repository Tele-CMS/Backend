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

    public class MasterInsuranceTypeController : CustomJsonApiController<MasterInsuranceType, int>
    {
        
        #region Construtor of the class
        public MasterInsuranceTypeController(
       IJsonApiContext jsonApiContext,
       IResourceService<MasterInsuranceType, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {   
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// this method is used for get request for insurance type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            return await base.GetAsync();
        }


        [HttpPost]        
        public override async Task<IActionResult> PostAsync([FromBody]MasterInsuranceType masterInsuranceTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);                   
                    MasterInsuranceType alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterInsuranceType>().Where(l => l.InsuranceType== masterInsuranceTypes.InsuranceType && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                    if (alreadyExist != null)// if master insurance types already exist
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.InsuarnceTypeAlreadyExist,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                        });
                    }
                    else
                    {
                        masterInsuranceTypes.OrganizationID = token.OrganizationID;
                        return await base.PostAsync(masterInsuranceTypes);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ModelState,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Not Found)
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
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]MasterInsuranceType masterInsuranceTypes)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                MasterInsuranceType alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterInsuranceType>().Where(l => l.InsuranceType == masterInsuranceTypes.InsuranceType && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if service code already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InsuarnceTypeAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, masterInsuranceTypes);
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