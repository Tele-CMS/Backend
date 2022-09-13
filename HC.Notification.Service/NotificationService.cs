using HC.Notification.Service.Model;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HC.Notification.Service
{
    public class NotificationService : IHostedService, IDisposable
    {
        private static System.Timers.Timer _timer;
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += ATimer_Elapsed;
            _timer.Interval = 60000;
            _timer.Enabled = true;
            return Task.CompletedTask;
            //EmailNotificationService.TestService();
            //return Task.CompletedTask;
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            NotificationDbService notificationServiceHelper = new NotificationDbService();
            Dictionary<string, object> masterData = notificationServiceHelper.GetMasterOrganizationData();
            if (masterData["MasterDatabaseDetail"] != null && ((List<MasterDatabaseModel>)masterData["MasterDatabaseDetail"]).Count > 0)
            {
                List<MasterDatabaseModel> databaseDetails = (List<MasterDatabaseModel>)masterData["MasterDatabaseDetail"];

                for (int dbCount = 0; dbCount < databaseDetails.Count; dbCount++)
                {
                    List<EmailTempleateViewModel> emailTempleateViewModel = new List<EmailTempleateViewModel>();
                    List<TemplateModel> templates = null;
                    OrganizationSMTPCommonModel organizationSMTPCommonModel = null;
                    Dictionary<string, object> templateDetail = notificationServiceHelper.GetEmailTemplate(databaseDetails[dbCount]);
                    if (templateDetail["TemplateModel"] != null && ((List<TemplateModel>)templateDetail["TemplateModel"]) != null)
                    {
                        templates = (List<TemplateModel>)templateDetail["TemplateModel"];
                    }

                    Dictionary<string, object> smtpDetails = notificationServiceHelper.GetSMTPDetails(databaseDetails[dbCount]);
                    if (smtpDetails["OrganizationSMTPCommonModel"] != null)
                    {
                        organizationSMTPCommonModel = (OrganizationSMTPCommonModel)smtpDetails["OrganizationSMTPCommonModel"];
                        organizationSMTPCommonModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPCommonModel.SMTPPassword);
                    }
                    if (organizationSMTPCommonModel != null && templates != null && templates.Count > 0)
                    {
                        try
                        {
                            Dictionary<string, object> patientDetail = notificationServiceHelper.GetPushNotificationAndDeviceTokenDetails(databaseDetails[dbCount]);
                            if (patientDetail["PatientDetail"] != null && ((List<PatientDetail>)patientDetail["PatientDetail"]).Count > 0)
                            {
                                List<PatientDetail> patientDetails = (List<PatientDetail>)patientDetail["PatientDetail"];
                                for (var i = 0; i < patientDetails.Count; i++)
                                {
                                    switch (patientDetails[i].NotificationType.Trim().ToLower())
                                    {
                                        case "pushnotification":
                                            PushNotificationService.SendPushNotification(patientDetails[i], databaseDetails[dbCount]);
                                            break;
                                        case "emailnotification":
                                            if (templates != null)
                                            {
                                                EmailNotificationService.SendEmailNotification(databaseDetails[dbCount], patientDetails[i], templates, organizationSMTPCommonModel);
                                            }
                                            break;
                                    }
                                }

                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            var st = new StackTrace(ex, true);
                            var frame = st.GetFrame(0);
                            int line = frame.GetFileLineNumber();
                            string exMessage = string.Format("\nMessage ---{0} \nHelpLink ---{1} \nStackTrace ---\n{2} \nTargetSite ---{3}", ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);

                            ErrorLogModel errorLogModel = new ErrorLogModel
                            {
                                ErrorLine = line,
                                ErrorMessage = exMessage,
                                ErrorTime = DateTime.UtcNow,
                                ErrorLogTypeId = 1,
                                OrganizationId = 3
                            };
                            notificationServiceHelper.InsertErrorLog(errorLogModel, databaseDetails[dbCount]);
                        }
                    }
                }
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Thread.Sleep(1000);
            return Task.CompletedTask;
        }
    }
}

