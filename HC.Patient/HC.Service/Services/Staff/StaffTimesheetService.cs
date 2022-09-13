using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Staff;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Staff
{
    public class StaffTimesheetService : BaseService, IStaffTimesheetService
    {
        private JsonModel response = null;
        private readonly IUserTimesheetByAppointmentTypeRepository _userTimesheetByAppointmentTypeRepository;
        private readonly IUserDetailedDriveTimeRepository _userDetailedDriveTimeRepository;
        private readonly ILocationService _locationService;

        public StaffTimesheetService(IUserTimesheetByAppointmentTypeRepository userTimesheetByAppointmentTypeRepository, IUserDetailedDriveTimeRepository userDetailedDriveTimeRepository, ILocationService locationService)
        {
            response = new JsonModel();     
            _userTimesheetByAppointmentTypeRepository = userTimesheetByAppointmentTypeRepository;
            _userDetailedDriveTimeRepository = userDetailedDriveTimeRepository;
            _locationService = locationService;
        }
        public JsonModel GetStaffTimesheetDataSheetView(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token)
        {
            return new JsonModel(_userTimesheetByAppointmentTypeRepository.GetStaffTimesheetDataSheetView<StaffTimesheetModel>(staffIds, startDate, endDate, weekValue, token).ToList(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }

        public JsonModel GetStaffTimesheetDataTabularView(string staffIds, DateTime? startDate, DateTime? endDate, int weekValue, TokenModel token)
        {
            List<StaffDetailedTimesheetModel> responseList = _userDetailedDriveTimeRepository.GetStaffTimesheetDataTabularView<StaffDetailedTimesheetModel>(staffIds, startDate, endDate, weekValue, token).ToList();
            var location = _locationService.GetLocationOffsets(token.LocationID, token);
            responseList.ForEach(x =>
            {
                x.StartDateTime = ConvertFromUtcTimeNew(x.StartDateTime,location, token);
                x.EndDateTime = ConvertFromUtcTimeNew(x.EndDateTime,location, token);
            });
            return new JsonModel(responseList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }
        //, _locationService.GetLocationOffsets(token.LocationID, token)
        public DateTime ConvertFromUtcTimeNew(DateTime date, LocationModel locationModel, TokenModel token)
        {
            return CommonMethods.ConvertFromUtcTimeWithOffset(date, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, token);
        }
        //Not In Use
        public JsonModel UpdateStaffTimesheet(List<StaffTimesheetModel> timesheetModel, TokenModel token)
        {
            List<UserTimesheetByAppointmentType> requestObjList = new List<UserTimesheetByAppointmentType>();
            UserTimesheetByAppointmentType requestObj = null;
            decimal hoursValue = 0;
            if (timesheetModel != null && timesheetModel.Count > 0)
            {
                timesheetModel.ForEach(x =>
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.SunDate.Date);
                                    hoursValue = x.Sunday;
                                    break;
                                }
                            case 2:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.MonDate.Date);
                                    hoursValue = x.Monday;
                                    break;
                                }
                            case 3:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.TueDate.Date);
                                    hoursValue = x.Tuesday;
                                    break;
                                }
                            case 4:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.WedDate.Date);
                                    hoursValue = x.Wednesday;
                                    break;
                                }
                            case 5:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.ThuDate.Date);
                                    hoursValue = x.Thursday;
                                    break;
                                }
                            case 6:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.FriDate.Date);
                                    hoursValue = x.Friday;
                                    break;
                                }
                            case 7:
                                {
                                    requestObj = _userTimesheetByAppointmentTypeRepository.Get(y => y.AppointmentTypeId == x.AppointmentTypeId && y.StaffId == x.StaffId && y.DateOfService.Date == x.SatDate.Date);
                                    hoursValue = x.Saturday;
                                    break;
                                }
                        }
                        if (requestObj != null)
                        {
                            requestObj.ActualTimeDuration = hoursValue;
                            requestObj.ExpectedTimeDuration = hoursValue;
                            requestObj.UpdatedBy = token.UserID;
                            requestObj.UpdatedDate = DateTime.UtcNow;
                            requestObjList.Add(requestObj);
                        }
                    }
                });
            }
            if (requestObjList.Count > 0)
            {
                _userTimesheetByAppointmentTypeRepository.Update(requestObjList.ToArray());
                _userTimesheetByAppointmentTypeRepository.SaveChanges();
            }
            return response;
        }

        public JsonModel SaveUserTimesheetDetails(StaffDetailedTimesheetModel staffDetailedTimesheet, TokenModel token)
        {
            UserTimesheetByAppointmentType userTimesheetByAppointmentType = null;
            if (staffDetailedTimesheet.Id > 0)
            {
                userTimesheetByAppointmentType = _userTimesheetByAppointmentTypeRepository.Get(x => x.Id == staffDetailedTimesheet.Id);
                if (userTimesheetByAppointmentType != null)
                {
                    //userTimesheetByAppointmentType.TotalDuration = Convert.ToDecimal(staffDetailedTimesheet.EndDateTime.Subtract(staffDetailedTimesheet.StartDateTime).TotalMinutes / 60);
                    userTimesheetByAppointmentType.ActualTimeDuration = staffDetailedTimesheet.ActualTimeDuration;
                    userTimesheetByAppointmentType.ExpectedTimeDuration = staffDetailedTimesheet.ExpectedTimeDuration;
                    userTimesheetByAppointmentType.StartDateTime = CommonMethods.ConvertUtcTime(staffDetailedTimesheet.StartDateTime, token);
                    userTimesheetByAppointmentType.EndDateTime = CommonMethods.ConvertUtcTime(staffDetailedTimesheet.EndDateTime, token);
                    //userTimesheetByAppointmentType.StatusId = staffDetailedTimesheet.StatusId;
                    userTimesheetByAppointmentType.Notes = staffDetailedTimesheet.Notes;
                    userTimesheetByAppointmentType.UpdatedBy = token.UserID;
                    userTimesheetByAppointmentType.Distance = staffDetailedTimesheet.Distance;
                    userTimesheetByAppointmentType.UpdatedDate = DateTime.UtcNow;
                    _userTimesheetByAppointmentTypeRepository.Update(userTimesheetByAppointmentType);
                }
                response = new JsonModel(new object(), StatusMessage.TimeSheetUpdate, (int)HttpStatusCode.OK);
            }
            else
            {
                userTimesheetByAppointmentType = new UserTimesheetByAppointmentType();
                AutoMapper.Mapper.Map(staffDetailedTimesheet, userTimesheetByAppointmentType);
                userTimesheetByAppointmentType.StartDateTime = CommonMethods.ConvertUtcTime(staffDetailedTimesheet.StartDateTime, token);
                userTimesheetByAppointmentType.EndDateTime = CommonMethods.ConvertUtcTime(staffDetailedTimesheet.EndDateTime, token);
                //userTimesheetByAppointmentType.TotalDuration = Convert.ToDecimal(staffDetailedTimesheet.EndDateTime.Subtract(staffDetailedTimesheet.StartDateTime).TotalMinutes / 60);
                userTimesheetByAppointmentType.CreatedBy = token.UserID;
                userTimesheetByAppointmentType.CreatedDate = DateTime.UtcNow;
                userTimesheetByAppointmentType.OrganizationId = token.OrganizationID;
                userTimesheetByAppointmentType.LocationId = token.LocationID;
                userTimesheetByAppointmentType.IsActive = true;
                userTimesheetByAppointmentType.IsDeleted = false;
                userTimesheetByAppointmentType.StatusId = _userTimesheetByAppointmentTypeRepository.GetTimesheetStatusId("approved", token);
                _userTimesheetByAppointmentTypeRepository.Create(userTimesheetByAppointmentType);
                response = new JsonModel(new object(), StatusMessage.TimeSheetAdd, (int)HttpStatusCode.OK);
            }
            _userTimesheetByAppointmentTypeRepository.SaveChanges();

            return response;
        }

        public JsonModel DeleteUserTimesheetDetails(int Id, TokenModel token)
        {
            UserTimesheetByAppointmentType userTimesheetByAppointmentType = _userTimesheetByAppointmentTypeRepository.Get(x => x.Id == Id);
            if (userTimesheetByAppointmentType != null)
            {
                userTimesheetByAppointmentType.IsDeleted = true;
                userTimesheetByAppointmentType.DeletedBy = token.UserID;
                userTimesheetByAppointmentType.DeletedDate = DateTime.UtcNow;
                _userTimesheetByAppointmentTypeRepository.Update(userTimesheetByAppointmentType);
                _userTimesheetByAppointmentTypeRepository.SaveChanges();
            }
            response = new JsonModel(new object(), StatusMessage.TimeSheetDelete, (int)HttpStatusCode.OK);
            return response;
        }

        public JsonModel GetUserTimesheetDetails(int Id, TokenModel token)
        {
            StaffDetailedTimesheetModel staffDetailedTimesheet = new StaffDetailedTimesheetModel();
            var location = _locationService.GetLocationOffsets(token.LocationID, token);
            UserTimesheetByAppointmentType userTimesheetByAppointmentType = _userTimesheetByAppointmentTypeRepository.Get(x => x.Id == Id);
            AutoMapper.Mapper.Map(userTimesheetByAppointmentType, staffDetailedTimesheet);
            staffDetailedTimesheet.StartDateTime = ConvertFromUtcTimeNew(staffDetailedTimesheet.StartDateTime,location, token);
            staffDetailedTimesheet.EndDateTime = ConvertFromUtcTimeNew(staffDetailedTimesheet.EndDateTime,location, token);
            response = new JsonModel(staffDetailedTimesheet, StatusMessage.FetchMessage, (int)HttpStatusCode.OK, "");
            return response;
        }

        public JsonModel UpdateUserTimesheetStatus(List<StaffDetailedTimesheetModel> staffDetailedTimesheet, TokenModel token)
        {
            XElement timesheetData = new XElement("Parent");
            staffDetailedTimesheet.ForEach(x =>
            {
                timesheetData.Add(new XElement("Child",
                    new XElement("Id", x.Id),
                    new XElement("StatusId", x.StatusId)
                        ));
            });
            SQLResponseModel sqlResponse= _userDetailedDriveTimeRepository.UpdateUserTimesheetStatus<SQLResponseModel>(timesheetData.ToString(), token).FirstOrDefault();
            return new JsonModel(new object(), sqlResponse.Message, sqlResponse.StatusCode, string.Empty);
        }

        public JsonModel SubmitUserTimesheet(string Ids, TokenModel token)
        {
            List<int> timesheetIds = Ids.Split(',').Select(Int32.Parse).ToList();
            List<UserTimesheetByAppointmentType> timesheetList = _userTimesheetByAppointmentTypeRepository.GetAll(x => timesheetIds.Contains(x.Id)).ToList();
            if (timesheetList != null && timesheetList.Count > 0)
            {
                timesheetList.ForEach(x => {
                    x.StatusId = _userTimesheetByAppointmentTypeRepository.GetTimesheetStatusId("submitted", token);
                    x.UpdatedBy = token.UserID;
                    x.UpdatedDate = DateTime.UtcNow;
                });
                _userTimesheetByAppointmentTypeRepository.Update(timesheetList.ToArray());
                _userTimesheetByAppointmentTypeRepository.SaveChanges();
            }
            return new JsonModel(new object(), StatusMessage.TimeSheetSubmitted, (int)HttpStatusCodes.OK, string.Empty);
        }
    }
}
