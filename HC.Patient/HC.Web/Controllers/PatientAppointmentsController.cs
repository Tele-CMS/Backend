using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Reports;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Web.Filters;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.Evaluation;
using Ical.Net.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/PatientAppointments")]
    [ActionFilter]
    public class PatientAppointmentsController : BaseController
    {
        private JsonModel response;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IPatientService _patientService;
        private readonly ILocationService _locationService;
        #region Construtor of the class
        public PatientAppointmentsController(IPatientAppointmentService patientAppointmentService, IPatientService patientService, ILocationService locationService)
        {
            _patientAppointmentService = patientAppointmentService;
            _patientService = patientService;
            _locationService = locationService;
        }
        #endregion

        /// <summary>
        /// This method will return all the appointments for scheduler satisfying the conditions of scheduler
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="staffIds"></param>
        /// <param name="patientIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="patientTags"></param>
        /// <param name="staffTags"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientAppointmentList")]
        public JsonResult GetPatientAppointmentList(string locationIds = "", string staffIds = "", string patientIds = "", DateTime? fromDate = null, DateTime? toDate = null, string patientTags = "", string staffTags = "", string timeZone = "India Standard Time")
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPatientAppointmentList(locationIds, staffIds, patientIds, fromDate, toDate, patientTags, staffTags, GetToken(HttpContext))));
        }
        /// <summary>
        /// Description - This method will save patient appointment.When the apoiontment will be single it will be saved directly
        /// When appointment will be recurring then this method will return all the recurrences in response.From UI users can reschedule appointments and then save bulk appointments
        /// </summary>
        /// <param name="patientApptList"></param>
        /// <param name="IsFinish"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePatientAppointment")]
        public JsonResult SavePatientAppointment([FromBody]List<PatientAppointmentModel> patientApptList, bool IsFinish = false, bool isAdmin = false)
        {
            try
            {
                TokenModel token = GetToken(HttpContext);
                LocationModel locationModal = _locationService.GetLocationOffsets(patientApptList[0].ServiceLocationID,token);
                if (patientApptList != null && patientApptList.Count > 0)
                {
                    if (!IsFinish)//recurrence case to show all virtual aapointments on screen
                    {
                        DateTime currentDate = patientApptList[0].StartDateTime;
                        //patientApptList[0].StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        //patientApptList[0].EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        //patientApptList[0].AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointment(String.Join(",", patientApptList[0].AppointmentStaffs.Select(x => x.StaffId).ToArray()), patientApptList[0].StartDateTime, patientApptList[0].EndDateTime, currentDate, patientApptList[0].PatientAppointmentId, patientApptList[0].PatientID, patientApptList[0].AppointmentTypeID, token);
                        patientApptList[0].AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointmentWithLocation(String.Join(",", patientApptList[0].AppointmentStaffs.Select(x => x.StaffId).ToArray())
                                , CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , currentDate, patientApptList[0].PatientAppointmentId, patientApptList[0].PatientID, patientApptList[0].AppointmentTypeID
                                , CommonMethods.GetCurrentOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName)
                                , token);
                        if (!string.IsNullOrEmpty(patientApptList[0].RecurrenceRule) && patientApptList[0].PatientAppointmentId == 0)
                        {
                            GetOccurencesFromRRule(patientApptList[0], patientApptList, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                            //response = _patientAppointmentService.SaveAppointment(patientApptList[0], patientApptList.Skip(1).ToList(), token);
                            
                            patientApptList[0].StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            patientApptList[0].EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            //patientApptList[0].StartDateTime=CommonMethods.ConvertFromUtcTimeWithOffset(patientApptList[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            //patientApptList[0].EndDateTime=CommonMethods.ConvertFromUtcTimeWithOffset(patientApptList[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientApptList.ForEach(x =>
                            {
                                x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                                x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                            });
                            
                            response = new JsonModel(patientApptList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                        }
                        else response = _patientAppointmentService.SaveAppointment(patientApptList[0], null, isAdmin, token);
                    }
                    else
                    {
                        patientApptList.ForEach(x =>
                        {
                            DateTime dtStartDatetime = x.StartDateTime;                          
                            x.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            x.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                            x.OffSet = (int)CommonMethods.GetCurrentOffset(dtStartDatetime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName);
                        });
                        response = _patientAppointmentService.SaveAppointment(patientApptList[0], patientApptList.Skip(1).ToList(), isAdmin, token);
                    }
                }
                return Json(response);
            }
            catch
            {
                return Json(new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError));
            }
        }
        [HttpPatch]
        [Route("DeleteAppointment")]
        public JsonResult DeleteAppointment(int appointmentId, int? parentAppointmentId, bool deleteSeries, bool isAdmin = false)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.DeleteAppointment(appointmentId, parentAppointmentId, deleteSeries, isAdmin, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetAppointmentDetails")]
        public JsonResult GetAppointmentDetails(int appointmentId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetAppointmentDetails(appointmentId, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("GetAppointmentDetailsWithPatient")]
        public JsonResult GetAppointmentDetailsWithPatient(int appointmentId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetAppointmentDetails(appointmentId, GetToken(HttpContext),true)));
        }
        [HttpGet]
        [Route("GetStaffAndPatientByLocation")]
        public JsonResult GetStaffAndPatientByLocation(string locationIds = "", string permissionKey = "", string isActiveCheckRequired = "", int organizationID = 0)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetStaffAndPatientByLocation(locationIds, permissionKey, isActiveCheckRequired, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetStaffByLocation")]
        public JsonResult GetStaffByLocation(string locationIds = "", string isActiveCheckRequired = "")
        {
            return Json(_patientAppointmentService.ExecuteFunctions<JsonModel>(() => _patientAppointmentService.GetStaffByLocation(locationIds, isActiveCheckRequired, GetToken(HttpContext))));
        }

        [NonAction]
        public List<PatientAppointmentModel> GetOccurencesFromRRule(PatientAppointmentModel patientAppointment, List<PatientAppointmentModel> patientApptList, decimal daylightOffset, decimal standardOffset,string timeZone, TokenModel token)
        {
            List<Occurrencess> occurences = new List<Occurrencess>();
            if (!string.IsNullOrEmpty(patientAppointment.RecurrenceRule))
            {
                #region recurrence rule

                RecurringComponent recurringComponent = new RecurringComponent();
                RecurrencePattern pattern = new RecurrencePattern();
                try
                {
                    pattern = new RecurrencePattern(patientAppointment.RecurrenceRule.Substring(0, patientAppointment.RecurrenceRule.IndexOf("EXDATE", 6) - 4));
                }
                catch (Exception)
                {
                    pattern = new RecurrencePattern(patientAppointment.RecurrenceRule);
                }
                pattern.RestrictionType = RecurrenceRestrictionType.NoRestriction;

                var us = new CultureInfo("en-US");

                var startDate = new CalDateTime(patientAppointment.StartDateTime, "UTC");
                var fromDate = new CalDateTime(patientAppointment.StartDateTime, "UTC");
                var toDate = new CalDateTime();
                if (pattern.Until != null && pattern.Until != DateTime.MinValue)
                {
                    toDate = new CalDateTime(pattern.Until, "UTC");
                }
                else if (pattern.Count != 0)
                {
                    if (pattern.Frequency.ToString() == "Monthly")
                    {
                        toDate = new CalDateTime(patientAppointment.EndDateTime.AddMonths(pattern.Interval * pattern.Count), "UTC");
                    }
                    else if (pattern.Frequency.ToString() == "Yearly")
                    {
                        toDate = new CalDateTime(patientAppointment.EndDateTime.AddYears(pattern.Interval * pattern.Count), "UTC");
                    }
                    else
                    {
                        toDate = new CalDateTime(patientAppointment.EndDateTime.AddDays(pattern.Interval * pattern.Count), "UTC");
                    }
                }
                //else
                //{
                //    toDate = new CalDateTime(patientAppointment.EndDateTime.AddYears(pattern.Count), "UTC");
                //}
                //else if (pattern.Count != 0)
                //{
                //    toDate = new CalDateTime(patientAppointment.EndDateTime.AddDays(pattern.Interval * pattern.Count), "UTC");
                //}
                //else
                //{
                //    toDate = new CalDateTime(patientAppointment.EndDateTime.AddYears(2), "UTC");
                //}


                var indexException = patientAppointment.RecurrenceRule.IndexOf("EXDATE", 6);

                var exceptionDate = patientAppointment.RecurrenceRule.Substring(indexException + 7);

                PeriodList periodList = new PeriodList(exceptionDate);
                recurringComponent.ExceptionDates.Add(periodList);
                recurringComponent.RecurrenceRules.Add(pattern);
                var evaluator = recurringComponent.GetService(typeof(IEvaluator)) as IEvaluator;
                occurences = evaluator.Evaluate(
                            startDate,
                            DateUtil.SimpleDateTimeToMatch(fromDate, startDate),
                            DateUtil.SimpleDateTimeToMatch(toDate, startDate),
                            true)
                            .OrderBy(o => o.StartTime).Select(l => new Occurrencess() { Occurrence = l })
                            .ToList();
                var endDate = new CalDateTime(patientAppointment.EndDateTime, "UTC");
                TimeSpan t = patientAppointment.EndDateTime - patientAppointment.StartDateTime;
                occurences.ForEach(m => m.Occurrence.Duration = t);
                //patientAppointment.RecurrencePattern = pattern;
                #endregion
            }
            List<PatientAppointmentModel> patientAppointmentList = new List<PatientAppointmentModel>();
            patientAppointmentList = occurences.Select(k => new PatientAppointmentModel()
            {
                AppointmentTypeID = patientAppointment.AppointmentTypeID,
                //EndDateTime = k.Occurrence.EndTime.AsUtc,//.AsSystemLocal,// _patientAppointmentService.LocalClientToServer(k.Occurrence.EndTime.AsSystemLocal,offset),
                EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.EndTime.AsUtc, daylightOffset, standardOffset, timeZone),
                IsClientRequired = patientAppointment.IsClientRequired,
                Notes = patientAppointment.Notes,
                ParentAppointmentId = patientAppointment.PatientAppointmentId,
                PatientID = patientAppointment.PatientID,
                PatientAddressID = patientAppointment.PatientAddressID,
                RecurrenceRule = patientAppointment.RecurrenceRule,
                //StartDateTime = k.Occurrence.StartTime.AsUtc,//.AsSystemLocal,//_patientAppointmentService.LocalClientToServer(k.Occurrence.StartTime.AsSystemLocal,offset),
                StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.StartTime.AsUtc, daylightOffset, standardOffset,timeZone),
                ServiceLocationID = patientAppointment.ServiceLocationID,
                OfficeAddressID = patientAppointment.OfficeAddressID,
                IsExcludedFromMileage = patientAppointment.IsExcludedFromMileage,
                IsDirectService = patientAppointment.IsDirectService,
                CustomAddressID = patientAppointment.CustomAddressID,
                CustomAddress = patientAppointment.CustomAddress,
                Latitude = patientAppointment.Latitude,
                Longitude = patientAppointment.Longitude,
                ApartmentNumber = patientAppointment.ApartmentNumber,
                IsTelehealthAppointment = patientAppointment.IsTelehealthAppointment,
                Mileage = patientAppointment.Mileage,
                DriveTime = patientAppointment.DriveTime,
                PatientInsuranceId = patientAppointment.PatientInsuranceId,
                AuthorizationId = patientAppointment.AuthorizationId,
                //AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointment(String.Join(",", patientAppointment.AppointmentStaffs.Select(x => x.StaffId).ToArray()), k.Occurrence.StartTime.AsUtc, k.Occurrence.EndTime.AsUtc, k.Occurrence.StartTime.Value, null, patientAppointment.PatientID, patientAppointment.AppointmentTypeID, token)
                AvailabilityMessages = _patientAppointmentService.CheckIsValidAppointmentWithLocation(String.Join(",", patientAppointment.AppointmentStaffs.Select(x => x.StaffId).ToArray()), CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.EndTime.AsUtc, daylightOffset, standardOffset,timeZone),
                            CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.EndTime.AsUtc, daylightOffset, standardOffset,timeZone),
                            CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.StartTime.AsUtc, daylightOffset, standardOffset,timeZone), null, patientAppointment.PatientID, patientAppointment.AppointmentTypeID, CommonMethods.GetCurrentOffset(CommonMethods.ConvertToUtcTimeWithOffset(k.Occurrence.StartTime.AsUtc, daylightOffset, standardOffset,timeZone), daylightOffset, standardOffset,timeZone), token)
            }).ToList();
            if (patientAppointmentList.Count > 1)
                patientApptList.AddRange(patientAppointmentList.Skip(1).ToList());
            return patientApptList;
        }
        [HttpPost]
        [Route("CheckIsValidAppointment")]
        public JsonResult CheckIsValidAppointment([FromBody]List<PatientAppointmentModel> patientAppointmentModel)
        {
            try
            {
                TokenModel token = GetToken(HttpContext);
                if (patientAppointmentModel != null && patientAppointmentModel.Count > 0)

                    response = new JsonModel()
                    {
                        data = (patientAppointmentModel != null && patientAppointmentModel.Count > 0) ?
                        _patientAppointmentService.CheckIsValidAppointment((patientAppointmentModel[0].AppointmentStaffs == null ? "" : String.Join(",", patientAppointmentModel[0].AppointmentStaffs.Select(x => x.StaffId).ToArray())),
                        CommonMethods.ConvertUtcTime(patientAppointmentModel[0].StartDateTime, token),
                        CommonMethods.ConvertUtcTime(patientAppointmentModel[0].EndDateTime, token),
                        patientAppointmentModel[0].StartDateTime,
                        patientAppointmentModel[0].PatientAppointmentId, patientAppointmentModel[0].PatientID, patientAppointmentModel[0].AppointmentTypeID,
                        token) : null,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
            }
            catch
            {
                response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError);
            }
            return Json(response);
        }


        [HttpPost]
        [Route("CheckIsValidAppointmentWithLocation")]
        public JsonResult CheckIsValidAppointmentWithLocation([FromBody]List<PatientAppointmentModel> patientAppointmentModel)
        {
            try
            {
                TokenModel token = GetToken(HttpContext);

                LocationModel locationModal = _locationService.GetLocationOffsets(patientAppointmentModel[0].LocationId,token);

                if (patientAppointmentModel != null && patientAppointmentModel.Count > 0)

                    response = new JsonModel()
                    {
                        data = (patientAppointmentModel != null && patientAppointmentModel.Count > 0) ?                        
                        _patientAppointmentService.CheckIsValidAppointmentWithLocation((patientAppointmentModel[0].AppointmentStaffs == null ? "" : String.Join(",", patientAppointmentModel[0].AppointmentStaffs.Select(x => x.StaffId).ToArray())),
                        CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset,locationModal.TimeZoneName),
                        CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel[0].EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName),
                        patientAppointmentModel[0].StartDateTime,
                        patientAppointmentModel[0].PatientAppointmentId, patientAppointmentModel[0].PatientID, patientAppointmentModel[0].AppointmentTypeID,
                        CommonMethods.GetCurrentOffset(patientAppointmentModel[0].StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName),
                        token) : null,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK 
                    };
            }
            catch
            {
                response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError);
            }
            return Json(response);
        }

        /// <summary>
        /// This action will get all the activities linked to the primary payer of the patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActivitiesForPatientPayer/{patientId}")]
        public JsonResult GetActivitiesForPatientPayer(int patientId, DateTime startDate, DateTime endDate)
        {
            return Json(_patientService.ExecuteFunctions(() => _patientService.GetActivitiesForPatientPayer(patientId, startDate, endDate, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get All authorization for a activity CPTCodes for a date for patient primary payer
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="patientId"></param>
        /// <param name="appointmentTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isAdmin"></param>
        /// <param name="patientInsuranceId"></param>
        /// <param name="authorizationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientAuthorizationData")]
        public JsonResult GetPatientAuthorizationData(Nullable<int> appointmentId, int patientId, int appointmentTypeId, DateTime startDate, DateTime endDate, bool isAdmin = false, Nullable<int> patientInsuranceId = null, Nullable<int> authorizationId = null)
        {
            return Json(_patientService.ExecuteFunctions(() => _patientService.GetPatientAuthorizationData(appointmentId, patientId, appointmentTypeId, startDate, endDate, isAdmin, patientInsuranceId, authorizationId, GetToken(HttpContext))));
        }


        /// <summary>
        /// Get Patient payer activities,patient addresses for scheduler
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="locationId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="patientInsuranceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDataForSchedulerByPatient")]
        public JsonResult GetDataForSchedulerByPatient(Nullable<int> patientId, int locationId, DateTime startDate, DateTime endDate, Nullable<int> patientInsuranceId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetDataForSchedulerByPatient(patientId, locationId, startDate, endDate, patientInsuranceId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Update authorization cptcode blocked unit when new code will be added or deleted in patient encounter
        /// </summary>
        /// <param name="authProcedureCPTLinkId"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("UpdateServiceCodeBlockedUnit/{authProcedureCPTLinkId}/{actionName}")]
        public JsonResult UpdateServiceCodeBlockedUnit(int authProcedureCPTLinkId, string actionName)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.UpdateServiceCodeBlockedUnit(authProcedureCPTLinkId, actionName, GetToken(HttpContext))));
        }

        /// <summary>
        /// cancel appointments with reason and reason type
        /// </summary>
        /// <param name="cancelApt"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelAppointments")]
        public JsonResult CancelAppointments([FromBody]CancelAppointmentModel cancelApt)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.CancelAppointments(cancelApt.Ids, cancelApt.CancelTypeId, cancelApt.CancelReason, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description - activate appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("ActivateAppointments")]
        public JsonResult ActivateAppointments(int appointmentId)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.ActivateAppointments(appointmentId, false, GetToken(HttpContext))));
        }

        /// <summary>
        /// Added status and authorization check when appointment will be approved
        /// </summary>
        /// <param name="apptStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAppointmentStatus")]
        public JsonResult UpdateAppointmentStatus([FromBody]AppointmentStatusModel apptStatus)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.UpdateAppointmentStatus(apptStatus, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Pending Appointment List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPendingAppointmentList")]
        public JsonResult GetPendingAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.PENDING;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get Cancelled Appointment List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCancelledAppointmentList")]
        public JsonResult GetCancelledAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.CANCELLED;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Tentative Appointment List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTentativeAppointmentList")]
        public JsonResult GetTentativeAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.TENTATIVE;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Accepted/Approved Appointment List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAcceptedOrApprovedAppointmentList")]
        public JsonResult GetAcceptedOrApprovedAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            string[] statusArray = { AppointmentStatus.ACCEPTED, AppointmentStatus.APPROVED };
            appointmentFilterModel.Status = string.Join(",", statusArray);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Patient's past and upcoming Appointment List
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPastAndUpcomingAppointmentsList")]
        public JsonResult GetPastAndUpcomingAppointmentsList(int patientId, DateTime dateTime)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPastAndUpcomingAppointmentsList(patientId, dateTime, GetToken(HttpContext))));
        }

        #region Invited Appointments
        [HttpGet]
        [Route("GetPendingInvitationAppointmentList")]
        public JsonResult GetPendingInvitationAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.INVITED;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("GetAcceptedInvitationAppointmentList")]
        public JsonResult GetAcceptedInvitationAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.INVITATION_ACCEPTED;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        [HttpGet]
        [Route("GetRejectedInvitationAppointmentList")]
        public JsonResult GetRejectedInvitationAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            appointmentFilterModel.Status = AppointmentStatus.INVITATION_REJECTED;
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPendingAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }
        #endregion Invited Appointments

        [HttpGet]
        [Route("GetLastNewAppointment")]
        public JsonResult GetLastNewAppintment(string providerId)
        {
            int staffId = 0;
            bool decripted = int.TryParse(providerId, out staffId);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(providerId.Replace(" ", "+")), out staffId);
            }
            var token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetLastNewAppintment(staffId, token)));
        }

        [HttpPost]
        [Route("GetPreviousAppointment")]
        public JsonResult GetPreviousAppointment(string providerId,int clientId)
        {
           
            int staffId = 0;
            bool decripted = int.TryParse(providerId, out staffId);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(providerId.Replace(" ", "+")), out staffId);
            }
            var token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetPreviousAppintment(staffId, clientId,token)));
        }

        [HttpGet]
        [Route("GetAppointmentDetailsAsList")]
        public JsonResult GetAppointmentDetailsAsList(int appointmentId)
        {
            var result = _patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetAppointmentDetails(appointmentId, GetToken(HttpContext)));
            if(result.data != null)
            {
                result.data = new List<PatientAppointmentModel>() { (PatientAppointmentModel) result.data };
            }
            return Json(result);
        }

        
        

        [HttpGet]
        [Route("GetLastUrgentCareCallStatus")]
        public JsonResult GetLastUrgentCareCallStatus(string userId)
        {

            int staffId = 0;
            bool decripted = int.TryParse(userId, out staffId);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(userId.Replace(" ", "+")), out staffId);
            }
            var token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetLastUrgentCareCallStatus(staffId, token)));
        }

        [HttpGet]
        [Route("GetLastUrgentCareCallStatusForPatientPortal")]
        public JsonResult GetLastUrgentCareCallStatusForPatientPortal(string userId)
        {
            int patientuserid = 0;
            bool decripted = int.TryParse(userId, out patientuserid);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(userId.Replace(" ", "+")), out patientuserid);
            }
            var token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetLastUrgentCareCallStatusForPatientPortal(patientuserid, token)));
        }
        [HttpGet]
        [Route("GetLastPatientFollowupDetails")]
        public JsonResult GetLastPatientFollowupDetails(string  userId)
        {
            int patientuserid = 0;
            bool decripted = int.TryParse(userId, out patientuserid);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(userId.Replace(" ", "+")), out patientuserid);
            }

            var token = GetToken(HttpContext);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetLastPatientFollowupDetails(patientuserid, token)));
        }

        [HttpGet]
        [Route("GetFilteredAppointmentList")]
        public JsonResult GetFilteredAppointmentList(string locationIds = "", string staffIds = "", string patientIds = "", DateTime? fromDate = null, DateTime? toDate = null, string patientTags = "", string staffTags = "", string timeZone = "India Standard Time", string appttype = "", string apptmode = "")
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetFilteredAppointmentList(locationIds, staffIds, patientIds, fromDate, toDate, patientTags, staffTags, appttype, apptmode, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetUrgentCareAppointmentList")]
        public JsonResult GetUrgentCareAppointmentList(PatientAppointmentFilterModel appointmentFilterModel)
        {
            string[] statusArray = { AppointmentStatus.ACCEPTED, AppointmentStatus.APPROVED };
            appointmentFilterModel.Status = string.Join(",", statusArray);
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetUrgentCareAppointmentList(appointmentFilterModel, GetToken(HttpContext))));
        }

        #region Appointments for Reporting
        [HttpGet]
        [Route("GetFilteredAppointmentsForReporting")]
       // [Authorize(Roles = "ADMIN, STAFF")]
        public IActionResult GetFilteredAppointmentsForReporting(AppointmentReportingFilterModel appointmentReportingFilter)
        {
            return Json(_patientAppointmentService.ExecuteFunctions(() => _patientAppointmentService.GetFilteredAppointmentsForReporting(appointmentReportingFilter, GetToken(HttpContext))));
        }
        #endregion

    }
}