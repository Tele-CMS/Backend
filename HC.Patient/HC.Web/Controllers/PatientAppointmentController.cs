//using Audit.WebApi;
using HC.Common.Filters;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.PatientAppointment;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.Evaluation;
using Ical.Net.Utility;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using HC.Model;
using HC.Common.HC.Common;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Common;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Web.Controllers
{
    [ServiceFilter(typeof(LogFilter))]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class PatientAppointmentController : CustomJsonApiController<PatientAppointment, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly HCOrganizationContext _context;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private JsonModel response;
        private CommonMethods commonMethods;
        #region Construtor of the class
        public PatientAppointmentController(
       IJsonApiContext jsonApiContext,
       IResourceService<PatientAppointment, int> resourceService,
       ILoggerFactory loggerFactory, ILogger<PatientAppointmentController> logger, HCOrganizationContext context,IPatientAppointmentService patientAppointmentService, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _patientAppointmentService = patientAppointmentService;
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                _context = context;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }

            catch
            {

            }
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
         {
            var disabledQueries = _jsonApiContext.GetControllerAttribute<DisableQueryAttribute>()?.QueryParams ?? QueryParams.None;
            FilterQuery filterQuery = new FilterQuery("", "", "");
            bool IsDateRange = false;
            string sDate = string.Empty;
            string eDate = string.Empty;
            List<int> PatientList = new List<int>();
            List<int> StaffList = new List<int>();
            List<int> LocationList = new List<int>();
            List<string> TagList = new List<string>();
            List<string> StaffTagList = new List<string>();
            //List<string> TagList = new List<string>();
            //var patientIds = from PT in _context.PatientTags
            //                 join MT in _context.MasterTags on PT.TagID equals MT.Id
            //                 where TagList.Contains(MT.Tag)
            //                 select PT.PatientID;

            if (_jsonApiContext.IncludedRelationships == null)
            {
                _jsonApiContext.IncludedRelationships = new List<string>();
               // _jsonApiContext.IncludedRelationships.Add("patient");
               // _jsonApiContext.IncludedRelationships.Add("patientencounter");
            }
            else
            {
               // _jsonApiContext.IncludedRelationships.Add("patient");
               // _jsonApiContext.IncludedRelationships.Add("patientencounter");
            }


            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.FROMDATE.ToString()) { sDate = p.Value; IsDateRange = true; filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.TODATE.ToString()) { eDate = p.Value; IsDateRange = true; filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.PATIENTID.ToString()) { PatientList.Add(int.Parse(p.Value)); filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.STAFFID.ToString()) { StaffList.Add(int.Parse(p.Value)); filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.LOCATIONID.ToString()) { LocationList.Add(int.Parse(p.Value)); filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.PATIENTTAGNAME.ToString()) { TagList.Add(p.Value); filterQuery = p; } });
            _jsonApiContext.QuerySet.Filters.ForEach(p => { if (p.Key.ToUpper() == PatientSearch.STAFFTAGNAME.ToString()) { StaffTagList.Add(p.Value); filterQuery = p; } });
            var filterlist = _jsonApiContext.QuerySet.Filters.Where(a => a.Key.ToUpper() == PatientSearch.PATIENTID.ToString() || a.Key.ToUpper() == PatientSearch.STAFFID.ToString() 
            || a.Key.ToUpper() == PatientSearch.LOCATIONID.ToString() || a.Key.ToUpper() == PatientSearch.PATIENTTAGNAME.ToString() || a.Key.ToUpper() == PatientSearch.FROMDATE.ToString()|| 
            a.Key.ToUpper() == PatientSearch.TODATE.ToString() || a.Key.ToUpper() == PatientSearch.STAFFTAGNAME.ToString()).ToList();
            foreach (var item in filterlist) { if (item.Key.ToUpper() == PatientSearch.PATIENTTAGNAME.ToString() || (item.Key.ToUpper() == PatientSearch.LOCATIONID.ToString() || 
                    item.Key.ToUpper() == PatientSearch.PATIENTID.ToString() || item.Key.ToUpper() == PatientSearch.STAFFID.ToString() || item.Key.ToUpper() == PatientSearch.STAFFTAGNAME.ToString()||
                    item.Key.ToUpper() == PatientSearch.FROMDATE.ToString() || item.Key.ToUpper() == PatientSearch.TODATE.ToString())) { _jsonApiContext.QuerySet.Filters.Remove(item); } }
            //Get All records
            var asyncPatientAppointments = await base.GetAsync();
            //
            var patientAppointments = (IQueryable<PatientAppointment>)(((ObjectResult)asyncPatientAppointments).Value);
            patientAppointments = patientAppointments.Include(k => k.Patient.PatientTags).AsQueryable();

            patientAppointments.ToList().ForEach(j => {
                if (j.AppointmentStaff != null)
                {
                    j.AppointmentStaff.ForEach(l => { l.Staffs = _context.Set<Staffs>().Where(h => h.Id == l.StaffID).FirstOrDefault(); });
                    j.AppointmentStaff.ForEach(l => { l.Staffs.StaffTags = _context.Set<StaffTags>().Where(h => h.StaffID == l.StaffID).ToList(); });
                }
            });


            var masterTags = _jsonApiContext.GetDbContextResolver().GetDbSet<MasterTags>().ToList();

            List<PatientAppointment> patientAppointment = patientAppointments.Where(k => k.ParentAppointmentID == null || k.ParentAppointmentID == 0).ToList();
            List<PatientAppointment> patientSoapAppointment = patientAppointments.Where(k => k.ParentAppointmentID != null && k.ParentAppointmentID != 0).ToList();

            if (PatientList.Count > 0) { patientAppointment = patientAppointment.Where(a => PatientList.Contains(Convert.ToInt32(a.PatientID))).ToList(); }

            //List<PatientAppointment> appointments = patientAppointment;
            //patientAppointment = new List<PatientAppointment>();

            //string OfficeStartHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Min(g => new{
            //    OfficeStartHour = g.OfficeStartHour
            //}).OfficeStartHour.Value.ToString("hh:mm:ss");

            //string OfficeEndHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Max(g => new {
            //    OfficeEndHour = g.OfficeEndHour
            //}).OfficeEndHour.Value.ToString("hh:mm:ss");


            //patientAppointment.Where(t2 => StaffList.Any(t1 => t2.AppointmentStaff.Contains(t1)));
            List<PatientAppointment> appointments = new List<PatientAppointment>();
            string OfficeStartHour = string.Empty;
            string OfficeEndHour = string.Empty;

            if (LocationList.Count > 0)
            {
                patientAppointment = patientAppointment.Where(a => LocationList.Contains(a.Patient!=null?a.Patient.LocationID:LocationList.FirstOrDefault())).ToList();

                //appointments = patientAppointment;
                //patientAppointment = new List<PatientAppointment>();

                //OfficeStartHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Min(k => k.OfficeStartHour).Value.ToString("hh:mm:ss");

                //OfficeEndHour = _dbContextResolver.GetDbSet<Location>().Where(j => LocationList.Contains(j.Id)).Max(g => g.OfficeEndHour.Value).ToString("hh:mm:ss");

                //patientAppointment.ForEach(k => { k.LocationOfficeStartHour = OfficeStartHour; k.LocationOfficeEndHour = OfficeEndHour; });
            }

            List<PatientAppointment> appointment2 = new List<PatientAppointment>();

            if (TagList.Count > 0)
            {
                patientAppointment.ForEach(appointment =>
                {
                    if (appointment.Patient!=null && appointment.Patient.PatientTags != null && appointment.Patient.PatientTags.Count() != 0)
                    {
                        appointment.Patient.PatientTags = appointment.Patient.PatientTags.Where(o => masterTags.Where(u=>TagList.Contains(u.Tag))
                        .Select(k=>k.Id).Contains(o.TagID)).ToList();
                        if (appointment.Patient.PatientTags != null && appointment.Patient.PatientTags.Count() != 0)
                        {
                            appointment2.Add(appointment);
                        }
                 }
                });
            }

            if ((appointment2 != null && appointment2.Count() != 0 )|| (TagList.Count > 0))
            {
                patientAppointment = new List<PatientAppointment>();
                patientAppointment = appointment2;
            }


            List<PatientAppointment> appointment3 = new List<PatientAppointment>();

            if (StaffTagList.Count > 0)
            {
                patientAppointment.ForEach(appointment =>
                {
                    if (appointment.AppointmentStaff!=null && appointment.AppointmentStaff.Count() != 0)
                    {
                        appointment.AppointmentStaff.ForEach(appstaff =>
                        {
                            appstaff.Staffs.StaffTags = appstaff.Staffs.StaffTags.Where(o => masterTags.Where(u => StaffTagList.Contains(u.Tag))
                            .Select(k => k.Id).Contains(o.TagID)).ToList();
                            if (appstaff.Staffs.StaffTags != null && appstaff.Staffs.StaffTags.Count() != 0)
                            {
                                appointment3.Add(appointment);
                            }
                        });
                    }
                });
            }

            if ((appointment3 != null && appointment3.Count() != 0) ||( StaffTagList.Count > 0 ))
            {
                patientAppointment = new List<PatientAppointment>();
                patientAppointment = appointment3;
            }

            if (StaffList.Count > 0)
            {
                patientAppointment.ForEach(appointment =>
                {

                    //appointment.LocationOfficeStartHour = OfficeStartHour;
                    //appointment.LocationOfficeEndHour = OfficeEndHour;
                    if (appointment.AppointmentStaff != null && appointment.AppointmentStaff.Count != 0)
                    {
                        appointment.AppointmentStaff = appointment.AppointmentStaff.Where(o => StaffList.Contains(o.StaffID)).ToList();
                        if (appointment.AppointmentStaff != null && appointment.AppointmentStaff.Count != 0)
                        {
                            appointments.Add(appointment);
                        }
                    }
                });
            }

           
            if (appointments != null && appointments.Count() != 0)
            {
                patientAppointment = new List<PatientAppointment>();
                patientAppointment = appointments;
            }
            patientAppointment.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.RecurrenceRule))
                {
                    RecurringComponent recurringComponent = new RecurringComponent();
                    RecurrencePattern pattern = new RecurrencePattern();
                    try
                    {
                        pattern = new RecurrencePattern(p.RecurrenceRule.Substring(0, p.RecurrenceRule.IndexOf("EXDATE", 6) - 4));
                    }
                    catch (Exception)
                    {
                        pattern = new RecurrencePattern(p.RecurrenceRule);
                    }
                    pattern.RestrictionType = RecurrenceRestrictionType.NoRestriction;

                    var us = new CultureInfo("en-US");

                    var startDate = new CalDateTime(p.StartDateTime, "UTC");
                    var fromDate = new CalDateTime(p.StartDateTime, "UTC");
                    var toDate = new CalDateTime();
                    if (pattern.Until != null && pattern.Until != DateTime.MinValue)
                    {
                        toDate = new CalDateTime(pattern.Until, "UTC");
                    }
                    else if (pattern.Count != 0 && pattern.Count != Int32.MinValue)
                    {
                        toDate = new CalDateTime(p.EndDateTime.AddDays(pattern.Interval * pattern.Count), "UTC");
                    }
                    else
                    {
                        toDate = new CalDateTime(p.EndDateTime.AddYears(2), "UTC");
                    }

                    //var evaluator = pattern.GetService(typeof(IEvaluator)) as IEvaluator;

                    var tt = DateUtil.SimpleDateTimeToMatch(toDate, startDate);



                    var indexException = p.RecurrenceRule.IndexOf("EXDATE", 6);

                    var exceptionDate = p.RecurrenceRule.Substring(indexException + 7);

                    PeriodList periodList = new PeriodList(exceptionDate);
                    recurringComponent.ExceptionDates.Add(periodList);
                    recurringComponent.RecurrenceRules.Add(pattern);

                    var evaluator = recurringComponent.GetService(typeof(IEvaluator)) as IEvaluator;

                    //p.Occurrences = evaluator.Evaluate(
                    //    startDate,
                    //    DateUtil.SimpleDateTimeToMatch(fromDate, startDate),
                    //    DateUtil.SimpleDateTimeToMatch(toDate, startDate),
                    //    true)
                    //    .OrderBy(o => o.StartTime).Select(l=>new Occurrences() { Occurrence=l })
                    //    .ToList();
                    var endDate = new CalDateTime(p.EndDateTime, "UTC");
                    TimeSpan t = p.EndDateTime - p.StartDateTime;
                    p.Occurrences.ForEach(m =>
                    {
                        m.Occurrence.Duration = t;

                        //if (p.Occurrences.IndexOf(m) > 0)
                        {
                            var patientSoap = patientSoapAppointment.Where(k => k.ParentAppointmentID == p.Id
                            && k.StartDateTime == m.Occurrence.StartTime.AsUtc && k.EndDateTime == m.Occurrence.EndTime.AsUtc).ToList();
                            if (patientSoap.Count != 0)
                            {
                                //p = patientSoap.FirstOrDefault();
                                m.ParentAppointmentID = Convert.ToInt32(patientSoap.FirstOrDefault().ParentAppointmentID);
                                m.AppointmentID = Convert.ToInt32(patientSoap.FirstOrDefault().Id);
                                //m.PatientEncounterID= Convert.ToInt32(patientSoap.FirstOrDefault().PatientEncounter.FirstOrDefault().Id);
                                var encounter = _dbContextResolver.GetDbSet<PatientEncounter>().Where(l => l.PatientAppointmentId == m.AppointmentID);
                                if (encounter.Count() > 0)
                                {
                                    m.PatientEncounterID = encounter.FirstOrDefault().Id;
                                }
                                
                            }
                            else
                            {
                                m.ParentAppointmentID = p.Id;
                                m.AppointmentID = null;
                                m.PatientEncounterID = null;
                            }
                        }
                    });
                    p.RecurrencePattern = pattern;

                    
                }
            });
            if (IsDateRange)
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();

                if (!DateTime.TryParse(sDate, out fromDate))
                {// handle parse failure
                    fromDate = DateTime.UtcNow;
                }
                if (!DateTime.TryParse(eDate, out toDate))
                {// handle parse failure
                    toDate = DateTime.UtcNow;
                }
                patientAppointment = patientAppointment.Where(a => a.StartDateTime > fromDate.AddDays(-1)).ToList();
                patientAppointment = patientAppointment.Where(a => a.StartDateTime < toDate.AddDays(1)).ToList();
            }
            ((ObjectResult)asyncPatientAppointments).Value = patientAppointment;

            return asyncPatientAppointments;
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int ID)
        {
            var asyncPatientAppointment = await base.GetAsync(ID);
            var patientAppointment = (PatientAppointment)((ObjectResult)asyncPatientAppointment).Value;

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
                    toDate = new CalDateTime(patientAppointment.EndDateTime.AddDays(pattern.Interval * pattern.Count), "UTC");
                }
                else
                {
                    toDate = new CalDateTime(patientAppointment.EndDateTime.AddYears(2), "UTC");
                }


                var indexException = patientAppointment.RecurrenceRule.IndexOf("EXDATE", 6);

                var exceptionDate = patientAppointment.RecurrenceRule.Substring(indexException + 7);

                PeriodList periodList = new PeriodList(exceptionDate);
                recurringComponent.ExceptionDates.Add(periodList);
                recurringComponent.RecurrenceRules.Add(pattern);




                var evaluator = recurringComponent.GetService(typeof(IEvaluator)) as IEvaluator;

                //patientAppointment.Occurrences = evaluator.Evaluate(
                //            startDate,
                //            DateUtil.SimpleDateTimeToMatch(fromDate, startDate),
                //            DateUtil.SimpleDateTimeToMatch(toDate, startDate),
                //            true)
                //            .OrderBy(o => o.StartTime).Select(l => new Occurrences() { Occurrence = l })
                //            .ToList();
                var endDate = new CalDateTime(patientAppointment.EndDateTime, "UTC");
                TimeSpan t = patientAppointment.EndDateTime - patientAppointment.StartDateTime;
                patientAppointment.Occurrences.ForEach(m => m.Occurrence.Duration = t);
                patientAppointment.RecurrencePattern = pattern;


                ((ObjectResult)asyncPatientAppointment).Value = patientAppointment;
                #endregion
            }
            return asyncPatientAppointment;
        }


        //[ValidateModel]
        //[HttpPost]
        //public override async Task<IActionResult> PostAsync([FromBody]PatientAppointment entity)
        //{
        //    await base.PostAsync(entity);
        //    if (entity.RecurrenceRule != null && entity.RecurrenceRule != string.Empty)
        //    {
        //        List<PatientAppointment> patientAppointmentList = GetOccurancesFromRRule(entity);

        //        await _context.PatientAppointment.AddRangeAsync(patientAppointmentList);
        //        await _context.SaveChangesAsync();
        //    }
        //    response = new JsonModel()
        //    {
        //        data = entity,
        //        Message = "Success",
        //        StatusCode = (int)HttpStatusCodes.Accepted
        //    };
        //    return Json(response);
        //}


        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]PatientAppointment entity)
        {
            return await base.PostAsync(entity);
        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PatientAppointment patientAppointment)
        {
            return await base.PatchAsync(id, patientAppointment);
        }
        #endregion


        [HttpPost("UpdatePatientAppointment")]
        public  List<PatientAppointmentsModel> UpdatePatientAppointment([FromBody]PatientAppointmentFilter payerInfoUpdateModel)
        {
            try
            {
                return  _patientAppointmentService.UpdatePatientAppointment(payerInfoUpdateModel);
                //return null;

            }
            catch (Exception)
            {
                throw;
            }
        }


        [NonAction]
        public List<PatientAppointment> GetOccurancesFromRRule(PatientAppointment patientAppointment)
        {
            return null;
            //if (!string.IsNullOrEmpty(patientAppointment.RecurrenceRule))
            //{
            //    #region recurrence rule

            //    RecurringComponent recurringComponent = new RecurringComponent();
            //    RecurrencePattern pattern = new RecurrencePattern();
            //    try
            //    {
            //        pattern = new RecurrencePattern(patientAppointment.RecurrenceRule.Substring(0, patientAppointment.RecurrenceRule.IndexOf("EXDATE", 6) - 4));
            //    }
            //    catch (Exception)
            //    {
            //        pattern = new RecurrencePattern(patientAppointment.RecurrenceRule);
            //    }
            //    pattern.RestrictionType = RecurrenceRestrictionType.NoRestriction;

            //    var us = new CultureInfo("en-US");

            //    var startDate = new CalDateTime(patientAppointment.StartDateTime, "UTC");
            //    var fromDate = new CalDateTime(patientAppointment.StartDateTime, "UTC");
            //    var toDate = new CalDateTime();
            //    if (pattern.Until != null && pattern.Until != DateTime.MinValue)
            //    {
            //        toDate = new CalDateTime(pattern.Until, "UTC");
            //    }
            //    else if (pattern.Count != 0)
            //    {
            //        toDate = new CalDateTime(patientAppointment.EndDateTime.AddDays(pattern.Interval * pattern.Count), "UTC");
            //    }
            //    else
            //    {
            //        toDate = new CalDateTime(patientAppointment.EndDateTime.AddYears(2), "UTC");
            //    }


            //    var indexException = patientAppointment.RecurrenceRule.IndexOf("EXDATE", 6);

            //    var exceptionDate = patientAppointment.RecurrenceRule.Substring(indexException + 7);

            //    PeriodList periodList = new PeriodList(exceptionDate);
            //    recurringComponent.ExceptionDates.Add(periodList);
            //    recurringComponent.RecurrenceRules.Add(pattern);




            //    var evaluator = recurringComponent.GetService(typeof(IEvaluator)) as IEvaluator;

            //    patientAppointment.Occurrences = evaluator.Evaluate(
            //                startDate,
            //                DateUtil.SimpleDateTimeToMatch(fromDate, startDate),
            //                DateUtil.SimpleDateTimeToMatch(toDate, startDate),
            //                true)
            //                .OrderBy(o => o.StartTime).Select(l => new Occurrences() { Occurrence = l })
            //                .ToList();
            //    var endDate = new CalDateTime(patientAppointment.EndDateTime, "UTC");
            //    TimeSpan t = patientAppointment.EndDateTime - patientAppointment.StartDateTime;
            //    patientAppointment.Occurrences.ForEach(m => m.Occurrence.Duration = t);
            //    patientAppointment.RecurrencePattern = pattern;


            //    #endregion
            //}

            //List<PatientAppointment> patientAppointmentList = new List<PatientAppointment>();
            //patientAppointmentList = patientAppointment.Occurrences.Select(k => new PatientAppointment(){
            //    AppointmentTypeID=patientAppointment.AppointmentTypeID,
            //    CreatedBy= patientAppointment.CreatedBy,
            //    CreatedDate= patientAppointment.CreatedDate,
            //    DeletedBy = null,
            //    DeletedDate=null,
            //    EndDateTime =  k.Occurrence.EndTime.AsSystemLocal,
            //    IsActive = patientAppointment.IsActive,
            //    IsClientRequired = patientAppointment.IsClientRequired,
            //    IsDeleted=false,
            //    Notes=patientAppointment.Notes,
            //    ParentAppointmentID=patientAppointment.Id,
            //    PatientID=patientAppointment.PatientID,
            //    PatientLocationID=patientAppointment.PatientLocationID,
            //    RecurrenceRule=patientAppointment.RecurrenceRule,
            //    ServiceAddressID=patientAppointment.ServiceAddressID,
            //    StartDateTime=k.Occurrence.StartTime.AsSystemLocal
                
                
            //}).ToList();
            //return patientAppointmentList;
        }


        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
    }
}
     