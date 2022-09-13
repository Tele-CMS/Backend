//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
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
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AppointmentTypeController : CustomJsonApiController<Entity.AppointmentType, int>
    {

        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        CommonMethods commonMethods = null;


        #region Construtor of the class
        public AppointmentTypeController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.AppointmentType, int> resourceService,
       ILoggerFactory loggerFactory, IPatientCommonService patientCommonService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                commonMethods = new CommonMethods();
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
            catch (Exception)
            {
                //Error   
            }
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// this method is used for get request for patient appointment
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
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]AppointmentType appointmentType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                    AppointmentType alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<AppointmentType>().Where(l => l.Name == appointmentType.Name && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                    if (alreadyExist != null)// if appointment type already A
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.AppointmentAlreadyExist,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                        });
                    }
                    else
                    {                       
                        appointmentType.OrganizationID = token.OrganizationID;
                        return await base.PostAsync(appointmentType);
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
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]AppointmentType appointmentType)
        {
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                AppointmentType alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<AppointmentType>().Where(l => l.Name == appointmentType.Name && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if appointment type already A
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.AppointmentAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                    });
                }
                else
                {
                    return await base.PatchAsync(id, appointmentType);
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
            try
            {
                TokenModel token = commonMethods.GetTokenDataModel(HttpContext);
                int OrganizationID = token.OrganizationID;

                Entity.PayerAppointmentTypes alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Entity.PayerAppointmentTypes>().Where(l => l.AppointmentTypeId == id && l.IsDeleted == false && l.IsActive == true /*&& l.OrganizationID == OrganizationID*/).FirstOrDefault();
                if (alreadyExist != null)// if already assigned to payer ..........Can't delete
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.AppointmentTypeAlreadyAssigned,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(Unprocessed Entity)
                    });
                }
                else
                {
                    return await base.DeleteAsync(id);
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

        #endregion

        #region Helping Methods
        #endregion


    }
}