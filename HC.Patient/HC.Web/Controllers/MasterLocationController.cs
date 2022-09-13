//using Audit.WebApi;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.PatientAppointment;
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

    public class MasterLocation : CustomJsonApiController<Entity.Location, int>
    {

        private readonly IPatientCommonService _patientCommonService;
        private readonly IDbContextResolver _dbContextResolver;
        
        #region Construtor of the class
        public MasterLocation(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.Location, int> resourceService,
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
            catch (Exception)
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
            return await base.GetAsync();
        }


        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]Location location)
        {

            try
            {   
                if (ModelState.IsValid)
                {
                    TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                    Location alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Location>().Where(l => l.LocationName == location.LocationName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                    if (alreadyExist != null)// if location type already exist
                    {
                        return Json(new
                        {
                            data = new object(),
                            Message = StatusMessage.LocationAlreadyExist,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                        });
                    }
                    else
                    {
                        location.OrganizationID = token.OrganizationID;
                        var loc = await base.PostAsync(location);
                        Location savedLocation = (Location)((ObjectResult)loc).Value;                        
                        StaffLocation staffLocation = new StaffLocation();
                        //staffLocation.IsDefault = true; //set default
                        staffLocation.LocationID = savedLocation.Id; // location ID
                        staffLocation.StaffId = token.StaffID;//staff ID
                        staffLocation.OrganizationID = token.OrganizationID;//staff ID
                        _dbContextResolver.GetDbSet<StaffLocation>().Add(staffLocation);
                        _dbContextResolver.GetContext().SaveChanges();

                        Response.StatusCode = (int)HttpStatusCodes.OK;
                        return Json(new
                        {
                            data = loc,
                            Message = StatusMessage.LocationSaved,
                            StatusCode = (int)HttpStatusCodes.OK
                        });
                        //return loc;
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
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]Location location)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                Location alreadyExist = _jsonApiContext.GetDbContextResolver().GetDbSet<Location>().Where(l => l.LocationName == location.LocationName && l.Id != id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID).FirstOrDefault();
                if (alreadyExist != null)// if location type already exist
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.LocationAlreadyExist,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//(UnprocessedEntity)
                    });
                }
                else
                {
                    //patch record
                    await base.PatchAsync(id, location);
                    
                    //return response
                    Response.StatusCode = (int)HttpStatusCodes.OK;
                    return Json(new
                    {
                        data = location,
                        Message = StatusMessage.LocationUpdated,
                        StatusCode = (int)HttpStatusCodes.OK
                    });
                    //return await base.PatchAsync(id, location);
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


        [HttpGet("GetStaffOfficeTime")]
        public Location GetStaffOfficeTime()
        {
            try
            {
                Location officeHours = new Location();
                if (_jsonApiContext.QuerySet.Filters.Count() > 0)
                {
                    List<int> LocationList = new List<int>();
                    FilterQuery filterQuery = new FilterQuery("", "", "");

                    _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.LOCATIONID.ToString()) { LocationList.Add(int.Parse(p.Value)); filterQuery = p; } });
                    if (LocationList.Count > 0)
                    {
                        //appointments = patientAppointment;
                        //patientAppointment = new List<PatientAppointment>();
                        officeHours.Id= _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Max(k => k.Id);
                        var offStartHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Min(k => k.OfficeStartHour);
                        if (offStartHour != null) { officeHours.OfficeStartHourStr = offStartHour.Value.ToString("HH:mm:ss"); }
                        var offEndHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Max(g => g.OfficeEndHour);
                        if(offEndHour != null) { officeHours.OfficeEndHourStr= offEndHour.Value.ToString("HH:mm:ss"); }
                        //officeHours.OfficeStartHourStr = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Min(k => k.OfficeStartHour).Value.ToString("HH:mm:ss");
                        //officeHours.OfficeEndHourStr = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Max(g => g.OfficeEndHour.Value).ToString("HH:mm:ss");
                    }
                }
                return officeHours;
            }
            catch (Exception )
            {
                throw;
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