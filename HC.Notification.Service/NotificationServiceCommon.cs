using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace HC.Notification.Service
{
    public class NotificationServiceCommon
    {
        public static string PushNotification_APIEndPoint = "https://fcm.googleapis.com/fcm/send";
        public static string PushNotification_APIEndPointToken = "AIzaSyAIi0QuaWkZ_DTrblv1sIAYKyrB1_xVqfQ";//"AIzaSyDZ1WWskbcaGE-WWZBeZB9ptvXTM7Rd6EQ";

        public static IList<T> DataReaderMapToList<T>(IDataReader dr)
        {
            IList<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch { continue; }
                }
                list.Add(obj);
            }
            return list;
        }

    }
}
