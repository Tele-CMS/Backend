using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HC.Patient.Service.IServices.AuditLog;
//using Audit.WebApi;
using HC.Model;
using HC.Common;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Web.Filters;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/AuditLog")]
    [ActionFilter]
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogService _auditLogService;        
        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Route("GetAuditLogsList")]
        public JsonResult GetAuditLogsList(string searchText = "", string createdBy = "", string patientName = "", string actionName = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            return Json(_auditLogService.ExecuteFunctions(() => _auditLogService.GetAuditLogList(searchText,createdBy, patientName, actionName, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetLoginLogsList")]
        public JsonResult GetLoginLogsList(string searchText = "", string createdBy = "", string patientName = "", string actionName = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", int roleID = 0)
        {
            return Json(_auditLogService.ExecuteFunctions(() => _auditLogService.GetLoginLogList(searchText,createdBy, patientName, actionName, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext), roleID)));
        }

        [HttpPatch]
        [Route("DeleteAuditLog")]
        public JsonResult DeleteAuditLog(int id)
        {
            return Json(_auditLogService.ExecuteFunctions(() => _auditLogService.DeleteAuditLog(id, GetToken(HttpContext))));
        }

    }
}