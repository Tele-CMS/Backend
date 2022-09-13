using HC.Model;
using HC.Patient.Model.Payer;
using HC.Patient.Service.Payer.Interfaces;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{   
    [Produces("application/json")]
    [Route("Payer")]
    [ActionFilter]
    public class PayerController : BaseController
    {        
        private readonly IPayerInformationService _payerServiceCodesService;

        #region Construtor of the class
        public PayerController(IPayerInformationService payerServiceCodesService)
        {
            _payerServiceCodesService = payerServiceCodesService;
        }
        #endregion

        #region Class Methods        
        /// <summary>
        /// Get All payer details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPayers")]
        public JsonResult GetAllPayers(string payerName, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerInformationByFilter(GetToken(HttpContext).OrganizationID, payerName, pageNumber, pageSize, sortColumn, sortOrder, true)));
        }

        /// <summary>
        /// <Description>Get master appointment types associated with payer with green else red.</Description>
        /// </summary>
        [HttpGet]
        [Route("GetAppointmentTypeForPayer/{payerId}")]
        public JsonResult GetAppointmentTypeForPayer(int payerId)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetMasterActivitiesForPayer(payerId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get payer service code detail by Id
        /// </summary>
        /// <param name="payerAppointmentTypeId"></param>
        /// <param name="payerServiceCodeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerServiceCodeDetailsById")]
        public JsonResult GetPayerServiceCodeDetailsById(int payerAppointmentTypeId = 0, int payerServiceCodeId = 0)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerServiceCodeDetailsById(payerAppointmentTypeId, payerServiceCodeId, GetToken(HttpContext))));
        }

        [HttpPatch]
        [Route("UpdatePayerAppointmentTypes")]
        public JsonResult UpdatePayerAppointmentTypes([FromBody]PayerAppointmentTypesModel payerAppointmentTypesModel)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.UpdatePayerAppointmentTypes(payerAppointmentTypesModel, GetToken(HttpContext))));
        }

        [HttpGet("GetPayerServiceCodes")]
        public JsonResult GetPayerServiceCodes(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder)//([FromBody]PayerSearchFilter payerSearchFilter)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerServiceCodesByFilter(id, id2, name, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext).OrganizationID)));
        }

        [HttpGet("GetMasterServiceCodes")]
        public JsonResult GetMasterServiceCodes(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetMasterServiceCodesByFilter(id, id2, name, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext).OrganizationID)));
        }

        [HttpGet("GetMasterServiceCodesEx")]
        public JsonResult GetMasterServiceCodesEx(int payerID)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetMasterServiceCodesEx(payerID, GetToken(HttpContext).OrganizationID)));
        }


        [HttpGet("GetPayerServiceCodesByPayerId")]
        public JsonResult GetPayerServiceCodesByPayerId(int payerID)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerServiceCodesByPayerId(payerID, GetToken(HttpContext).OrganizationID)));
        }

        [HttpGet("GetPayerServiceCodesEx")]
        public JsonResult GetPayerServiceCodesEx(int payerID, int activityID)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerServiceCodesEx(payerID, activityID, GetToken(HttpContext).OrganizationID)));
        }

        [HttpGet("GetPayerActivity")]
        public JsonResult GetPayerActivity(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerActivityByFilter(id, id2, name, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext).OrganizationID)));
        }

        [HttpGet("GetPayerActivityCodes")]
        public JsonResult GetPayerActivityCodes(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.GetPayerActivityCodesByFilter(id, id2, name, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext).OrganizationID)));
        }

        /// <summary>
        /// this will delete payer service code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeletePayerServiceCode")]
        public JsonResult DeletePayerServiceCode(int id)
        {
            return Json(_payerServiceCodesService.ExecuteFunctions(() => _payerServiceCodesService.DeletePayerServiceCode(id, GetToken(HttpContext))));
        }
        #endregion
    }
}