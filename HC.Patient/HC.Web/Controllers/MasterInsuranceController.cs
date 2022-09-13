using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Repositories.Interfaces;
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
    public class MasterInsuranceController : CustomJsonApiController<Entity.InsuranceCompanies, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        
        #region Construtor of the class
        public MasterInsuranceController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.InsuranceCompanies, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {
                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// this method is used for get request for insurance companies
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
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]InsuranceCompanies insuranceCompany)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                InsuranceCompanies alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<InsuranceCompanies>().Where(l => l.Name == insuranceCompany.Name && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if InsuranceCompanies already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InsuranceCompaniesAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {

                    insuranceCompany.OrganizationID = token.OrganizationID;
                    return await base.PostAsync(insuranceCompany);
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
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]InsuranceCompanies insuranceCompany)
        {

            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                InsuranceCompanies alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<InsuranceCompanies>().Where(l => l.Name == insuranceCompany.Name && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if InsuranceCompanies already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InsuranceCompaniesAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, insuranceCompany);
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
            TokenModel tokenModel = CommonMethods.GetTokenDataModel(HttpContext);
            List<RecordDependenciesModel> recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.InsuranceCompanies, true, tokenModel).ToList();
            if (recordDependenciesModel != null && recordDependenciesModel.Exists(a => a.TableName.Contains(DatabaseTables.PayerAppointmentTypes) && a.TotalCount > 0))
            {
                JsonModel response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
                return Json(response);
            }
            else
            {
                return await base.DeleteAsync(id);
            }
        }
        #endregion

        #region Helping Methods
        #endregion
    }
}