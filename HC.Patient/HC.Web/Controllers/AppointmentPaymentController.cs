using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("AppointmentPayment")]
    [ActionFilter]
    public class AppointmentPaymentController : BaseController
    {
        private readonly IAppointmentPaymentService _appointmentPaymentService;
        public AppointmentPaymentController(
           IAppointmentPaymentService appointmentPaymentService
            )
        {
            _appointmentPaymentService = appointmentPaymentService;
        }

        [HttpPost]
        [Route("Payments")]
        public JsonResult AppointmentPayments([FromBody] PaymentFilterModel paymentFilterModel)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetAppointmentPaymentList(paymentFilterModel, GetToken(HttpContext))));
        }
        [HttpPost]
        [Route("Refunds")]
        public JsonResult AppointmentRefunds([FromBody] RefundFilterModel refundFilterModel)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetAppointmentRefundList(refundFilterModel, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("ClientPayments")]
        public JsonResult AppointmentClientPayments([FromBody] PaymentFilterModel paymentFilterModel)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetClientAppointmentPaymentList(paymentFilterModel, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("ClientNetAppointmentPayment")]
        public JsonResult GetClientNetAppointmentPayment(int clientId)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetClientNetAppointmentPayment( clientId, GetToken(HttpContext))));
        }
        [HttpPost]
        [Route("ClientRefunds")]
        public JsonResult AppointmentClientRefunds([FromBody] RefundFilterModel refundFilterModel)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetClientAppointmentRefundList(refundFilterModel, GetToken(HttpContext))));
        }

       
        [HttpGet]
        [Route("GetPaymentPdf")]
        public IActionResult GetPaymentPdf( PaymentFilterModel paymentFilterModel)
        {
            MemoryStream tempStream = null;
            tempStream = _appointmentPaymentService.GetPaymentPdf(paymentFilterModel, GetToken(HttpContext));
            return File((tempStream != null ? tempStream : new MemoryStream()), "application/pdf", Convert.ToString(paymentFilterModel.StaffId) + "-" + "PaymentPDF");
        }
        [HttpGet("GetPastUpcomingAppointment")]
        public JsonResult GetPastUpcomingAppointment(string locationIds, string staffIds, string patientIds)
        {
            return Json(_appointmentPaymentService.ExecuteFunctions(() => _appointmentPaymentService.GetPastUpcomingAppointment(locationIds, staffIds, patientIds, GetToken(HttpContext))));
        }
    }
}