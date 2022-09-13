using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.NotificationSetting;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Service.IServices;
using HC.Service;
using System;
using static HC.Common.Enums.CommonEnum;
using HC.Common;

namespace HC.Patient.Service.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        //private readonly INotificationRepository _notificationSettingRepository;
        //private readonly ILocationRepository _locationRepository;
        //private JsonModel response;
        //private readonly IMapper _mapper;

        //public NotificationService(INotificationRepository notificationSettingRepository, ILocationRepository locationRepository, IMapper mapper)
        //{
        //    _notificationSettingRepository = notificationSettingRepository;
        //    _locationRepository = locationRepository;
        //    _mapper = mapper;
        //}
        //public JsonModel SaveNotification(NotificationModel notificationModel, TokenModel tokenModel)
        //{

        //    if (notificationModel != null)
        //    {
        //        notificationModel.OrganizationID = tokenModel.OrganizationID;

        //        var notification = _mapper.Map<Notifications>(notificationModel);
        //        notification.CreatedBy = tokenModel.UserID;

        //        _notificationSettingRepository.Create(notification);
        //        int result = _notificationSettingRepository.SaveChanges();
        //        if (result > 0)
        //        {
        //            var notificationTypeMapping = _mapper.Map<NotificationTypeMapping>(notification);
        //            notificationTypeMapping.CreatedBy = tokenModel.UserID;
        //            notificationTypeMapping.Message = notificationModel.Message;

        //            if (!notificationModel.IsMobileUser)
        //                notificationTypeMapping.NotificationTypeId = (int)NotificationType.PushNotification;
        //            else
        //                notificationTypeMapping.NotificationTypeId = (int)NotificationType.TextNotification;

        //            _notificationSettingRepository.SaveNotificationTypeMapping(notificationTypeMapping);
        //        }
        //        response = new JsonModel()
        //        {
        //            Message = StatusMessage.AddNotification,
        //            StatusCode = (int)HttpStatusCodes.OK,
        //            data = new object()
        //        };
        //    }
        //    return null;
        //}
        private readonly INotificationRepository _notificationSettingRepository;
        private readonly ILocationRepository _locationRepository;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);

        public NotificationService(INotificationRepository notificationSettingRepository, ILocationRepository locationRepository)
        {
            _notificationSettingRepository = notificationSettingRepository;
            _locationRepository = locationRepository;
        }
        public void SaveNotification(PushNotificationModel pushNotificationModel, TokenModel tokenModel)
        {
            var masternotificationActionType = _notificationSettingRepository.GetMasterNotificationActionType(pushNotificationModel.TypeId, pushNotificationModel.SubTypeId);
            if (masternotificationActionType == null)
            {
                MasterNotificationActionType masterNotificationActionTypes = new MasterNotificationActionType()
                {
                    TypeId = (int)pushNotificationModel.TypeId,
                    SubTypeId = (int)pushNotificationModel.SubTypeId,
                    OrganizationID=tokenModel.OrganizationID,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = tokenModel.UserID,
                    CreatedDate = DateTime.UtcNow,
                };
                masternotificationActionType = _notificationSettingRepository.SaveMasterNotificationActionType(masterNotificationActionTypes);
            }
            if (masternotificationActionType != null)
            {
                var type = Enum.GetName(typeof(NotificationType), pushNotificationModel.NotificationType).ToString();
                var masterNotificationType = _notificationSettingRepository.GetMasterNotificationType(type);
                if (masterNotificationType == null)
                {
                    MasterNotificationType masterNotificationTypes = new MasterNotificationType()
                    {
                        Type = type,
                        OrganizationID = tokenModel.OrganizationID,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = tokenModel.UserID,
                        CreatedDate = DateTime.UtcNow,
                    };
                    masterNotificationType = _notificationSettingRepository.SaveMasterNotificationType(masterNotificationTypes);
                }


                if (masterNotificationType != null)
                {
                    Notifications notification = null;
                    if (pushNotificationModel != null)
                    {
                        notification = new Notifications();
                        notification.StaffId = pushNotificationModel.StaffId;
                        notification.PatientId = pushNotificationModel.PatientId;
                        notification.UserId = pushNotificationModel.UserId;
                        notification.OrganizationID = tokenModel.OrganizationID;
                        notification.ActionTypeId = masternotificationActionType.ID;
                        notification.IsActive = true;
                        notification.IsDeleted = false;
                        notification.CreatedBy = tokenModel.UserID;
                        notification.CreatedDate = DateTime.UtcNow;
                        _notificationSettingRepository.Create(notification);
                        _notificationSettingRepository.SaveChanges();
                    }


                    NotificationTypeMapping notificationTypeMapping = new NotificationTypeMapping();
                    notificationTypeMapping = SaveNotificationTypeMapping(tokenModel, notification, notificationTypeMapping);
                    notificationTypeMapping.NotificationTypeId = masterNotificationType.Id;
                    notificationTypeMapping.Message = CommonMethods.Encrypt(pushNotificationModel.Message);
                    _notificationSettingRepository.SaveNotificationTypeMapping(notificationTypeMapping);
                }
            }
        }
        

        private static NotificationTypeMapping SaveNotificationTypeMapping(TokenModel tokenModel, Notifications notificationSetting, NotificationTypeMapping notificationTypeMapping)
        {
            notificationTypeMapping.NotificationId = notificationSetting.Id;
            notificationTypeMapping.OrganizationID = tokenModel.OrganizationID;
            notificationTypeMapping.IsActive = true;
            notificationTypeMapping.IsDeleted = false;
            notificationTypeMapping.IsSendNotification = false;
            notificationTypeMapping.IsReadNotification = false;
            notificationTypeMapping.IsReceivedNotification = false;
            notificationTypeMapping.CreatedBy = tokenModel.UserID;
            notificationTypeMapping.CreatedDate = DateTime.UtcNow;
            return notificationTypeMapping;
        }
  
       
    }
}
