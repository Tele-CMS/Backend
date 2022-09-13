using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Availability;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.StaffAvailability;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.StaffAvailability
{
    public class StaffAvailabilityService : BaseService, IStaffAvailabilityService
    {
        private readonly IStaffAvailabilityRepository _staffAvailabilityRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly HCOrganizationContext _context;
        private readonly ILocationService _locationService;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        public StaffAvailabilityService(
            IStaffAvailabilityRepository staffAvailabilityRepository, 
            ILocationRepository locationRepository, 
            HCOrganizationContext context,
            ILocationService locationService)
        {
            _staffAvailabilityRepository = staffAvailabilityRepository;
            _locationRepository = locationRepository;
            _context = context;
            _locationService = locationService;
        }

        public JsonModel SaveStaffAvailabilty(AvailabilityModel entity, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {

                try
                {
                    List<Entity.StaffAvailability> dbStaffAvailabilityInsertList = new List<Entity.StaffAvailability>();
                    List<Entity.StaffAvailability> dbStaffAvailabilityUpdateList = new List<Entity.StaffAvailability>();
                    List<int> availabilityIds = new List<int>();

                    //Days ids
                    entity.Days.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Avaliable ids
                    entity.Available.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Unavaliable ids
                    entity.Unavailable.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //for update
                    dbStaffAvailabilityUpdateList = _staffAvailabilityRepository.GetAll(a => availabilityIds.Contains(a.Id)).ToList();  //_context.StaffAvailability.Where(a => availabilityIds.Contains(a.Id)).ToList();

                    foreach (var day in entity.Days)
                    {
                        if (day.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == day.Id)
                                {
                                    a.IsDeleted = day.IsDeleted;
                                    a.DayId = day.DayId;
                                    a.StartTime = (day.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (day.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = day.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.StartTime = (day.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (day.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.EndTime, token) : (DateTime?)null);
                            staffAvailability.DayId = day.DayId;
                            staffAvailability.StaffAvailabilityTypeID = day.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = day.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var avaliable in entity.Available)
                    {
                        if (avaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == avaliable.Id)
                                {
                                    a.IsDeleted = avaliable.IsDeleted;
                                    a.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = avaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.EndTime, token) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = avaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = avaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var unavaliable in entity.Unavailable)
                    {
                        if (unavaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == unavaliable.Id)
                                {
                                    a.IsDeleted = unavaliable.IsDeleted;
                                    a.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = unavaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.EndTime, token) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = unavaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = unavaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    if (dbStaffAvailabilityInsertList.Count > 0)
                        _staffAvailabilityRepository.Create(dbStaffAvailabilityInsertList.ToArray());

                    if (dbStaffAvailabilityUpdateList.Count > 0)
                        _staffAvailabilityRepository.Update(dbStaffAvailabilityUpdateList.ToArray());

                    //save changes
                    _staffAvailabilityRepository.SaveChanges();

                    //transaction commit
                    transaction.Commit();

                    //Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.SavedStaffAvailability,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    };
                }
                catch (Exception ex)
                {
                    //on error transaction rollback
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }

        public JsonModel GetStaffAvailabilty(string staffID, TokenModel token, bool isLeaveNeeded)
        {
            try
            {
                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                AvailabilityModel list = new AvailabilityModel();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                //list.StaffID = staffID;

                list.Days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                {
                  
                    Id = y.Id,
                    DayId = y.DayId,
                    DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                    StartTime = y.StartTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.StartTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.EndTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Available = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.Date), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.StartTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.EndTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Unavailable = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.Date),_locationService.GetLocationOffsets(y.LocationId,token), token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.StartTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? ConvertFromUtcTimeNew(Convert.ToDateTime(y.EndTime), _locationService.GetLocationOffsets(y.LocationId, token), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                if (isLeaveNeeded)
                    list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());

                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public DateTime ConvertFromUtcTimeNew(DateTime date,LocationModel locationModel,TokenModel token)
        {
            return CommonMethods.ConvertFromUtcTimeWithOffset(date, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, token);
        }
        public JsonModel SaveStaffAvailabiltyWithLocation(AvailabilityModel entity, TokenModel token)
        {
            var location = _locationService.GetLocationOffsets(entity.LocationId, token);
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {
                    List<Entity.StaffAvailability> dbStaffAvailabilityInsertList = new List<Entity.StaffAvailability>();
                    List<Entity.StaffAvailability> dbStaffAvailabilityUpdateList = new List<Entity.StaffAvailability>();
                    List<int> availabilityIds = new List<int>();
                    //Location location = new Location();
                    decimal daylightOffset = 0;
                    decimal standardOffset = 0;
                    string timeZone = "";
                    //Days ids
                    entity.Days.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Avaliable ids
                    entity.Available.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Unavaliable ids
                    entity.Unavailable.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //for update
                    dbStaffAvailabilityUpdateList = _staffAvailabilityRepository.GetAll(a => availabilityIds.Contains(a.Id)).ToList();  //_context.StaffAvailability.Where(a => availabilityIds.Contains(a.Id)).ToList();

                  
                    if (location != null)
                    {
                        daylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                        standardOffset = (((decimal)location.StandardTime) * 60);
                        timeZone = location.TimeZoneName;
                    }

                    foreach (var day in entity.Days)
                    {
                        if (day.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == day.Id)
                                {
                                    a.IsDeleted = day.IsDeleted;
                                    a.DayId = day.DayId;
                                    a.StartTime = (day.StartTime != null && a.StartTime != null && a.StartTime != day.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.StartTime, daylightOffset, standardOffset, timeZone) : a.StartTime);
                                    a.EndTime = (day.EndTime != null && a.EndTime != null && a.EndTime != day.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.EndTime, daylightOffset, standardOffset, timeZone) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = day.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.StartTime = (day.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.StartTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.EndTime = (day.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.EndTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.DayId = day.DayId;
                            staffAvailability.StaffAvailabilityTypeID = day.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = day.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var avaliable in entity.Available)
                    {
                        if (avaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == avaliable.Id)
                                {
                                    a.IsDeleted = avaliable.IsDeleted;
                                    a.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (avaliable.StartTime != null && a.StartTime != null && a.StartTime != avaliable.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.StartTime, daylightOffset, standardOffset, timeZone) : a.StartTime);
                                    a.EndTime = (avaliable.EndTime != null && a.EndTime != null && a.EndTime != avaliable.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.EndTime, daylightOffset, standardOffset, timeZone) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = avaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.StartTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.EndTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = avaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = avaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var unavaliable in entity.Unavailable)
                    {
                        if (unavaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == unavaliable.Id)
                                {
                                    a.IsDeleted = unavaliable.IsDeleted;
                                    a.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (unavaliable.StartTime != null && a.StartTime != null && a.StartTime != unavaliable.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.StartTime, daylightOffset, standardOffset, timeZone) : a.StartTime);
                                    a.EndTime = (unavaliable.EndTime != null && a.EndTime != null && a.EndTime != unavaliable.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.EndTime, daylightOffset, standardOffset, timeZone) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = unavaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.StartTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.EndTime, daylightOffset, standardOffset, timeZone) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = unavaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = unavaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    if (dbStaffAvailabilityInsertList.Count > 0)
                        _staffAvailabilityRepository.Create(dbStaffAvailabilityInsertList.ToArray());

                    if (dbStaffAvailabilityUpdateList.Count > 0)
                        _staffAvailabilityRepository.Update(dbStaffAvailabilityUpdateList.ToArray());

                    //save changes
                    _staffAvailabilityRepository.SaveChanges();

                    //transaction commit
                    transaction.Commit();

                    //Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.SavedStaffAvailability,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    };
                }
                catch (Exception ex)
                {
                    //on error transaction rollback
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }

        public JsonModel GetStaffAvailabilityWithLocation(string staffID, int locationId, bool isLeaveNeeded, TokenModel token)
        {
            try
            {
                AvailabilityModel list = new AvailabilityModel();
                //Location location = new Location();
                decimal daylightOffset = 0;
                decimal standardOffset = 0;
                string timeZoneName = "";
                //location = _locationRepository.GetByID(locationId);

                var locationModal = _locationService.GetLocationOffsets(locationId, token);
                if (locationModal != null)
                {
                    daylightOffset = (((decimal)locationModal.DaylightSavingTime) * 60);
                    standardOffset = (((decimal)locationModal.StandardTime) * 60);
                    timeZoneName = locationModal.TimeZoneName;
                }

                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                //list.StaffID = staffID;
                list.LocationId = locationId;
               
                list.Days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                {
                    Id = y.Id,
                    DayId = y.DayId,
                    DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Available = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Unavailable = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,//null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                if (isLeaveNeeded)
                    list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());
                
                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //public JsonModel GetStaffAvailabilityWithLocationMobile(string staffID, int locationId, bool isLeaveNeeded, TokenModel token, DateTime date)
        //{
        //    try
        //    {
        //        AvailabilityModel list = new AvailabilityModel();
        //        //Location location = new Location();
        //        decimal daylightOffset = 0;
        //        decimal standardOffset = 0;
        //        string timeZoneName = "";
        //        //location = _locationRepository.GetByID(locationId);

        //        var locationModal = _locationService.GetLocationOffsets(locationId, token);
        //        if (locationModal != null)
        //        {
        //            daylightOffset = (((decimal)locationModal.DaylightSavingTime) * 60);
        //            standardOffset = (((decimal)locationModal.StandardTime) * 60);
        //            timeZoneName = locationModal.TimeZoneName;
        //        }

        //        List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
        //        IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
        //        var dayId = weekDays.Where(z => z.Day == date.DayOfWeek.ToString()).First().Id;
        //        //  IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true).AsQueryable();
        //        IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true && (a.DayId == dayId || Convert.ToString(CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(a.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token).DayOfWeek) == Convert.ToString(date.DayOfWeek))).AsQueryable();
        //        //list.StaffID = staffID;
        //        list.LocationId = locationId;
        //        list.Available = new List<AvailabilityStatusModel>();
        //        var Availablities = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
        //        {
        //            Id = y.Id,
        //            Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //            StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //            EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //            StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
        //            StaffID = y.StaffID,
        //            IsDeleted = y.IsDeleted,
        //            LocationId = y.LocationId
        //        }).ToList();

        //        foreach (var available in Availablities)
        //        {
        //            List<AvailabilityStatusModel> AvailableSlots = new List<AvailabilityStatusModel>();
        //            DateTime StartTime = (DateTime)available.StartTime;
        //            DateTime DayEndTime = (DateTime)available.EndTime;
        //            DateTime EndTime = (DateTime)available.EndTime;
        //            while (StartTime < DayEndTime)
        //            {
        //                EndTime = StartTime.AddMinutes(30);
        //                AvailableSlots.Add(new AvailabilityStatusModel
        //                {
        //                    Id = available.Id,
        //                    Date = available.Date,
        //                    StartTime = StartTime,
        //                    EndTime = EndTime,
        //                    StaffAvailabilityTypeID = available.StaffAvailabilityTypeID,
        //                    StaffID = available.StaffID,
        //                    IsDeleted = available.IsDeleted,
        //                    LocationId = available.LocationId
        //                });
        //                StartTime = EndTime;
        //            }
        //            list.Available.AddRange(AvailableSlots);
        //        }
        //        list.Days = new List<DayModel>();
        //        if (list.Available.Count == 0)
        //        {
        //            var days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
        //            {
        //                Id = y.Id,
        //                DayId = y.DayId,
        //                DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
        //                StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //                EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //                StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
        //                StaffID = y.StaffID,
        //                IsDeleted = y.IsDeleted,
        //                LocationId = y.LocationId
        //            }).ToList();


        //            foreach (var day in days)
        //            {
        //                List<DayModel> DaySlots = new List<DayModel>();
        //                DateTime StartTime = (DateTime)day.StartTime;
        //                DateTime DayEndTime = (DateTime)day.EndTime;
        //                DateTime EndTime = (DateTime)day.EndTime;
        //                while (StartTime < DayEndTime)
        //                {
        //                    EndTime = StartTime.AddMinutes(30);
        //                    DaySlots.Add(new DayModel
        //                    {
        //                        Id = day.Id,
        //                        DayId = day.DayId,
        //                        DayName = day.DayName,
        //                        StartTime = StartTime,   //(DateTime?) StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,  //(DateTime?) StartTime,
        //                        EndTime = EndTime, //(DateTime?)EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null, //(DateTime?) EndTime,
        //                        StaffAvailabilityTypeID = day.StaffAvailabilityTypeID,
        //                        StaffID = day.StaffID,
        //                        IsDeleted = day.IsDeleted,
        //                        LocationId = day.LocationId
        //                    });
        //                    StartTime = EndTime;

        //                }
        //                list.Days.AddRange(DaySlots);
        //            }

        //        }

        //        list.Unavailable = new List<AvailabilityStatusModel>();
        //        var Unavailablities = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
        //        {
        //            Id = y.Id,
        //            Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,//null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, token) : (DateTime?)null,
        //            StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //            EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
        //            StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
        //            StaffID = y.StaffID,
        //            IsDeleted = y.IsDeleted,
        //            LocationId = y.LocationId
        //        }).ToList();


        //        foreach (var unavailable in Unavailablities)
        //        {
        //            List<AvailabilityStatusModel> UnavailableSlots = new List<AvailabilityStatusModel>();
        //            DateTime StartTime = (DateTime)unavailable.StartTime;
        //            DateTime DayEndTime = (DateTime)unavailable.EndTime;
        //            DateTime EndTime = (DateTime)unavailable.EndTime;
        //            while (StartTime < DayEndTime)
        //            {
        //                EndTime = StartTime.AddMinutes(30);
        //                UnavailableSlots.Add(new AvailabilityStatusModel
        //                {
        //                    Id = unavailable.Id,
        //                    Date = unavailable.Date,
        //                    StartTime = StartTime,
        //                    EndTime = EndTime,
        //                    StaffAvailabilityTypeID = unavailable.StaffAvailabilityTypeID,
        //                    StaffID = unavailable.StaffID,
        //                    IsDeleted = unavailable.IsDeleted,
        //                    LocationId = unavailable.LocationId
        //                });
        //                StartTime = EndTime;
        //            }
        //            list.Unavailable.AddRange(UnavailableSlots);
        //        }
        //        if (list.Available.Count > 0)
        //        {
        //            foreach (var unavailable in list.Unavailable)
        //            {
        //                if (list.Available.Exists(x => x.StartTime == unavailable.StartTime && x.EndTime == unavailable.EndTime))
        //                {
        //                    list.Available.Where(x => x.StartTime == unavailable.StartTime && x.EndTime == unavailable.EndTime).FirstOrDefault().isAvailable = "No";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (var unavailable in list.Unavailable)
        //            {
        //                if (list.Days.Exists(x => x.StartTime.Value.TimeOfDay == unavailable.StartTime.Value.TimeOfDay && x.EndTime.Value.TimeOfDay == unavailable.EndTime.Value.TimeOfDay))
        //                {
        //                    list.Days.Where(x => x.StartTime.Value.TimeOfDay == unavailable.StartTime.Value.TimeOfDay && x.EndTime.Value.TimeOfDay == unavailable.EndTime.Value.TimeOfDay).FirstOrDefault().isAvailable = "No";
        //                }
        //            }
        //        }

        //        if (isLeaveNeeded)
        //            list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());

        //        if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
        //        {
        //            response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        public JsonModel GetStaffAvailabilityWithLocationMobile(string staffID, int locationId, bool isLeaveNeeded, TokenModel token, DateTime date)
        {
            try
            {
                AvailabilityModel list = new AvailabilityModel();
                //Location location = new Location();
                decimal daylightOffset = 0;
                decimal standardOffset = 0;
                string timeZoneName = "";
                int timeinterval=30;
                int providerid = 0;
                bool decripted = int.TryParse(staffID, out providerid);
                if (!decripted)
                {
                    Int32.TryParse(Common.CommonMethods.Decrypt(staffID.Replace(" ", "+")), out providerid);
                }
                //location = _locationRepository.GetByID(locationId);

                var locationModal = _locationService.GetLocationOffsets(locationId, token);
                if (locationModal != null)
                {
                    daylightOffset = (((decimal)locationModal.DaylightSavingTime) * 60);
                    standardOffset = (((decimal)locationModal.StandardTime) * 60);
                    timeZoneName = locationModal.TimeZoneName;
                }
                Staffs staffdata = _context.Staffs.Where(x => x.Id == providerid && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (staffdata != null)
                {
                    timeinterval = staffdata.TimeInterval ?? 30;
                }

                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                var dayId = weekDays.Where(z => z.Day == date.DayOfWeek.ToString()).First().Id;
                //  IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true && (a.DayId == dayId || Convert.ToString(CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(a.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token).DayOfWeek) == Convert.ToString(date.DayOfWeek))).AsQueryable();
                //list.StaffID = staffID;
                list.LocationId = locationId;
                list.Available = new List<AvailabilityStatusModel>();
                var Availablities = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();

                foreach (var available in Availablities)
                {
                    List<AvailabilityStatusModel> AvailableSlots = new List<AvailabilityStatusModel>();
                    DateTime StartTime = (DateTime)available.StartTime;
                    DateTime DayEndTime = (DateTime)available.EndTime;
                    DateTime EndTime = (DateTime)available.EndTime;
                    while (StartTime < DayEndTime)
                    {
                        //EndTime = StartTime.AddMinutes(30);
                        EndTime = StartTime.AddMinutes(timeinterval);
                        AvailableSlots.Add(new AvailabilityStatusModel
                        {
                            Id = available.Id,
                            Date = available.Date,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            StaffAvailabilityTypeID = available.StaffAvailabilityTypeID,
                            StaffID = available.StaffID,
                            IsDeleted = available.IsDeleted,
                            LocationId = available.LocationId
                        });
                        StartTime = EndTime;
                    }
                    list.Available.AddRange(AvailableSlots);
                }
                list.Days = new List<DayModel>();
                if (list.Available.Count == 0)
                {
                    var days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                    {
                        Id = y.Id,
                        DayId = y.DayId,
                        DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                        StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                        EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                        StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                        StaffID = y.StaffID,
                        IsDeleted = y.IsDeleted,
                        LocationId = y.LocationId
                    }).ToList();


                    foreach (var day in days)
                    {
                        List<DayModel> DaySlots = new List<DayModel>();
                        DateTime StartTime = (DateTime)day.StartTime;
                        DateTime DayEndTime = (DateTime)day.EndTime;
                        DateTime EndTime = (DateTime)day.EndTime;
                        while (StartTime < DayEndTime)
                        {
                            //EndTime = StartTime.AddMinutes(30);
                            EndTime = StartTime.AddMinutes(timeinterval);
                            DaySlots.Add(new DayModel
                            {
                                Id = day.Id,
                                DayId = day.DayId,
                                DayName = day.DayName,
                                StartTime = StartTime,   //(DateTime?) StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,  //(DateTime?) StartTime,
                                EndTime = EndTime, //(DateTime?)EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null, //(DateTime?) EndTime,
                                StaffAvailabilityTypeID = day.StaffAvailabilityTypeID,
                                StaffID = day.StaffID,
                                IsDeleted = day.IsDeleted,
                                LocationId = day.LocationId
                            });
                            StartTime = EndTime;

                        }
                        list.Days.AddRange(DaySlots);
                    }

                }

                list.Unavailable = new List<AvailabilityStatusModel>();
                var Unavailablities = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,//null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, locationModal.TimeZoneName, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();


                foreach (var unavailable in Unavailablities)
                {
                    List<AvailabilityStatusModel> UnavailableSlots = new List<AvailabilityStatusModel>();
                    DateTime StartTime = (DateTime)unavailable.StartTime;
                    DateTime DayEndTime = (DateTime)unavailable.EndTime;
                    DateTime EndTime = (DateTime)unavailable.EndTime;
                    while (StartTime < DayEndTime)
                    {
                        //EndTime = StartTime.AddMinutes(30);
                        EndTime = StartTime.AddMinutes(timeinterval);
                        UnavailableSlots.Add(new AvailabilityStatusModel
                        {
                            Id = unavailable.Id,
                            Date = unavailable.Date,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            StaffAvailabilityTypeID = unavailable.StaffAvailabilityTypeID,
                            StaffID = unavailable.StaffID,
                            IsDeleted = unavailable.IsDeleted,
                            LocationId = unavailable.LocationId
                        });
                        StartTime = EndTime;
                    }
                    list.Unavailable.AddRange(UnavailableSlots);
                }
                if (list.Available.Count > 0)
                {
                    foreach (var unavailable in list.Unavailable)
                    {
                        if (list.Available.Exists(x => x.StartTime == unavailable.StartTime && x.EndTime == unavailable.EndTime))
                        {
                            list.Available.Where(x => x.StartTime == unavailable.StartTime && x.EndTime == unavailable.EndTime).FirstOrDefault().isAvailable = "No";
                        }
                    }
                }
                else
                {
                    foreach (var unavailable in list.Unavailable)
                    {
                        if (list.Days.Exists(x => x.StartTime.Value.TimeOfDay == unavailable.StartTime.Value.TimeOfDay && x.EndTime.Value.TimeOfDay == unavailable.EndTime.Value.TimeOfDay))
                        {
                            list.Days.Where(x => x.StartTime.Value.TimeOfDay == unavailable.StartTime.Value.TimeOfDay && x.EndTime.Value.TimeOfDay == unavailable.EndTime.Value.TimeOfDay).FirstOrDefault().isAvailable = "No";
                        }
                    }
                }

                if (isLeaveNeeded)
                    list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());

                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public JsonModel GetStaffAvailabilityDateAndTime(string staffID, DateTime availableDate,string startTime, string endTime,int locationId,  TokenModel token)
        {
            try
            {
                AvailabilityModel list = new AvailabilityModel();
                //Location location = new Location();
                decimal daylightOffset = 0;
                decimal standardOffset = 0;
                string timeZone = "";
               var location = _locationService.GetLocationOffsets(locationId,token);
                if (location != null)
                {
                    daylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                    standardOffset = (((decimal)location.StandardTime) * 60);
                    timeZone = location.TimeZoneName;
                }

                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                //list.StaffID = staffID;
                list.LocationId = locationId;

                list.Days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                {
                    Id = y.Id,
                    DayId = y.DayId,
                    DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Available = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Unavailable = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, timeZone, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
               

                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
