using HC.Notification.Service.Model;
using HC.Patient.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HC.Notification.Service
{
    public static class PushNotificationService
    {
        public static void SendPushNotification(PatientDetail patientDetail, MasterDatabaseModel databaseDetail)
        {
            if (patientDetail.DeviceToken == null)
                return;
            try
            {
                NotificationDbService notificationServiceHelper = new NotificationDbService();
                WebRequest tRequest = WebRequest.Create(NotificationServiceCommon.PushNotification_APIEndPoint);
                tRequest.Method = "POST";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", NotificationServiceCommon.PushNotification_APIEndPointToken));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = patientDetail.DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = patientDetail.SetPushNotificationMessage
                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    string sResponseFromServer = tReader.ReadToEnd();
                                    var result = JsonConvert.DeserializeObject<FcmResponce>(sResponseFromServer);
                                    if (result != null && result.success == 1)
                                    {
                                        //string message;
                                        //if(patientDetail.NotificationAction == "ChatMessage")
                                        //{
                                        //    message = CommonMethods.Encrypt(patientDetail.SetPushNotificationMessage);
                                        //}
                                        //else
                                        //{
                                        //    message = patientDetail.SetPushNotificationMessage;
                                        //}
                                        notificationServiceHelper.UpdateNotificationStatus(patientDetail.MappingId, patientDetail.SetPushNotificationMessage, databaseDetail);
                                    }

                                }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
