using HC.Common.Enums;
using HC.Patient.Model.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace HC.Patient.Service.Services.Notification
{
    public static class PushNotificationsService
    {

        public static void SendPushNotificationForMobile(PushMobileNotificationModel notificationDetails)
        {
            if (notificationDetails.DeviceToken == null)
                return;

            try
            {
                WebRequest tRequest = WebRequest.Create(CommonEnum.FCMAPIConfig.PushNotification_APIEndPoint);
                tRequest.Method = "POST";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", CommonEnum.FCMAPIConfig.PushNotification_APIEndPointToken));
                tRequest.Headers.Add(string.Format("Sender: id={0}", CommonEnum.FCMAPIConfig.PushNotification_SenderKey));
                tRequest.ContentType = "application/json";

                var payload = new
                {
                    to = notificationDetails.DeviceToken,
                    priority = notificationDetails.NotificationPriority,
                    notification = new
                    {
                        body = notificationDetails.Message,
                        sound = "default",
                        badge = 0,
                    },
                    data = notificationDetails.Data,
                    notificationType = notificationDetails.NotificationType,
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

                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    string sResponseFromServer = tReader.ReadToEnd();
                                    //var result = JsonConvert.DeserializeObject<FCMResponseModel>(sResponseFromServer);
                                    //if (result != null && result.success == 1)
                                    //{
                                    //    _logger.LogInformation($"({result})");
                                    //    // reminderDbService.ReminderLogs(databaseDetails, result, true);
                                    //    //notificationService.UpdateSentPushNotification(notificationDetails);
                                    //}
                                }
                    }
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
            }
        }


    }
}
