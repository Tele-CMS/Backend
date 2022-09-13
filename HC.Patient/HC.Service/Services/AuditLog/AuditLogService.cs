using HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Service.IServices.AuditLog;
using System;
using System.Collections.Generic;
using HC.Model;
using HC.Patient.Model.AuditLog;
using System.Linq;
using static HC.Common.Enums.CommonEnum;
using HC.Common.HC.Common;
using HC.Service;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Model.MasterData;

namespace HC.Patient.Service.Services.AuditLog
{
    public class AuditLogService : BaseService, IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILocationService _locationService;

        public AuditLogService(IAuditLogRepository auditLogRepository, ILocationService locationService)
        {
            _auditLogRepository = auditLogRepository;
            _locationService = locationService;
        }
        public void AccessLogs(string screenName, string action, int? patientId, int? userId, TokenModel tokenModel, string LoginAttempt)
        {
            AuditLogs auditLog = new AuditLogs();
            auditLog.ScreenName = screenName;
            auditLog.Action = action;
            auditLog.CreatedById = userId;
            auditLog.CreatedDate = DateTime.UtcNow;
            auditLog.EncryptionCode = CommonMethods.GetHashValue(
                          screenName.Trim() +
                          action.Trim() +
                          (auditLog.OldValue != null ? auditLog.OldValue : string.Empty) +
                          (auditLog.NewValue != null ? auditLog.NewValue : string.Empty) +
                          (auditLog.AuditLogColumnId != null ? Convert.ToString(auditLog.AuditLogColumnId) : string.Empty) +
                          (auditLog.CreatedById != null ? Convert.ToString(auditLog.CreatedById) : string.Empty) +
                          Convert.ToString(auditLog.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt")) +
                          (patientId != null ? Convert.ToString(patientId) : string.Empty) +
                          (tokenModel.IPAddress != null ? tokenModel.IPAddress.Trim() : string.Empty) +
                          Convert.ToString(tokenModel.OrganizationID) +
                          Convert.ToString(tokenModel.LocationID)
                          );
            auditLog.PatientId = patientId;
            auditLog.IPAddress = !string.IsNullOrEmpty(tokenModel.IPAddress) ? tokenModel.IPAddress : null;
            //auditLog.LocationID = tokenModel.LocationID > 0 ? tokenModel.LocationID : 1;
            auditLog.LocationID = tokenModel.LocationID > 0 ? tokenModel.LocationID : 101;
            auditLog.OrganizationID = tokenModel.OrganizationID;
            auditLog.LoginAttempt = LoginAttempt;
            _auditLogRepository.Create(auditLog);
            _auditLogRepository.SaveChanges();
        }

        public JsonModel GetAuditLogList(string searchText, string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            List<AuditLogModel> auditLogList = _auditLogRepository.GetAuditLogList<AuditLogModel>(searchText,createdBy, patientName, action, fromDate, toDate, token.OrganizationID, token.LocationID, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (auditLogList != null && auditLogList.Count > 0)
            {
                auditLogList.ForEach(x => { x.LogDate = ConvertFromUtcTimeNew(x.LogDate,_locationService.GetLocationOffsets(token.LocationID,token), token); });
                return new JsonModel()
                {
                    data = auditLogList,
                    meta = new Meta()
                    {
                        TotalRecords = auditLogList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(auditLogList[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }
        public DateTime ConvertFromUtcTimeNew(DateTime date, LocationModel locationModel, TokenModel token)
        {
            return CommonMethods.ConvertFromUtcTimeWithOffset(date, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, token);
        }
        public JsonModel GetLoginLogList(string searchText,string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token, int roleID)
        {
            List<AuditLogModel> auditLogList = _auditLogRepository.GetLoginLogList<AuditLogModel>(searchText, createdBy, patientName, action, fromDate, toDate, token.OrganizationID, token.LocationID, pageNumber, pageSize, sortColumn, sortOrder, roleID).ToList();
            if (auditLogList != null && auditLogList.Count > 0)
            {
                auditLogList.ForEach(x => { x.LogDate = ConvertFromUtcTimeNew(x.LogDate, _locationService.GetLocationOffsets(token.LocationID, token), token); });

                return new JsonModel()
                {
                    data = auditLogList,
                    meta = new Meta()
                    {
                        TotalRecords = auditLogList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(auditLogList[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public JsonModel DeleteAuditLog(int id, TokenModel token)
        {
                try
                {
                AuditLogs auditLogs = _auditLogRepository.Get(a => a.Id == id);
                if(auditLogs != null)
                {
                    auditLogs.IsActive = false;
                    auditLogs.IsDeleted = true;
                    _auditLogRepository.Update(auditLogs);
                    _auditLogRepository.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DeleteAuditLog,
                        StatusCode = (int)HttpStatusCodes.OK
                    };

                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
                    

                }
                catch
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ServerError,
                        StatusCode = (int)HttpStatusCodes.InternalServerError
                    };
                }
        }

    }
}
