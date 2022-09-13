using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Service.IServices.AdminDashboard;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AdminDashboard")]
    [ActionFilter]
    public class AdminDashboardController : BaseController
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        /// <summary>       
        /// <Description> get total clients of the organization</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTotalClientCount")]
        public JsonResult GetTotalClientCount()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetTotalClientCount(GetToken(HttpContext))));
        }

        /// <summary>        
        /// <Description> get total revenue of the organization</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTotalRevenue")]
        public JsonResult GetTotalRevenue()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetTotalRevenue(GetToken(HttpContext))));
        }

        /// <summary>        
        /// <Description> To Do Static Count</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOnlineUsers")]
        public JsonResult GetOnlineUsers()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => new JsonModel() { data = 5, Message = StatusMessage.FetchMessage, StatusCode = (int)HttpStatusCodes.OK }));
        }

        /// <summary>        
        /// <Description> get organization's current year's authorization</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrganizationAuthorization")]
        public JsonResult GetOrganizationAuthorization(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetOrganizationAuthorization(pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }

        /// <summary>        
        /// <Description> get organization's Encounter(Visits)</Description>
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetOrganizationEncounter")]
        public JsonResult GetOrganizationEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetOrganizationEncounter(pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }

        [HttpGet("GetStaffEncounters")]
        public JsonResult GetStaffEncounters(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetStaffEncounter(pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }

        /// <summary>        
        /// <Description> this will get current year registered clients</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRegiesteredClientCount")]
        public JsonResult GetRegiesteredClientCount()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetRegiesteredClientCount(GetToken(HttpContext))));
        }

        /// <summary>        
        /// <Description> get admin dashboard data</Description>
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAdminDashboardData")]
        public JsonResult GetAdminDashboardData()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetAdminDashboardData(GetToken(HttpContext))));
        }

        /// <summary>
        /// this will get the client's chart info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetClientStatusChart")]
        public JsonResult GetClientStatusChart()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetClientStatusChart(GetToken(HttpContext))));
        }

        /// <summary>
        /// this will get the client's chart info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDashboardBasicInfo")]
        public JsonResult GetDashboardBasicInfo()
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetDashboardBasicInfo(GetToken(HttpContext))));
        }
        [HttpGet("GetAppointmentsDataForGraph")]
        public JsonResult GetAppointmentsDataForGraph(AppointmentDataFilterModel appointmentDataFilterModel)

        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetAppointmentsDataForGraph(appointmentDataFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// GetOrganizationAppointments in dashboard
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="staffIds"></param>
        /// <param name="patientIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="status"></param>        
        /// <param name="filterModel"></param>
        /// <returns></returns>
        [HttpGet("GetOrganizationAppointments")]
        public JsonResult GetOrganizationAppointments(FilterModel filterModel, string locationIds, string staffIds, string patientIds, DateTime fromDate, DateTime toDate, string status = "")
        {
            return Json(_adminDashboardService.ExecuteFunctions(() => _adminDashboardService.GetOrganizationAppointments(filterModel, locationIds, staffIds, patientIds, fromDate, toDate, status, GetToken(HttpContext))));
        }

    }
}