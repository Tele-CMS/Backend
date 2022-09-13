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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{   
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MasterCustomLabelController : CustomJsonApiController<Entity.MasterCustomLabels, int>
    {
        
        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        
        #region Construtor of the class
        public MasterCustomLabelController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.MasterCustomLabels, int> resourceService,
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
            #region organization filter
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            int OrganizationID = token.OrganizationID;
            _jsonApiContext.QuerySet.Filters.Add(new FilterQuery("OrganizationID", OrganizationID.ToString(), ""));
            #endregion

            #region get key filter step 1
            List<FilterQuery> filterQueryforKey = new List<FilterQuery>();
            List<string> typeKey = new List<string>();
            List<int> roleTypeIds = new List<int>();
            filterQueryforKey = _jsonApiContext.QuerySet.Filters.Where(a => a.Key.ToUpper() == PatientSearch.TYPEKEY.ToString()).ToList();
            if (filterQueryforKey.Count() > 0)
            {
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.TYPEKEY.ToString()) { typeKey.Add((p.Value.ToString())); } });
                _jsonApiContext.QuerySet.Filters.RemoveAll(a => a.Key.ToLower() == PatientSearch.TYPEKEY.ToString().ToLower());
            }
            #endregion

            #region async call
            var asyncMasterCustomLabels = await base.GetAsync();
            #endregion

            #region filter by key type step 2            
            if (filterQueryforKey.Count() > 0)
            {
                List<UserRoleType> userRoleTypeObj = new List<UserRoleType>();
                userRoleTypeObj = _jsonApiContext.GetDbContextResolver().GetDbSet<UserRoleType>().Where(l => typeKey.Contains(l.TypeKey) && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == OrganizationID).ToList();                
                roleTypeIds = userRoleTypeObj.Select(a => a.Id).ToList();
                List<MasterCustomLabels> masterObj = new List<MasterCustomLabels>();
                masterObj = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterCustomLabels>().Where(l => roleTypeIds.Contains(l.RoleTypeID) && l.OrganizationID == OrganizationID && l.IsDeleted == false && l.IsActive == true).ToList();
                ((ObjectResult)asyncMasterCustomLabels).Value = masterObj;
                return asyncMasterCustomLabels;
            }

            #endregion

            #region filter by role type id
            List<FilterQuery> filterQuery = new List<FilterQuery>();
            
            filterQuery = _jsonApiContext.QuerySet.Filters.Where(a => a.Key.ToUpper() == PatientSearch.ROLETYPEID.ToString()).ToList();
            if (filterQuery.Count() > 1)
            {
                _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.ROLETYPEID.ToString()) { roleTypeIds.Add(Convert.ToInt32(p.Value)); } });
                List<MasterCustomLabels> masterObj = new List<MasterCustomLabels>();
                _jsonApiContext.QuerySet.Filters.RemoveAll(a => a.Key == PatientSearch.ROLETYPEID.ToString());
                masterObj = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterCustomLabels>().Where(l => roleTypeIds.Contains(l.RoleTypeID) && l.OrganizationID== OrganizationID && l.IsDeleted == false && l.IsActive == true).ToList();
                ((ObjectResult)asyncMasterCustomLabels).Value = masterObj;
                return asyncMasterCustomLabels;
            }
            #endregion
            return asyncMasterCustomLabels;
        }

        [HttpPost]        
        public override async Task<IActionResult> PostAsync([FromBody]MasterCustomLabels masterLabel)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            MasterCustomLabels alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterCustomLabels>().Where(l => l.CustomLabelName == masterLabel.CustomLabelName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
            if (alreadyExist != null)// if MasterCustomLabels already exist
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.CustomLabelAlreadyExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                });
            }
            else
            {
                masterLabel.OrganizationID = token.OrganizationID;
                return Json(new
                {
                    data = await base.PostAsync(masterLabel),
                    Message = StatusMessage.CustomLabelSaved,
                    StatusCode = (int)HttpStatusCodes.OK
                });

                //return await base.PostAsync(masterLabel);
            }
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]MasterCustomLabels masterLabel)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                MasterCustomLabels alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterCustomLabels>().Where(l => l.CustomLabelName == masterLabel.CustomLabelName && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if MasterCustomLabels already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.CustomLabelAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    masterLabel.OrganizationID = token.OrganizationID;
                    return Json(new
                    {
                        data = await base.PatchAsync(id, masterLabel),
                        Message = StatusMessage.CustomLabelUpdated,
                        StatusCode = (int)HttpStatusCodes.OK
                    });
                    //return await base.PatchAsync(id, masterLabel);
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