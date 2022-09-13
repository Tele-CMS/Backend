using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Audit.WebApi;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using static HC.Common.Enums.CommonEnum;
using JsonApiDotNetCore.Internal.Query;
using Microsoft.AspNetCore.Http.Internal;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Payer;
using System.Net.Http;
using System.Net;
using HC.Common.HC.Common;
using HC.Common;
using HC.Model;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Web.Controllers
{   
    public class InsuranceCompaniesController : CustomJsonApiController<Entity.InsuranceCompanies, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        
        #region Construtor of the class
        public InsuranceCompaniesController(
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
            catch
            {

            }
        }
        #endregion



        [HttpPost("SavePayerInformation")]
        public async Task<JsonResult> PostAsync([FromBody]PayerInformationModel entity)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                
                /////////////
                InsuranceCompanies insuranceCompanies = new InsuranceCompanies();
                insuranceCompanies.Address = entity.Address;
                insuranceCompanies.Email = entity.Email;
                insuranceCompanies.InsuranceTypeId = entity.InsuranceTypeID;
                insuranceCompanies.Name = entity.PayerName;
                insuranceCompanies.Phone = entity.Phone;
                insuranceCompanies.TPLCode = entity.TPLCode;
                insuranceCompanies.IsActive = entity.IsActive;
                insuranceCompanies.Fax = entity.Fax;
                insuranceCompanies.Zip = entity.Zip;
                insuranceCompanies.City = entity.City;
                insuranceCompanies.StateID = entity.StateID;
                insuranceCompanies.CreatedBy = entity.CreatedBy;
                insuranceCompanies.CountryID = entity.CountryID;
                insuranceCompanies.CarrierPayerID = entity.CarrierPayerID;
                insuranceCompanies.IsEDIPayer = entity.IsEDIPayer;
                insuranceCompanies.AdditionalClaimInfo = entity.AdditionalClaimInfo;
                insuranceCompanies.Latitude = entity.Latitude;
                insuranceCompanies.Longitude = entity.Longitude;
                insuranceCompanies.ApartmentNumber = entity.ApartmentNumber;                
                insuranceCompanies.IsPractitionerIsRenderingProvider = entity.IsPractitionerIsRenderingProvider;
                insuranceCompanies.Form1500PrintFormat = entity.Form1500PrintFormat;
                
                ///
                insuranceCompanies.OrganizationID = token.OrganizationID;
                //
                insuranceCompanies.DayClubByProvider = entity.DayClubByProvider;
                _dbContextResolver.GetDbSet<InsuranceCompanies>().Add(insuranceCompanies);
                int result = await _dbContextResolver.GetContext().SaveChangesAsync();
                return Json(new
                {
                    data = insuranceCompanies,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPatch("UpdatePayerInformation")]
        public async Task<JsonResult> PatchAsync([FromBody]PayerInformationModel entity)
        {
            try
            {
                InsuranceCompanies insuranceCompanies = _dbContextResolver.GetDbSet<InsuranceCompanies>().Where(k => k.Id == entity.PayerID).FirstOrDefault();
                if (insuranceCompanies != null)
                {
                    insuranceCompanies.Address = entity.Address;
                    insuranceCompanies.Email = entity.Email;
                    insuranceCompanies.InsuranceTypeId = entity.InsuranceTypeID;
                    insuranceCompanies.Name = entity.PayerName;
                    insuranceCompanies.Phone = entity.Phone;
                    insuranceCompanies.TPLCode = entity.TPLCode;
                    insuranceCompanies.IsActive = entity.IsActive;
                    insuranceCompanies.Fax = entity.Fax;
                    insuranceCompanies.Zip = entity.Zip;
                    insuranceCompanies.City = entity.City;
                    insuranceCompanies.StateID = entity.StateID;
                    insuranceCompanies.CountryID = entity.CountryID;
                    insuranceCompanies.CarrierPayerID = entity.CarrierPayerID;
                    insuranceCompanies.DayClubByProvider = entity.DayClubByProvider;
                    insuranceCompanies.IsEDIPayer = entity.IsEDIPayer;
                    insuranceCompanies.AdditionalClaimInfo = entity.AdditionalClaimInfo;
                    insuranceCompanies.Latitude = entity.Latitude;
                    insuranceCompanies.Longitude = entity.Longitude;
                    insuranceCompanies.ApartmentNumber = entity.ApartmentNumber;                    
                    insuranceCompanies.IsPractitionerIsRenderingProvider = entity.IsPractitionerIsRenderingProvider;
                    insuranceCompanies.Form1500PrintFormat = entity.Form1500PrintFormat;                    

                    int result = await _dbContextResolver.GetContext().SaveChangesAsync();
                    return Json(new
                    {
                        data = insuranceCompanies,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    });
                }
                else
                {
                    return Json(new
                    {
                        data = 0,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    });

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPatch("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
    }
}