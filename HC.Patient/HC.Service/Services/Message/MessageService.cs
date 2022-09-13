using HC.Common;
using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Message;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Message;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Message;
using HC.Patient.Service.Services.Notification;
using HC.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Message
{
    public class MessageService : BaseService, IMessageService
    {
        private readonly HCOrganizationContext _context;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageRecepientsRepository _messageRecepientsRepository;
        private readonly IMessageDocumentRepository _messageDocumentRepository;
        private readonly ILocationService _locationService;
        private readonly IStaffRepository _staffRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        public MessageService(IUserRepository userRepository, IPatientRepository patientRepository, IStaffRepository staffRepository, IMessageRepository messageRepository, IMessageRecepientsRepository messageRecepientsRepository, IMessageDocumentRepository messageDocumentRepository, HCOrganizationContext context, ILocationService locationService)
        {
            _context = context;
            _messageRecepientsRepository = messageRecepientsRepository;
            _messageRepository = messageRepository;
            _messageDocumentRepository = messageDocumentRepository;
            _locationService = locationService;
            _staffRepository = staffRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }
        public JsonModel Compose(MessageComposeModel messageComposeModel, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    Entity.Message message = new Entity.Message()
                    {
                        CreatedBy = token.UserID,
                        CreatedDate = CurrentDate,
                        IsFavourite = messageComposeModel.IsFavourite,
                        OrganizationId = token.OrganizationID,
                        Subject = CommonMethods.Encrypt(messageComposeModel.Subject), //encrypt subject
                        Text = CommonMethods.Encrypt(messageComposeModel.Text), //encrypt message text
                        MessageDate = CurrentDate,
                        FromUserID = messageComposeModel.FromUserId,
                        ParentMessageId = messageComposeModel.ParentMessageId
                    };
                    message.MessageRecepients = new List<MessageRecepient>();
                    foreach (int to in messageComposeModel.ToUserIds)
                    {
                        message.MessageRecepients.Add(new MessageRecepient
                        {
                            ToUserID = to,
                            MessageDate = CurrentDate,
                            CreatedBy = token.UserID,
                            CreatedDate = CurrentDate
                        });
                    }

                    //save documents
                    if (messageComposeModel.Base64 != null)
                    {
                        messageComposeModel.Id = message.MessageID;
                        List<MessageDocuments> messageDocs = MessageAttachedDocuments(messageComposeModel, token);
                        message.MessageDocuments = messageDocs;
                    }

                    //save message recepients
                    message = _messageRepository.AddMessage(message);

                    //transaction commit
                    transaction.Commit();

                    InboxDataModel inboxDataModel = new InboxDataModel()
                    {
                        MessageId = message.MessageID,
                        Subject = messageComposeModel.Subject,
                        Text = messageComposeModel.Subject,
                        MessageDate = CurrentDate,
                        Unread = true,
                        IsFavourite = message.IsFavourite,
                        AttachedDocs = Convert.ToString(message.MessageDocuments == null ? 0 : message.MessageDocuments.Count()),
                        AttachedDocument = null,
                        ParentMessageId = message.ParentMessageId,
                        FromInbox = true,
                    };
                    this.SendPushNotificationForMailbox(inboxDataModel, messageComposeModel.FromUserId, message.MessageRecepients, token);

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.MessageSent,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = e.Message,
                        StatusCode = (int)HttpStatusCodes.InternalServerError,//(Not Found)
                        AppError = e.Message
                    };
                }
            }
        }
        public void SendPushNotificationForMailbox(InboxDataModel inboxData, int fromUserId, ICollection<MessageRecepient> toUsers, TokenModel token)
        {
            string image = string.Empty;

            Staffs staffs = _staffRepository.GetAll(x => x.UserID == fromUserId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            if (staffs != null && staffs.FirstName != null)
            {
                //image = string.IsNullOrEmpty(staffs.PhotoThumbnailPath) ? staffs.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffs.PhotoThumbnailPath);
                inboxData.FromName = staffs.FirstName + " " + staffs.LastName;
                inboxData.FromEmail = staffs.Email;
                inboxData.IsStaff = true;
            }
            else
            {
                Patients patients = _patientRepository.GetAll(x => x.UserID == fromUserId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                //image = string.IsNullOrEmpty(patients.PhotoThumbnailPath) ? patients.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patients.PhotoThumbnailPath);
                PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patients.FirstName, patients.MiddleName, patients.LastName, patients.DOB, patients.Email, patients.SSN, patients.MRN, null, null, null, null, null, null, null).FirstOrDefault();
                inboxData.FromName = pHIDecryptedModel.FirstName + " " + pHIDecryptedModel.LastName;
                inboxData.FromEmail = pHIDecryptedModel.EmailAddress;
                inboxData.IsStaff = false;
            }

            inboxData.StatusName = "Mail";
            //inboxData.Thumbnail = image;

            foreach (var user in toUsers)
            {
                int toUserID = user.ToUserID;
                Staffs staff = _staffRepository.GetAll(x => x.UserID == toUserID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (staff != null && staff.FirstName != null)
                {
                    image = string.IsNullOrEmpty(staff.PhotoThumbnailPath) ? staff.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staff.PhotoThumbnailPath);
                    inboxData.ToName = staff.FirstName + " " + staff.LastName;
                }
                else
                {
                    Patients patient = _patientRepository.GetAll(x => x.UserID == toUserID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    image = string.IsNullOrEmpty(patient.PhotoThumbnailPath) ? patient.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patient.PhotoThumbnailPath);
                    PHIDecryptedModel pHIDecryptedModel = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patient.FirstName, patient.MiddleName, patient.LastName, patient.DOB, patient.Email, patient.SSN, patient.MRN, null, null, null, null, null, null, null).FirstOrDefault();
                    inboxData.ToName = pHIDecryptedModel.FirstName + " " + pHIDecryptedModel.LastName;
                }

                inboxData.Thumbnail = image;

                string deviceToken = _userRepository.GetByID(toUserID).DeviceToken;

                PushMobileNotificationModel pushMobileNotification = new PushMobileNotificationModel();
                pushMobileNotification.DeviceToken = deviceToken;
                pushMobileNotification.Message = "Your mailbox has received a new message.";
                pushMobileNotification.NotificationPriority = PushNotificationPriority.High;
                pushMobileNotification.NotificationType = CommonEnum.NotificationType.EmailNotification.ToString();
                pushMobileNotification.Data = inboxData;
                PushNotificationsService.SendPushNotificationForMobile(pushMobileNotification);
            }

        }

        //, _locationService.GetLocationOffsets(token.LocationID, token)
        private DateTime ConvertFromUtcTimeNew(DateTime date, LocationModel locationModel, TokenModel token)
        {
            return CommonMethods.ConvertFromUtcTimeWithOffset(date, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, token);
        }
        public JsonModel GetAllInboxData(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<InboxDataModel> inboxDataModels = _messageRepository.GetInboxData<InboxDataModel>(forStaff, token, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (inboxDataModels != null && inboxDataModels.Count > 0)
            {
                //Decrypt subject of message
                inboxDataModels.ForEach(a =>
                {
                    a.Subject = CommonMethods.Decrypt(a.Subject);
                    a.Thumbnail = !string.IsNullOrEmpty(a.Thumbnail) ? a.IsStaff == true ? CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.Thumbnail) : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.Thumbnail) : "";
                    //a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, _locationService.GetLocationOffsets(token.LocationID, token), token);
                    a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, _locationService.GetLocationOffsets(101, token), token);
                    a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').ToList() : null;
                });
                return new JsonModel()
                {
                    data = inboxDataModels,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = inboxDataModels[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(inboxDataModels[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public JsonModel GetSentMessageData(bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            try
            {
                List<SentDataModel> sentDataModels = _messageRepository.GetSentMessageData<SentDataModel>(forStaff, token, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
                if (sentDataModels != null && sentDataModels.Count > 0)
                {
                    //Decrypt subject of message
                    sentDataModels.ForEach(a =>
                    {
                        a.Subject = CommonMethods.Decrypt(a.Subject);
                        a.Thumbnail = !string.IsNullOrEmpty(a.Thumbnail) ? a.IsStaff == true ? CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.Thumbnail) : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.Thumbnail) : "";
                        //a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, _locationService.GetLocationOffsets(token.LocationID, token), token);
                        a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, _locationService.GetLocationOffsets(101, token), token);
                        a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').ToList() : null;
                    });
                    return new JsonModel()
                    {
                        data = sentDataModels,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = sentDataModels[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(sentDataModels[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                    };
                }
                else
                {
                    return new JsonModel() { data = new object(), Message = StatusMessage.NotFound, StatusCode = (int)HttpStatusCodes.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel DeleteInboxMessage(MessageDeleteModel messageDeleteModel, TokenModel token)
        {
            try
            {
                List<MessageRecepient> messageRecepientList = _messageRecepientsRepository.GetAll(a => messageDeleteModel.Id.ToList().Contains(a.MessageId) && a.ToUserID == token.UserID && a.IsDeleted == false).ToList();
                if (messageRecepientList.Count > 0 && messageRecepientList != null)
                {
                    messageRecepientList.ForEach(a => { a.DeletedBy = token.UserID; a.DeletedDate = DateTime.UtcNow; a.IsDeleted = true; });
                    _messageRecepientsRepository.Update(messageRecepientList.ToArray());
                    _messageRecepientsRepository.SaveChanges();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DeleteMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
        public JsonModel DeleteSentMessage(MessageDeleteModel messageDeleteModel, TokenModel token)
        {
            try
            {
                List<Entity.Message> messageList = _messageRepository.GetAll(a => messageDeleteModel.Id.Contains(a.MessageID) && a.FromUserID == token.UserID && a.IsDeleted == false).ToList();
                if (messageList != null && messageList.Count > 0)
                {
                    messageList.ForEach(a => { a.DeletedBy = token.UserID; a.DeletedDate = DateTime.UtcNow; a.IsDeleted = true; });
                    _messageRepository.Update(messageList.ToArray());
                    _messageRepository.SaveChanges();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DeleteMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
        public JsonModel ChangeMessageStatus(int MessageId, bool Unread, TokenModel token)
        {
            try
            {
                MessageRecepient messageRecepient = _messageRecepientsRepository.Get(a => a.MessageId == MessageId && a.IsDeleted == false);
                if (messageRecepient != null)
                {
                    messageRecepient.Unread = Unread;
                    messageRecepient.UpdatedBy = token.UserID;
                    messageRecepient.UpdatedDate = DateTime.UtcNow;
                    _messageRecepientsRepository.Update(messageRecepient);
                    _messageRecepientsRepository.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.MessageStatus,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NotFound,
                    AppError = e.Message
                };
            }
        }
        public JsonModel ChangeFavouriteMessageStatus(int MessageId, bool FromInbox, bool IsFavourite, TokenModel token)
        {
            try
            {
                if (FromInbox)
                {
                    MessageRecepient messageRecepient = _messageRecepientsRepository.Get(a => a.MessageId == MessageId && a.ToUserID == token.UserID && a.IsDeleted == false);
                    if (messageRecepient != null)
                    {
                        messageRecepient.IsFavourite = IsFavourite;
                        messageRecepient.UpdatedBy = token.UserID;
                        messageRecepient.UpdatedDate = DateTime.UtcNow;
                        _messageRecepientsRepository.Update(messageRecepient);
                        _messageRecepientsRepository.SaveChanges();

                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.MessageFavouriteStatus,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
                else
                {
                    Entity.Message message = _messageRepository.Get(a => a.MessageID == MessageId && a.FromUserID == token.UserID && a.IsDeleted == false);
                    if (message != null)
                    {
                        message.IsFavourite = IsFavourite;
                        message.UpdatedBy = token.UserID;
                        message.UpdatedDate = DateTime.UtcNow;
                        _messageRepository.Update(message);
                        _messageRepository.SaveChanges();

                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.MessageFavouriteStatus,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NotFound,
                    AppError = e.Message
                };
            }
        }
        public JsonModel GetMessageById(int MessageId, TokenModel token)
        {
            try
            {
                MessageDetailModel messageDetailModel = _messageRepository.GetMessageDetail<MessageDetailModel>(MessageId, token).FirstOrDefault();

                if (messageDetailModel != null)
                {
                    messageDetailModel.Subject = CommonMethods.Decrypt(messageDetailModel.Subject);
                    messageDetailModel.Text = CommonMethods.Decrypt(messageDetailModel.Text);
                    messageDetailModel.Thumbnail = !string.IsNullOrEmpty(messageDetailModel.Thumbnail) ? messageDetailModel.IsStaff == true ? CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, messageDetailModel.Thumbnail) : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, messageDetailModel.Thumbnail) : "";
                    //messageDetailModel.MessageDate = ConvertFromUtcTimeNew(messageDetailModel.MessageDate, _locationService.GetLocationOffsets(token.LocationID, token), token);
                    messageDetailModel.MessageDate = ConvertFromUtcTimeNew(messageDetailModel.MessageDate, _locationService.GetLocationOffsets(101, token), token);
                    messageDetailModel.AttachedDocument = !string.IsNullOrEmpty(messageDetailModel.AttachedDocs) ? messageDetailModel.AttachedDocs.Split(',').Select(x => CommonMethods.CreateImageUrl(token.Request, ImagesPath.MessageDocuments, x.Trim())).ToList() : null;
                    //change status of message
                    MessageRecepient messageRecepient = _messageRecepientsRepository.Get(a => a.MessageId == MessageId && a.Unread == true && a.IsDeleted == false);
                    if (messageRecepient != null)
                    {
                        messageRecepient.Unread = false;
                        messageRecepient.UpdatedBy = token.UserID;
                        messageRecepient.UpdatedDate = DateTime.UtcNow;
                        _messageRecepientsRepository.Update(messageRecepient);
                        _messageRecepientsRepository.SaveChanges();
                        //////////
                    }
                    return new JsonModel()
                    {
                        data = messageDetailModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };

            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
        public JsonModel ForwardMessages(int MessageId, TokenModel token)
        {
            MessageDetailModel messageDetailModel = _messageRepository.GetMessageDetail<MessageDetailModel>(MessageId, token).FirstOrDefault();
            if (messageDetailModel != null)
            {
                ForwardMessageDetailModel forwardMessageDetailModel = new ForwardMessageDetailModel();
                messageDetailModel.Subject = "Fwd: " + CommonMethods.Decrypt(messageDetailModel.Subject);
                messageDetailModel.Text = CommonMethods.Decrypt(messageDetailModel.Text);
                AutoMapper.Mapper.Map(messageDetailModel, forwardMessageDetailModel);
                forwardMessageDetailModel.ParentMessageId = null;//forward should always new message
                forwardMessageDetailModel.ForwardReply = "Fwd: \n" + "From: " + forwardMessageDetailModel.FromName + " \n" + "Date:" + ConvertFromUtcTimeNew(messageDetailModel.MessageDate, _locationService.GetLocationOffsets(token.LocationID, token), token) + " \n\n" + "Message \n";
                return new JsonModel()
                {
                    data = forwardMessageDetailModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public JsonModel ReplyMessages(int MessageId, TokenModel token)
        {
            MessageDetailModel messageDetailModel = _messageRepository.GetMessageDetail<MessageDetailModel>(MessageId, token).FirstOrDefault();
            if (messageDetailModel != null)
            {
                ForwardMessageDetailModel forwardMessageDetailModel = new ForwardMessageDetailModel();
                messageDetailModel.Subject = "Re: " + CommonMethods.Decrypt(messageDetailModel.Subject);
                messageDetailModel.Text = CommonMethods.Decrypt(messageDetailModel.Text);
                AutoMapper.Mapper.Map(messageDetailModel, forwardMessageDetailModel);
                forwardMessageDetailModel.ForwardReply = "On " + forwardMessageDetailModel.MessageDate + "  " + ConvertFromUtcTimeNew(messageDetailModel.MessageDate, _locationService.GetLocationOffsets(token.LocationID, token), token) + " wrote : \n ";
                return new JsonModel()
                {
                    data = forwardMessageDetailModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public JsonModel GetFavouriteMessageList(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<FavouriteMessageModel> favouriteMessageModelList = _messageRepository.GetFavouriteMessageList<FavouriteMessageModel>(token, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (favouriteMessageModelList.Count > 0)
            {
                var location = _locationService.GetLocationOffsets(token.LocationID, token);
                favouriteMessageModelList.ForEach(a =>
                {
                    a.Subject = CommonMethods.Decrypt(a.Subject); a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, location, token);
                    a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').ToList() : null;
                });
                return new JsonModel()
                {
                    data = favouriteMessageModelList,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = favouriteMessageModelList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(favouriteMessageModelList[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public JsonModel GetDeleteMessageList(TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<DeletedMessageModel> deleteMessageModelList = _messageRepository.GetDeletedMessageList<DeletedMessageModel>(token, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (deleteMessageModelList.Count > 0)
            {
                var location = _locationService.GetLocationOffsets(token.LocationID, token);
                deleteMessageModelList.ForEach(a =>
                {
                    a.Subject = CommonMethods.Decrypt(a.Subject); a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, location, token);
                    a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').ToList() : null;
                });
                return new JsonModel()
                {
                    data = deleteMessageModelList,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = deleteMessageModelList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(deleteMessageModelList[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public DownloadMessageDocumentModel GetMessageDocument(int messageId, string name, TokenModel token)
        {
            try
            {
                //get document
                MessageDocuments messageDoc = _messageDocumentRepository.Get(a => a.MessageID == messageId && a.Name == name);

                //Save folder Directory
                string DirectoryUrl = ImagesPath.MessageDocuments;

                if (File.Exists(Directory.GetCurrentDirectory() + DirectoryUrl + messageDoc.Name))
                {
                    DownloadMessageDocumentModel downloadDocumentModel = new DownloadMessageDocumentModel();
                    string base64string = Directory.GetCurrentDirectory() + DirectoryUrl + messageDoc.Name;

                    Byte[] bytes = File.ReadAllBytes(base64string);
                    MemoryStream memoryFile = new MemoryStream(bytes);
                    String file = Convert.ToBase64String(bytes);

                    downloadDocumentModel.File = memoryFile;
                    downloadDocumentModel.MessageId = messageDoc.MessageID;
                    downloadDocumentModel.Name = messageDoc.Name;
                    downloadDocumentModel.Extenstion = Path.GetExtension(messageDoc.Name);
                    downloadDocumentModel.ApplicationType = CommonMethods.GetMimeType(downloadDocumentModel.Extenstion);
                    return downloadDocumentModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                string appError = e.Message;
                return null;
            }
        }
        public JsonModel UsersDropDown(bool isStaff, string searchText, TokenModel token)
        {
            try
            {
                List<UsersDataModel> usersDataModel = _messageRepository.UsersDropDown<UsersDataModel>(isStaff, searchText, token).ToList();

                if (usersDataModel != null)
                {
                    return new JsonModel()
                    {
                        data = usersDataModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };

            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }
        public JsonModel GetThreadMessages(int parentMessageId, bool forStaff, TokenModel token, string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<InboxDataModel> inboxDataModels = _messageRepository.GetThreadMessages<InboxDataModel>(parentMessageId, forStaff, token, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (inboxDataModels.Count > 0)
            {
                var location = _locationService.GetLocationOffsets(token.LocationID, token);
                //Decrypt subject of message
                inboxDataModels.ForEach(a =>
                {
                    a.Subject = CommonMethods.Decrypt(a.Subject);
                    a.Text = CommonMethods.Decrypt(a.Text);
                    a.Thumbnail = !string.IsNullOrEmpty(a.Thumbnail) ? a.IsStaff == true ? CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.Thumbnail) : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.Thumbnail) : "";
                    a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, location, token);
                    a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').Select(x => CommonMethods.CreateImageUrl(token.Request, ImagesPath.MessageDocuments , x.Trim())).ToList() : null;

                    //change status of message
                    MessageRecepient messageRecepient = _messageRecepientsRepository.Get(r => r.MessageId == a.MessageId && r.Unread == true && r.IsDeleted == false);
                    if (messageRecepient != null)
                    {
                        messageRecepient.Unread = false;
                        messageRecepient.UpdatedBy = token.UserID;
                        messageRecepient.UpdatedDate = DateTime.UtcNow;
                        _messageRecepientsRepository.Update(messageRecepient);
                        _messageRecepientsRepository.SaveChanges();
                        //////////
                    }
                });
                return new JsonModel()
                {
                    data = inboxDataModels,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = inboxDataModels[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(inboxDataModels[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                };
            }
            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound
            };
        }
        public JsonModel GetMessageCounts(bool forStaff, TokenModel token)
        {
            try
            {
                MessageCountModel messageCountModel = _messageRepository.GetMessageCounts<MessageCountModel>(forStaff, token).FirstOrDefault();
                if (messageCountModel != null)
                {
                    return new JsonModel()
                    {
                        data = messageCountModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<MessageDocuments> MessageAttachedDocuments(MessageComposeModel messageComposeModel, TokenModel token)
        {
            try
            {
                List<MessageDocuments> messageDocList = new List<MessageDocuments>();
                string organizationName = _context.Organization.Where(a => a.Id == token.OrganizationID).FirstOrDefault().OrganizationName;

                #region saveDoc
                foreach (var item in messageComposeModel.Base64)
                {
                    MessageDocuments messageDocuments = new MessageDocuments();

                    item.Value.Replace("\"", "");
                    //string[] extensionArr = { "jpg", "jpeg", "png", "txt", "docx", "doc", "xlsx", "pdf", "pptx" };
                    //getting data from base64 url                    
                    string base64Data = Regex.Match(item.Key, @"(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = item.Value;

                    //out from the loop if document extenstion not exist in list of extensionArr
                    //if (!extensionArr.Contains(extension)) { goto Finish; }

                    //create directory
                    //string webRootPath = Directory.GetCurrentDirectory()+ "\\PatientDocuments";
                    string webRootPath = Directory.GetCurrentDirectory();

                    //save folder
                    string DirectoryUrl = ImagesPath.MessageDocuments;

                    if (!Directory.Exists(webRootPath + DirectoryUrl))
                    {
                        Directory.CreateDirectory(webRootPath + DirectoryUrl);
                    }

                    string fileName = organizationName + "_Message_" + DateTime.UtcNow.TimeOfDay.ToString();

                    //update file name remove unsupported attr.
                    fileName = fileName.Replace(" ", "_").Replace(":", "_");

                    //create path for save location
                    string path = webRootPath + DirectoryUrl + fileName + "." + extension;

                    //convert files into base
                    Byte[] bytes = Convert.FromBase64String(base64Data);
                    //save int the directory
                    File.WriteAllBytes(path, bytes);

                    //create db path
                    //string uploadPath = @"/Documents/ClientDocuments/" + fileName + "." + extension;

                    messageDocuments.MessageID = messageComposeModel.Id;
                    messageDocuments.Name = fileName + "." + extension;
                    messageDocList.Add(messageDocuments);

                }

                return messageDocList;
                //save into db
                //_messageDocumentRepository.Create(messageDocList.ToArray());
                //_messageDocumentRepository.SaveChanges();
                #endregion


                //Finish:;
                //return new JsonModel()
                //{
                //    data = new object(),
                //    Message = StatusMessage.InvaildFormat,
                //    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                //};
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MessagesInfoFromSignalRModel GetMessagesInfoFromSignalR(bool forStaff, HttpContext httpContext)
        {
            MessagesInfoFromSignalRModel messagesInfoFromSignalRModel = new MessagesInfoFromSignalRModel();
            //create token
            TokenModel token = CommonMethods.GetTokenDataModel(httpContext);

            //message counts
            MessageCountModel messageCountModel = _messageRepository.GetMessageCounts<MessageCountModel>(forStaff, token).FirstOrDefault();
            messagesInfoFromSignalRModel.MessageCount = messageCountModel;

            //inbox data
            List<InboxDataModel> inboxDataModels = _messageRepository.GetInboxData<InboxDataModel>(forStaff, token, "", "", 1, 10, "", "").ToList();
            if (inboxDataModels != null && inboxDataModels.Count > 0)
            {
                var location = _locationService.GetLocationOffsets(token.LocationID, token);
                //Decrypt subject of message
                inboxDataModels.ForEach(a =>
                {
                    a.Subject = CommonMethods.Decrypt(a.Subject);
                    a.Thumbnail = !string.IsNullOrEmpty(a.Thumbnail) ? a.IsStaff == true ? CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.Thumbnail) : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, a.Thumbnail) : "";
                    a.MessageDate = ConvertFromUtcTimeNew(a.MessageDate, location, token);
                    a.AttachedDocument = !string.IsNullOrEmpty(a.AttachedDocs) ? a.AttachedDocs.Split(',').ToList() : null;
                });
                messagesInfoFromSignalRModel.InboxData = inboxDataModels;
            }

            //return model
            return messagesInfoFromSignalRModel;
        }
    }
}
