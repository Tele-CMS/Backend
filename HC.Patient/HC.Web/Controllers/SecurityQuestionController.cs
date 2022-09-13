//using Audit.WebApi;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
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

    public class SecurityQuestionController : CustomJsonApiController<SecurityQuestions, int>
    {
        CommonMethods commonMethods = null;

        #region Construtor of the class
        public SecurityQuestionController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.SecurityQuestions, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                commonMethods = new CommonMethods();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// this method is used for get request for question
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
        public override async Task<IActionResult> PostAsync([FromBody]SecurityQuestions securityQuestions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                    SecurityQuestions alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<SecurityQuestions>().Where(l => l.Question == securityQuestions.Question && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                    if (alreadyExist != null)// if question already exist
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.QuestionAlreadyExist,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                        });
                    }
                    else
                    {   
                        securityQuestions.OrganizationID = token.OrganizationID;
                        return await base.PostAsync(securityQuestions);
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
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]SecurityQuestions securityQuestions)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                SecurityQuestions alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<SecurityQuestions>().Where(l => l.Question == securityQuestions.Question && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if question already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.QuestionAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {   
                    return await base.PatchAsync(id, securityQuestions);
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