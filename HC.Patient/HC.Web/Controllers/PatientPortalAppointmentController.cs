using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Web.Filters;
using HC.Patient.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientPortalAppointment")]
    [ActionFilter]
    public class PatientPortalAppointmentController : BaseController
    {
        private readonly IStaffAvailabilityService _staffAvailabilityService;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IStaffService _staffService;
        private readonly IStaffRepository _staffRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        public PatientPortalAppointmentController(IStaffAvailabilityService staffAvailabilityService, IStaffService staffService, IStaffRepository staffRepository, IPatientAppointmentService patientAppointmentService, IGlobalCodeService globalCodeService, IHubContext<ChatHub> chatHubContext)
        {
            _staffAvailabilityService = staffAvailabilityService;
            _patientAppointmentService = patientAppointmentService;
            _staffService = staffService;
            _staffRepository = staffRepository;
            _globalCodeService = globalCodeService;
            _chatHubContext = chatHubContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listingFiltterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffs")]
        public JsonResult GetStaffs(ListingFiltterModel listingFiltterModel)
        {
            return Json(GetStaff(listingFiltterModel, GetToken(HttpContext)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet("GetStaffAvailability")]
        public JsonResult GetStaffAvailability(string staffID)
        {
            return Json(_staffAvailabilityService.ExecuteFunctions(() => _staffAvailabilityService.GetStaffAvailabilty(staffID, GetToken(HttpContext), false)));
        }

        /// <summary>
        /// Description - Save Appointment from Patient Portal
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePatientAppointment")]
        public JsonResult SavePatientAppointment([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.SaveAppointmentFromPatientPortal(patientAppointmentModel, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("BookNewAppointment")]
        public JsonResult BookNewAppointment([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            TokenModel token = GetToken(HttpContext);
            patientAppointmentModel.PatientID = patientAppointmentModel.PatientID!=null && patientAppointmentModel.PatientID != 0? patientAppointmentModel.PatientID:  token.StaffID;
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.BookNewAppointementFromPatientPortal(patientAppointmentModel,token)));
        }




        [HttpPost]
        [Route("RescheduleAppointment")]
        public JsonResult RescheduleAppointment([FromBody] RescheduleModel rescheduleModel)
        {
            TokenModel token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.RescheduleAppointment(rescheduleModel, token)));
        }

        [NonAction]
        private JsonModel GetStaff(ListingFiltterModel listingFiltterModel, TokenModel tokenModel)
        {
            listingFiltterModel.pageSize = 100;
            List<StaffModels> staffModels = _staffRepository.GetStaff<StaffModels>(listingFiltterModel, tokenModel).ToList();
            if (staffModels != null && staffModels.Count > 0)
            {
                List<StaffDropDownModel> staffListing = staffModels.Select(a => new StaffDropDownModel()
                {
                    StaffID = a.StaffID,
                    Name = a.FirstName + " " + a.LastName
                }).ToList();

                response = new JsonModel(staffListing, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NoContent;
                response = new JsonModel(new object(), UserAccountNotification.NoDataFound, (int)HttpStatusCodes.NoContent);//(Status Ok)}
            }
            return response;
        }

        /// <summary>
        /// Description - Delete appointment from patient portal when it will be in pending status
        /// </summary>
        /// <param name="Id">AppointmentId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteAppointment/{Id}")]
        public JsonResult DeleteAppointment(int Id)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.DeleteAppointment(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - Save Appointment from Patient Portal for Mobile
        /// </summary>
        /// <param name="patientAppointmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePatientAppointmentForMobile")]
        public JsonResult SavePatientAppointmentForMobile([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.UpdateAppointmentFromPatientPortal(patientAppointmentModel, GetToken(HttpContext))));
        }

        [HttpPost]
        [Route("BookNewFreeAppointment")]
        public JsonResult BookNewFreeAppointment([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            TokenModel token = GetToken(HttpContext);
            //patientAppointmentModel.PatientID = token.StaffID;
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.BookNewFreeAppointementFromPatientPortal(patientAppointmentModel, token)));
        }

        [HttpPost]
        [Route("BookUrgentCareAppointment")]
        public JsonResult BookUrgentCareAppointment([FromBody] PatientAppointmentModel patientAppointmentModel)
        {
            TokenModel token = GetToken(HttpContext);
            //patientAppointmentModel.StartDateTime
            patientAppointmentModel.PatientID = patientAppointmentModel.PatientID != null && patientAppointmentModel.PatientID != 0 ? patientAppointmentModel.PatientID : token.StaffID;
            var result = _patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.BookUrgentCareAppointment(patientAppointmentModel, token));
            if(result.StatusCode==200 && result.data!=null)
            {
                int staffuserid = _patientAppointmentService.GetStaffUserId((int)(result.data));
                _chatHubContext.Clients.Group(staffuserid.ToString()).SendAsync("CallProviderUrgentCare", result.data);
            }
                return Json(result);
        }

        [HttpPost]
        [Route("UrgentCareRefundAppointmentFee")]
        public JsonResult UrgentCareRefundAppointmentFee(int appointmentId)
        {
            
            var result = _patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.UrgentCareRefundAppointmentFee(appointmentId, GetToken(HttpContext)));
            if(result.StatusCode == 200)
            {
                int patientuserid = _patientAppointmentService.GetPatientUserId(appointmentId);
                _chatHubContext.Clients.Group(patientuserid.ToString()).SendAsync("CallPatientUrgentCare", appointmentId);
            }
            return Json(result);
        }

    }
}