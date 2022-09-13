using HC.Common;
using HC.Common.Enums;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Notification
{
    public class NotificationRepository : RepositoryBase<Notifications>, INotificationRepository
    {
        private HCOrganizationContext _context;
        public NotificationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetAllUnReadALerts<T>(TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@UserId",tokenModel.UserID),
                new SqlParameter("@OrganizationId ", tokenModel.OrganizationID )
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.NS_GetAllUnReadNotification.ToString(), parameters.Length, parameters).AsQueryable();

        }

        public void SaveNotificationTypeMapping(NotificationTypeMapping notificationTypeMapping)
        {
            _context.NotificationTypeMappings.Add(notificationTypeMapping);
            _context.SaveChanges();
        }

        public void SaveNotificationTypeMapping(List<NotificationTypeMapping> notificationTypeMapping)
        {
            _context.UpdateRange(notificationTypeMapping);
            _context.SaveChanges();
        }

        public IQueryable<T> GetPatientNotificationList<T>(int PatientID, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PatientId", PatientID),
                new SqlParameter("@PageNumber",pageNumber),
                new SqlParameter("@PageSize ", pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetNotificationList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetPatientPortalNotificationList<T>(int PatientID, int pageNumber, int pageSize, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PatientId", PatientID),
                new SqlParameter("@PageNumber",pageNumber),
                new SqlParameter("@PageSize ", pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetNotificationList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public List<NotificationTypeMapping> ReadChatAndAllNotification(int patientId, bool isChatNotification, TokenModel tokenModel)
        {
            List<NotificationTypeMapping> notificationTypeMapping = new List<NotificationTypeMapping>();

            if (isChatNotification)
            {
                notificationTypeMapping = _context.Notifications
                        .Join(_context.NotificationTypeMappings, n => n.Id, nt => nt.NotificationId, (n, nt) => new { n, nt })
                        .Join(_context.MasterNotificationTypes, ntt => ntt.nt.NotificationTypeId, mnt => mnt.Id, (ntt, mnt) => new { ntt, mnt })
                        .Join(_context.MasterNotificationActionTypes, nttt => nttt.ntt.n.ActionTypeId, mnat => mnat.ID, (nttt, mnat) => new { nttt, mnat })
                        .Where(m => m.nttt.ntt.n.PatientId == patientId && m.nttt.ntt.nt.IsReadNotification == false && m.nttt.mnt.Type == "PushNotification")
                .Select(an => new NotificationTypeMapping
                {
                    Id = an.nttt.ntt.nt.Id,
                    IsReadNotification = an.nttt.ntt.nt.IsReadNotification,
                    IsReceivedNotification = an.nttt.ntt.nt.IsReceivedNotification,
                    IsSendNotification = an.nttt.ntt.nt.IsSendNotification,
                    Message = an.nttt.ntt.nt.Message,
                    NotificationId = an.nttt.ntt.nt.NotificationId,
                    NotificationTypeId = an.nttt.ntt.nt.NotificationTypeId,
                    Subject = an.nttt.ntt.nt.Subject,
                    CreatedBy = an.nttt.ntt.nt.CreatedBy,
                    CreatedDate = an.nttt.ntt.nt.CreatedDate,
                    DeletedBy = an.nttt.ntt.nt.DeletedBy,
                    DeletedDate = an.nttt.ntt.nt.DeletedDate,
                    IsActive = an.nttt.ntt.nt.IsActive,
                    IsDeleted = an.nttt.ntt.nt.IsDeleted,
                    UpdatedBy = an.nttt.ntt.nt.UpdatedBy,
                    UpdatedDate = an.nttt.ntt.nt.UpdatedDate,
                }).ToList();
            }
            else
            {
                notificationTypeMapping = _context.Notifications
                        .Join(_context.NotificationTypeMappings, n => n.Id, nt => nt.NotificationId, (n, nt) => new { n, nt })
                        .Join(_context.MasterNotificationTypes, ntt => ntt.nt.NotificationTypeId, mnt => mnt.Id, (ntt, mnt) => new { ntt, mnt })
                        .Join(_context.MasterNotificationActionTypes, nttt => nttt.ntt.n.ActionTypeId, mnat => mnat.ID, (nttt, mnat) => new { nttt, mnat })
                        .Where(m => m.nttt.ntt.n.PatientId == patientId && m.nttt.ntt.nt.IsReadNotification == false && m.nttt.mnt.Type == "PushNotification")
                        .Select(an => new NotificationTypeMapping
                        {
                            Id = an.nttt.ntt.nt.Id,
                            IsReadNotification = an.nttt.ntt.nt.IsReadNotification,
                            IsReceivedNotification = an.nttt.ntt.nt.IsReceivedNotification,
                            IsSendNotification = an.nttt.ntt.nt.IsSendNotification,
                            Message = an.nttt.ntt.nt.Message,
                            NotificationId = an.nttt.ntt.nt.NotificationId,
                            NotificationTypeId = an.nttt.ntt.nt.NotificationTypeId,
                            Subject = an.nttt.ntt.nt.Subject,
                            CreatedBy = an.nttt.ntt.nt.CreatedBy,
                            CreatedDate = an.nttt.ntt.nt.CreatedDate,
                            DeletedBy = an.nttt.ntt.nt.DeletedBy,
                            DeletedDate = an.nttt.ntt.nt.DeletedDate,
                            IsActive = an.nttt.ntt.nt.IsActive,
                            IsDeleted = an.nttt.ntt.nt.IsDeleted,
                            UpdatedBy = an.nttt.ntt.nt.UpdatedBy,
                            UpdatedDate = an.nttt.ntt.nt.UpdatedDate,
                        }).ToList();
            }
            return notificationTypeMapping;
        }

        public IQueryable<T> ReadChatAndAllNotificationForPateint<T>(int PatientID, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PatientID",PatientID),
                new SqlParameter("@OrganizationId ", tokenModel.OrganizationID )
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetAllUnReadNotificationForPatient.ToString(), parameters.Length, parameters).AsQueryable();

        }
        public MasterNotificationActionType SaveMasterNotificationActionType(MasterNotificationActionType masterNotificationActionType)
        {
            _context.MasterNotificationActionTypes.Add(masterNotificationActionType);
            _context.SaveChanges();
            return _context.MasterNotificationActionTypes.Where(x => x.ID == masterNotificationActionType.ID && x.IsDeleted == false).FirstOrDefault();
        }
        public MasterNotificationActionType GetMasterNotificationActionType(NotificationActionType notificationActionType, NotificationActionSubType notificationActionSubType)
        {
            return _context.MasterNotificationActionTypes.Where(x => x.TypeId == (int)notificationActionType && x.SubTypeId == (int)notificationActionSubType && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }
        public MasterNotificationType SaveMasterNotificationType(MasterNotificationType masterNotificationType)
        {
            _context.MasterNotificationTypes.Add(masterNotificationType);
            _context.SaveChanges();
            return _context.MasterNotificationTypes.Where(x => x.Id == masterNotificationType.Id && x.IsDeleted == false).FirstOrDefault();
        }
        public MasterNotificationType GetMasterNotificationType(string type)
        {
            return _context.MasterNotificationTypes.Where(x => x.Type == type && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
        }

        public string GetNotificationMessage(string fullName, int? staffId, int? appointmentId, string notificationType, LocationModel locationModel = null)
        {
            string FullName = string.Empty;
            PatientAppointment patientAppointment = new PatientAppointment();
            string AppointmentDate = string.Empty;

            if (fullName != string.Empty && fullName != null)
            {
                FullName = fullName;
                //Patients patients = new Patients();
                //patients = _context.Patients.Where(x => x.Id == patientId).FirstOrDefault();
                //FullName = Convert.ToString(CommonMethods.Decrypt(Convert.ToBase64String(patients.FirstName.ToArray())) + " " + CommonMethods.Decrypt(Convert.ToBase64String(patients.LastName.ToArray())));
            }
            if (staffId > 0 && staffId != null)
            {
                Staffs staffs = new Staffs();
                staffs = _context.Staffs.Where(x => x.Id == staffId).FirstOrDefault();
                FullName = staffs.FirstName + " " + staffs.LastName;
            }
            if (appointmentId > 0 && appointmentId != null)
            {
                patientAppointment = _context.PatientAppointment.Where(x => x.Id == appointmentId).FirstOrDefault();
                //if (locationModel == null)
                //{
                //    AppointmentDate = patientAppointment.StartDateTime.ToString("dddd, dd MMMM yyyy") + " " + "between" + " " + patientAppointment.StartDateTime.ToString("h:mm tt") + " " + "-" + " " + patientAppointment.EndDateTime.ToString("h:mm tt") + ".";
                //}
                //else
                //{
                //    patientAppointment.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointment.StartDateTime, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName);
                //    patientAppointment.EndDateTime  = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointment.EndDateTime, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName);
                //    AppointmentDate = patientAppointment.StartDateTime.ToString("dddd, dd MMMM yyyy") + " " + "between" + " " + patientAppointment.StartDateTime.ToString("h:mm tt") + " " + "-" + " " + patientAppointment.EndDateTime.ToString("h:mm tt") + ".";
                //}
                AppointmentDate = patientAppointment.StartDateTime.ToString("dddd, dd MMMM yyyy") + " " + "between" + " " + patientAppointment.StartDateTime.ToString("h:mm tt") + " " + "-" + " " + patientAppointment.EndDateTime.ToString("h:mm tt") + ".";
            }

            string message = string.Empty;
            switch (notificationType.ToLower())
            {
                case "requested":
                    message = NotificationMessage.AppointmentRequested + " " + FullName + " " + "on" + " " + AppointmentDate;
                    break;
                case "approved":
                    message = NotificationMessage.AppointmentAccepted + " " + FullName + " " + "on" + " " + AppointmentDate;
                    break;
                case "cancelled":
                    message = NotificationMessage.AppointmentCancelled + " " + FullName + " " + "on" + " " + AppointmentDate;
                    break;
                case "rescheduled":
                    message = NotificationMessage.AppointmentRescheduled + " " + FullName + " " + "on" + " " + AppointmentDate;
                    break;
                case "rejected":
                    message = NotificationMessage.AppointmentRejected + " " + FullName + " " + "due to" + " " + patientAppointment.CancelReason + " " + "on" + " " + AppointmentDate;
                    break;
                case "sessioninvitation":
                    message = FullName + " " + NotificationMessage.GroupSessionInvitationNotification;
                    break;
            }
            return message;

        }
        public NotificationModel MarkReadNotification(TokenModel token)
        {
            SqlParameter[] parameters = {new SqlParameter("@UserID", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutputForNotificationInfo(SQLObjects.ADM_GetNotificationDashboard.ToString(), parameters.Length, parameters);
        }

        public string GetNotificationMessage(object p, object staffId, object id, string v)
        {
            throw new NotImplementedException();
        }

        public string GetNotificationMessage(object p, int? staffId, object id, string v)
        {
            throw new NotImplementedException();
        }
    }
}
