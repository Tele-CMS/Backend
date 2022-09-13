
using HC.Notification.Service.Model;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HC.Notification.Service
{
    public static class EmailService
    {
        public static EmailTempleateViewModel SetEmailTemplate(PatientDetail patientDetails, TemplateModel templateModel)
        {
            EmailTempleateViewModel emailTempleateViewModel = new EmailTempleateViewModel
            {
                MappingId = patientDetails.MappingId,
                EndDateTimeUtc = patientDetails.EndDateTimeUtc,
                StartDateTimeUtc = patientDetails.StartDateTimeUtc,
                PatientName = !ReferenceEquals(patientDetails.PatientFirstName, null) ? string.Concat(patientDetails.PatientFirstName.Length > 2 ? patientDetails.PatientFirstName.Substring(0, 3) : patientDetails.PatientFirstName, "*** ", patientDetails.PatientLastName.Length > 2 ? patientDetails.PatientLastName.Substring(0, 3) : patientDetails.PatientLastName, "***") : string.Empty,
                StaffName = patientDetails.StaffName,
                PatientOrStaffName = !ReferenceEquals(patientDetails.PatientFirstName, null) ? string.Concat(patientDetails.PatientFirstName.Length > 2 ? patientDetails.PatientFirstName.Substring(0, 3) : patientDetails.PatientFirstName, "*** ", patientDetails.PatientLastName.Length > 2 ? patientDetails.PatientLastName.Substring(0, 3) : patientDetails.PatientLastName, "***") : string.Empty,
                ToEmail = patientDetails.PatientEmail,
                Address = !ReferenceEquals(patientDetails.Address, null) ? patientDetails.Address : string.Empty,
                AppointmentType = patientDetails.AppointmentType,
                EmailTemplate = templateModel.Template,
                CareManagerDetails = string.Format(": {0}, Email: {1}, and Contact number: {2}", patientDetails.CareManagerName, patientDetails.CareManagerEmail, patientDetails.CareManagerPhoneNumber)
            };
            return emailTempleateViewModel;
        }
        public static void SendEmailNotification(MasterDatabaseModel masterDatabaseDetail, EmailTempleateViewModel emailTempleateViewModel, OrganizationSMTPCommonModel organizationSMTPCommonModel, string vEmailSubject)
        {
            if (!string.IsNullOrEmpty(emailTempleateViewModel.ToEmail))
            {
                NotificationDbService notificationServiceHelper = new NotificationDbService();
                try
                {
                    // email to patient and staff
                    string toEmail, domainName;
                    toEmail = emailTempleateViewModel.ToEmail;
                    domainName = "Overture Healthcare Solutions";
                    var emailHtml = emailTempleateViewModel.EmailTemplate;
                    emailHtml = emailHtml.Replace("{{patientOrStaffName}}", emailTempleateViewModel.PatientOrStaffName);
                    emailHtml = emailHtml.Replace("{{appointmentType}}", emailTempleateViewModel.AppointmentType);
                    emailHtml = emailHtml.Replace("{{patientName}}", emailTempleateViewModel.PatientName);
                    emailHtml = emailHtml.Replace("{{address}}", emailTempleateViewModel.Address);
                    emailHtml = emailHtml.Replace("{{date}}", emailTempleateViewModel.StartDateTimeUtc.ToString("dddd, dd MMMM yyyy"));
                    emailHtml = emailHtml.Replace("{{startEndTime}}", string.Concat(emailTempleateViewModel.StartDateTimeUtc.ToString("hh:mm tt"), " to ", emailTempleateViewModel.EndDateTimeUtc.ToString("hh:mm tt")));
                    emailHtml = emailHtml.Replace("{{date}}", emailTempleateViewModel.StartDateTimeUtc.ToString("dddd, dd MMMM yyyy"));
                    emailHtml = emailHtml.Replace("###CareManager###", emailTempleateViewModel.CareManagerDetails);

                    if (!ReferenceEquals(emailTempleateViewModel, null))
                    {
                        emailHtml = emailHtml.Replace("{{withName}}", emailTempleateViewModel.StaffName);

                        SendEmailAsync(toEmail, vEmailSubject, emailHtml, organizationSMTPCommonModel, domainName, emailTempleateViewModel.MappingId, masterDatabaseDetail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static void SendEmailAsync(string toEmail, string subject, string message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName, int mappingId, MasterDatabaseModel masterDatabaseDetail)
        {
            NotificationDbService notificationServiceHelper = new NotificationDbService();
            try
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    MailMessage mailMessage = new MailMessage();
                    string FROM = organizationSMTPModel.SMTPUserName;
                    string SMTP_USERNAME = organizationSMTPModel.NetworkUserName;//"AKIAVKYOTFMTCBWCEYBB";
                    string SMTP_PASSWORD = organizationSMTPModel.SMTPPassword;
                    string HOST = organizationSMTPModel.ServerName;
                    int PORT = int.Parse(organizationSMTPModel.Port);
                    string BODY = message;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress(FROM);
                    mailMessage.To.Add(new MailAddress(toEmail));
                    mailMessage.Subject = subject;
                    mailMessage.Body = BODY;
                    using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
                    {
                        client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                        client.EnableSsl = true;

                        client.Send(mailMessage);
                        notificationServiceHelper.UpdateNotificationStatus(mappingId, message, masterDatabaseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("\nMessage ---{0} \nHelpLink ---{1} \nStackTrace ---\n{2} \nTargetSite ---{3}", ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);
                Console.WriteLine(ex.Message);
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                int line = frame.GetFileLineNumber();

                ErrorLogModel errorLogModel = new ErrorLogModel
                {
                    ErrorLine = line,
                    ErrorMessage = exMessage,
                    ErrorTime = DateTime.UtcNow,
                    ErrorLogTypeId = 1,
                    OrganizationId = 3
                };
                notificationServiceHelper.InsertErrorLog(errorLogModel, masterDatabaseDetail);
            }

            //if (toEmail == null)
            //    return;
            //NotificationDbService notificationServiceHelper = new NotificationDbService();
            //ErrorLogModel errorLogModel = new ErrorLogModel();
            //try
            //{
            //    MailMessage mail = new MailMessage();
            //    mail.From = new MailAddress(organizationSMTPModel.SMTPUserName);
            //    mail.To.Add(toEmail);
            //    mail.Subject = subject;
            //    mail.IsBodyHtml = true;
            //    mail.Body = message;
            //    SmtpClient client = null;
            //    using (client = new SmtpClient(organizationSMTPModel.ServerName))
            //    {
            //        client.Port = int.Parse(organizationSMTPModel.Port);
            //        client.Credentials = new System.Net.NetworkCredential(organizationSMTPModel.SMTPUserName, organizationSMTPModel.SMTPPassword);
            //        client.EnableSsl = true;
            //        client.Send(mail);
            //        notificationServiceHelper.UpdateNotificationStatus(mappingId, message, masterDatabaseDetail);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string exMessage = string.Format("\nMessage ---{0} \nHelpLink ---{1} \nStackTrace ---\n{2} \nTargetSite ---{3}", ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);
            //    Console.WriteLine(exMessage);
            //    errorLogModel.ErrorMessage = exMessage;               
            //    errorLogModel.ErrorTime = DateTime.UtcNow;
            //    errorLogModel.ErrorLogTypeId = (int)ErrorLogType.EmailNotification;
            //    notificationServiceHelper.InsertErrorLog(errorLogModel, masterDatabaseDetail);
            //}
        }
        public static void SendEmailAsyncTest(String subject, String message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName, MasterDatabaseModel masterDatabaseDetail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("smarthealth2019.user@gmail.com");
                mail.To.Add("robert05@yopmail.com");
                mail.Subject = subject;
                mail.Body = message;
                SmtpClient client = null;
                using (client = new SmtpClient(organizationSMTPModel.ServerName))
                {
                    client.Port = int.Parse(organizationSMTPModel.Port);
                    client.Credentials = new System.Net.NetworkCredential("smarthealth2019.user@gmail.com", "Password@123");
                    client.EnableSsl = true;
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
