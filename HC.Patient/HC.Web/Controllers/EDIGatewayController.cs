using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Audit.WebApi;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using HC.Patient.Entity;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Service.IServices.Patient;
using Microsoft.Extensions.Logging;
using static HC.Common.Enums.CommonEnum;
using JsonApiDotNetCore.Internal.Query;
using Microsoft.AspNetCore.Http.Internal;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Common;
using HC.Model;
using JsonApiDotNetCore.Models;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class EDIGatewayController : CustomJsonApiController<EDIGateway, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IPatientCommonService _patientCommonService;
        private readonly IPatientService _patientService;
        
        #region Construtor of the class
        public EDIGatewayController(
       IJsonApiContext jsonApiContext,
       IResourceService<EDIGateway, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IPatientService patientService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                _patientCommonService = patientCommonService;
                _patientService = patientService;
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
        /// this method is used for get request for master Location
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));

            var ediResults = await base.GetAsync();
            List<EDIGateway> ediList = (List<EDIGateway>)((ObjectResult)ediResults).Value;
            ediList.ForEach(a => a.FTPPassword = (CommonMethods.Decrypt(a.FTPPassword)));
            ((ObjectResult)ediResults).Value = ediList;
            return ediResults;

        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int ID)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));

            var ediResults = await base.GetAsync(ID);
            EDIGateway ediList = (EDIGateway)((ObjectResult)ediResults).Value;
            ediList.FTPPassword = CommonMethods.Decrypt(ediList.FTPPassword);
            ((ObjectResult)ediResults).Value = ediList;
            return ediResults;
        }

        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]EDIGateway ediGateway)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                    EDIGateway alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<EDIGateway>().Where(l => l.ClearingHouseName == ediGateway.ClearingHouseName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                    if (alreadyExist != null)// if cleaing house already exist
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.ClearingHouseAlreadyExist,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                        });
                    }
                    else
                    {
                        ediGateway.FTPPassword = CommonMethods.Encrypt(ediGateway.FTPPassword);
                        ediGateway.OrganizationID = token.OrganizationID;
                        return await base.PostAsync(ediGateway);
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
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]EDIGateway ediGateway)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                EDIGateway alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<EDIGateway>().Where(l => l.ClearingHouseName == ediGateway.ClearingHouseName && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if clearing house already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.ClearingHouseAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(ediGateway.FTPPassword))
                    {
                        ediGateway.FTPPassword = CommonMethods.Encrypt(ediGateway.FTPPassword);
                        AttrAttribute FTPPassword = new AttrAttribute(AttrToUpdate.FTPPassword.ToString(), AttrToUpdate.FTPPassword.ToString());
                        _jsonApiContext.AttributesToUpdate.Remove(FTPPassword);
                        _jsonApiContext.AttributesToUpdate.Add(FTPPassword, ediGateway.FTPPassword);
                    }
                    return await base.PatchAsync(id, ediGateway);
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