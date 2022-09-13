
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.NotificationSetting;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Repositories.Interfaces;
using HC.Common.Model.OrganizationSMTP;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Common.Services;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientAlertService : BaseService, IPatientAlertService
    {
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientAlertRepository _patientAlertRepository;
        private readonly ILocationService _locationService;
        private readonly IChatRepository _chatRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _env;
        public PatientAlertService(IPatientAlertRepository patientAlertRepository, ILocationService locationService, IChatRepository chatRepository,
           IPatientRepository patientRepository, ITokenRepository tokenRepository, 
           
            IOrganizationSMTPRepository organizationSMTPRepository, IEmailService emailService, IHostingEnvironment env)
        {
            _patientAlertRepository = patientAlertRepository;
            _locationService = locationService;
            _chatRepository = chatRepository;
            _patientRepository = patientRepository;
            _tokenRepository = tokenRepository;
            _organizationSMTPRepository = organizationSMTPRepository;
            _emailService = emailService;
            _env = env;
        }

        public JsonModel GetPatientAlerts(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientAlertModel> patientAlerts = _patientAlertRepository.GetPatientAlerts<PatientAlertModel>(patientFilterModel, tokenModel).ToList();
            if (patientAlerts != null && patientAlerts.Count > 0)
            {
                response = new JsonModel(patientAlerts, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientAlerts, patientFilterModel);
            }
            return response;
        }

        
        public bool SavePatientAlerts(string alertDetails, int patientId, int masterAlertTypeId, int? documentId, TokenModel tokenModel)
        {
            PatientAlerts patientAlerts = new PatientAlerts();
            bool isSuccess = false;
            try
            {
                patientAlerts.AlertDetails = alertDetails;
                patientAlerts.FillDate = DateTime.Now;
                patientAlerts.IsActive = true;
                patientAlerts.IsAlert = true;
                patientAlerts.IsDeleted = false;
                patientAlerts.LoadDate = DateTime.Now;
                patientAlerts.OrganizationId = tokenModel.OrganizationID;
                patientAlerts.PatientId = patientId;
                patientAlerts.MasterAlertTypeId = masterAlertTypeId;
                patientAlerts.PatientDocumentId = documentId;
                patientAlerts.CreatedBy = tokenModel.UserID;
                patientAlerts.CreatedDate = DateTime.UtcNow;
                _patientAlertRepository.Create(patientAlerts);
                var result = _patientAlertRepository.SaveChanges();
                if (result > 0)
                {
                    isSuccess = true;
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        //public SignalRNotificationForBulkMessageModel SendBulkMessagePatientAlerts(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel)
        //{
        //    List<PatientAlertUserModel> bulkMessageMemberData = _patientAlertRepository.GetAllPatientAlertsUsers<PatientAlertUserModel>(patientFilterModel, tokenModel).ToList();
        //    List<Chat> chats = new List<Chat>();
        //    MemoryStream memoryStream = new MemoryStream();
        //    LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
        //    if (bulkMessageMemberData != null && bulkMessageMemberData.Count > 0)
        //    {
        //        using (var transaction = _patientAlertRepository.StartTransaction())
        //        {
        //            try
        //            {
        //                BulkMessage bulkMessage = new BulkMessage();
        //                chats = bulkMessageMemberData.Select(patient => new Chat
        //                {
        //                    FromUserId = tokenModel.UserID,
        //                    ToUserId = patient.UserId,
        //                    Message = CommonMethods.Encrypt(patientFilterModel.Message),
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = tokenModel.UserID,
        //                    OrganizationID = tokenModel.OrganizationID,
        //                    IsDeleted = false,
        //                    IsActive = true,
        //                    ChatDate = DateTime.UtcNow,
        //                }).ToList();

        //                _chatRepository.Create(chats.ToArray());
        //                _chatRepository.SaveChanges();

        //                SaveChatNotification(chats, (int)NotificationActionType.ChatMessage, tokenModel);

        //                if (bulkMessageMemberData != null && bulkMessageMemberData.Count() > 0 && chats != null && chats.Count() > 0)
        //                {
        //                    List<PrintPatientBulkMessageStatus> printPatientEmails = new List<PrintPatientBulkMessageStatus>();
        //                    bulkMessageMemberData.ForEach(x =>
        //                    {
        //                        PrintPatientBulkMessageStatus printPatientEmail = new PrintPatientBulkMessageStatus();
        //                        printPatientEmail.FirstName = x.FirstName;
        //                        printPatientEmail.LastName = x.LastName;
        //                        printPatientEmail.Email = x.Email;
        //                        printPatientEmail.Dob = DateTime.Parse(x.Dob).ToString("MM/dd/yyyy");
        //                        printPatientEmail.Date = CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("MM/dd/yyyy");
        //                        printPatientEmail.Status = x.IsActive == true ? "Active" : "In Active";
        //                        printPatientEmails.Add(printPatientEmail);
        //                    });

        //                    //SaveBulkEmailAndMessageReportLogs(filterModel, tokenModel, bulkMessageMemberData);

        //                    if (printPatientEmails != null && printPatientEmails.Count() > 0)
        //                    {
        //                        memoryStream = GenerateMessageExcelReport(patientFilterModel, tokenModel, locationModel, printPatientEmails);
        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //    SignalRNotificationForBulkMessageModel signalRModel = new SignalRNotificationForBulkMessageModel();
        //    signalRModel.MemoryStream = memoryStream;
        //    signalRModel.Chat = chats;
        //    return signalRModel; // new JsonModel(messageCount, string.Format("Email log: No. of messages sent are {0}, and no. of messages not sent are {1}", messageCount.SendCount, messageCount.UnSendCount), (int)HttpStatusCodes.OK);
        //}
        //public SignalRNotificationForBulkEmailModel SendBulkEmailPatientAlerts(PatientAlertFilterModel filterModel, TokenModel tokenModel)
        //{
        //    List<PatientAlertUserModel> bulkMessagePatientModel = _patientAlertRepository.GetAllPatientAlertsUsers<PatientAlertUserModel>(filterModel, tokenModel).ToList();
        //    BuklEmailSuccessMessageCountModel messageCount = new BuklEmailSuccessMessageCountModel();
        //    LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
        //    MemoryStream memoryStream = new MemoryStream();
        //    SignalRNotificationForBulkEmailModel signalRNotificationForBulkEmailModel = new SignalRNotificationForBulkEmailModel();
        //    if (bulkMessagePatientModel != null && bulkMessagePatientModel.Count > 0)
        //    {
        //        using (var transaction = _patientAlertRepository.StartTransaction())
        //        {
        //            try
        //            {
        //                BulkMessage bulkMessage = new BulkMessage();
        //                bulkMessage.Message = CommonMethods.Encrypt(filterModel.Message);
        //                bulkMessage.Subject = CommonMethods.Encrypt(filterModel.Subject);
        //                bulkMessage.ModuleType = LogModuleType.MemberAlerts;
        //                bulkMessage.CreatedDate = DateTime.UtcNow;
        //                bulkMessage.CreatedBy = tokenModel.UserID;
        //                bulkMessage.OrganizationID = tokenModel.OrganizationID;
        //                bulkMessage.IsDeleted = false;
        //                bulkMessage.IsActive = true;
        //                _bulkMessageRepository.Create(bulkMessage);
        //                _bulkMessageRepository.SaveChanges();

        //                List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping = bulkMessagePatientModel.Select(patientid => new BulkMessageAndPatientMapping
        //                {
        //                    PatientId = patientid.PatientId,
        //                    MessageId = bulkMessage.MessageId,
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = tokenModel.UserID,
        //                    OrganizationID = tokenModel.OrganizationID,
        //                    IsDeleted = false,
        //                    IsActive = true,
        //                    UserId = patientid.UserId
        //                }).ToList();


        //                _bulkMessageAndPatientMappingRepository.Create(bulkMessageAndPatientMapping.ToArray());
        //                _bulkMessageAndPatientMappingRepository.SaveChanges();

        //                SendBulkEmailPatient(bulkMessagePatientModel, filterModel.Message, filterModel.Subject, bulkMessageAndPatientMapping, bulkMessage.MessageId, tokenModel);

        //                _bulkMessageAndPatientMappingRepository.Update(bulkMessageAndPatientMapping.ToArray());
        //                _bulkMessageAndPatientMappingRepository.SaveChanges();

        //                SaveEmailNotification(bulkMessageAndPatientMapping, (int)NotificationActionType.BulkEmail, tokenModel);

        //                //messageCount = bulkMessageAndPatientMapping.Select(x => new BuklEmailSuccessMessageCountModel
        //                //{
        //                //    SendCount = bulkMessageAndPatientMapping.Where(y => y.IsMessageSend == true && y.MessageId == bulkMessage.MessageId).Count(),
        //                //    UnSendCount = bulkMessageAndPatientMapping.Where(y => y.IsMessageSend == false && y.MessageId == bulkMessage.MessageId).Count(),
        //                //}).FirstOrDefault();

        //                // SaveBulkEmailAndMessageReportLogs(filterModel, tokenModel, bulkMessageMemberData);

        //                signalRNotificationForBulkEmailModel.MemoryStream = GenerateEmailExcelReport(filterModel, bulkMessagePatientModel, memoryStream, LogModuleType.MemberAlerts, bulkMessageAndPatientMapping, tokenModel, locationModel);
        //                signalRNotificationForBulkEmailModel.BulkMessageAndPatientMapping = bulkMessageAndPatientMapping;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //    return signalRNotificationForBulkEmailModel; // new JsonModel(messageCount, string.Format("Email log: No. of messages sent are {0}, and no. of messages not sent are {1}", messageCount.SendCount, messageCount.UnSendCount), (int)HttpStatusCodes.OK);
        //}
        //private void SaveChatNotification(List<Chat> chats, int actionId, TokenModel token)
        //{
        //    List<NotificationModel> notifications = chats.Select(chat => new NotificationModel
        //    {
        //        PatientId = _patientRepository.GetPatientByUserId(chat.ToUserId),
        //        StaffId = _tokenRepository.GetStaffByuserID(chat.FromUserId).Id,
        //        ActionTypeId = actionId,
        //        ChatId = chat.Id,
        //        Message = chat.Message,
        //    }).ToList();
        //    if (notifications.Count() > 0)
        //    {
        //        _notificationService.SaveNotification(notifications, token);
        //    }
        //}
        //private MemoryStream GenerateMessageExcelReport(PatientAlertFilterModel filterModel, TokenModel tokenModel, LocationModel locationModel, List<PrintPatientBulkMessageStatus> printPatientEmails)
        //{
        //    using (var excel = new ExcelPackage())
        //    {
        //        excel.Workbook.Worksheets.Add("Worksheet1");

        //        var headerRow = new List<string[]>() { new string[] { "First Name", "Last Name", "Email", "DOB", "Message", "Date", "Status" } };

        //        // Determine the header range (e.g. A1:D1)
        //        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        worksheet.DefaultRowHeight = 12;

        //        worksheet.Row(1).Height = 20;
        //        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(1).Style.Font.Bold = true;

        //        worksheet.Cells[1, 1].Value = "Message";
        //        worksheet.Cells[1, 2].Value = filterModel.Message;
        //        worksheet.Cells[1, 2, 1, 6].Merge = true;

        //        worksheet.Row(2).Height = 20;
        //        worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        worksheet.Row(2).Style.Font.Bold = true;


        //        worksheet.Column(1).Width = 30;
        //        worksheet.Column(2).Width = 30;
        //        worksheet.Column(3).Width = 30;
        //        worksheet.Column(4).Width = 20;
        //        worksheet.Column(5).Width = 20;
        //        worksheet.Column(6).Width = 20;


        //        worksheet.Cells[2, 1].Value = "First Name"; //headerRow[0];
        //        worksheet.Cells[2, 2].Value = "Last Name"; //headerRow[2];
        //        worksheet.Cells[2, 3].Value = "Email"; //headerRow[1];
        //        worksheet.Cells[2, 4].Value = "DOB"; //headerRow[3];
        //        worksheet.Cells[2, 5].Value = "Date"; //headerRow[5];
        //        worksheet.Cells[2, 6].Value = "Status"; //headerRow[6];

        //        //worksheet.Cells["A1"].Value = "Report";
        //        //// Popular header row data
        //        //worksheet.Cells["A1"].LoadFromArrays(headerRow);

        //        worksheet.Cells[3, 1].LoadFromCollection(printPatientEmails);

        //        return ReadFileAndSaveLocally(excel, "MessageLog", LogModuleType.MemberAlerts, tokenModel, locationModel, (int)MemberEmailAndMessageReportType.Message);
        //    }
        //}
        //private MemoryStream ReadFileAndSaveLocally(ExcelPackage excel, string logType, string moduleType, TokenModel tokenModel, LocationModel locationModel, int reportType)
        //{

        //    string webRootPath = Directory.GetCurrentDirectory();

        //    //add your custom path
        //    webRootPath = webRootPath + ImagesPath.HRAEmailAndMessageReport;

        //    //check 
        //    if (!Directory.Exists(webRootPath))
        //    {
        //        Directory.CreateDirectory(webRootPath);
        //    }
        //    string fileName = logType + "_" + moduleType + "Report-" + CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
        //    //var excelPrint = excel;
        //    string filePath = string.Format("{0}/{1}", webRootPath, fileName);
        //    FileInfo fileInfo = new FileInfo(filePath);
        //    excel.SaveAs(fileInfo);
        //    MemoryStream memStream = new MemoryStream();
        //    using (FileStream fileStream = File.OpenRead(filePath))
        //    {

        //        memStream.SetLength(fileStream.Length);
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
        //    }

        //    SaveHRAEmailAndMessageReportLog(tokenModel, fileName, locationModel, reportType, moduleType);
        //    return memStream;
        //}
        //private void SaveHRAEmailAndMessageReportLog(TokenModel tokenModel, string fileName, LocationModel locationModal, int reportType, string moduleType)
        //{
        //    HRAEmailAndMessageReportLog hraEmailAndMessageReportLog = new HRAEmailAndMessageReportLog();
        //    hraEmailAndMessageReportLog.CreatedDate = DateTime.UtcNow;
        //    hraEmailAndMessageReportLog.IsActive = true;
        //    hraEmailAndMessageReportLog.IsDeleted = false;
        //    hraEmailAndMessageReportLog.FileName = fileName;
        //    hraEmailAndMessageReportLog.ModuleType = moduleType;
        //    hraEmailAndMessageReportLog.OrganizationId = tokenModel.OrganizationID;
        //    hraEmailAndMessageReportLog.ReportDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, locationModal.TimeZoneName);
        //    hraEmailAndMessageReportLog.ReportType = reportType;
        //    hraEmailAndMessageReportLog.UserID = tokenModel.UserID;
        //    _hraEmailAndMessageReportLogRepository.Create(hraEmailAndMessageReportLog);
        //    _hraEmailAndMessageReportLogRepository.SaveChanges();
        //}
        //public void SendBulkEmailPatient(List<PatientAlertUserModel> patientInfo, string message, string vEmailSubject, List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, int messageId, TokenModel tokenModel)
        //{
        //    OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
        //    OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
        //    AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
        //    organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);
        //    var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + "/templates/bulk-email-template.html");

        //    foreach (var data in bulkMessageAndPatientMapping)
        //    {
        //        bool isSend = false;
        //        PatientAlertUserModel patient = patientInfo.Where(w => w.PatientId == data.PatientId).FirstOrDefault();

        //        if (!string.IsNullOrEmpty(patient.Email))
        //        {
        //            string htmlFile = string.Empty;
        //            htmlFile = emailHtml;
        //            //htmlFile = htmlFile.Replace("{{patientName}}", string.Concat(patient.FirstName.Substring(0, 3), "*** ", patient.LastName.Substring(0, 3), "***"));
        //            htmlFile = htmlFile.Replace("{{message}}", message);
        //            isSend = _emailService.SendEmail(patient.Email, vEmailSubject, htmlFile, organizationSMTPDetailModel).Result;
        //        }
        //        data.IsMessageSend = isSend ? true : false;
        //    }
        //}
        //private void SaveEmailNotification(List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, int actionId, TokenModel token)
        //{
        //    List<NotificationModel> notifications = bulkMessageAndPatientMapping.Where(x => x.IsMessageSend == true && x.IsActive == true && x.IsDeleted == false).Select(y => new NotificationModel
        //    {
        //        PatientId = y.PatientId,
        //        StaffId = _tokenRepository.GetStaffByuserID(token.UserID).Id,
        //        ActionTypeId = actionId,
        //    }).ToList();
        //    if (notifications.Count() > 0)
        //    {
        //        _notificationService.SaveNotification(notifications, token);
        //    }
        //}
        //private MemoryStream GenerateEmailExcelReport(PatientAlertFilterModel filterModel, List<PatientAlertUserModel> bulkMessagePatientModel, MemoryStream memoryStream, string moduleKey, List<BulkMessageAndPatientMapping> bulkMessageAndPatientMapping, TokenModel tokenModel, LocationModel locationModel)
        //{
        //    string plainText = HtmlToPlainText(filterModel.Message);


        //    if (bulkMessagePatientModel != null && bulkMessagePatientModel.Count() > 0 && bulkMessageAndPatientMapping != null && bulkMessageAndPatientMapping.Count() > 0)
        //    {
        //        List<PrintPatientBulkEmailSentStatus> printPatientEmail = (from l1 in bulkMessagePatientModel
        //                                                                   join l2 in bulkMessageAndPatientMapping
        //                                                                   on l1.PatientId equals l2.PatientId
        //                                                                   select new PrintPatientBulkEmailSentStatus
        //                                                                   {
        //                                                                       FirstName = l1.FirstName,
        //                                                                       LastName = l1.LastName,
        //                                                                       Dob = DateTime.Parse(l1.Dob).ToString("MM/dd/yyyy"),
        //                                                                       Email = l1.Email,
        //                                                                       Date = CommonMethods.ConvertToUtcTimeWithOffset(DateTime.UtcNow, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString("MM/dd/yyyy"),
        //                                                                       Status = l2.IsMessageSend == true ? "Sent" : "Failed",
        //                                                                   }).ToList();

        //        if (printPatientEmail != null && printPatientEmail.Count() > 0)
        //        {
        //            using (var excel = new ExcelPackage())
        //            {
        //                excel.Workbook.Worksheets.Add("Worksheet1");

        //                //var headerRow = new List<string[]>() { new string[] { "First Name", "Last Name", "Email", "DOB", "Template", "Subject", "Date", "Status" } };

        //                // Determine the header range (e.g. A1:D1)
        //                //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

        //                // Target a worksheet
        //                var worksheet = excel.Workbook.Worksheets["Worksheet1"];

        //                worksheet.DefaultRowHeight = 12;
        //                worksheet.Row(1).Height = 20;
        //                worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(1).Style.Font.Bold = true;

        //                worksheet.Cells[1, 1].Value = "Subject";
        //                worksheet.Cells[1, 2].RichText.Text += filterModel.Subject;
        //                worksheet.Cells[1, 2].Style.Font.Bold = false;
        //                worksheet.Cells[1, 2, 1, 7].Merge = true;

        //                worksheet.DefaultRowHeight = 12;
        //                worksheet.Row(2).Height = 20;
        //                worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(2).Style.Font.Bold = true;

        //                worksheet.Cells[2, 1].Value = "Message";
        //                worksheet.Cells[2, 2].RichText.Text += plainText;
        //                worksheet.Cells[2, 2].Style.Font.Bold = false;
        //                worksheet.Cells[2, 2, 2, 7].Merge = true;

        //                worksheet.Row(3).Height = 20;
        //                worksheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(3).Style.Font.Bold = true;
        //                worksheet.Cells[3, 1, 3, 6].Merge = true;

        //                worksheet.Row(4).Height = 20;
        //                worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(4).Style.Font.Bold = true;

        //                worksheet.Column(1).Width = 30;
        //                worksheet.Column(2).Width = 30;
        //                worksheet.Column(3).Width = 40;
        //                worksheet.Column(4).Width = 30;
        //                worksheet.Column(5).Width = 30;
        //                worksheet.Column(6).Width = 30;

        //                worksheet.Cells[4, 1].Value = "First Name"; //headerRow[0];
        //                worksheet.Cells[4, 2].Value = "Last Name"; //headerRow[1];
        //                worksheet.Cells[4, 3].Value = "Email"; //headerRow[2];
        //                worksheet.Cells[4, 4].Value = "DOB"; //headerRow[3];
        //                worksheet.Cells[4, 5].Value = "Date"; //headerRow[5];
        //                worksheet.Cells[4, 6].Value = "Status"; //headerRow[6];

        //                //worksheet.Cells["A1"].Value = "Hello World!";
        //                // Popular header row data
        //                //worksheet.Cells["headerRange"].LoadFromArrays(headerRow);
        //                worksheet.Row(4).Height = 20;
        //                worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                worksheet.Row(4).Style.Font.Bold = false;
        //                worksheet.Cells[5, 1].LoadFromCollection(printPatientEmail);

        //                return ReadFileAndSaveLocally(excel, "EmailLog", moduleKey, tokenModel, locationModel, (int)MemberEmailAndMessageReportType.Email);

        //            }
        //        }
        //    }

        //    return memoryStream;
        //}
        //private static string HtmlToPlainText(string html)
        //{
        //    if (string.IsNullOrWhiteSpace(html))
        //    {
        //        return null;
        //    }
        //    const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
        //    const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
        //    const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
        //    var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
        //    var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
        //    var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
        //    var text = html;
        //    //Decode html specific characters
        //    text = System.Net.WebUtility.HtmlDecode(text);
        //    //Remove tag whitespace/line breaks
        //    text = tagWhiteSpaceRegex.Replace(text, "><");
        //    //Replace <br /> with line breaks
        //    text = lineBreakRegex.Replace(text, Environment.NewLine);
        //    //Strip formatting
        //    text = stripFormattingRegex.Replace(text, string.Empty);

        //    RegexOptions options = RegexOptions.None;
        //    Regex regex = new Regex("[ ]{2,}", options);
        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        text = regex.Replace(text, " ");
        //    }
        //    return text;
        //}
    }
}
