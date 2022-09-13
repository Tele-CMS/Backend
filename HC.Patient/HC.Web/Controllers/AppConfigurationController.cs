//using Audit.WebApi;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.PatientCommon.Interfaces;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
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
    //[Authorize(Roles = "Doctor")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AppConfigurationController : CustomJsonApiController<Entity.AppConfigurations, int>
    {
        private HCOrganizationContext _context;
        CommonMethods commonMethods = null;
        #region Construtor of the class
        public AppConfigurationController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.AppConfigurations, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, HCOrganizationContext context, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _context = context;
                commonMethods = new CommonMethods();
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
            catch (Exception)
            {

            }
        }
        #endregion

        #region Class Methods
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;

            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            return await base.GetAsync();
        }

        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]AppConfigurations appCon)
        {
            TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
            AppConfigurations alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<AppConfigurations>().Where(l => l.Key == appCon.Key && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
            if (alreadyExist != null)// if AppConfigurations already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.AppConfigurationAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                appCon.OrganizationID = token.OrganizationID;
                return await base.PostAsync(appCon);
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]AppConfigurations appCon)
        {
            try
            {
                AppConfigurations alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<AppConfigurations>().Where(l => l.Key == appCon.Key && l.Id != id && l.IsDeleted == false && l.IsActive == true).FirstOrDefault();
                if (alreadyExist != null)// if mastertag already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.AppConfigurationAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, appCon);
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


        [HttpPost("UpdateAppConfiguration")]
        public List<AppConfigurations> UpdateAppConfiguration([FromBody]JArray entity)
        {
            //XmlDocument doc = new XmlDocument();
            //XNode node = JsonConvert.DeserializeXNode(entity.ToString(), "Root");


            List<AppConfigurations> requestAppConfigList = entity.ToObject<List<AppConfigurations>>();
            List<AppConfigurations> dbObjList = _context.AppConfigurations.ToList();
            List<AppConfigurations> insertAppConfigList = new List<AppConfigurations>();
            AppConfigurations updateObj = new AppConfigurations();
            foreach (var item in requestAppConfigList)
            {
                updateObj = dbObjList.Find(a => a.Id == item.Id);
                if (!ReferenceEquals(updateObj, null) && updateObj.IsActive == true && updateObj.IsDeleted == false)
                {
                    updateObj.Value = item.Value;
                }
                _context.AppConfigurations.Update(updateObj);                
            }
            _context.SaveChanges();
            return insertAppConfigList;
        }
        #endregion

        #region Helping Methods
        #endregion


    }
}