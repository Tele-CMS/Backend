using HC.Notification.Service.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HC.Notification.Service
{
    public static class EmailNotificationService
    {
        public static void SendEmailNotification(MasterDatabaseModel databaseDetails, PatientDetail patientDetails, List<TemplateModel> template, OrganizationSMTPCommonModel organizationSMTPCommonModel)
        {
            EmailTempleateViewModel emailTempleateViewModel = new EmailTempleateViewModel();
            TemplateModel templateModel = new TemplateModel();

            switch (patientDetails.NotificationAction.Trim().ToLower())
            {
                case "creatappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.CreateAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
                case "deleteappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.DeleteAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
                case "cancelappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.CancelAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
                case "requestappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.RequestAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    emailTempleateViewModel.PatientOrStaffName = patientDetails.StaffName;
                    emailTempleateViewModel.ToEmail = patientDetails.StaffEmail;
                    emailTempleateViewModel.StaffName = null;
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
                case "approvedappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.RequestAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
                case "taskassign1":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.TaskAssign).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.TaskAssignNotification);
                    break;
                case "activateappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.ActivateAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;

                case "tentativeappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.TentativeAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;

                case "acceptappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.AcceptAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;

                case "autoconfirmtentativeappointment":
                    templateModel = template.Where(x => x.EmailTypeId == (int)EmailTemplateType.AutoConfirmTentativeAppointment).FirstOrDefault();
                    emailTempleateViewModel = EmailService.SetEmailTemplate(patientDetails, templateModel);
                    EmailService.SendEmailNotification(databaseDetails, emailTempleateViewModel, organizationSMTPCommonModel, EmailNotificationSubject.AppointmentNotification);
                    break;
            }
        }

        public static void TestService()
        {
            try
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
                                                    SendEmailNotification(databaseDetails[dbCount], patientDetails[i], templates, organizationSMTPCommonModel);
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
                //EmailService emailService = new EmailService();
                //NotificationDbService notificationServiceHelper = new NotificationDbService();
                //Dictionary<string, object> masterData = notificationServiceHelper.GetMasterOrganizationData();
                //List<MasterDatabaseModel> databaseDetails = (List<MasterDatabaseModel>)masterData["MasterDatabaseDetail"];
                //for (int dbCount = 0; dbCount < databaseDetails.Count; dbCount++)
                //{

                //    Dictionary<string, object> patientDetail = notificationServiceHelper.GetPushNotificationAndDeviceTokenDetails(databaseDetails[dbCount]);

                //    if (patientDetail["PatientDetail"] != null && ((List<PatientDetail>)patientDetail["PatientDetail"]).Count > 0)
                //    {
                //        List<PatientDetail> patientDetails = (List<PatientDetail>)patientDetail["PatientDetail"];
                //        for (var i = 0; i < patientDetails.Count; i++)
                //        {
                //            switch (patientDetails[i].NotificationType.Trim().ToLower())
                //            {
                //                case "pushnotification":
                //                    PushNotificationService.SendPushNotification(patientDetails[i], databaseDetails[dbCount]);
                //                    break;
                //                case "emailnotification":
                //                    SendEmailNotification(emailService, notificationServiceHelper, databaseDetails[dbCount], patientDetails[i]);
                //                    break;
                //            }
                //        }

                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

